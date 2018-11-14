﻿using System.Threading.Tasks;
using AdvantageTool.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdvantageTool.Pages.Platforms
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DetailsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public PlatformModel Platform { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platform = await _context.Platforms.FindAsync(id);
            if (platform == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (platform.UserId != user.Id)
            {
                return NotFound();
            }

            Platform = new PlatformModel(platform);

            return Page();
        }
    }
}
