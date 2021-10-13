using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdvantageTool.Utility.Video
{
    public class VimeoInfoGrabber : IVideoInfoGrabber
    {
        private static readonly string VimeoURL = "https://vimeo.com/api/v2/video/{0}.json";

        public string VideoId { get; set; }

        public VimeoInfoGrabber(string videoId)
        {
            VideoId = videoId;
        }

        public VideoInfoBaseModel GetVideoInfo()
        {
            if (string.IsNullOrWhiteSpace(VideoId))
            {
                throw new Exception("VideoId cannot be null or empty");
            }

            var result = new HttpClient().GetAsync(string.Format(VimeoURL, VideoId)).Result;

            var jsonResult = result.Content.ReadAsStringAsync().Result;

            var modelResult = JsonConvert.DeserializeObject<VimeoJsonResult>(jsonResult.TrimStart('[').TrimEnd(']'));

            return new VideoInfoBaseModel() { CoverUrl = modelResult.Thumbnail_large, Name = modelResult.Title, VideoId = modelResult.Id.ToString() };
        }

        private class VimeoJsonResult
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
            public string Upload_date { get; set; }
            public string Thumbnail_small { get; set; }
            public string Thumbnail_medium { get; set; }
            public string Thumbnail_large { get; set; }
            public int User_id { get; set; }
            public string User_name { get; set; }
            public string User_url { get; set; }
            public string User_portrait_small { get; set; }
            public string User_portrait_medium { get; set; }
            public string User_portrait_large { get; set; }
            public string User_portrait_huge { get; set; }
            public int Stats_number_of_likes { get; set; }
            public int Stats_number_of_plays { get; set; }
            public int Stats_number_of_comments { get; set; }
            public int Duration { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public string Tags { get; set; }
            public string Embed_privacy { get; set; }
        }
    }
}
