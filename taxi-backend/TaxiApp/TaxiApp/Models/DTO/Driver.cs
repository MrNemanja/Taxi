using Microsoft.AspNetCore.Http;
using System;
using TaxiApp.Models.Enums;

namespace TaxiApp.Models.DTO
{
    public class Driver
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Request { get; set; }
        public int Rating { get; set; }
        public bool Blocked { get; set; }
    }
}
