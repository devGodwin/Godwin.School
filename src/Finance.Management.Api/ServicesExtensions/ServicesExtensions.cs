using System.Security.Claims;
using System.Text;
using Finance.Management.Api.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Finance.Management.Api.ServicesExtensions;

public static class ServicesExtensions
{
    public static void AddBearerAuthentication( this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        services.AddAuthentication(x =>
            { 
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents()
                {
                   OnTokenValidated = async context =>
                   {
                       await Task.Delay(0);
                       string userData = context.Principal?.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;

                       var claims = new List<Claim>()
                       {
                           new Claim(ClaimTypes.Role, userData)
                       };

                       var appIdentity = new ClaimsIdentity(claims, CommonConstants.AppAuthIdentity);
                       context.Principal?.AddIdentity(appIdentity);
                   }
                };
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = configuration["BearerTokenConfig:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["BearerTokenConfig:Key"])),
                    ValidAudience = configuration["BearerTokenConfig:Audience"]
                };
            });
    }
}