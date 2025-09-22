using Microsoft.AspNetCore.Http;
using System;
using TaxiApp.Models.Enums;

namespace TaxiApp.Models.DTO
{
    public class UserProfile
    {
        public string Email { get; set; }
        public string Name { get; set; } 
        public string Surname { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public UserRoles.Roles Role { get; set; }
        public string ImageUrl { get; set; }
        public VerificationRequest.Request Verified { get; set; }


    }
}
