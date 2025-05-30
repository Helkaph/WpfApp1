using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows;
using Newtonsoft.Json;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для APIWindow.xaml
    /// </summary>
    public partial class APIWindow : Window
    {
        private readonly string _apiUrl = "https://localhost:4444/TransferSimulator.exe/fullName";
        private string _currentFullName = null;
        public APIWindow()
        {
            InitializeComponent();
        }

        private async void GetAPIData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fullname = await GetFullNameFromApi();
                DataTextBlock.Text = fullname;
                _currentFullName = fullname;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task<string> GetFullNameFromApi()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(_apiUrl);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<FullNameResponse>(json);

                return result?.Value ?? "Неизвестно";
            }
        }
        private void CheckAccuracy_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_currentFullName))
            {
                CheckResult.Text = "Данные не загружены";
                return;
            }

            bool isValid = IsValidFullName(_currentFullName);

            CheckResult.Text = isValid ? "Получены корректные данные" : "В данных обнаружено недопустимые символы";
            CheckResult.Foreground = isValid ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red;
        }

        private bool IsValidFullName(string fullName)
        {
            return Regex.IsMatch(fullName, @"^[\p{L} \-]+$");
        }
        public class FullNameResponse
        {
            [JsonProperty("value")]
            public string Value { get; set; }
        }
    }
}
