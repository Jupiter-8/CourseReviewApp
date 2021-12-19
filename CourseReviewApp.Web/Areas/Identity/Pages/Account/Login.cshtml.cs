using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using CourseReviewApp.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IUserService _userService;
        private readonly IEmailSenderService _emailSenderService;

        public LoginModel(SignInManager<AppUser> signInManager,
            ILogger<LoginModel> logger, IUserService userService, IEmailSenderService emailSenderService, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userService = userService;
            _emailSenderService = emailSenderService;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                AppUser user = await _userService.GetUser(u => u.UserName == Input.Username);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The username or password is incorrect.");
                    return Page();
                }
                else if (!user.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "User is blocked.");
                    return Page();
                }

                var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    if(user.LockoutMessageSent && user.LockoutEnd.Value <= DateTime.UtcNow)
                    {
                        user.LockoutMessageSent = false;
                        await _userManager.UpdateAsync(user);
                    }
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    if(!user.LockoutMessageSent)
                    {
                        await _emailSenderService.SendDefaultMessageEmailAsync("Account lockout",
                            "Your account has been locked out due to too many failed login attempts.", user.Email);
                        user.LockoutMessageSent = true;
                        await _userManager.UpdateAsync(user);
                    }

                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    if (!user.EmailConfirmed)
                    {
                        ModelState.AddModelError(string.Empty, "User's account email is not confirmed.");
                        return Page();
                    }
                    ModelState.AddModelError(string.Empty, "The username or password is incorrect.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
