using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;

namespace CourseReviewApp.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserService _userService;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Username (used to login)")]
            [RegularExpression("[A-Za-z][A-Za-z0-9_]{5,19}",
            ErrorMessage = "The username must be at least 6 and at max 20 characters, it must starts with a letter. Digits and the _ symbol are allowed.")]
            public string Username { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            [StringLength(16, ErrorMessage = "Phone number must be between {2} and {1} characters long", MinimumLength = 1)]
            public string PhoneNumber { get; set; }

            [Display(Name = "Firstname")]
            [StringLength(35, ErrorMessage = "Firstname must be between {2} and {1} characters long", MinimumLength = 1)]
            public string FirstName { get; set; }

            [Display(Name = "Lastname")]
            [StringLength(35, ErrorMessage = "Lastname must be between {2} and {1} characters long", MinimumLength = 1)]
            public string LastName { get; set; }
        }
        
        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Input = new InputModel
            {
                Username = userName,
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName
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

            user.UserName = Input.Username;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.PhoneNumber = Input.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(user);
            if(!updateResult.Succeeded)
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
