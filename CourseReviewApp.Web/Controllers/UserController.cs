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
                throw new InvalidOperationException($"User with id: {id} not found.");

            return View(Mapper.Map<ChangeUserStatusVm>(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserStatus(ChangeUserStatusVm viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.IsActive = !viewModel.IsActive;
                string word = viewModel.IsActive ? "active" : "blocked";
                await _userService.ChangeUserStatus(viewModel.Id, viewModel.IsActive);
                await _emailSenderService.SendDefaultMessageEmailAsync(viewModel.Email, "User information message",
                    $"The status of your account has been changed to {word} by admin.");
                TempData["UsersManagementMsgModal"] = viewModel.IsActive ? $"The status of the {viewModel.UserName} user has been changed to Active"
                    : $"The status of the {viewModel.UserName} user has been changed to Blocked";

                return RedirectToAction("UserManagement");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(int id)
        {
            AppUser user = await _userService.GetUser(u => u.Id == id);
            if (user == null)
                throw new InvalidOperationException($"User with id: {id} not found.");

            return View(Mapper.Map<UserVm>(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(UserVm viewModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.DeleteUser(viewModel.Id, viewModel.Roles);
                await _emailSenderService.SendDefaultMessageEmailAsync(viewModel.Email, "User information message",
                    $"Your account has been deleted by admin.");
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
                throw new InvalidOperationException($"User with id: {id} not found.");

            return View(Mapper.Map<UserVm>(user));
        }

        [HttpGet]
        public async Task<IActionResult> AssignModeratorRole(int id)
        {
            AppUser user = await _userService.GetUser(u => u.Id == id);
            if (user == null)
                throw new InvalidOperationException($"User with id: {id} not found.");
            if (user.Roles.Contains("Moderator"))
            {
                TempData["UsersManagementMsgModal"] = $"Selected user already has the Moderator role.";
                return RedirectToAction("UserManagement");
            }
            ViewBag.ActionName = "AssignModeratorRole";

            return View(Mapper.Map<AssignModeratorRoleVm>(user));
        }

        [HttpPost]
        public async Task<IActionResult> AssignModeratorRole(AssignModeratorRoleVm viewModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.AssignModeratorRole(viewModel.Id);
                await _emailSenderService.SendDefaultMessageEmailAsync(viewModel.Email, "User information message",
                    $"A moderator role has been assigned to your account.");
                TempData["UsersManagementMsgModal"] = $"The moderator role has been assigned to the user {viewModel.FullName}";

                return RedirectToAction("UserManagement");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UnassignModeratorRole(int id)
        {
            AppUser user = await _userService.GetUser(u => u.Id == id);
            if (user == null)
                throw new InvalidOperationException($"User with id: {id} not found.");
            if (!user.Roles.Contains("Moderator"))
            {
                TempData["UsersManagementMsgModal"] = $"Selected user hasn't got the Moderator role.";
                return RedirectToAction("UserManagement");
            }
            ViewBag.ActionName = "UnassignModeratorRole";

            return View("AssignModeratorRole", Mapper.Map<AssignModeratorRoleVm>(user));
        }

        [HttpPost]
        public async Task<IActionResult> UnassignModeratorRole(AssignModeratorRoleVm viewModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.UnassignModeratorRole(viewModel.Id);
                await _emailSenderService.SendDefaultMessageEmailAsync(viewModel.Email, "User information message",
                    $"Your account has lost its moderator role.");
                TempData["UsersManagementMsgModal"] = $"The moderator role has been unassigned from the user {viewModel.FullName}";

                return RedirectToAction("UserManagement");
            }

            return View(viewModel);
        }
    }
}
