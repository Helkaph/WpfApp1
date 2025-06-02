using System;
using System.Diagnostics;
using System.Windows;
using WpfApp1.Services;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private readonly string _username;
        private readonly bool _isAdmin;
        private readonly AuthService _authService;
        public ChangePasswordWindow(string username , bool isAdmin = false)
        {
            InitializeComponent();
            _username = username;
            _isAdmin = isAdmin;
            _authService = new AuthService(App.DbContext);

            if (_isAdmin)
                CurrentPasswordPanel.Visibility = Visibility.Collapsed;
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string result;

            if (_isAdmin)
                result = _authService.ChangePasswordAsAdmin(_username, NewPasswordBox.Password, ConfirmPasswordBox.Password);
            else
            result = _authService.ChangePassword(_username, CurrentPasswordBox.Password, NewPasswordBox.Password, ConfirmPasswordBox.Password);
            
            if (result == "Пароль успешно изменён")
            {
                MessageBox.Show(result);
                this.Close();
            }
            else
            {
                MessageTextBlock.Text = result;
            }
        }
    }
}
