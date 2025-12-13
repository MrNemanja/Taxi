using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Tls;
using System;
using System.IO;
using System.Threading.Tasks;
using TaxiApp.Models.DTO;
using TaxiApp.Services;
using System.Security.Claims;
using System.Collections.Generic;
using System.Net.Mail;
using TaxiApp.Interfaces;

namespace TaxiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm]UserRegister userRegister)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string[] recieve = await userService.AddUser(userRegister);

            if (recieve[1].Equals("err"))
            {
                ModelState.AddModelError(recieve[1], recieve[0]);
                return BadRequest(ModelState);
            }
            else if (recieve[1].Equals("ok"))
            {
                return Ok(200);
            }
            else return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registerGoogle")]
        public async Task<IActionResult> GoogleRegister([FromForm] UserGoogleRegister userGoogleRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string[] recieve = await userService.AddGoogleUser(userGoogleRegister);

            if (recieve[1].Equals("err"))
            {
                ModelState.AddModelError(recieve[1], recieve[0]);
                return BadRequest(ModelState);
            }
            else if (recieve[1].Equals("ok"))
            {
                return Ok(200);
            }
            else return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogIn([FromForm]UserLogin userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string result = await userService.GetLoggedUser(userLogin);

            if (result.Equals("err"))
            {
                ModelState.AddModelError(result, "Invalid username or password");
                return BadRequest(ModelState);
            }
            else
            {
                userLogin.Token = result;
                return Ok(userLogin);
            }
            
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("profile")]
        public async Task<IActionResult> GetProfile([FromForm]string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserProfile userProfile = await userService.GetUserProfile(username);

            if (userProfile == null)
            {
                ModelState.AddModelError("err", "Error with trying to show user profile..");
                return BadRequest(ModelState);
            }
            else return Ok(userProfile);
            
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("image")]
        public async Task<IActionResult> GetProfileImage(string fileName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string path = Path.Combine(".\\images", fileName);
            var image = System.IO.File.OpenRead(path);

            if (image == null)
            {
                ModelState.AddModelError("err", "Error with trying to send profile image..");
                return BadRequest(ModelState);
            }
            else return File(image, "image/jpg");

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateProfile([FromForm]UserUpdate userUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool valid = await userService.UpdateUser(userUpdate);

            return Ok(valid);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("isGoogle")]
        public async Task<IActionResult> IsGoogle([FromForm] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool recieve = await userService.IsGoogleUser(username);

            return Ok(recieve);
        }


    }
}
