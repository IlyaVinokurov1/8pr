using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Weather_Vinokurov.Classes;
using Weather_Vinokurov.Models;

namespace Weather_Vinokurov
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataResponse responce;
        public MainWindow()
        {
            InitializeComponent();

        }
        public async void Iint()
        {
            parent.Children.Clear();
            responce = await GetWeather.Get(58.009671f, 56.226184f);
            foreach (Forecast forecast in responce.forecasts)
            {
                Days.Items.Add(forecast.date.ToString("dd.MM.yyyy"));
            }
            Create(0);
        }
        public void Create(int idForecast)
        {
            foreach (Hour hour in responce.forecasts[idForecast].hours)
            {
                parent.Children.Add(new Elements.Item(hour));
            }
        }


        private void SelectDay(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Create(Days.SelectedIndex);
        }

        private void UpdateWeather(object sender, RoutedEventArgs e)
        {
            Iint();
        }
    }
}