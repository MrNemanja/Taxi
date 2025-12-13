using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models.Enums;


namespace TaxiApp.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public UserRoles.Roles Role { get; set; }
        public string ImageUrl { get; set; }
        public bool Google { get; set; }



    }
}
