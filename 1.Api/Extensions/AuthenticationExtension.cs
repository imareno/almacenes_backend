using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Almacen._1.Api.Extensions
{
    public static class AuthenticationExtension
    {
        private const string SecretKey = "d8a0f0336f9cfdacea6354c5ef19295bc2a93af0f482169e1dff82b7e74da6f5-!33j49ja943jsh";
        private const string Issuer = "www.oopp.gob.bo";

        public static void ConfigureAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, //true
                        ValidateAudience = false, //false
                        ValidateLifetime = true, //true
                        ValidateIssuerSigningKey = true, //true
                        ValidIssuer = Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
    }
}
