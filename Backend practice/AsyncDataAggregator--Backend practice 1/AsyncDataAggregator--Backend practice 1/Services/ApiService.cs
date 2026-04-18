using AsyncDataAggregator__Backend_practice_1.Models;
using System;
using System.Collections.Generic;
using System.Net.Http; //how the hell do you know to use this?
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using AsyncDataAggregator__Backend_practice_1.Helpers;



namespace AsyncDataAggregator__Backend_practice_1.Services
{
    public class ApiService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        //i need to set up a HttpClient singleton. what is this?
        // i need one HttpClient instance that is shared across all methods i call.
        // ive already set up a private static readonly HttpClient _httpClient, and ibe initialze it where it is declared.
        //now, i need to set an optional timeout and defualt headers
        public ApiService()
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(35);//sets timeout to 30 seconds
            //set a default header
            
        }

        public async Task<List<User>> GetUsersAsync()
        {
            string url = "https://jsonplaceholder.typicode.com/users";

            return await RetryHandler.ExecuteWithRetry(async () =>
            {
                string json = await _httpClient.GetStringAsync(url);

                try
                {
                    var users = JsonSerializer.Deserialize<List<User>>(json);
                    if (users == null)
                    {
                        await FileLogger.LogErrorAsync("Deserialization returned null for users.");
                        throw new JsonException("Deserialized users is null.");
                    }

                    return users;
                }
                catch (JsonException ex)
                {
                    await FileLogger.LogErrorAsync($"Deserialization failed for users: {ex.Message}");
                    throw;
                }
            });
        }


        public async Task<Timings> GetPrayerTimesAsync()
        {
            string url = "http://api.aladhan.com/v1/timingsByCity?city=Nairobi&country=Kenya&method=2";

            return await RetryHandler.ExecuteWithRetry(async () =>
            {
                string json = await _httpClient.GetStringAsync(url);

                try
                {
                    var response = JsonSerializer.Deserialize<PrayerTimingResponse>(json);
                    if (response == null)
                    {
                        await FileLogger.LogErrorAsync("Deserialization returned null for prayer timing response.");
                        throw new JsonException("Deserialized prayer timing response is null.");
                    }

                    var data = response.data;
                    if (data == null)
                    {
                        await FileLogger.LogErrorAsync("Deserialized prayer timing response has null data.");
                        throw new JsonException("Deserialized prayer timing data is null.");
                    }

                    var timing = data.timings;
                    if (timing == null)
                    {
                        await FileLogger.LogErrorAsync("Deserialized prayer timing response has null timing.");
                        throw new JsonException("Deserialized prayer timing is null.");
                    }

                    return timing;
                }
                catch (JsonException ex)
                {
                    await FileLogger.LogErrorAsync($"Deserialization failed for prayer times: {ex.Message}");
                    throw;
                }
            });
        }
    }
}
