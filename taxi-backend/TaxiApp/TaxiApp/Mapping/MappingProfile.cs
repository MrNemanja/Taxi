using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.IO;
using TaxiApp.Models;
using TaxiApp.Models.DTO;

namespace TaxiApp.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserRegister>().ReverseMap();
            CreateMap<User, UserLogin>().ReverseMap();
            CreateMap<UserProfile, User>().ReverseMap();
            CreateMap<User, UserGoogleRegister>().ReverseMap();
            CreateMap<Ride, Drive>().ReverseMap();


        }
    }
}
