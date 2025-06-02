using System;
using System.Security.Policy;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static AppDbContext DbContext { get; set; }
        private ServiceProvider _serviceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            _serviceProvider = services.BuildServiceProvider();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            try
            {
                optionsBuilder.UseSqlServer("Server=SEPHIROTH\\SQLEXPRESS;Database=Exam;Trusted_Connection=True;TrustServerCertificate=True;User Id=Sephiroth\\Sasha;Password=2206");
                DbContext = new AppDbContext(optionsBuilder.Options);
                DbContext.Database.OpenConnection();
                DbContext.Database.CloseConnection();

                var loginWindow = new LoginWindow(DbContext);
                loginWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Подключение завершено ошибкой:\n{ex.Message}\nЗавершение работы приложения");
                Environment.Exit(1);
            }
        }
    }

}
