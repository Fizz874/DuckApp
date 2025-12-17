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

            var dllPath = builder.Configuration["Dao:Path"];
            var className = builder.Configuration["Dao:Type"];

            builder.Services.AddScoped<BLC>(provider =>
            {
                // Tutaj ASP.NET wywo³a ten kod przy ka¿dym ¿¹daniu HTTP
                // Mo¿esz tu dodaæ logikê sprawdzania czy œcie¿ka jest poprawna, jeœli chcesz
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();


            app.Run();
        }
    }
}