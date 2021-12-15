using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourseReviewApp.Model.DataModels;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class CourseOwnerSettingsModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public CourseOwnerSettingsModel(
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Brand name")]
            [StringLength(50, ErrorMessage = "Brand name must be between {2} and {1} characters long", MinimumLength = 1)]
            public string BrandName { get; set; }

            [Display(Name = "Website URL")]
            [StringLength(100, ErrorMessage = "Website URL must be between {2} and {1} characters long", MinimumLength = 1)]
            public string WebsiteUrl { get; set; }

            public bool CourseInfoEmailsEnabled { get; set; }
        }

        private async Task LoadAsync(AppUser user)
        {
            CourseOwner owner = (CourseOwner)user;
            Input = new InputModel
            {
                BrandName = owner.BrandName,
                WebsiteUrl = owner.WebsiteUrl,
                CourseInfoEmailsEnabled = owner.CourseInfoEmailsEnabled
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            CourseOwner owner = (CourseOwner)user;
            owner.BrandName = Input.BrandName;
            owner.WebsiteUrl = Input.WebsiteUrl;
            owner.CourseInfoEmailsEnabled = Input.CourseInfoEmailsEnabled;

            var updateResult = await _userManager.UpdateAsync(owner);
            if (!updateResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to update user.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
