using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Services.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string subject, string body, string recipient = null, IList<string> bccs = null); 
        Task SendDefaultMessageEmailAsync(string subject, string message, string recipient = null, IList<string> bccs = null);
    }
}
