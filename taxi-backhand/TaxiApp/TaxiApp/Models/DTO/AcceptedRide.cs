using System;

namespace TaxiApp.Models.DTO
{
    public class AcceptedRide
    {
        public Guid Id { get; set; }
        public double Price { get; set; }
        public int Time { get; set; }

        public AcceptedRide() { }
        public AcceptedRide(Guid id, double price, int time)
        {
            Id = id;
            Price = price;
            Time = time;
        }

    }
}
