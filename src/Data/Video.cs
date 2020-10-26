using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvantageTool.Data
{
    public class Video
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        public string VideoId { get; set; }

        public VideoType VideoType { get; set; }

        /// <summary>
        /// The user that created this platform registration.
        /// </summary>
        public AdvantageToolUser User { get; set; }
    }

    public enum VideoType
    {
        Unknown = 0,
        Youtube = 1,
        Vimeo = 2
    }
}
