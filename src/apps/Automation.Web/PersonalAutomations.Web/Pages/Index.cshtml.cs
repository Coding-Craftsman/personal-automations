using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalAutomations.Web.Data.Classes;
using PersonalAutomations.Web.Interfaces;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PersonalAutomations.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PersonalAutomations.Web.Data.ApplicationDbContext _context;

        private readonly ILogger<IndexModel> _logger;

        private readonly IMessageProcessor _messageProcessor;

        public IndexModel(ILogger<IndexModel> logger, PersonalAutomations.Web.Data.ApplicationDbContext contex, IMessageProcessor MessageProcessor)
        {
            _logger = logger;
            _context = contex;
            _messageProcessor = MessageProcessor;
        }

        [BindProperty]
        public List<AutomationAction> ActiveAutomations { get; set; } = default!;

        public void OnGet()
        {
            ActiveAutomations = _context.AutomationActions.Include(p => p.Parameters).Where(a => a.IsActive).ToList();
        }

        public IActionResult OnPostRunAutomation(string ActionKeyword, List<ActionParameter> parameters)
        {
            var action = _context.AutomationActions.Include(p => p.Parameters).Where(a => a.ActionKeyword == ActionKeyword).FirstOrDefault();
            if (action != null)
            {
                // Add the values to the parameters before we send it to the message queue
                foreach(var item in parameters)
                {
                    action.Parameters.Where(p => p.Name == item.Name).First().Value = item.Value;
                }                

                // Serialize the object into Json to send to the message queue
                var messageBody = JsonConvert.SerializeObject(action);

                // pusht the message to the queue
                _messageProcessor.PublishMessage(messageBody);

                var AlertMessage = "Message Sent!";
                
                // set the message to the session so the web page can display it to the user
                HttpContext.Session.SetString("AlertMessage", AlertMessage);

                // redirect back to the default page so it loads all the automations back up correctly
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
