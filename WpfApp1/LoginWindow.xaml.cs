using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Data;
using WpfApp1.Services;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private AppDbContext _context;
        private readonly AuthService _authService;
        public LoginWindow(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            _authService = new AuthService(_context);
            Loaded += async (_, _) => await EnsureDefaultAdminAsync();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false;
            var result = _authService.Authenticate(UsernameBox.Text, PasswordBox.Password);

            if (result == "1")
            {
                MessageBox.Show("Вы успешно авторизовались.");
            }
            else if (result == "2")
            {
                MessageBox.Show("Вы успешно авторизовались.");
                new AdminWindow().Show();
                this.Close();
            }
            else if (result == "CHANGE")
            {
                new ChangePasswordWindow(UsernameBox.Text).Show();
            }
            else
            {
                MessageTextBlock.Text = result;
            }
            LoginButton.IsEnabled = true;
        }
        private async Task EnsureDefaultAdminAsync()
        {
            try
            {
                if (!await _context.Users.AnyAsync(u => u.Role == 2))
                {
                    var admin = new Models.User()
                    {
                        UserName = "Admin",
                        HashPassword = PasswordService.HashPassword("Administrator123@!"),
                        Role = 2
                    };
                    _context.Users.Add(admin);
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Создан администратор по умолчанию. Данные уточняйте в руководстве администратора");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка при создании администратора: " + ex.Message);
            }
        }
    }
}