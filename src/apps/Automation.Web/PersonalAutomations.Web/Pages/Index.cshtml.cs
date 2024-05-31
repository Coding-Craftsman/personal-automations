using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalAutomations.Web.Data.Classes;

namespace PersonalAutomations.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PersonalAutomations.Web.Data.ApplicationDbContext _context;

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, PersonalAutomations.Web.Data.ApplicationDbContext contex)
        {
            _logger = logger;
            _context = contex;
        }

        [BindProperty]
        public List<AutomationAction> ActiveAutomations { get; set; } = default!;
        
        public void OnGet()
        {
            ActiveAutomations = _context.AutomationActions.Where(a => a.IsActive).ToList();
        }
    }
}
