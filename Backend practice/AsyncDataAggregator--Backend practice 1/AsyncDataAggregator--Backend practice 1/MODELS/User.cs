using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace AsyncDataAggregator__Backend_practice_1.Models
{
    public class User
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }


    }
}
//all the classes in model are pure data containers, so they hold no logic, just properties that match the JSON structure of the API response.
//This allows us to easily desterialize the JSON data into C# objects using libraries like System.Text.Json .