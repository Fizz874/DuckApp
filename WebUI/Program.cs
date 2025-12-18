using Strzelecki_Baranowski.DuckApp.BL;

namespace Strzelecki_Baranowski.DuckApp.WebUI
{

    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var dllPath = builder.Configuration["Dao:Path"];
            var className = builder.Configuration["Dao:Type"];

            builder.Services.AddScoped<BLC>(provider =>
            {
                // Tutaj ASP.NET wywo³a ten kod przy ka¿dym ¿¹daniu HTTP
                if (string.IsNullOrEmpty(dllPath) || string.IsNullOrEmpty(className))
                {
                    throw new InvalidOperationException("Path to DAO in appsettings.json was not found");
                }
                return new BLC(dllPath, className);
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
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.UseSession();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Ducks}/{action=Index}/{id?}")
                .WithStaticAssets();


            app.Run();
        }
    }
}