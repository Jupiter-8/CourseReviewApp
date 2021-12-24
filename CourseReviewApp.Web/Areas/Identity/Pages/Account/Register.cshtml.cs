using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using CourseReviewApp.Web.Services.Interfaces;
using CourseReviewApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;

        public RegisterModel(UserManager<AppUser> userManager, ILogger<RegisterModel> logger,
            IEmailSenderService emailSenderService, IFileService fileTool, IUserService userService)
        {
            _userManager = userManager;
            _logger = logger;
            _emailSenderService = emailSenderService;
            _fileService = fileTool;
            _userService = userService;
        }

        [BindProperty]
        public RegisterUserVm Input { get; set; }

        public async Task OnGetAsync()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string returnUrl = "~/Identity/Account/Login";
            if (ModelState.IsValid)
            {
                var userTuple = await CreateUser(Input);
                var result = await _userManager.CreateAsync(userTuple.Item1, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userTuple.Item1, userTuple.Item2.Name);
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(userTuple.Item1);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userTuple.Item1.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    string body = await _fileService.LoadMessageHtml("account_confirmation.html");
                    body = body.Replace("{username}", $"{Input.Firstname} {Input.Lastname}");
                    body = body.Replace("{href}", callbackUrl);
                    await _emailSenderService.SendEmailAsync("Confirm your email", body, Input.Email);
                    TempData["LoginModalMsg"] = "Your account has been created. If you want to log in, you must first confirm your email. " +
                        "A message with a confirmation link has been sent to your email adress.";

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private async Task<Tuple<AppUser, Role>> CreateUser(RegisterUserVm viewModel)
        {
            Role role = await _userService.GetRole(r => r.Id == viewModel.UserType);
            if (role == null)
                throw new InvalidOperationException("Role not exists.");

            switch (role.RoleValue)
            {
                case RoleValue.Course_client:
                    return new Tuple<AppUser, Role>(new CourseClient
                    {
                        UserName = viewModel.Username,
                        Email = viewModel.Email,
                        FirstName = viewModel.Firstname,
                        LastName = viewModel.Lastname,
                        RegistrationDate = DateTime.Now,
                        AvatarPath = "default_user_avatar.jpg",
                        IsActive = true,
                    }, role);

                case RoleValue.Course_owner:
                    return new Tuple<AppUser, Role>(new CourseOwner
                    {
                        UserName = viewModel.Username,
                        Email = viewModel.Email,
                        FirstName = viewModel.Firstname,
                        LastName = viewModel.Lastname,
                        RegistrationDate = DateTime.Now,
                        BrandName = viewModel.Brandname,
                        WebsiteUrl = viewModel.WebsiteUrl,
                        AvatarPath = "default_user_avatar.jpg",
                        IsActive = true,
                    }, role);

                default:
                    return null;
            }
        }
    }
}
