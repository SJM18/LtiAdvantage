using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvantageTool.Utility.Video
{
    public class YoutubeInfoGrabber : IVideoInfoGrabber
    {
        private static readonly string YoutubeURL = "https://img.youtube.com/vi/{0}/hqdefault.jpg";

        public string VideoId { get; set; }
        public Data.Video Video { get; }

        public YoutubeInfoGrabber(Data.Video video)
        {
            Video = video;
        }

        public VideoInfoBaseModel GetVideoInfo()
        {
            return new VideoInfoBaseModel()
            {
                CoverUrl = string.Format(YoutubeURL, this.Video.VideoId),
                Name = this.Video.VideoId,
                VideoId = this.Video.VideoId
            };
        }
    }
}
