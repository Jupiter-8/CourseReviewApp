using CourseReviewApp.Web.ViewModels;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Services.Interfaces
{
    public interface IFileService
    {
        public Task<string> SaveCourseImage(AddOrEditCourseVm viewModel, string destFolder);
        public Task<string> SaveUserAvatar(AddOrEditUserAvatarVm viewModel, string destFolder);
        public Task SavePrivacyFile(AddOrEditPrivacyFileVm viewModel, string destFolder, string fileName);
        public Task<string> LoadMessageHtml(string messageName);
        public void DeleteFile(string filePath);
        public bool FileExists(string filePath);
    }
}
