using System.Collections.Generic;
using System.Threading.Tasks;
using TaxiApp.Models.DTO;

namespace TaxiApp.Interfaces
{
    public interface IDriverService
    {
        Task<string> GetUserReq(string username);
        Task<List<Driver>> GetAllDrivers();
        Task<string> RejectDriver(string username);
        Task<string> AcceptDriver(string username);
        Task<bool> AddRating(Rating rating);
        Task<bool> BlockDriver(string username);
        Task<bool> UnblockDriver(string username);
        Task<bool> IsBlocked(string username);

    }
}
