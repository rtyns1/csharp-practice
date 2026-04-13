using System;
using System.Collections.Generic;
using System.Text;

namespace AsyncDataAggregator__Backend_practice_1.Models
{
    public class AggregateData
    {
        // combination from the 3 APIs, returns one row of final output combining one user with prayer times
        public string? UserName { get; set; }
        public string? Fajr { get; set; }
        public string? Dhuhr { get; set; }
        public string? Asr { get; set; }
        public string? Maghrib { get; set; }
        public string? Isha { get; set; }
    }
}
