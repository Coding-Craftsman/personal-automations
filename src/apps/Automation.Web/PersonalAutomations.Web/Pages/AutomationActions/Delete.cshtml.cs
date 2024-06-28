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
    public class DeleteModel : PageModel
    {
        private readonly PersonalAutomations.Web.Data.ApplicationDbContext _context;

        public DeleteModel(PersonalAutomations.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var automationaction = await _context.AutomationActions.FindAsync(id);
            if (automationaction != null)
            {
                AutomationAction = automationaction;
                _context.AutomationActions.Remove(AutomationAction);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
