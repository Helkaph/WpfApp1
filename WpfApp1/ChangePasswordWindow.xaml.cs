using System;
using System.Collections.Generic;
using System.Linq;
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
        public ChangePasswordWindow(string username)
        {
            InitializeComponent();
            _username = username;
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var auth = new AuthService();
            var result = auth.ChangePassword(_username, CurrentPasswordBox.Password, NewPasswordBox.Password, ConfirmPasswordBox.Password);

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
