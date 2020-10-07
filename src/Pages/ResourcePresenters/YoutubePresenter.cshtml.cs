using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using LtiAdvantage.Lti;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace AdvantageTool.Pages.ResourcePresenters
{
    [IgnoreAntiforgeryToken]
    public class YoutubePresenterModel : PageModel
    {
        /// <summary>
        /// The error discovered while parsing the request.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// A copy of the id_token for diagnostic purposes.
        /// </summary>
        public string IdToken { get; set; }

        ///// <summary>
        ///// The parsed JWT header from id_token. Null if invalid token.
        ///// </summary>
        //public JwtHeader JwtHeader { get; set; }

        /// <summary>
        /// Wrapper around the request payload.
        /// </summary>
        public LtiResourceLinkRequest LtiRequest { get; set; }

        public string YoutubeVideoId { get; set; }

        public void OnGet(string LtiRequest)
        {
            this.LtiRequest = JsonConvert.DeserializeObject<LtiResourceLinkRequest>(LtiRequest);
            this.YoutubeVideoId = this.LtiRequest.ResourceLink.Id.Remove(0, 2);
        }

        public async Task<IActionResult> OnPostAsync(string LtiRequest)
        {
            //this.Error = LtiRequest;

            this.LtiRequest = JsonConvert.DeserializeObject<LtiResourceLinkRequest>(LtiRequest);
            this.YoutubeVideoId = this.LtiRequest.ResourceLink.Id.Remove(0, 3);

            if (!ModelState.IsValid)
            {
                
            }

            return Page();
        }
    }
}
