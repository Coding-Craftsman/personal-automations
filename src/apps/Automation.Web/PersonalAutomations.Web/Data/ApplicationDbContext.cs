using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PersonalAutomations.Web.Data.Classes;

namespace PersonalAutomations.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<AutomationAction> AutomationActions { get; set; }

        public DbSet<ActionParameter> ActionParameters { get; set; }

        public DbSet<MessageHistory> MessageHistory { get; set; }

        public DbSet<AutomationEmail> RegistrationEmails { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
