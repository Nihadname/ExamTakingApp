using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1;

public static class ServiceRegistration
{
    public static void Register(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("AppConnectionString"))
        );
        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireDigit = true;
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
      

    }
}