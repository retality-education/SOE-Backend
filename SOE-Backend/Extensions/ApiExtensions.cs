using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SOEBackend.Endpoints;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace SOEBackend.Extensions
{
    public static class ApiExtensions
    {
        public static void AddMappedEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapAuthEndpoints();
            app.MapStoryEndpoints();
            app.MapUserEndpoints();
            app.MapCatalogEndpoints();
        }

        public static void AddApiAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // ПРИОРИТЕТ 1: Пробуем получить Access Token из заголовка
                            if (context.Request.Headers.TryGetValue("Authorization", out var headerValue))
                            {
                                var token = headerValue.ToString().Replace("Bearer ", "");
                                if (!string.IsNullOrEmpty(token))
                                {
                                    context.Token = token;
                                    return Task.CompletedTask;
                                }
                            }

                            // ПРИОРИТЕТ 2: Если в заголовке не было, пробуем из куки
                            // (для совместимости с веб-страницами, ТОЛЬКО ACCESS TOKEN)
                            context.Token = context.Request.Cookies["cool-cooka"];

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }
    }
}
