using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Application.Services
{
    public class TokenServices(IOptions<JwtSettings> options) : ITokenServices
    {
        private readonly JwtSettings settings = options.Value;

        public string CreateToken(string userId, string email, string userName, IEnumerable<string> roles)
        {
            // Private Claims => User
            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, userName)
            };

            Claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Header

            var token = new JwtSecurityToken
                (
                issuer: settings.Issuer,
                audience: settings.Audience,
                claims: Claims,
                expires: DateTime.Now.AddMinutes(settings.ExpirationInMinutes),
                signingCredentials: credentials
                ); 
           
            return new JwtSecurityTokenHandler().WriteToken(token); // Payload  
        }
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int ExpirationInMinutes { get; set; } = 60;
    }
}