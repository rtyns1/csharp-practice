using System;
using System.IO;
using AsyncDataAggregator__Backend_practice_1;
using AsyncDataAggregator__Backend_practice_1.Helpers;

namespace AsyncDataAggregator__Backend_practice_1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Testing async FileLogger...");
            await FileLogger.LogErrorAsync ("Test async message - program started");
            Console.WriteLine("Check error.log in the project folder.");
        }
    }
}
