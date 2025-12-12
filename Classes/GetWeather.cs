using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Weather_Vinokurov.Classes
{
    public class GetWeather
    {
        public static string Url = "http://api.weather.yandex.ru/v2/forecast";
        public static string Key = "demo_yandex_weather_api_key_ca6d09349ba0";
        public static async void Get(float lat, float lon)
        {
            using(HttpClient Client = new HttpClient())
            {
                using (HttpRequestMessage Request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"{Url}?Lat = {lat}&lon={lon}"))
                {
                    Request.Headers.Add("X-Yandex-Weather-Key", Key);
                    using(var Response = await Client.SendAsync(Request))
                    {
                        string DataResponse = await Response.Content.ReadAsStringAsync();
                    }
                }
                    
            }
        }
    }
}
