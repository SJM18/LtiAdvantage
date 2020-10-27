using AdvantageTool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvantageTool.Pages.Videos
{
    public class VideoModel
    {
        public int Id { get; set; }

        public string VideoId { get; set; }

        public VideoType VideoType { get; set; }
    }
}
