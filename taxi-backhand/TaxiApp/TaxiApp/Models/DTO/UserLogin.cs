using System;
using TaxiApp.Models.Enums;

namespace TaxiApp.Models.DTO
{
    public class UserLogin
    {

        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

    }

}
