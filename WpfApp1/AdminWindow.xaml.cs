using System.Windows;
using System.Windows.Controls;
using WpfApp1.Services;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private readonly AuthService _authService = new();
        public AdminWindow()
        {
            InitializeComponent();
            RefreshUsers();
        }

        private void RefreshUsers()
        {
            UsersList.ItemsSource = _authService.GetAllUsers();
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            var username = NewUserNameBox.Text;
            var password = NewPasswordBox.Password;
            var role = ((ComboBoxItem)RoleComboBox.SelectedItem)?.Content.ToString();
            AdminMessageTextBlock.Text = _authService.RegisterUser(username, password, role);
            RefreshUsers();
        }
        private void UnblockButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersList.SelectedItem is User selectedUser)
            {
                AdminMessageTextBlock.Text = _authService.UnblockUser(selectedUser.UserName);
                RefreshUsers();
            }
        }
        private void APIButton_Click(object sender, RoutedEventArgs e)
        {
            new APIWindow().Show();
        }
    }
}
