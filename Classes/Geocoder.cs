
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Weather_Vinokurov.Classes
{
    public class Geocoder
    {
        private static string GeocoderUrl = "https://geocode-maps.yandex.ru/1.x/";
        private static string ApiKey = "ec11b9dd-0f2d-4afd-8cae-ed8a50dc60b1";

        public static async Task<(float lat, float lon)> GetCoordinates(string city)
        {
            string url = $"{GeocoderUrl}?apikey={ApiKey}&geocode={city}&format=json";

            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync(url);
                dynamic json = JsonConvert.DeserializeObject(response);

                // Извлекаем координаты из ответа
                string pos = json.response.GeoObjectCollection.featureMember[0]
                    .GeoObject.Point.pos;

                // pos возвращается в формате "долгота широта"
                var coords = pos.Split(' ');
                float lon = float.Parse(coords[0]);
                float lat = float.Parse(coords[1]);

                return (lat, lon);
            }
        }
    }
}