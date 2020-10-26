using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AdvantageTool.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdvantageTool.Pages.Videos
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(ApplicationDbContext context,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public VideoModel VideoModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _context.Videos.FirstOrDefault(v => v.Id == id.Value);

            VideoModel = new VideoModel()
            {
                Id = result.Id,
                VideoId = result.VideoId,
                VideoType = result.VideoType
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var video = _context.Videos.FirstOrDefault(v => v.Id == VideoModel.Id);

            video.VideoId = VideoModel.VideoId;

            _context.Videos.Update(video);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToPage("./Index");
        }

        private bool PlatformExists(int id)
        {
            return _context.Platforms.Any(e => e.Id == id);
        }
    }
}
