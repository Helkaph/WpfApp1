using System.Windows;
using WpfApp1.Services;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var auth = new AuthService();
            LoginButton.IsEnabled = false;
            var result = auth.Authenticate(UsernameBox.Text, PasswordBox.Password);

            if (result == "User")
            {
                MessageBox.Show("Вы успешно авторизовались.");
            }
            else if (result == "Admin")
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
        }
    }
}