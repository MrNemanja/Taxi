using System;
using System.Security.Policy;

namespace TaxiApp.Models.DTO
{
    public class FullRide
    {
        public Guid Id { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public double Price { get; set; }
        public string DriverToMeDuration { get; set; }
        public string DriveDuration { get; set; }
        public string NameD { get; set; }
        public string SurnameD { get; set; }
        public string NameC { get; set; }
        public string SurnameC { get; set; }

    }
}
