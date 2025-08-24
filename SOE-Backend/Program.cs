using Application.Interfaces.Auth;
using Application.Interfaces.Caching;
using Application.Interfaces.CloudinaryService;
using Application.Interfaces.EmailSender;
using Application.Interfaces.Repositories;
using Application.Interfaces.RestoreCode;
using Application.Services;
using AutoMapper;
using Infrastructure.Authentication;
using Infrastructure.Caching;
using Infrastructure.ImageService;
using Infrastructure.RestorePasswordEmail;
using Infrastructure.RestorePasswordEmail.EmailSender;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Persistence;
using Persistence.Mapping;
using Persistence.Repositories;
using Serilog;
using Serilog.Events;
using SOEBackend.CustomAttributes;
using SOEBackend.Extensions;
using SOEBackend.Middlewares;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
    .CreateLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);
    var services = builder.Services;
    var configuration = builder.Configuration;

    builder.Host.UseSerilog();

    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddSerilog();
    });

    builder.Services.AddSingleton<IMapper>(serviceProvider =>
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<EntityMappingProfile>();
        }, loggerFactory);

        configuration.AssertConfigurationIsValid();
        return configuration.CreateMapper();
    });

    services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));
    services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
    services.Configure<CloudinaryOptions>(configuration.GetSection(nameof(CloudinaryOptions)));
    services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));

    services.AddApiAuthentication(builder.Configuration);

    services.AddEndpointsApiExplorer();

    services.AddSwaggerGen();

    services.AddDbContext<SoeBackendContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<ValidateRefreshTokenFilter>();

    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    services.AddScoped<ICatalogDataRepository, CatalogDataRepository>();
    services.AddScoped<IBookRepository, BookRepository>();

    services.AddScoped<AuthService>();
    services.AddScoped<UserService>();
    services.AddScoped<CatalogService>();

    services.AddScoped<IJwtProvider, JwtProvider>();
    services.AddScoped<IEmailSender, SendGridEmailSender>();
    services.AddScoped<IPasswordHasher, PasswordHasher>();
    services.AddScoped<ICloudinaryService, CloudinaryService>();
    services.AddScoped<IRestoreCodeProvider, RestoreCodeProvider>();
    services.AddScoped<IEmailSender, SendGridEmailSender>();
    
    services.AddMemoryCache();
    services.AddSingleton<ICacheService, MemoryCacheService>();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseMiddleware<ExceptionMiddleware>();

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.AddMappedEndpoints();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}