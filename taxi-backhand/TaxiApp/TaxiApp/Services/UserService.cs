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
    public class UserService: IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfigurationSection secretkey;

        public UserService(ApplicationDbContext context, IConfiguration config, IMapper mapper)
        {
            secretkey = config.GetSection("SecretKey");
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> GetLoggedUser(UserLogin userLogin)
        {
            string result;

            User thisUser = _mapper.Map<User>(userLogin);
            List<User> users = await _context.Users.ToListAsync();

            if(users.Count == 0)
            {
                result = "err";
                return result;
            }

            User last = users.Last();

            try
            {
                foreach (User userr in users)
                { 
                 
                    if (userr.Username.Equals(thisUser.Username))
                    {
                        if (BCrypt.Net.BCrypt.Verify(thisUser.Password, userr.Password))
                        {

                            List<Claim> claims = new List<Claim>();
                            if (userr.Role == UserRoles.Roles.Admin) claims.Add(new Claim(ClaimTypes.Role, "admin"));
                            else if (userr.Role == UserRoles.Roles.RegularUser) claims.Add(new Claim(ClaimTypes.Role, "regularUser"));
                            else if (userr.Role == UserRoles.Roles.Driver) claims.Add(new Claim(ClaimTypes.Role, "driver"));

                            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey.Value));
                            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                            var tokeOptions = new JwtSecurityToken(
                                issuer: "http://localhost:44306",
                                claims: claims,
                                expires: DateTime.Now.AddMinutes(30),
                                signingCredentials: signinCredentials
                            );
                            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                            result = tokenString;
                            return result;
                        }
                        else
                        {
                            result = "err";
                            return result;
                        }

                    }
                    if (userr.Username.Equals(last.Username))
                    {
                        result = "err";
                        return result;
                    }

                }
                result = "err";
                return result;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task<string[]> AddUser(UserRegister userRegister)
        {
            string[] check = new string[2];

            User user = _mapper.Map<User>(userRegister);
            user.ImageUrl = userRegister.ImageUrl.FileName;
            user.Google = false;

            List<User> users = await _context.Users.ToListAsync();

            try
            {
                if (users.Count == 0)
                {
                    if (userRegister.Role == UserRoles.Roles.Driver)
                    {
                        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                        _context.Users.Add(user);

                        DriverRequest driverRequest = new DriverRequest();
                        driverRequest.Username = userRegister.Username;
                        driverRequest.Verified = VerificationRequest.Request.PENDING;
                        driverRequest.AverageRating = 0;
                        driverRequest.Blocked = false;
                        _context.Drivers.Add(driverRequest);

                        _context.SaveChanges();
                        SaveFileToServer(userRegister.ImageUrl);
                        check[0] = "Successful registration.";
                        check[1] = "ok";
                        return check;

                    }
                    else
                    {
                        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                        _context.Users.Add(user);
                        _context.SaveChanges();
                        SaveFileToServer(userRegister.ImageUrl);
                        check[0] = "Successful registration.";
                        check[1] = "ok";
                        return check;
                    }
                }
                else
                {
                   
                    User last = users.Last();

                    foreach (User userr in users)
                    {
                        if (user.Username.Equals(userr.Username))
                        {
                            check[0] = "User with this username already exists!";
                            check[1] = "err";
                            return check;
                        }

                        if(user.Email.Equals(userr.Email))
                        {
                            check[0] = "User with this email already exists!";
                            check[1] = "err";
                            return check;
                        }

                        if (userr.Username.Equals(last.Username))
                        {
                            if (userRegister.Role == UserRoles.Roles.Driver)
                            {
                                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                                _context.Users.Add(user);

                                DriverRequest driverRequest = new DriverRequest();
                                driverRequest.Username = userRegister.Username;
                                driverRequest.Verified = VerificationRequest.Request.PENDING;
                                _context.Drivers.Add(driverRequest);

                                _context.SaveChanges();
                                SaveFileToServer(userRegister.ImageUrl);
                                check[0] = "Successful registration.";
                                check[1] = "ok";
                            }
                            else
                            {
                                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                                _context.Users.Add(user);
                                _context.SaveChanges();
                                SaveFileToServer(userRegister.ImageUrl);
                                check[0] = "Successful registration.";
                                check[1] = "ok";
                            }
                           
                        }

                    }
                    return check;
                }
            }

            catch (Exception e)
            {
                return null;
            }


        }
        public async Task<string[]> AddGoogleUser(UserGoogleRegister userGoogleRegister)
        {
            string[] check = new string[2];

            User user = _mapper.Map<User>(userGoogleRegister);
            user.Google = true;

            List<User> users = await _context.Users.ToListAsync();

            try
            {
                if (users.Count == 0)
                {
                    if (userGoogleRegister.Role == UserRoles.Roles.Driver)
                    {
                        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                        _context.Users.Add(user);

                        DriverRequest driverRequest = new DriverRequest();
                        driverRequest.Username = userGoogleRegister.Username;
                        driverRequest.Verified = VerificationRequest.Request.PENDING;
                        driverRequest.AverageRating = 0;
                        driverRequest.Blocked = false;
                        _context.Drivers.Add(driverRequest);

                        _context.SaveChanges();

                        check[0] = "Successful registration.";
                        check[1] = "ok";
                        return check;

                    }
                    else
                    {
                        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                        _context.Users.Add(user);
                        _context.SaveChanges();


                        check[0] = "Successfull registration!";
                        check[1] = "ok";
                        return check;
                    }
                }
                else
                {

                    User last = users.Last();

                    foreach (User userr in users)
                    {
                        if (user.Username.Equals(userr.Username))
                        {
                            check[0] = "User with this username already exists!";
                            check[1] = "err";
                            return check;
                        }

                        if (user.Email.Equals(userr.Email))
                        {
                            check[0] = "User with this email already exists!";
                            check[1] = "err";
                            return check;
                        }

                        if (userr.Username.Equals(last.Username))
                        {
                            if (userGoogleRegister.Role == UserRoles.Roles.Driver)
                            {
                                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                                _context.Users.Add(user);

                                DriverRequest driverRequest = new DriverRequest();
                                driverRequest.Username = userGoogleRegister.Username;
                                driverRequest.Verified = VerificationRequest.Request.PENDING;
                                _context.Drivers.Add(driverRequest);

                                _context.SaveChanges();

                                check[0] = "Successful registration.";
                                check[1] = "ok";
                                return check;
                            }
                            else
                            {
                                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                                _context.Users.Add(user);
                                _context.SaveChanges();

                                check[0] = "Successfull registration!";
                                check[1] = "ok";
                                return check;
                            }
                        }

                    }
                    return check;
                }
            }

            catch (Exception e)
            {
                return null;
            }


        }
        public async Task<UserProfile> GetUserProfile(string username)
        {
            List<User> users = await _context.Users.ToListAsync();
            List<DriverRequest> drivers = await _context.Drivers.ToListAsync();

            foreach(User user in users)
            {
                if(user.Username.Equals(username))
                {
                    if (user.Role == UserRoles.Roles.Driver)
                    {
                        foreach (DriverRequest driverRequest in drivers)
                        {
                            if (driverRequest.Username.Equals(username))
                            {
                                UserProfile profile = _mapper.Map<UserProfile>(user);
                                profile.Verified = driverRequest.Verified;
                                return profile;
                            }
                        }
                    }
                    else
                    {
                        UserProfile profile = _mapper.Map<UserProfile>(user);
                        return profile;
                    }
                    
                }
            }

            return null;

        }
        public async Task<bool> UpdateUser(UserUpdate userUpdate)
        {
            List<User> users = await _context.Users.ToListAsync();

            try
            {
                foreach (User user in users)
                {
                    if (user.Username.Equals(userUpdate.Username))
                    {
                        if (userUpdate.ImageUrl == null)
                        {
                            user.Email = userUpdate.Email;
                            user.Name = userUpdate.Name;
                            user.Surname = userUpdate.Surname;
                            user.Address = userUpdate.Address;
                            user.Birthday = userUpdate.Birthday;

                            _context.Entry(user).State = EntityState.Modified;
                            _context.SaveChanges();

                            return true;

                        }
                        else
                        {

                            if (!userUpdate.ImageUrl.FileName.Equals(user.ImageUrl))
                            {

                                var filePath = Path.Combine(".\\images", user.ImageUrl);
                                if (File.Exists(filePath)) File.Delete(filePath);
                                SaveFileToServer(userUpdate.ImageUrl);
                            }

                            user.Email = userUpdate.Email;
                            user.Name = userUpdate.Name;
                            user.Surname = userUpdate.Surname;
                            user.Address = userUpdate.Address;
                            user.ImageUrl = userUpdate.ImageUrl.FileName;
                            user.Birthday = userUpdate.Birthday;
                            user.Google = false;

                            _context.Entry(user).State = EntityState.Modified;
                            _context.SaveChanges();


                            return true;
                        }

                    }
                }

                return false;
            }
            catch(Exception e)
            {
                return false;
            }

        }
        private async void SaveFileToServer(IFormFile file)
        {
            var filePath = Path.Combine(".\\images", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        public async Task<bool> AddAdmin(string username, string password)
        {
            List<User> users = await _context.Users.ToListAsync();

            try
            {
                if (users.Count == 0)
                {
                        string filePath = "C:\\Users\\Nemanja\\Desktop\\projektslike\\admin.jpg";
                        string fileName = "admin.jpg";
                        IFormFile file;

                        using (var stream = new FileStream(filePath, FileMode.Open))
                        {
                            file = new FormFile(stream, 0, stream.Length, "name", fileName)
                            {
                                Headers = new HeaderDictionary(),
                                ContentType = "image/jpeg"
                            };
                        

                        User user = new User();
                        user.Username = username;
                        user.Password = password; 
                        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                        user.Address = "Adminova 15";
                        user.Email = "app257438@gmail.com";
                        user.Name = "Admin";
                        user.Surname = "Adminic";
                        user.Birthday = new DateTime(1980, 10, 15);
                        user.Role = UserRoles.Roles.Admin;
                        user.ImageUrl = file.FileName;
                        user.Google = false;



                        _context.Users.Add(user);
                        _context.SaveChanges();
                        SaveFileToServer(file);

                         };

                        Console.WriteLine("An admin has been added successfully");

                        return true;
                   
                }
                else
                {

                    User last = users.Last();

                    foreach (User userr in users)
                    {
                        if (username.Equals(userr.Username))
                        {
                            Console.WriteLine("Admin already exists!");
                            return false;
                        }

                        if (userr.Username.Equals(last.Username))
                        {
                            string filePath = "C:\\Users\\Nemanja\\Desktop\\projektslike\\admin.jpg";
                            string fileName = "admin.jpg";
                            IFormFile file;

                            using (var stream = new FileStream(filePath, FileMode.Open))
                            {
                                file = new FormFile(stream, 0, stream.Length, "name", fileName)
                                {
                                    Headers = new HeaderDictionary(),
                                    ContentType = "image/jpeg"
                                };
                            

                            User user = new User();
                            user.Username = username;
                            user.Password = password;
                            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                            user.Address = "Adminova 15";
                            user.Email = "app257438@gmail.com";
                            user.Name = "Admin";
                            user.Surname = "Adminic";
                            user.Birthday = new DateTime(1980, 10, 15);
                            user.Role = UserRoles.Roles.Admin;
                            user.ImageUrl = file.FileName;
                            user.Google = false;



                            _context.Users.Add(user);
                            _context.SaveChanges();
                            SaveFileToServer(file);

                            };

                            Console.WriteLine("An admin has been added successfully");

                            return true;

                        }

                    }
                    return false;
                }
            }

            catch (Exception e)
            {
                return false;
            }




        }

        public async Task<bool> IsGoogleUser(string username)
        {
            List<User> users = await _context.Users.ToListAsync();

            foreach (User user in users)
            {
                if(user.Username.Equals(username))
                {
                    if(user.Google == true) return true;
                    else if(user.Google == false) return false;
                }
            }

            return false;
        }

    }
   
}
