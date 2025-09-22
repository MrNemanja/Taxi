using Org.BouncyCastle.Bcpg.OpenPgp;
using System;

namespace TaxiApp.Models.DTO
{
    public class Rating
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
    }
}
