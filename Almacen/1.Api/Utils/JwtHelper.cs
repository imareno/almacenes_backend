using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api._2.Common.Models.System;


namespace Api._1.API.Utils
{
    public static class JwtHelper
    {
        private static readonly string SecretKey = "d6a0f0471f9cfdacef5354c5ef19294ac2a93af0f482169e1dff82b7e74da6f5";
        private static readonly string Issuer = "www.oopp.gob.bo";
        private static readonly string Audience = "viaticos.oopp.gob.bo";
        public static string GenerateJwt(string subject, int expirationMinutes, params Claim[] additionalClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Sub, subject),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };
            
            claims = claims.Concat(additionalClaims).ToArray();
            
            var token = new JwtSecurityToken(
                Issuer,
                Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: creds);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        public static string RefreshAccessToken(string currentAccessToken, int expirationMinutes)
        {
            var principal = GetPrincipalFromToken(currentAccessToken, SecretKey);
            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                throw new SecurityTokenException(Constants.TokenInvalido);
            }
            return CloneTokenWithNewExpiration(currentAccessToken, SecretKey, expirationMinutes);
        }

        private static ClaimsPrincipal GetPrincipalFromToken(string token, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }
      
        private static string CloneTokenWithNewExpiration(string currentToken, string secretKey, int newExpirationMinutes)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var existingTokenClaims = tokenHandler.ReadToken(currentToken) as JwtSecurityToken;
            
            var claims = new List<Claim>(existingTokenClaims.Claims);
            
            var expirationClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
            if (expirationClaim != null)
            {
                claims.Remove(expirationClaim);
            }
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(newExpirationMinutes).ToString(), ClaimValueTypes.Integer64));
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(newExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var newToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(newToken);
        }
    }

    
}
