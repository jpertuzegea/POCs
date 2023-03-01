//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Marzo 2023</date>
//-----------------------------------------------------------------------

using Microsoft.IdentityModel.Tokens;
using POC_JWT_NetCore6.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace POC_JWT_NetCore6.Services
{
    public class TokenServices
    {

        private readonly IConfiguration configuration;

        public TokenServices(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public string GenerateToken()
        {
            var Claims = new[] {
                new Claim(JwtRegisteredClaimNames.UniqueName, "JorgePertuz"),

                new Claim("UserId", "1"),
                new Claim("UserEmail", "Jpertuzegea@hotmail.com"),
                new Claim("UserFullName", "Jorge David Pertuz Egea"),
                new Claim("UserNetwork", "Jpertuzegea"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Genera un GUID por cada token
            };

            JWTAuthentication JWTAuthenticationSection = configuration.GetSection("JWTAuthentication").Get<JWTAuthentication>();

            SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTAuthenticationSection.Secret));
            var Credenciales = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            DateTime Expiration = DateTime.Now.AddMinutes(JWTAuthenticationSection.ExpirationInMinutes);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "",
               audience: "",
               claims: Claims,
               expires: Expiration,
               signingCredentials: Credenciales,
               notBefore: DateTime.Now.AddMilliseconds(2)
               );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }



}
