using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using AdvantageTool.Data;
using AdvantageTool.Utility;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdvantageTool.Pages.Platforms
{
    /// <summary>
    /// Platform configuration.
    /// </summary>
    public class PlatformModel
    {
        public PlatformModel() { }

        public PlatformModel(HttpRequest request, IUrlHelper url, Platform platform = null)
        {
            if (platform == null)
            {
                PlatformId = CryptoRandom.CreateUniqueId(8);
            }
            else
            {
                Id = platform.Id;
                AccessTokenUrl = platform.AccessTokenUrl;
                AuthorizeUrl = platform.AuthorizeUrl;
                Issuer = platform.Issuer;
                JwkSetUrl = platform.JwkSetUrl;
                Name = platform.Name;
                PlatformId = platform.PlatformId;

                ClientId = platform.ClientId;
                PrivateKey = platform.PrivateKey;
            }

            DeepLinkingLaunchUrl = url.Page("/Tool", null, new { platformId = PlatformId }, request.Scheme);
            LaunchUrl = url.Page("/Tool", null, new { platformId = PlatformId }, request.Scheme);
            LoginUrl = url.Page("/OidcLogin", null, null, request.Scheme);
        }

        /// <summary>
        /// Primary key.
        /// </summary>
        public int Id { get; set; }

        #region Platform properties

        [LocalhostUrl]
        [Required]
        [Display(Name = "AccessTokenUrl", Description = "AccessTokenUrlDescription")]
        public string AccessTokenUrl { get; set; }

        [LocalhostUrl]
        [Required]
        [Display(Name = "AuthorizationUrl", Description = "AuthorizationUrlDescription")]
        public string AuthorizeUrl { get; set; }

        [Required]
        [Display(Name = "Issuer", Description = "IssuerDescription")]
        public string Issuer { get; set; }

        [LocalhostUrl]
        [Required]
        [Display(Name = "JwkSetUrl", Description = "JwkSetUrlDescription")]
        public string JwkSetUrl { get; set; }

        [Required]
        [Display(Name = "DisplayName")]
        public string Name { get; set; }

        /// <summary>
        /// Not displayed.
        /// 
        /// Unique identifier for the platform / authorization server.
        /// Used to create AS-specific redirect URIs as a means to
        /// identify the AS a particular response came from. See BCP
        /// Protecting Redirect-Based Flows.
        /// </summary>
        public string PlatformId { get; set; }

        #endregion

        #region Tool properties

        /// <summary>
        /// Tool's OpenID Client ID
        /// </summary>
        [Required]
        [Display(Name = "ClientId")]
        public string ClientId { get; set; }
                
        /// <summary>
        /// Deep linking launch url.
        /// </summary>
        [LocalhostUrl]
        [Display(Name = "DeepLinkingLaunchUrl", Description = "DeepLinkingLaunchUrlDescription")]
        public string DeepLinkingLaunchUrl { get; set; }

        /// <summary>
        /// Tool launch url.
        /// </summary>
        [Display(Name = "LaunchUrl", Description = "LaunchUrlDescription")]
        public string LaunchUrl { get; set; }

        /// <summary>
        /// OIDC login initiation url.
        /// </summary>
        [Display(Name = "LoginUrl", Description = "LoginUrlDescription")]
        public string LoginUrl { get; set; }

        /// <summary>
        /// Tool's private key in PEM format
        /// </summary>
        [Required]
        [Display(Name = "PrivateKey", Description = "PrivateKeyDescription")]
        public string PrivateKey { get; set; }

        #endregion

        public async Task DiscoverEndpoints(IHttpClientFactory factory)
        {
            var httpClient = factory.CreateClient();
            var disco = await httpClient.GetDiscoveryDocumentAsync(Issuer);
            if (!disco.IsError)
            {
                AccessTokenUrl = disco.TokenEndpoint;
                AuthorizeUrl = disco.AuthorizeEndpoint;
                JwkSetUrl = disco.JwksUri;
            }
            else if (AccessTokenUrl.IsPresent())
            {
                disco = await httpClient.GetDiscoveryDocumentAsync(AccessTokenUrl);
                if (!disco.IsError)
                {
                    AccessTokenUrl = disco.TokenEndpoint;
                    AuthorizeUrl = disco.AuthorizeEndpoint;
                    JwkSetUrl = disco.JwksUri;
                }
                else if (AuthorizeUrl.IsPresent())
                {
                    disco = await httpClient.GetDiscoveryDocumentAsync(AuthorizeUrl);
                    if (!disco.IsError)
                    {
                        AccessTokenUrl = disco.TokenEndpoint;
                        AuthorizeUrl = disco.AuthorizeEndpoint;
                        JwkSetUrl = disco.JwksUri;
                    }
                }
                else if (JwkSetUrl.IsPresent())
                {
                    disco = await httpClient.GetDiscoveryDocumentAsync(JwkSetUrl);
                    if (!disco.IsError)
                    {
                        AccessTokenUrl = disco.TokenEndpoint;
                        AuthorizeUrl = disco.AuthorizeEndpoint;
                        JwkSetUrl = disco.JwksUri;
                    }
                }
            }
        }

        public void UpdateEntity(Platform platform)
        {
            platform.AccessTokenUrl = AccessTokenUrl;
            platform.AuthorizeUrl = AuthorizeUrl;
            platform.Name = Name;
            platform.Issuer = Issuer;
            platform.JwkSetUrl = JwkSetUrl;
            platform.PlatformId = PlatformId;

            platform.ClientId = ClientId;
            platform.PrivateKey = PrivateKey;
        }
    }
}
