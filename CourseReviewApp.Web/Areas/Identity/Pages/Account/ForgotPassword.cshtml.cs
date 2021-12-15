using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Web.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IFileService _fileService;

        public ForgotPasswordModel(UserManager<AppUser> userManager, IEmailSenderService emailSenderService, IFileService fileTool)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    ModelState.AddModelError(string.Empty, "User does not exist or his email is not confirmed.");
                    return Page();
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                string body = await _fileService.LoadMessageHtml("reset_password.html");
                body = body.Replace("{href}", callbackUrl);

                await _emailSenderService.SendEmailAsync(Input.Email, "Reset Password", body);
                TempData["LoginModalMsg"] = "A message with a password reset link has been sent to your email adress.";

                return LocalRedirect("~/Identity/Account/Login");
            }

            return Page();
        }
    }
}
