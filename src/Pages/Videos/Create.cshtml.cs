using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AdvantageTool.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdvantageTool.Pages.Videos
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(ApplicationDbContext context,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }


        [BindProperty]
        public VideoModel VideoModel { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (_context.Videos.Any(v => v.VideoId == VideoModel.VideoId))
            {
                ModelState.AddModelError($"{nameof(Video)}.{nameof(Video.VideoId)}",
                    $"This {nameof(VideoModel.VideoId)} is already added.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            //await Platform.DiscoverEndpoints(_httpClientFactory);

            var user = await _context.GetUserAsync(User);
            var video = new Video()
            {
                User = user,
                VideoId = VideoModel.VideoId,
                VideoType = VideoModel.VideoType
            };


            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}