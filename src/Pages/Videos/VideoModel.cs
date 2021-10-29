using AdvantageTool.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdvantageTool.Pages.Videos
{
    public class VideoModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "VideoId", Description = "VideoIdDescription")]
        public string VideoId { get; set; }

        [Required]
        [Display(Name = "VideoType")]
        public VideoType VideoType { get; set; }
    }
}
