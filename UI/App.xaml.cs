using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Strzelecki_Baranowski.DuckApp.BL;
using Strzelecki_Baranowski.DuckApp.UI;
using Microsoft.Extensions.DependencyInjection;
using Strzelecki_Baranowski.DuckApp.ViewModels;

namespace Strzelecki_Baranowski.DuckApp.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private IServiceProvider serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();


            var serviceColl = new ServiceCollection();

            //serviceColl.AddSingleton<IConfiguration>(config);

            serviceColl.AddSingleton<BLC>(provider =>
            {
                var section = config.GetSection("Dao");

                return new BLC(section["Path"], section["Type"]);
            });

            serviceColl.AddSingleton<MainViewModel>();
            serviceColl.AddSingleton<MainWindow>();

            serviceProvider = serviceColl.BuildServiceProvider();

            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);

           

            //var section = config.GetSection("Dao");
            //var businessLogic = new BLC(section["Path"], section["Type"]);

            //var mainWindow = new MainWindow(businessLogic);
            //mainWindow.Show();
        }

    }

}
