using System.Threading.Tasks;
using TaxiApp.Models.DTO;

namespace TaxiApp.Interfaces
{
    public interface IUserService
    {
        Task<string> GetLoggedUser(UserLogin userLogin);
        Task<string[]> AddUser(UserRegister userRegister);
        Task<string[]> AddGoogleUser(UserGoogleRegister userGoogleRegister);
        Task<UserProfile> GetUserProfile(string username);
        Task<bool> UpdateUser(UserUpdate userUpdate);
        Task<bool> AddAdmin(string username, string password);
        Task<bool> IsGoogleUser(string username);

    }
}
