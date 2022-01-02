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
                TempData["LoginModalMsg"] = "If the provided email belongs to a registered user," +
                    " we will send to it a message with a password reset link.";
                if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                    return LocalRedirect("~/Identity/Account/Login");

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                string body = await _fileService.LoadMessageHtml("reset_password.html");
                body = body.Replace("{href}", callbackUrl);
                body = body.Replace("{username}", $"{user.FirstName} {user.LastName}");
                await _emailSenderService.SendEmailAsync("Password reset", body, Input.Email);

                return LocalRedirect("~/Identity/Account/Login");
            }

            return Page();
        }
    }
}
