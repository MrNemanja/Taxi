using System;
using System.Security.Policy;

namespace TaxiApp.Models.DTO
{
    public class MyRide
    {
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
