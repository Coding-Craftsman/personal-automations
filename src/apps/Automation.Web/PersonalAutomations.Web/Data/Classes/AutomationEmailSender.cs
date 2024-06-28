using Microsoft.AspNetCore.Identity.UI.Services;

namespace PersonalAutomations.Web.Data.Classes
{
    public class AutomationEmailSender : IEmailSender
    {
        private ApplicationDbContext _context;

        public AutomationEmailSender(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            AutomationEmail mail = new AutomationEmail()
            {
                Email = email,
                MessageSubject = subject,
                MessageBody = htmlMessage
            };

            _context.RegistrationEmails.Add(mail);

            await _context.SaveChangesAsync();
        }
    }
}
