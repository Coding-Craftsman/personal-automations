using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalAutomations.Web.Data.Classes;
using PersonalAutomations.Web.Interfaces;
using Newtonsoft.Json;

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
            ActiveAutomations = _context.AutomationActions.Where(a => a.IsActive).ToList();
        }

        public IActionResult OnPostRunAutomation(string ActionKeyword)
        {
            var action = _context.AutomationActions.Where(a => a.ActionKeyword == ActionKeyword).FirstOrDefault();
            if (action != null)
            {
                var parameters = _context.ActionParameters.Where(p => p.ActionID == action.ID).ToList();
            

                AutomationMessagePayload payload = new AutomationMessagePayload()
                {
                    Action = action,
                    Parameters = parameters
                };

                var messageBody = JsonConvert.SerializeObject(payload);

                _messageProcessor.PublishMessage(messageBody);

                var AlertMessage = "Message Sent!";

                HttpContext.Session.SetString("AlertMessage", AlertMessage);

                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
