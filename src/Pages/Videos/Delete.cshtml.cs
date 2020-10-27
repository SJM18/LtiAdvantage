using System.Linq;
using System.Threading.Tasks;
using AdvantageTool.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdvantageTool.Pages.Videos
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public VideoModel VideoModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var video = _context.Videos.FirstOrDefault(v => v.Id == id);
            if (video == null)
            {
                return NotFound();
            }

            VideoModel = new VideoModel()
            {
                Id = video.Id,
                VideoId = video.VideoId,
                VideoType = video.VideoType
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = _context.Videos.FirstOrDefault(v => v.Id == id);
            if (video != null)
            {
                _context.Videos.Remove(video);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
