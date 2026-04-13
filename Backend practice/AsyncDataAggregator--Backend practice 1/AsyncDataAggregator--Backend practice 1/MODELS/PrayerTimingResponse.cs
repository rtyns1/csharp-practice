using System;
using System.Collections.Generic;
using System.Text;

namespace AsyncDataAggregator__Backend_practice_1.Models
{
    public class PrayerTimingResponse
    {
        public int Code { get; set; }
        public string? status { get; set; }
        public Data? data { get; set; }

        // data is nested so we have to deal with that
      
    }
    public class Data
    {
        public Timings? timing { get; set; }
    }

    public class Timings
    {
        public string? Fajr { get; set; }
        public string? Dhuhr { get; set; }
        public string? Asr { get; set; }
        public string? Maghrib { get; set; }
        public string? Isha { get; set; }
    }

}
// pure data containers, albeit i had to learn withthe nested stuff