using Application.Models;
using Application.Services;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

await new FileServer(args).Run();

public class FileServer
{
    private WebApplication _app;
    public FileServer(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        AddDefaultServices(builder.Services);
        AddServices(builder.Services);
        //builder.Services.AddMenuItem(x =>
        //{
        //    x.Name = "Upload";
        //    x.AddRole(new Admin());
        //    x.Url = "/Upload";
        //});

        _app = builder.Build();

        Configure(_app);
    }

    private void AddDefaultServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddRazorPages();

        services.AddControllers(config =>
        {
            var policy = new AuthorizationPolicyBuilder()
                             .RequireAuthenticatedUser()
                             .Build();
            config.Filters.Add(new AuthorizeFilter(policy));
        });
        services.AddSession();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.Cookie.IsEssential = true;
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.Name = "Cookie";
            options.LoginPath = "/account/login";
            options.LogoutPath = "/account/logout";
        });

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.ConsentCookie.IsEssential = true;
            options.CheckConsentNeeded = context => false;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });
        services.AddAuthorization();
    }

    private void AddServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IAuthenticationStateProvider, AuthenticationStateProvider>();
        services.AddSingleton<IHashingAlgorithm, HashingAlgorithm>(); //temp singleton for IApplicationSecurity
        services.AddSingleton<IApplicationSecurity, ApplicationSecurity>(); //temp Singleton for in-memory data save
        services.AddTransient<IBlobProvider, BlobProvider>();
    }

    private void Configure(WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.UseCookiePolicy();
        app.UseStaticFiles();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.UseSession();
    }

    public async Task Run()
    {
        await _app.RunAsync();
    }
}

public static class MenuBuilder
{
    public static void AddMenuItem(this IServiceCollection services, Action<MenuItem> action)
    {
        var menuItem = new MenuItem();
        action(menuItem);
        services.AddTransient<IMenuItem>(_ => menuItem);
    }
}
