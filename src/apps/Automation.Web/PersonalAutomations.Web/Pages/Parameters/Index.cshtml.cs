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

namespace PersonalAutomations.Web.Pages.Parameters
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly PersonalAutomations.Web.Data.ApplicationDbContext _context;

        public IndexModel(PersonalAutomations.Web.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ActionParameter> ActionParameter { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ActionParameter = await _context.ActionParameters.ToListAsync();
        }
    }
}
