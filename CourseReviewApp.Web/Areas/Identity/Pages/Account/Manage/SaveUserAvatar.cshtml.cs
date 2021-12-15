using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Web.ViewModels;
using CourseReviewApp.Web.Services.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class SaveUserAvatarModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _hostingEnv;

        public SaveUserAvatarModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IFileService imageTool, IWebHostEnvironment hostingEnv)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileService = imageTool;
            _hostingEnv = hostingEnv;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public IFormFile Image { get; set; }
            public string AvatarPath { get; set; }
            public bool ToDelete { get; set; }
        }

        private async Task LoadAsync(AppUser user)
        {
            Input = new InputModel
            {
                AvatarPath = user.AvatarPath
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
            AddOrEditUserAvatarVm viewModel;
            string destFolder = Path.Combine(_hostingEnv.WebRootPath, "Images\\Avatars");

            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.Image == null && !string.IsNullOrEmpty(user.AvatarPath) && Input.ToDelete
                && user.AvatarPath != "default_user_avatar.jpg")
            {
                _fileService.DeleteFile(Path.Combine(destFolder, user.AvatarPath));
                user.AvatarPath = "default_user_avatar.jpg";
            }
            else if (Input.Image != null)
            {
                viewModel = new AddOrEditUserAvatarVm
                {
                    AvatarPath = Input.AvatarPath,
                    Image = Input.Image
                };
                if (!string.IsNullOrEmpty(user.AvatarPath))
                    viewModel.IsNew = false;
                string avatarPath = await _fileService.SaveUserAvatar(viewModel, destFolder);
                user.AvatarPath = avatarPath;
            }
            else
                return RedirectToPage();

            var updateResult = await _userManager.UpdateAsync(user);
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

