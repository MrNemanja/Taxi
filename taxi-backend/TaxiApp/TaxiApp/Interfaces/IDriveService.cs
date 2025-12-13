using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaxiApp.Models.DTO;

namespace TaxiApp.Interfaces
{
    public interface IDriveService
    {
        Task<bool> OrderNewRide(OrderRide orderRide);
        Task<bool> IsOrder(string username);
        Task<Tuple<double, int>> IsAccepted(string username);
        Task<List<Ride>> GetRides(string username);
        Task<bool> AcceptRide(AcceptRide acceptRide);
        Task<bool> DeleteRide(string username);
        Task<bool> ConfirmRide(string username);
        Task<RideStarts> BlockUser(string username);
        Task<RideStarts> BlockUserD(string username);
        Task<List<MyRide>> GetMyEndRides(string username);
        Task<List<MyRide>> GetMyPreviousRides(string username);
        Task<List<FullRide>> GetAllRides();
        Task<bool> SetRating(Guid id);
        Task<Tuple<bool, Guid>> IsRate(string username);

    }
}
