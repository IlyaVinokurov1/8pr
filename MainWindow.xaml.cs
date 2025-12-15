using System.Windows;
using Weather_Vinokurov.Classes;
using Weather_Vinokurov.Models;

namespace Weather_Vinokurov
{
    public partial class MainWindow : Window
    {
        DataResponse response;
        private float currentLat = 58.009671f;
        private float currentLon = 56.226184f;
        private string currentCity = "Пермь";

        public MainWindow()
        {
            InitializeComponent();    
            LoadWeatherForCity(currentCity);
        }

        private async void GetWeatherByCity(object sender, RoutedEventArgs e)
        {
            string city = txtCity.Text.Trim();
            if (!string.IsNullOrEmpty(city))
            {
                await LoadWeatherForCity(city);
            }
        }

        public async Task LoadWeatherForCity(string city)
        {
            try
            {
                var coordinates = await Geocoder.GetCoordinates(city);
                currentLat = coordinates.lat;
                currentLon = coordinates.lon;
                currentCity = city;
                await LoadWeather(currentLat, currentLon);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении координат: {ex.Message}");
            }
        }

        public async Task LoadWeather(float lat, float lon)
        {
            parent.Children.Clear();
            Days.Items.Clear();

            response = await GetWeather.Get(lat, lon);

            if (response?.forecasts != null)
            {
                foreach (Forecast forecast in response.forecasts)
                {
                    Days.Items.Add(forecast.date.ToString("dd.MM.yyyy"));
                }
                Create(0);
            }
        }

        public void Create(int idForecast)
        {
            if (response?.forecasts == null || idForecast >= response.forecasts.Count)
                return;

            parent.Children.Clear();
            foreach (Hour hour in response.forecasts[idForecast].hours)
            {
                parent.Children.Add(new Elements.Item(hour));
            }
        }

        private void SelectDay(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Days.SelectedIndex >= 0)
            {
                Create(Days.SelectedIndex);
            }
        }

        private async void UpdateWeather(object sender, RoutedEventArgs e)
        {
            await LoadWeather(currentLat, currentLon);
        }
    }
}