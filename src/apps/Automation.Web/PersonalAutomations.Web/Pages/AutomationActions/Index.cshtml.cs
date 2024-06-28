using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonalAutomations.Web.Data;
using PersonalAutomations.Web.Data.Classes;

namespace PersonalAutomations.Web.Pages.AutomationActions
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly PersonalAutomations.Web.Data.ApplicationDbContext _context;

        public IndexModel(PersonalAutomations.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AutomationAction> AutomationAction { get;set; } = default!;

        public async Task OnGetAsync()
        {
            AutomationAction = await _context.AutomationActions.ToListAsync();
        }
    }
}
