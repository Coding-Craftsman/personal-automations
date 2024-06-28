using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonalAutomations.Web.Data;
using PersonalAutomations.Web.Data.Classes;

namespace PersonalAutomations.Web.Pages.Parameters
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly PersonalAutomations.Web.Data.ApplicationDbContext _context;

        public CreateModel(PersonalAutomations.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            //setupActionList();

            return Page();
        }

        [BindProperty]
        public ActionParameter ActionParameter { get; set; } = default!;

        //[BindProperty]
        //public List<SelectListItem> ActionsList { get; set; } = default!;

        //[BindProperty]
        //public string SelectedAction { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                //setupActionList();
                return Page();
            }

            //ActionParameter.AutomationActionID = Convert.ToInt32(SelectedAction);
            _context.ActionParameters.Add(ActionParameter);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        //private void setupActionList()
        //{
        //    ActionsList = _context.AutomationActions.Where(a => a.IsActive).Select(a =>
        //                                                    new SelectListItem
        //                                                    {
        //                                                        Value = a.ID.ToString(),
        //                                                        Text = a.Name
        //                                                    }).ToList();
        //}
    }
}
