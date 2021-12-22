using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IFileService _fileService;

        public ResendEmailConfirmationModel(UserManager<AppUser> userManager, IEmailSenderService emailSenderService, IFileService fileTool)
        {
            _userManager = userManager;
            _emailSenderService = emailSenderService;
            _fileService = fileTool;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);

            string body = await _fileService.LoadMessageHtml("account_confirmation.html");
            body = body.Replace("{username}", Input.Email);
            body = body.Replace("{href}", callbackUrl);

            await _emailSenderService.SendEmailAsync("Confirm your email", body, Input.Email);
            TempData["LoginModalMsg"] = "A message with a confirmation link has been sent to your email adress.";

            return LocalRedirect("~/Identity/Account/Login");
        }
    }
}
