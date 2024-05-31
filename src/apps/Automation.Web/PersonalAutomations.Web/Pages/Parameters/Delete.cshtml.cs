using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonalAutomations.Web.Data;
using PersonalAutomations.Web.Data.Classes;

namespace PersonalAutomations.Web.Pages.Parameters
{
    public class DeleteModel : PageModel
    {
        private readonly PersonalAutomations.Web.Data.ApplicationDbContext _context;

        public DeleteModel(PersonalAutomations.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ActionParameter ActionParameter { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actionparameter = await _context.ActionParameters.FirstOrDefaultAsync(m => m.ID == id);

            if (actionparameter == null)
            {
                return NotFound();
            }
            else
            {
                ActionParameter = actionparameter;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actionparameter = await _context.ActionParameters.FindAsync(id);
            if (actionparameter != null)
            {
                ActionParameter = actionparameter;
                _context.ActionParameters.Remove(ActionParameter);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
