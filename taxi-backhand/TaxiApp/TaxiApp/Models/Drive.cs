using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models.Enums;

namespace TaxiApp.Models
{
    public class Drive
    {
        public Guid Id { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public double Price { get; set; }
        public DateTime DriveStarts { get; set; }
        public TimeSpan DriverToMeDuration { get; set; }
        public TimeSpan DriveDuration { get; set; }
        public bool Ended { get; set; }
        public bool Accepted { get; set; }
        public bool Rating { get; set; }
       
    }
}
