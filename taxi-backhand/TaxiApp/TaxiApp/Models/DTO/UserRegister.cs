using Microsoft.AspNetCore.Http;
using System;
using TaxiApp.Models.Enums;

namespace TaxiApp.Models.DTO
{
    public class UserRegister
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public UserRoles.Roles Role { get; set; }
        public IFormFile ImageUrl { get; set; }

    }
}
