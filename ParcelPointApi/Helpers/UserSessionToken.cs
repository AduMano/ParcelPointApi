using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ParcelPointApi.Helpers
{
    public static class UserSessionToken
    {
        public static string GenerateJwtToken()
        {
            // Load environment variables
            DotNetEnv.Env.Load();

            // Fetch required environment variables
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

            // Validate environment variables
            if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new InvalidOperationException("JWT environment variables are not properly configured.");
            }

            // Create security key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()), // Random User ID
                new Claim(JwtRegisteredClaimNames.UniqueName, "access-token")      // Custom label
            };

            // Create the token
            var token = new JwtSecurityToken(
                issuer: issuer,             // Issuer from environment
                audience: audience,         // Audience from environment
                claims: claims,
                notBefore: DateTime.UtcNow, // Start time
                expires: null,              // No expiration
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token); // Serialize the token
        }
    }
}
