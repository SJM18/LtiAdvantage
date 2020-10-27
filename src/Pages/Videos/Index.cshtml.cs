using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvantageTool.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdvantageTool.Pages.Videos
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<VideoModel> VideoModels { get; set; }

        public async Task OnGetAsync()
        {
            VideoModels = (await _context.GetAllVideo()).Select(s => new VideoModel() { Id = s.Id, VideoId = s.VideoId, VideoType = s.VideoType }).ToList();
            //var user = await _context.GetUserAsync(User);
            //Platforms = user.Platforms
            //    .OrderBy(p => p.Name)
            //    .Select(p => new PlatformModel(Request, Url, p))
            //    .ToList();
        }
    }
}
