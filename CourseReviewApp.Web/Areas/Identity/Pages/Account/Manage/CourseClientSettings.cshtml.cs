using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourseReviewApp.Model.DataModels;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Areas.Identity.Pages.Account.Manage
{
    public class CourseClientSettingsModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public CourseClientSettingsModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
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
            public bool ReviewInfoEmailsEnabled { get; set; }
        }

        private async Task LoadAsync(AppUser user)
        {
            CourseClient client = (CourseClient)user;
            Input = new InputModel
            {
                ReviewInfoEmailsEnabled = client.ReviewInfoEmailsEnabled
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

            CourseClient client = (CourseClient)user;
            client.ReviewInfoEmailsEnabled = Input.ReviewInfoEmailsEnabled;

            var updateResult = await _userManager.UpdateAsync(client);
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
