using Application.Core.Abstractions.Authentication;
using Authentication.Abstractions;
using Authentication.Constants;
using Authentication.Options;
using Authentication.Providers;
using Domain.UserAggregate.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Authentication.UserContext;
using Authentication.Cryptography;

namespace Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = "Bearer";
            opt.DefaultChallengeScheme = "Bearer";
            opt.DefaultScheme = "Bearer";
        }).AddJwtBearer("Bearer", opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration[JwtDefaults.IssuerSettingsKey],
                ValidAudience = configuration[JwtDefaults.AudienceSettingsKey],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration[JwtDefaults.SecurityKeySettingsKey]!))
            };
        });

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SettingsKey));

        services.AddScoped<IClaimsProvider, ClaimsProvider>();

        services.AddScoped<IPasswordChecker, PasswordChecker>();

        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddScoped<IUserContext, UserContextProvider>();

        return services;
    }
}