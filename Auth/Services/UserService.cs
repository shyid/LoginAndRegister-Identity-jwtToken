using Auth.Identity;
using Auth.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Mail;
using System.Text;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Auth.Services
{
    public class UserService : IUserService
    {
        private IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public UserService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        

        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("shadyxdey.com", "JWT Auth Demo");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);

        }
    }

 
    
}
