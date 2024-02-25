using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Notes.Data;
using Notes.Data.Models;
using Notes.Services.Data;
using Notes.Services.Data.Interfaces;
using System.Security.Principal;

namespace Note
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); builder.Services.AddDbContext<NoteDbContext>(options =>
            options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddEntityFrameworkStores<NoteDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<INoteService, NoteService>();

            builder.Services.AddControllersWithViews()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                });
            builder.Services.AddSignalR();

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

            app.UseRouting();
            app.UseAuthentication(); // <--- Removed unnecessary semicolon here

            app.UseAuthorization();

            app.UseEndpoints(config =>
            {
                config.MapControllerRoute(
                    name: "areas",
                    pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                config.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                config.MapRazorPages();
            });

            app.Run();
        }
    }
}
