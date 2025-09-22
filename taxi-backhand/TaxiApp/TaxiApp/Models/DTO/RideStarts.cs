using System;

namespace TaxiApp.Models.DTO
{
    public class RideStarts
    {
        public Guid Id { get; set; }
        public bool Blocked { get; set; }
        public int DriveSeconds { get; set; }

        public int DriverSeconds { get; set; }

        public RideStarts() { }

        public RideStarts(Guid id, bool blocked, int driverSeconds, int driveSeconds)
        {
            Id = id;
            Blocked = blocked;
            DriveSeconds = driveSeconds;
            DriverSeconds = driverSeconds;
        }
    }
}
