using Mango.Services.CouponAPI.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Mango.Services.CouponAPI.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder, AppSettings appSettings)
        {
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings!.Secret)),
                    ValidateIssuer = true,
                    ValidIssuer = appSettings!.Issuer,
                    ValidateAudience = true,
                    ValidAudience = appSettings!.Audience
                };
            });

            builder.Services.AddAuthorization();

            return builder;
        }
    }
}
