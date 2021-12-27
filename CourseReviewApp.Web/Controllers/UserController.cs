using AutoMapper;
using CourseReviewApp.Model.DataModels;
using CourseReviewApp.Services.Interfaces;
using CourseReviewApp.Web.Services.Interfaces;
using CourseReviewApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IEmailSenderService _emailSenderService;

        public UserController(ILogger logger, IMapper mapper, UserManager<AppUser> userManager,
             IUserService userService, IEmailSenderService emailSenderService)
            : base(logger, mapper, userManager)
        {
            _userService = userService;
            _emailSenderService = emailSenderService;
        }
        
        [HttpGet]
        public async Task<IActionResult> UserManagement()
        {
            IEnumerable<UserVm> userVms = Mapper.Map<IEnumerable<UserVm>>(await _userService.GetUsers());
            return View(userVms.Where(u => !u.Roles.Contains("Admin")));
        }

        [HttpGet]
        public async Task<IActionResult> ChangeUserStatus(int id)
        {
            AppUser user = await _userService.GetUser(u => u.Id == id);
            if (user == null)
                return NotFound();
            ViewBag.Title = user.IsActive ? "Block user account" : "Unblock user account";
            ViewBag.Action = "ChangeUserStatus";

            return View("EditUser", Mapper.Map<EditUserVm>(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserStatus(EditUserVm viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.IsActive = !viewModel.IsActive;
                string word = viewModel.IsActive ? "active" : "blocked";
                await _userService.ChangeUserStatus(viewModel.Id, viewModel.IsActive);
                await _emailSenderService.SendDefaultMessageEmailAsync("User information message",
                    $"The status of your account has been changed to {word} by admin.", viewModel.Email);
                TempData["UsersManagementMsgModal"] = $"The status of the {viewModel.UserName} user has been changed to {word}.";

                return RedirectToAction("UserManagement");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(int id)
        {
            AppUser user = await _userService.GetUser(u => u.Id == id);
            if (user == null)
                return NotFound();

            return View(Mapper.Map<UserVm>(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(UserVm viewModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.DeleteUser(viewModel.Id, viewModel.Roles);
                await _emailSenderService.SendDefaultMessageEmailAsync("User information message",
                    "Your account has been deleted by admin.", viewModel.Email);
                TempData["UsersManagementMsgModal"] = $"The {viewModel.Username} user has been deleted.";

                return RedirectToAction("UserManagement");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(int id)
        {
            AppUser user = await _userService.GetUser(u => u.Id == id);
            if (user == null)
                return NotFound();

            return View(Mapper.Map<UserVm>(user));
        }

        [HttpGet]
        public async Task<IActionResult> AssignModeratorRole(int id)
        {
            AppUser user = await _userService.GetUser(u => u.Id == id);
            if (user == null)
                return NotFound();
            if (user.Roles.Contains("Moderator"))
            {
                TempData["UsersManagementMsgModal"] = $"Selected user already has got the Moderator role.";
                return RedirectToAction("UserManagement");
            }
            ViewBag.Title = "Assign moderator role to user";
            ViewBag.Action = "AssignModeratorRole";

            return View("EditUser", Mapper.Map<EditUserVm>(user));
        }

        [HttpPost]
        public async Task<IActionResult> AssignModeratorRole(EditUserVm viewModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.AssignModeratorRoleToUser(viewModel.Id);
                await _emailSenderService.SendDefaultMessageEmailAsync("User information message",
                    "A moderator role has been assigned to your account.", viewModel.Email);
                TempData["UsersManagementMsgModal"] = $"The moderator role has been assigned to the user {viewModel.UserName}";

                return RedirectToAction("UserManagement");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UnassignModeratorRole(int id)
        {
            AppUser user = await _userService.GetUser(u => u.Id == id);
            if (user == null)
                return NotFound();
            if (!user.Roles.Contains("Moderator"))
            {
                TempData["UsersManagementMsgModal"] = $"Selected user hasn't got the Moderator role.";
                return RedirectToAction("UserManagement");
            }
            ViewBag.Title = "Unassign moderator role from user";
            ViewBag.Action = "UnassignModeratorRole";

            return View("EditUser", Mapper.Map<EditUserVm>(user));
        }

        [HttpPost]
        public async Task<IActionResult> UnassignModeratorRole(EditUserVm viewModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.UnassignModeratorRoleFromUser(viewModel.Id);
                await _emailSenderService.SendDefaultMessageEmailAsync("User information message",
                    "Your account has lost its moderator role.", viewModel.Email);
                TempData["UsersManagementMsgModal"] = $"The moderator role has been unassigned from the user {viewModel.UserName}";

                return RedirectToAction("UserManagement");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DisableUserLockout(int id)
        {
            AppUser user = await _userService.GetUser(u => u.Id == id);
            if (user == null)
                return NotFound();
            if (user.LockoutEnd.Value <= DateTimeOffset.Now)
            {
                TempData["UsersManagementMsgModal"] = $"Selected user account is not locked out.";
                return RedirectToAction("UserManagement");
            }
            ViewBag.Title = "Disable user lockout";
            ViewBag.Action = "DisableUserLockout";

            return View("EditUser", Mapper.Map<EditUserVm>(user));
        }

        [HttpPost]
        public async Task<IActionResult> DisableUserLockout(EditUserVm viewModel) 
        {
            if (ModelState.IsValid)
            {
                await _userService.DisableUserLockout(viewModel.Id);
                await _emailSenderService.SendDefaultMessageEmailAsync("User information message",
                    "Your account lockout has been removed by Admin.", viewModel.Email);
                TempData["UsersManagementMsgModal"] = $"The lockout has been removed from the user {viewModel.UserName}";

                return RedirectToAction("UserManagement");
            }

            return View(viewModel);
        }
    }
}
