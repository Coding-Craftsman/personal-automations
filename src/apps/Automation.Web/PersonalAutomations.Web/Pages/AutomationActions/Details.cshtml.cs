using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonalAutomations.Web.Data;
using PersonalAutomations.Web.Data.Classes;

namespace PersonalAutomations.Web.Pages.AutomationActions
{
    public class DetailsModel : PageModel
    {
        private readonly PersonalAutomations.Web.Data.ApplicationDbContext _context;

        public DetailsModel(PersonalAutomations.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public AutomationAction AutomationAction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var automationaction = await _context.AutomationActions.FirstOrDefaultAsync(m => m.ID == id);
            if (automationaction == null)
            {
                return NotFound();
            }
            else
            {
                AutomationAction = automationaction;
            }
            return Page();
        }
    }
}
