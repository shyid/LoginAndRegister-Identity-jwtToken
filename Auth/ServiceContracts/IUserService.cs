using Auth.Identity;
using Microsoft.AspNetCore.Identity;

namespace Auth.ServiceContracts
{
    public interface IUserService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }
}
