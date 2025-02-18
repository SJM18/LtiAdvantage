﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AdvantageTool.Data;
using IdentityModel;
using IdentityModel.Client;
using LtiAdvantage.IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;

namespace AdvantageTool.Utility
{
    /// <summary>
    /// Service available via dependency injection to get an access token from the issuer.
    /// </summary>
    public class AccessTokenService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Create an instance of the AccessTokenService.
        /// </summary>
        /// <param name="context">The application database context to look up the issuer's token endpoint.</param>
        /// <param name="httpClientFactory">The HttpClient factory.</param>
        public AccessTokenService(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Get an access token from the issuer.
        /// </summary>
        /// <param name="issuer">The issuer.</param>
        /// <param name="scope">The scope to request.</param>
        /// <returns>The token response.</returns>
        public async Task<TokenResponse> GetAccessTokenAsync(string signingIssuer, string issuer, string scope)
        {
            if (issuer.IsMissing())
            {
                return TokenResponse.FromException<TokenResponse>(new ArgumentNullException(nameof(issuer)));


            }

            if (scope.IsMissing())
            {
                return TokenResponse.FromException<TokenResponse>(new ArgumentNullException(nameof(scope)));
            }

            var platform = await _context.GetPlatformByIssuerAsync(issuer);
            if (platform == null)
            {
                return TokenResponse.FromException<TokenResponse>(new Exception("Cannot find platform registration."));
            }

            // Use a signed JWT as client credentials.
            var payload = new JwtPayload();
            payload.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, signingIssuer));
            payload.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, platform.ClientId));
            payload.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, platform.AccessTokenUrl));
            payload.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString()));
            payload.AddClaim(new Claim(JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(DateTime.UtcNow.AddSeconds(-5)).ToString()));
            payload.AddClaim(new Claim(JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(DateTime.UtcNow.AddMinutes(5)).ToString()));
            payload.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, CryptoRandom.CreateUniqueId(32)));

            var handler = new JwtSecurityTokenHandler();
            var credentials = PemHelper.SigningCredentialsFromPemString(platform.PrivateKey);
            var jwt = handler.WriteToken(new JwtSecurityToken(new JwtHeader(credentials), payload));


            var jwtClientCredentials = new JwtClientCredentialsTokenRequest
            {
                Address = platform.AccessTokenUrl,
                ClientId = platform.ClientId,
                Jwt = jwt,
                Scope = scope,
                ClientAssertion = new ClientAssertion() { Type = OidcConstants.ClientAssertionTypes.JwtBearer, Value = jwt },
                Parameters = !string.IsNullOrWhiteSpace(scope) ? new Dictionary<string, string>() { { OidcConstants.TokenRequest.Scope, scope } } : null,
                GrantType = OidcConstants.GrantTypes.ClientCredentials
            };

            var httpClient = _httpClientFactory.CreateClient();
            return await httpClient.RequestTokenAsync(jwtClientCredentials, default); //RequestClientCredentialsTokenWithJwtAsync(jwtClientCredentials);
        }
    }
}
