using System;
using System.Threading.Tasks;
using AsyncDataAggregator__Backend_practice_1.Services;
using AsyncDataAggregator__Backend_practice_1.Helpers;

namespace AsyncDataAggregator__Backend_practice_1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Async Data Aggregator ===\n");
            Console.WriteLine("Fetching users from JSONPlaceholder...");
            Console.WriteLine("Fetching prayer times from Aladhan API...\n");

            try
            {
                var apiService = new ApiService();

                // Fetch both APIs in parallel
                var usersTask = apiService.GetUsersAsync();
                var prayerTimesTask = apiService.GetPrayerTimesAsync();

                await Task.WhenAll(usersTask, prayerTimesTask);

                var users = await usersTask;
                var prayerTimes = await prayerTimesTask;

                if (users == null || users.Count == 0)
                {
                    Console.WriteLine("No users found.");
                    return;
                }

                if (prayerTimes == null)
                {
                    Console.WriteLine("No prayer times found.");
                    return;
                }

                Console.WriteLine("\n=== RESULTS ===\n");

                foreach (var user in users)
                {
                    Console.WriteLine($"User: {user.name}");
                    Console.WriteLine($"Email: {user.email}");
                    Console.WriteLine("\nPrayer Times for Nairobi:");
                    Console.WriteLine($"Fajr: {prayerTimes.Fajr}");
                    Console.WriteLine($"Dhuhr: {prayerTimes.Dhuhr}");
                    Console.WriteLine($"Asr: {prayerTimes.Asr}");
                    Console.WriteLine($"Maghrib: {prayerTimes.Maghrib}");
                    Console.WriteLine($"Isha: {prayerTimes.Isha}");
                    Console.WriteLine(new string('-', 40));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn error occurred: {ex.Message}");
                await FileLogger.LogErrorAsync($"Program execution failed: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}