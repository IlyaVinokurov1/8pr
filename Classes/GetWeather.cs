using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Weather_Vinokurov.Models;

namespace Weather_Vinokurov.Classes
{
    public class GetWeather
    {
        public static string Url = "https://api.weather.yandex.ru/v2/forecast";
        public static string Key = "demo_yandex_weather_api_key_ca6d09349ba0";
        private static CacheService _cacheService = new CacheService();

        public static async Task<DataResponce> Get(float lat, float lon, string city = "", bool forceApi = false)
        {
            if (string.IsNullOrEmpty(city))
            {
                city = "Unknown";
            }

            bool shouldUseApi = forceApi || await _cacheService.ShouldUpdateFromApiAsync(city, lat, lon);

            if (!shouldUseApi)
            {
                var cachedData = await _cacheService.GetCachedWeatherAsync(city, lat, lon);
                if (cachedData != null)
                {
                    return cachedData;
                }
            }

            bool canRequest = await _cacheService.CanMakeRequestAsync();
            if (!canRequest)
            {
                var cachedData = await _cacheService.GetCachedWeatherAsync(city, lat, lon);
                if (cachedData != null)
                {
                    return cachedData;
                }

                var remaining = await _cacheService.GetRemainingRequestsAsync();
                var nextTime = await _cacheService.GetNextRequestTimeAsync();

                if (nextTime.HasValue)
                {
                    var timeLeft = nextTime.Value - System.DateTime.Now;
                    throw new System.Exception($"До запроса {timeLeft.Minutes} минут {timeLeft.Seconds} секунд. Сегодня: {remaining}");
                }
                else
                {
                    throw new System.Exception($"Дневной лимит запросов. Осталось {remaining}");
                }
            }

            DataResponce dataResponse = null;
            string url = $"{Url}?lat={lat}&lon={lon}".Replace(",", ".");

            using (HttpClient Client = new HttpClient())
            {
                using (HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    Request.Headers.Add("X-Yandex-Weather-Key", Key);

                    using (var Response = await Client.SendAsync(Request))
                    {
                        string ContentResponse = await Response.Content.ReadAsStringAsync();
                        dataResponse = JsonConvert.DeserializeObject<DataResponce>(ContentResponse);
                    }
                }
            }

            if (dataResponse != null && !string.IsNullOrEmpty(city))
            {
                await _cacheService.SaveToCacheAsync(city, lat, lon, dataResponse);
            }

            await _cacheService.RegisterRequestAsync();

            return dataResponse;
        }
    }
}