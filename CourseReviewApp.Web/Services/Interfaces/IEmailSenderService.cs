using System.Threading.Tasks;

namespace CourseReviewApp.Web.Services.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string recipient, string subject, string body); 
        Task SendDefaultMessageEmailAsync(string recipient, string subject, string message);
    }
}
