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
    /// Логика взаимодействия для окна "Валидация данных"
    /// </summary>
    public partial class APIWindow : Window
    {
        private readonly string _apiUrl = "http://localhost:4444/TransferSimulator/inn";
        private string _currentInn = null;
        public APIWindow()
        {
            InitializeComponent();
        }

        private async void GetAPIData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string inn = await GetInnFromApi();
                DataTextBlock.Text = inn;
                _currentInn = inn;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task<string> GetInnFromApi()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(_apiUrl);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<InnResponse>(json);

                return result?.Value ?? "Неизвестно";
            }
        }
        private void CheckAccuracy_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_currentInn))
            {
                CheckResult.Text = "Данные не загружены";
                return;
            }

            bool isValid = IsValidInn(_currentInn);

            CheckResult.Text = isValid ? "Получен корректный ИНН" : "Полученные данные не корректны";
            CheckResult.Foreground = isValid ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red;
        }

        private bool IsValidInn(string inn)
        {
            return !string.IsNullOrWhiteSpace(inn) && Regex.IsMatch(inn.Trim(), @"^\d{10}$");


        }
        public class InnResponse
        {
            [JsonProperty("value")]
            public string Value { get; set; }
        }
    }
}
