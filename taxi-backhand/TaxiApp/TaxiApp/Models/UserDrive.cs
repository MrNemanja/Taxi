using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Models.Enums;

namespace TaxiApp.Models
{
    public class UserDrive
    {
        public Guid Id { get; set; }
        public string DUsername { get; set; }
        public string RUUsername { get; set; }
    }
}
