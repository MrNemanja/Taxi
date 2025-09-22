using System;
using TaxiApp.Models.Enums;

namespace TaxiApp.Models.DTO
{
    public class Ride
    {
        public Guid Id { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        

    }
}

