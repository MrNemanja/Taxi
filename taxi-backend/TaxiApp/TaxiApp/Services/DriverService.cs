using AutoMapper;
using System;
using TaxiApp.Models.DTO;
using TaxiApp.Models;
using BCrypt.Net;
using Org.BouncyCastle.Crypto.Generators;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using TaxiApp.Models.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using TaxiApp.Models.Enums;
using TaxiApp.Interfaces;

namespace TaxiApp.Services
{
    public class DriverService : IDriverService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfigurationSection secretkey;

        public DriverService(ApplicationDbContext context, IConfiguration config, IMapper mapper)
        {
            secretkey = config.GetSection("SecretKey");
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> GetUserReq(string username)
        {
            string request = "";

            List<DriverRequest> drivers = await _context.Drivers.ToListAsync();

            foreach (DriverRequest driver in drivers)
            {
                if (driver.Username.Equals(username))
                {
                    request = driver.Verified.ToString();
                }
            }
            return request;
        }
        public async Task<List<Driver>> GetAllDrivers()
        {
            List<Driver> drivers = new List<Driver>();

            List<User> users = await _context.Users.ToListAsync();
            List<DriverRequest> driverR = await _context.Drivers.ToListAsync();

            foreach (User user in users)
            {
                if (user.Role == UserRoles.Roles.Driver)
                {
                    foreach (DriverRequest request in driverR)
                    {
                        if (request.Username.Equals(user.Username))
                        {
                            Driver driver = new Driver();
                            driver.Username = user.Username;
                            driver.Name = user.Name;
                            driver.Surname = user.Surname;
                            driver.Request = request.Verified.ToString();
                            driver.Rating = request.AverageRating;
                            driver.Blocked = request.Blocked;

                            drivers.Add(driver);
                        }
                    }
                }
            }

            return drivers;
        }

        public async Task<string> RejectDriver(string username)
        {

            List<User> users = await _context.Users.ToListAsync();
            List<DriverRequest> driverRequests = await _context.Drivers.ToListAsync();

            
            foreach (DriverRequest request in driverRequests)
                {
                    if (request.Username.Equals(username))
                    {
                        request.Verified = VerificationRequest.Request.REJECTED;

                        _context.Entry(request).State = EntityState.Modified;
                        _context.SaveChanges();

                    }
                }
            foreach(User user in users)
            {
                if (user.Username.Equals(username)) return user.Email;
            }

            return "";
        }
        public async Task<string> AcceptDriver(string username)
        {

            List<User> users = await _context.Users.ToListAsync();
            List<DriverRequest> driverRequests = await _context.Drivers.ToListAsync();


            foreach (DriverRequest request in driverRequests)
            {
                if (request.Username.Equals(username))
                {
                    request.Verified = VerificationRequest.Request.ACCEPTED;

                    _context.Entry(request).State = EntityState.Modified;
                    _context.SaveChanges();

                }
            }
            foreach (User user in users)
            {
                if (user.Username.Equals(username)) return user.Email;
            }

            return "";
        }

        public async Task<bool> AddRating(Rating rating)
        {

            List<UserDrive> userDrives = await _context.UserDrives.ToListAsync();
            List<Drive> drives = await _context.Drives.ToListAsync();
            List<DriverRequest> driverRequests = await _context.Drivers.ToListAsync();


            foreach (Drive drive in drives)
            {
                if(drive.Id == rating.Id)
                {
                    drive.Rating = false;

                    foreach (UserDrive userDrive in userDrives)
                    {
                        if(userDrive.Id == drive.Id)
                        {
                            foreach(DriverRequest driverRequest in driverRequests)
                            {
                                if(driverRequest.Username.Equals(userDrive.DUsername))
                                {
                                    if (driverRequest.AverageRating == 0) driverRequest.AverageRating += rating.Rate;
                                    else driverRequest.AverageRating = (driverRequest.AverageRating + rating.Rate) / 2;

                                    _context.Entry(driverRequest).State = EntityState.Modified;
                                    _context.Entry(drive).State = EntityState.Modified;
                                    _context.SaveChanges();

                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;

            
        }

        public async Task<bool> BlockDriver(string username)
        {

            List<DriverRequest> driverRequests = await _context.Drivers.ToListAsync();


            foreach (DriverRequest request in driverRequests)
            {
                if (request.Username.Equals(username))
                {
                    request.Blocked = true;

                    _context.Entry(request).State = EntityState.Modified;
                    _context.SaveChanges();

                    return true;

                }
            }

            return false;
        }

        public async Task<bool> UnblockDriver(string username)
        {

            List<DriverRequest> driverRequests = await _context.Drivers.ToListAsync();


            foreach (DriverRequest request in driverRequests)
            {
                if (request.Username.Equals(username))
                {
                    request.Blocked = false;

                    _context.Entry(request).State = EntityState.Modified;
                    _context.SaveChanges();

                    return true;

                }
            }

            return false;
        }

        public async Task<bool> IsBlocked(string username)
        {

            List<DriverRequest> driverRequests = await _context.Drivers.ToListAsync();


            foreach (DriverRequest request in driverRequests)
            {
                if (request.Username.Equals(username))
                {
                    if (request.Blocked == true) return true;
                    else return false;

                }
            }

            return false;
        }

    }
}
