using System.Threading.Tasks;

namespace SampleAngular.Api.Identity
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
