using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CourseReviewApp.DAL.EntityFramework;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Web.ViewModels;
using CourseReviewApp.Web.Services.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IFileService _fileService;

        public RegisterModel(UserManager<AppUser> userManager, ILogger<RegisterModel> logger,
            IEmailSenderService emailSenderService, AppDbContext dbContext, IFileService fileTool)
        {
            _userManager = userManager;
            _logger = logger;
            _emailSenderService = emailSenderService;
            _dbContext = dbContext;
            _fileService = fileTool;
        }

        [BindProperty]
        public RegisterUserVm Input { get; set; }

        public async Task OnGetAsync()
        {
            await LoadUserRoles();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string returnUrl = "~/Identity/Account/Login";
            if (ModelState.IsValid)
            {
                var user = await CreateUser(Input);
                var result = await _userManager.CreateAsync(user.Item1, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Item1);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Item1.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    string body = await _fileService.LoadMessageHtml("account_confirmation.html");
                    body = body.Replace("{username}", $"{Input.Firstname} {Input.Lastname}");
                    body = body.Replace("{href}", callbackUrl);

                    await _emailSenderService.SendEmailAsync(Input.Email, "Confirm your email", body);
                    await _userManager.AddToRoleAsync(user.Item1, user.Item2.Name);
                    TempData["LoginModalMsg"] = "Your account has been created. If you want to log in, you must first confirm your email. " +
                        "A message with a confirmation link has been sent to your email adress.";

                    return LocalRedirect(returnUrl);
                }
                await LoadUserRoles();
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private async Task LoadUserRoles()
        {
            ViewData["Roles"] = new SelectList(_dbContext.Roles.Where(r => r.Name != "Admin" && r.Name != "Moderator")
                .OrderBy(r => r.Name)
                .Select(r => new
                {
                    Text = r.Name.Replace('_', ' '),
                    Value = r.Id
                }), "Value", "Text", (await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleValue == RoleValue.Course_client)).Id);
        }

        private async Task<Tuple<AppUser, Role>> CreateUser(RegisterUserVm viewModel)
        {
            Role role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == viewModel.UserType);

            if (role == null)
            {
                throw new InvalidOperationException("Role not exists.");
            }

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
                default: return null;
            }
        }
    }
}
