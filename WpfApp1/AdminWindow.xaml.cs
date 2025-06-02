using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Services;
using WpfApp1.Models;
using System.Diagnostics;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private readonly AuthService _authService;
        public AdminWindow()
        {
            InitializeComponent();
            _authService = new AuthService(App.DbContext);
            RefreshUsers();
        }

        private void RefreshUsers()
        {
            UsersList.ItemsSource = null;
            UsersList.ItemsSource = _authService.GetAllUsers();
        }

        private void AddUserButton_CLick(object sender, RoutedEventArgs e)
        {
            var username = NewUserNameBox.Text;
            var password = NewPasswordBox.Password;
            var roleString = ((ComboBoxItem)RoleComboBox.SelectedItem)?.Content.ToString();
            int role = roleString == "Admin" ? 2 : 1;
            Debug.WriteLine(role.ToString());
            AdminMessageTextBlock.Text = _authService.RegisterUser(username, password, role);
            RefreshUsers();
        }
        private void UnblockButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersList.SelectedItem is User selectedUser)
            {
                string message;
                if (selectedUser.IsBlocked)
                {
                    message = _authService.UnblockUser(selectedUser.UserName);
                }
                else
                {
                    selectedUser.IsBlocked = true;
                    App.DbContext.SaveChanges();
                    message = "Пользователь заблокирован";
                }
                AdminMessageTextBlock.Text = _authService.UnblockUser(message);
                RefreshUsers();
            }
        }
        private void APIButton_Click(object sender, RoutedEventArgs e)
        {
            new APIWindow().Show();
        }
        private void ExitButton_Click(Object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Id" || e.PropertyName == "IsBlocked" || e.PropertyName == "LastLogin")
                e.Column.IsReadOnly = true;
            if (e.PropertyName == "FailAttempts" || e.PropertyName == "HashPassword" || e.PropertyName == "IsFirstPassword")
                e.Cancel = true;
        }
        private void UsersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersList.SelectedItems is User selectedUser)
            {
                UnblockButton.Content = selectedUser.IsBlocked ? "Разблокировать выбранного пользователя" : "Заблокировать выбранного пользователя";
            }
        }
        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersList.SelectedItem is User selectedUser)
            {
                ChangePasswordWindow passwordWindow = new ChangePasswordWindow(selectedUser.UserName, true);
                passwordWindow.Owner = this;
                passwordWindow.ShowDialog();

                RefreshUsers();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя из списка.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
