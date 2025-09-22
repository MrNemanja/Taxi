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
using System.Diagnostics.Eventing.Reader;
using System.Timers;
using TaxiApp.Interfaces;

namespace TaxiApp.Services
{
    public class DriveService: IDriveService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfigurationSection secretkey;

        public DriveService(ApplicationDbContext context, IConfiguration config, IMapper mapper)
        {
            secretkey = config.GetSection("SecretKey");
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> OrderNewRide(OrderRide orderRide)
        {
            Drive drive = new Drive();
            UserDrive userDrive = new UserDrive();
            Guid guid = Guid.NewGuid();

            drive.Id = guid;
            drive.StartAddress = orderRide.StartAddress;
            drive.EndAddress = orderRide.EndAddress;
            drive.Accepted = false;
            drive.Rating = false;

            userDrive.Id = guid;
            userDrive.RUUsername = orderRide.Username;
            userDrive.DUsername = " ";

            _context.Drives.Add(drive);
            _context.UserDrives.Add(userDrive);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> IsOrder(string username)
        {
            List<UserDrive> userDrives = await _context.UserDrives.ToListAsync();
            List<Drive> drives = await _context.Drives.ToListAsync();

            foreach (UserDrive userDrive in userDrives)
            {
                if(userDrive.RUUsername.Equals(username))
                {
                    foreach(Drive drive in drives)
                    {
                        if(drive.Id == userDrive.Id)
                        {
                            if (drive.Accepted == false && drive.DriverToMeDuration.Hours == 0 && drive.DriverToMeDuration.Minutes == 0
                                && drive.DriverToMeDuration.Seconds == 0 && drive.DriveDuration.Hours == 0 && drive.DriveDuration.Minutes == 0
                                && drive.DriveDuration.Seconds == 0)
                            {
                                return true;
                            }
                            else break;
                        }    
                    }
                }
            }

            return false;
        }

        public async Task<Tuple<double, int>> IsAccepted(string username)
        {
            List<UserDrive> userDrives = await _context.UserDrives.ToListAsync();
            List<Drive> drives = await _context.Drives.ToListAsync();

            foreach (UserDrive userDrive in userDrives)
            {
                if (userDrive.RUUsername.Equals(username))
                {
                    foreach(Drive drive in drives)
                    {
                        if(userDrive.Id == drive.Id)
                        {
                            if(drive.Accepted == true)
                            {
                                if ((drive.DriverToMeDuration.Hours != 0 || drive.DriverToMeDuration.Minutes != 0 
                                    || drive.DriverToMeDuration.Seconds != 0) && (drive.DriveDuration.Hours == 0 
                                    && drive.DriveDuration.Minutes == 0 && drive.DriveDuration.Seconds == 0))
                                {
                                    Tuple<double, int> data = new Tuple<double, int>(drive.Price, drive.DriverToMeDuration.Minutes);
                                    return data;
                                }
                            }
                        }
                    }
                }
            }

            return new Tuple<double, int>(0,0);
        }

        public async Task<List<Ride>> GetRides(string username)
        {
            List<Drive> drives = await _context.Drives.ToListAsync();
            List<UserDrive> userDrives = await _context.UserDrives.ToListAsync();
            List<Ride> rides = new List<Ride>();

            foreach(UserDrive userDrive in userDrives)
            {
                if(userDrive.DUsername != null && userDrive.DUsername.Equals(username))
                {
                    foreach (Drive drive in drives)
                    {
                        if (drive.Id == userDrive.Id)
                        {
                            if (drive.Accepted == true && drive.Ended == false)
                            {
                                return rides;
                            }
                        }
                        
                    }
                }
            }

            foreach(Drive drive in drives)
            {
                if(drive.Accepted == false)
                {
                    Ride ride = _mapper.Map<Ride>(drive);
                    rides.Add(ride);
                }
            }

            return rides;
        }

        public async Task<bool> AcceptRide(AcceptRide acceptRide)
        {
            List<Drive> drives = await _context.Drives.ToListAsync();
            List<UserDrive> userDrives = await _context.UserDrives.ToListAsync();

            foreach(Drive drive in drives)
            {
                if(drive.Id == acceptRide.Id)
                {
                    Random random = new Random();
                    drive.Price = random.NextDouble() * 1000;
                    drive.DriverToMeDuration = new TimeSpan(0, random.Next(1,2), 0);
                    drive.Accepted = true;

                    _context.Entry(drive).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }

            foreach(UserDrive userDrive in userDrives)
            {
                if(userDrive.Id == acceptRide.Id)
                {
                    userDrive.DUsername = acceptRide.Username;

                    _context.Entry(userDrive).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }

            return true;
        }

        public async Task<bool> DeleteRide(string username)
        {
            List<Drive> drives = await _context.Drives.ToListAsync();
            List<UserDrive> userDrives = await _context.UserDrives.ToListAsync();

            foreach (UserDrive userDrive in userDrives)
            {
                if (userDrive.RUUsername.Equals(username))
                {
                    foreach(Drive drive in drives)
                    {
                        if (userDrive.Id == drive.Id && (drive.DriverToMeDuration.Hours > 0 || drive.DriverToMeDuration.Minutes > 0
                            || drive.DriverToMeDuration.Seconds > 0 && (drive.DriveDuration.Hours == 0 && drive.DriveDuration.Minutes == 0
                            && drive.DriveDuration.Seconds == 0)))
                        {
                            _context.Drives.Remove(drive);
                            _context.UserDrives.Remove(userDrive);
                            _context.SaveChanges();

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public async Task<bool> ConfirmRide(string username)
        {
            List<UserDrive> userDrives = await _context.UserDrives.ToListAsync();
            List<Drive> drives = await _context.Drives.ToListAsync();

            foreach (UserDrive userDrive in userDrives)
            {
                if (userDrive.RUUsername.Equals(username))
                {
                    foreach (Drive drive in drives)
                    {
                        if (userDrive.Id == drive.Id && drive.Ended == false)
                        {

                        
                            Random random = new Random();
                            drive.DriveDuration = new TimeSpan(0, random.Next(1,2), random.Next(60));
                            drive.DriveStarts = DateTime.Now;

                            _context.Entry(drive).State = EntityState.Modified;
                            _context.SaveChanges();

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public async Task<RideStarts> BlockUser(string username)
        {
            List<UserDrive> userDrives = await _context.UserDrives.ToListAsync();
            List<Drive> drives = await _context.Drives.ToListAsync();

            foreach (UserDrive userDrive in userDrives)
            {
                if (userDrive.RUUsername.Equals(username))
                {
                    foreach (Drive drive in drives)
                    {
                        if (userDrive.Id == drive.Id)
                        {
                            if ((drive.DriveDuration.Hours > 0 || drive.DriveDuration.Minutes > 0 
                                || drive.DriveDuration.Seconds > 0) && drive.Ended == false)
                            {
                                RideStarts rideStarts = new RideStarts();
                                rideStarts.Id = drive.Id;
                                rideStarts.Blocked = true;

                                if (((DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second)
                                   - (drive.DriveStarts.Hour * 3600 + drive.DriveStarts.Minute * 60
                                   + drive.DriveStarts.Second)) >= (drive.DriverToMeDuration.Hours * 3600
                                   + drive.DriverToMeDuration.Minutes * 60 + drive.DriverToMeDuration.Seconds))
                                {
                                    rideStarts.DriverSeconds = 0;
                                }
                                else
                                {
                                    rideStarts.DriverSeconds = ((drive.DriverToMeDuration.Hours * 3600 + drive.DriverToMeDuration.Minutes * 60 + drive.DriverToMeDuration.Seconds)
                                        - ((DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second) - (drive.DriveStarts.Hour * 3600
                                        + drive.DriveStarts.Minute * 60 + drive.DriveStarts.Second)));
                                }

                                if (((DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second)
                                  - (drive.DriveStarts.Hour * 3600 + drive.DriveStarts.Minute * 60
                                  + drive.DriveStarts.Second)) >= ((drive.DriveDuration.Hours + drive.DriverToMeDuration.Hours) * 3600
                                        + (drive.DriveDuration.Minutes + drive.DriverToMeDuration.Minutes) * 60
                                        + (drive.DriveDuration.Seconds + drive.DriverToMeDuration.Seconds)))
                                {
                                    drive.Ended = true;

                                    _context.Entry(drive).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                                else
                                {

                                    rideStarts.DriveSeconds = ((drive.DriveDuration.Hours + drive.DriverToMeDuration.Hours) * 3600
                                        + (drive.DriveDuration.Minutes + drive.DriverToMeDuration.Minutes) * 60
                                        + (drive.DriveDuration.Seconds + drive.DriverToMeDuration.Seconds)) -
                                        ((DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second) -
                                        (drive.DriveStarts.Hour * 3600 + drive.DriveStarts.Minute * 60 + drive.DriveStarts.Second));

                                }

                                return rideStarts;
                            }
                            break;
                        }
                    }
                }
            }

            return new RideStarts(new Guid(), false, 0, 0);
        }

        public async Task<RideStarts> BlockUserD(string username)
        {
            List<UserDrive> userDrives = await _context.UserDrives.ToListAsync();
            List<Drive> drives = await _context.Drives.ToListAsync();

            foreach (UserDrive userDrive in userDrives)
            {
                if (userDrive.DUsername.Equals(username))
                {
                    foreach (Drive drive in drives)
                    {
                        if (userDrive.Id == drive.Id)
                        {
                            if ((drive.DriveDuration.Hours > 0 || drive.DriveDuration.Minutes > 0
                                || drive.DriveDuration.Seconds > 0) && drive.Ended == false)
                            {
                                RideStarts rideStarts = new RideStarts();
                                rideStarts.Id = drive.Id;
                                rideStarts.Blocked = true;

                                if (((DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second) 
                                   - (drive.DriveStarts.Hour * 3600 + drive.DriveStarts.Minute * 60 
                                   + drive.DriveStarts.Second)) >= (drive.DriverToMeDuration.Hours * 3600 
                                   + drive.DriverToMeDuration.Minutes * 60 + drive.DriverToMeDuration.Seconds))
                                {
                                    rideStarts.DriverSeconds = 0;
                                }
                                else
                                {
                                    rideStarts.DriverSeconds = ((drive.DriverToMeDuration.Hours * 3600 + drive.DriverToMeDuration.Minutes * 60 + drive.DriverToMeDuration.Seconds)
                                        - ((DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second) - (drive.DriveStarts.Hour * 3600
                                        + drive.DriveStarts.Minute * 60 + drive.DriveStarts.Second)));
                                }

                                if (((DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second)
                                  - (drive.DriveStarts.Hour * 3600 + drive.DriveStarts.Minute * 60
                                  + drive.DriveStarts.Second)) >= ((drive.DriveDuration.Hours + drive.DriverToMeDuration.Hours) * 3600
                                        + (drive.DriveDuration.Minutes + drive.DriverToMeDuration.Minutes) * 60
                                        + (drive.DriveDuration.Seconds + drive.DriverToMeDuration.Seconds)))
                                {
                                    drive.Ended = true;

                                    _context.Entry(drive).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                                else
                                {

                                    rideStarts.DriveSeconds = (((drive.DriveDuration.Hours + drive.DriverToMeDuration.Hours) * 3600
                                        + (drive.DriveDuration.Minutes + drive.DriverToMeDuration.Minutes) * 60
                                        + (drive.DriveDuration.Seconds + drive.DriverToMeDuration.Seconds)) -
                                        ((DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second) -
                                        (drive.DriveStarts.Hour * 3600 + drive.DriveStarts.Minute * 60 + drive.DriveStarts.Second)));

                                }


                                return rideStarts;
                            }
                            break;
                        }
                    }
                }
            }

            return new RideStarts(new Guid(), false, 0, 0);
        }


        public async Task<List<MyRide>> GetMyEndRides(string username)
        {
            List<UserDrive> userDrives = await _context.UserDrives.ToListAsync();
            List<Drive> drives = await _context.Drives.ToListAsync();
            List<User> users = await _context.Users.ToListAsync();
            MyRide myRide = new MyRide();
            List<MyRide> myRides = new List<MyRide>();

            foreach(UserDrive userDrive in userDrives)
            {
                if(userDrive.DUsername.Equals(username))
                {
                    foreach(Drive drive in drives)
                    {
                        if(drive.Id == userDrive.Id)
                        {
                            if (drive.Ended == true)
                            {
                                myRide.StartAddress = drive.StartAddress;
                                myRide.EndAddress = drive.EndAddress;
                                myRide.Price = drive.Price;

                                foreach (User user in users)
                                {
                                    if (userDrive.RUUsername.Equals(user.Username))
                                    {
                                        myRide.Name = user.Name;
                                        myRide.Surname = user.Surname;
                                        myRides.Add(myRide);
                                        break;

                                    }
                                }
                            }
                        }
                    }
                    
                }
                myRide = new MyRide();

            }

            

            return myRides;

        }

        public async Task<List<MyRide>> GetMyPreviousRides(string username)
        {
            List<UserDrive> userDrives = await _context.UserDrives.ToListAsync();
            List<Drive> drives = await _context.Drives.ToListAsync();
            List<User> users = await _context.Users.ToListAsync();
            MyRide myRide = new MyRide();
            List<MyRide> myRides = new List<MyRide>();

            foreach (UserDrive userDrive in userDrives)
            {
                if (userDrive.RUUsername.Equals(username))
                {
                    foreach (Drive drive in drives)
                    {
                        if (drive.Id == userDrive.Id)
                        {
                            if (drive.Ended == true)
                            {
                                myRide.StartAddress = drive.StartAddress;
                                myRide.EndAddress = drive.EndAddress;
                                myRide.Price = drive.Price;

                                foreach (User user in users)
                                {
                                    if (userDrive.DUsername.Equals(user.Username))
                                    {
                                        myRide.Name = user.Name;
                                        myRide.Surname = user.Surname;
                                        myRides.Add(myRide);
                                        break;

                                    }
                                }
                            }
                        }
                    }

                }
                myRide = new MyRide();

            }



            return myRides;

        }

        public async Task<List<FullRide>> GetAllRides()
        {
            List<UserDrive> userDrives = await _context.UserDrives.ToListAsync();
            List<Drive> drives = await _context.Drives.ToListAsync();
            List<User> users = await _context.Users.ToListAsync();
            FullRide ride = new FullRide();
            List<FullRide> rides = new List<FullRide>();


            foreach (Drive drive in drives)
            {
                if (drive.Accepted == true)
                {
                    if ((drive.DriverToMeDuration.Hours != 0 || drive.DriverToMeDuration.Minutes != 0 || drive.DriverToMeDuration.Seconds != 0)
                        && (drive.DriveDuration.Hours == 0 && drive.DriveDuration.Minutes == 0 && drive.DriveDuration.Seconds == 0))
                    {
                        ride.Id = drive.Id;
                        ride.StartAddress = drive.StartAddress;
                        ride.EndAddress = drive.EndAddress;
                        ride.Price = drive.Price;
                        ride.DriverToMeDuration = drive.DriverToMeDuration.ToString();
                        ride.DriveDuration = "The ride hasn't started yet.";

                        foreach (UserDrive userDrive in userDrives)
                        {
                            if (userDrive.Id == drive.Id)
                            {
                                foreach (User user in users)
                                {
                                    if (user.Username.Equals(userDrive.RUUsername))
                                    {
                                        ride.NameC = user.Name;
                                        ride.SurnameC = user.Surname;
                                    }
                                    else if (user.Username.Equals(userDrive.DUsername))
                                    {
                                        ride.NameD = user.Name;
                                        ride.SurnameD = user.Surname;
                                    }
                                }
                                rides.Add(ride);
                                ride = new FullRide();
                                break;
                            }
                        }
                    }
                    else if ((((DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second) - (drive.DriveStarts.Hour * 3600
                              + drive.DriveStarts.Minute * 60 + drive.DriveStarts.Second)) < (drive.DriverToMeDuration.Hours * 3600 +
                              drive.DriverToMeDuration.Minutes * 60 + drive.DriverToMeDuration.Seconds)) && (((DateTime.Now.Hour * 3600
                              + DateTime.Now.Minute * 60 + DateTime.Now.Second) - (drive.DriveStarts.Hour * 3600
                              + drive.DriveStarts.Minute * 60 + drive.DriveStarts.Second)) < ((drive.DriveDuration.Hours + drive.DriverToMeDuration.Hours) * 3600
                                        + (drive.DriveDuration.Minutes + drive.DriverToMeDuration.Minutes) * 60
                                        + (drive.DriveDuration.Seconds + drive.DriverToMeDuration.Seconds))))
                    {
                        ride.Id = drive.Id;
                        ride.StartAddress = drive.StartAddress;
                        ride.EndAddress = drive.EndAddress;
                        ride.Price = drive.Price;
                        ride.DriverToMeDuration = new TimeSpan((drive.DriverToMeDuration.Hours - (DateTime.Now.Hour - drive.DriveStarts.Hour)),
                            (drive.DriverToMeDuration.Minutes - (DateTime.Now.Minute - drive.DriveStarts.Minute)),
                            (drive.DriverToMeDuration.Seconds - (DateTime.Now.Second - drive.DriveStarts.Second))).ToString();

                        ride.DriveDuration = new TimeSpan(((drive.DriverToMeDuration.Hours + drive.DriveDuration.Hours) - (DateTime.Now.Hour - drive.DriveStarts.Hour)),
                            ((drive.DriverToMeDuration.Minutes + drive.DriveDuration.Minutes) - (DateTime.Now.Minute - drive.DriveStarts.Minute)),
                            ((drive.DriverToMeDuration.Seconds + drive.DriveDuration.Seconds) - (DateTime.Now.Second - drive.DriveStarts.Second))).ToString();

                        foreach (UserDrive userDrive in userDrives)
                        {
                            if (userDrive.Id == drive.Id)
                            {
                                foreach (User user in users)
                                {
                                    if (user.Username.Equals(userDrive.RUUsername))
                                    {
                                        ride.NameC = user.Name;
                                        ride.SurnameC = user.Surname;
                                    }
                                    else if (user.Username.Equals(userDrive.DUsername))
                                    {
                                        ride.NameD = user.Name;
                                        ride.SurnameD = user.Surname;
                                    }
                                }
                                rides.Add(ride);
                                ride = new FullRide();
                                break;
                            }
                        }
                    }
                    else if ((((DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second) - (drive.DriveStarts.Hour * 3600
                              + drive.DriveStarts.Minute * 60 + drive.DriveStarts.Second)) >= (drive.DriverToMeDuration.Hours * 3600 +
                              drive.DriverToMeDuration.Minutes * 60 + drive.DriverToMeDuration.Seconds)) && (((DateTime.Now.Hour * 3600
                              + DateTime.Now.Minute * 60 + DateTime.Now.Second) - (drive.DriveStarts.Hour * 3600
                              + drive.DriveStarts.Minute * 60 + drive.DriveStarts.Second)) < ((drive.DriveDuration.Hours + drive.DriverToMeDuration.Hours) * 3600
                                        + (drive.DriveDuration.Minutes + drive.DriverToMeDuration.Minutes) * 60
                                        + (drive.DriveDuration.Seconds + drive.DriverToMeDuration.Seconds))))
                    {
                        ride.Id = drive.Id;
                        ride.StartAddress = drive.StartAddress;
                        ride.EndAddress = drive.EndAddress;
                        ride.Price = drive.Price;
                        ride.DriverToMeDuration = "The driver has arrived.";
                        ride.DriveDuration = new TimeSpan(((drive.DriverToMeDuration.Hours + drive.DriveDuration.Hours) - (DateTime.Now.Hour - drive.DriveStarts.Hour)),
                            ((drive.DriverToMeDuration.Minutes + drive.DriveDuration.Minutes) - (DateTime.Now.Minute - drive.DriveStarts.Minute)),
                            ((drive.DriverToMeDuration.Seconds + drive.DriveDuration.Seconds) - (DateTime.Now.Second - drive.DriveStarts.Second))).ToString();


                        foreach (UserDrive userDrive in userDrives)
                        {
                            if (userDrive.Id == drive.Id)
                            {
                                foreach (User user in users)
                                {
                                    if (user.Username.Equals(userDrive.RUUsername))
                                    {
                                        ride.NameC = user.Name;
                                        ride.SurnameC = user.Surname;
                                    }
                                    else if (user.Username.Equals(userDrive.DUsername))
                                    {
                                        ride.NameD = user.Name;
                                        ride.SurnameD = user.Surname;
                                    }
                                }
                                rides.Add(ride);
                                ride = new FullRide();
                                break;
                            }
                        }
                    }
                    else if ((((DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second) - (drive.DriveStarts.Hour * 3600
                              + drive.DriveStarts.Minute * 60 + drive.DriveStarts.Second)) >= (drive.DriverToMeDuration.Hours * 3600 +
                              drive.DriverToMeDuration.Minutes * 60 + drive.DriverToMeDuration.Seconds)) && (((DateTime.Now.Hour * 3600
                              + DateTime.Now.Minute * 60 + DateTime.Now.Second) - (drive.DriveStarts.Hour * 3600
                              + drive.DriveStarts.Minute * 60 + drive.DriveStarts.Second)) >= ((drive.DriveDuration.Hours + drive.DriverToMeDuration.Hours) * 3600
                                        + (drive.DriveDuration.Minutes + drive.DriverToMeDuration.Minutes) * 60
                                        + (drive.DriveDuration.Seconds + drive.DriverToMeDuration.Seconds))))
                    {
                        ride.Id = drive.Id;
                        ride.StartAddress = drive.StartAddress;
                        ride.EndAddress = drive.EndAddress;
                        ride.Price = drive.Price;
                        ride.DriverToMeDuration = "The ride is over.";
                        ride.DriveDuration = "The ride is over.";

                        foreach (UserDrive userDrive in userDrives)
                        {
                            if (userDrive.Id == drive.Id)
                            {
                                foreach (User user in users)
                                {
                                    if (user.Username.Equals(userDrive.RUUsername))
                                    {
                                        ride.NameC = user.Name;
                                        ride.SurnameC = user.Surname;
                                    }
                                    else if (user.Username.Equals(userDrive.DUsername))
                                    {
                                        ride.NameD = user.Name;
                                        ride.SurnameD = user.Surname;
                                    }
                                }
                                rides.Add(ride);
                                ride = new FullRide();
                                break;
                            }
                        }
                    }
                }

            }

            return rides;
        
        }

        public async Task<bool> SetRating(Guid id)
        {
            List<Drive> drives = await _context.Drives.ToListAsync();
  
            foreach(Drive drive in drives)
            {
                if (drive.Id == id)
                {
                    drive.Rating = true;
                    
                    _context.Entry(drive).State = EntityState.Modified;
                    _context.SaveChanges();

                    return true;
                }
            }

            return false;
        }

        public async Task<Tuple<bool, Guid>> IsRate(string username)
        {
            List<Drive> drives = await _context.Drives.ToListAsync();
            List<UserDrive> userDrives = await _context.UserDrives.ToListAsync();
           
            foreach(UserDrive userDrive in userDrives)
            {
                if(userDrive.RUUsername.Equals(username))
                {
                    foreach(Drive drive in drives)
                    {
                        if(drive.Id == userDrive.Id)
                        {
                            if (drive.Rating == true) return new Tuple<bool, Guid>(true, drive.Id);
                        }
                    }
                }
            }

            return new Tuple<bool, Guid>(false, new Guid());

        }



    }
}
