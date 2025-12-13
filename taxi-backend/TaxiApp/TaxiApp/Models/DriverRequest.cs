using TaxiApp.Models.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Models
{
    public class DriverRequest
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public VerificationRequest.Request Verified { get; set; }
        public int AverageRating { get; set; }
        public bool Blocked { get; set; }
    }
}
