using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using CourseReviewApp.Web.ViewModels;
using CourseReviewApp.Web.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Services.Classes
{
    public class FileService : BaseService, IFileService
    {
        private readonly IWebHostEnvironment _hostingEnv;

        public FileService(IWebHostEnvironment hostingEnv, ILogger logger) : base(logger)
        {
            _hostingEnv = hostingEnv;
        }

        public void DeleteFile(string filePath)
        {
            try
            {
                string destPath = Path.Combine(_hostingEnv.WebRootPath, filePath);
                File.Delete(destPath);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }

        public bool FileExists(string filePath)
        {
            try
            {
                string destPath = Path.Combine(_hostingEnv.WebRootPath, filePath);
                return File.Exists(destPath);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
            return true;
        }

        public async Task<string> SaveCourseImage(AddOrEditCourseVm viewModel, string destFolder)
        {
            try
            {
                destFolder = Path.Combine(_hostingEnv.WebRootPath, destFolder);
                if (viewModel.Id.HasValue && viewModel.Image != null
                && viewModel.ImagePath != "default_course_image.jpg")
                {
                    File.Delete(Path.Combine(destFolder, viewModel.ImagePath));
                }

                string fileName = Guid.NewGuid().ToString() + "_" + viewModel.Image.FileName;
                string filePath = Path.Combine(destFolder, fileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.Image.CopyToAsync(fileStream);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
            return null;
        }

        public async Task<string> SaveUserAvatar(AddOrEditUserAvatarVm viewModel, string destFolder)
        {
            try
            {
                if (viewModel.Image != null && viewModel.AvatarPath != "default_user_avatar.jpg"
                && !viewModel.IsNew)
                {
                    File.Delete(Path.Combine(destFolder, viewModel.AvatarPath));
                }

                string fileName = Guid.NewGuid().ToString() + "_" + viewModel.Image.FileName;
                string filePath = Path.Combine(destFolder, fileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.Image.CopyToAsync(fileStream);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
            return null;
        }

        public async Task SavePrivacyFile(AddOrEditPrivacyFileVm viewModel, string destFolder, string fileName)
        {
            try
            {
                destFolder = Path.Combine(_hostingEnv.WebRootPath, destFolder);
                string filePath = Path.Combine(destFolder, fileName);
                if (viewModel.File != null && !viewModel.IsNew)
                    File.Delete(filePath);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.File.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }

        public async Task<string> LoadMessageHtml(string messageName)
        {
            try
            {
                string srcFolder = Path.Combine(_hostingEnv.WebRootPath, $"Sites\\{messageName}");
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(srcFolder))
                {
                    body = await reader.ReadToEndAsync();
                }

                return body;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
            return null;
        }
    }
}