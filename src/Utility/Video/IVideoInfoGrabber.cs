using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvantageTool.Utility.Video
{
    public interface IVideoInfoGrabber
    {
        public string VideoId { get; set; }
        VideoInfoBaseModel GetVideoInfo();
    }
}
