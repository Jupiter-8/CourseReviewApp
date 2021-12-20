using CourseReviewApp.Web.Services.Interfaces;
using CourseReviewApp.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Services.Classes
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _hostingEnv;

        public FileService(IWebHostEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        public void DeleteFile(string filePath)
        {
            string destPath = Path.Combine(_hostingEnv.WebRootPath, filePath);
            File.Delete(destPath);
        }

        public bool FileExists(string filePath)
        {
            string destPath = Path.Combine(_hostingEnv.WebRootPath, filePath);
            return File.Exists(destPath);
        }

        public async Task<string> SaveCourseImage(AddOrEditCourseVm viewModel, string destFolder)
        {
            destFolder = Path.Combine(_hostingEnv.WebRootPath, destFolder);
            if (viewModel.Id.HasValue && viewModel.Image != null
            && viewModel.ImagePath != "default_course_image.jpg")
            {
                File.Delete(Path.Combine(destFolder, viewModel.ImagePath));
            }

            string fileName = Guid.NewGuid().ToString() + "_" + viewModel.Image.FileName;
            string filePath = Path.Combine(destFolder, fileName);
            using (FileStream fileStream = new(filePath, FileMode.Create))
            {
                await viewModel.Image.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public async Task<string> SaveUserAvatar(AddOrEditUserAvatarVm viewModel, string destFolder)
        {
            if (viewModel.Image != null && viewModel.AvatarPath != "default_user_avatar.jpg"
                && !viewModel.IsNew)
            {
                File.Delete(Path.Combine(destFolder, viewModel.AvatarPath));
            }

            string fileName = Guid.NewGuid().ToString() + "_" + viewModel.Image.FileName;
            string filePath = Path.Combine(destFolder, fileName);
            using (FileStream fileStream = new(filePath, FileMode.Create))
            {
                await viewModel.Image.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public async Task SaveTermsAndConditionsFile(AddOrEditTermsAndConditionsFileVm viewModel, string destFolder, string fileName)
        {
            destFolder = Path.Combine(_hostingEnv.WebRootPath, destFolder);
            string filePath = Path.Combine(destFolder, fileName);
            if (viewModel.File != null && !viewModel.IsNew)
                File.Delete(filePath);

            using (FileStream fileStream = new(filePath, FileMode.Create))
            {
                await viewModel.File.CopyToAsync(fileStream);
            }
        }

        public async Task<string> LoadMessageHtml(string messageName)
        {
            string srcFolder = Path.Combine(_hostingEnv.WebRootPath, $"Sites\\{messageName}");
            string body = string.Empty;
            using (StreamReader reader = new(srcFolder))
            {
                body = await reader.ReadToEndAsync();
            }

            return body;
        }
    }
}