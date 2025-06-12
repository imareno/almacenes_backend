using System.IdentityModel.Tokens.Jwt;

namespace Api._1.API.Utils
{
    public class RequestHelper
    {
        public Guid GetIdClaim(HttpRequest request, string claimKey)
        {
            var authHeader = request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var token = new JwtSecurityTokenHandler().ReadToken(authHeader) as JwtSecurityToken;
            return Guid.Parse(token.Claims.First(claim => claim.Type == claimKey).Value);
        }

        public string GetStringValueClaim(HttpRequest request, string claimKey)
        {
            var authHeader = request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var token = new JwtSecurityTokenHandler().ReadToken(authHeader) as JwtSecurityToken;
            return token.Claims.First(claim => claim.Type == claimKey).Value;
        }
    }
}
