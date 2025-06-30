using System.Globalization;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskManagmentSystem.Filters;
using TaskManagmentSystem.Hubs;
using TaskManagmentSystem.Models;
using TaskManagmentSystem.Notifications;
using TaskManagmentSystem.Notifications.Interfaces;
using TaskManagmentSystem.Repositories;
using TaskManagmentSystem.Repositories.Interfaces;
using TaskManagmentSystem.Srvices;
using TaskManagmentSystem.Srvices.Interfaces;
using TaskManagmentSystem.Srvicese;

namespace TaskManagmentSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });

            builder.Services.AddSignalR();

            builder.Services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                config.UseSqlServerStorage(builder.Configuration.GetConnectionString("hangfirecs"));
            });
            builder.Services.AddHangfireServer();

            builder.Services.AddScoped<INotificationFactory,NotificationFactory>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<INotificationScheduler, NotificationScheduler>();
            builder.Services.AddScoped<INotificationDispatcher, NotificationDispatcher>();
            builder.Services.AddScoped<IWorkSpaceRepository, WorkSpaceRepository>();
            builder.Services.AddScoped<IWorkSpaceService, WorkSpaceService>();
            builder.Services.AddScoped<ITeamAppUserRepository, TeamAppUserRepository>();
            builder.Services.AddScoped<ITeamRepository, TeamRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITeamInvitationRepository, TeamInvitationRepository>();
            builder.Services.AddScoped<ITeamAppUserService, TeamAppUserService>();
            builder.Services.AddScoped<ITeamService, TeamService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITeamInvitationService, TeamInvitationService>();
            //builder.Services.AddScoped<TeamPermissionsFilter>();


            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ar")
                };
                options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseHangfireDashboard();

            app.UseRouting();

            var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Workspace}/{action=ShowAll}/{id?}");

            app.MapHub<NotificationsHub>("/hubs/taskNotification");
            app.Run();
        }
    }
}
