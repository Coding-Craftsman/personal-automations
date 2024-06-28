using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalAutomations.Web.Data;
using PersonalAutomations.Web.Data.Classes;

namespace PersonalAutomations.Web.Pages.Parameters
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly PersonalAutomations.Web.Data.ApplicationDbContext _context;

        public EditModel(PersonalAutomations.Web.Data.ApplicationDbContext context)
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

            var actionparameter =  await _context.ActionParameters.FirstOrDefaultAsync(m => m.ID == id);
            if (actionparameter == null)
            {
                return NotFound();
            }
            ActionParameter = actionparameter;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ActionParameter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActionParameterExists(ActionParameter.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ActionParameterExists(int id)
        {
            return _context.ActionParameters.Any(e => e.ID == id);
        }
    }
}
