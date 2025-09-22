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
    public class DriveController: ControllerBase
    {
        private readonly IDriveService driveService;

        public DriveController(IDriveService driveService)
        {
            this.driveService = driveService;
        }

        [Authorize(Roles = "regularUser")]
        [HttpPost]
        [Route("order")]
        public async Task<IActionResult> OrderRide([FromForm] OrderRide orderRide)
        {
            bool valid = false;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            valid = await driveService.OrderNewRide(orderRide);

            if (valid) return Ok(200);
            else return BadRequest();
            
        }

        [Authorize(Roles = "regularUser")]
        [HttpPost]
        [Route("isOrder")]
        public async Task<IActionResult> IsOrder([FromForm] string username)
        {
            bool valid = false;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            valid = await driveService.IsOrder(username);

            return Ok(valid);

        }

        [Authorize(Roles = "regularUser")]
        [HttpPost]
        [Route("isAccept")]
        public async Task<IActionResult> IsAccept([FromForm] string username)
        {
            Tuple<double, int> data;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            data = await driveService.IsAccepted(username);

            return Ok(data);

        }

        [Authorize(Roles = "driver")]
        [HttpPost]
        [Route("getRides")]
        public async Task<IActionResult> GetRides([FromForm] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Ride> rides = await driveService.GetRides(username);

            return Ok(rides);

        }

        [Authorize(Roles = "driver")]
        [HttpPost]
        [Route("acceptRide")]
        public async Task<IActionResult> AcceptRide([FromForm]AcceptRide acceptRide)
        {
            bool valid = false;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          valid = await driveService.AcceptRide(acceptRide);

            return Ok(valid);
        }

        [Authorize(Roles = "regularUser")]
        [HttpPost]
        [Route("deleteRide")]
        public async Task<IActionResult> DeleteRide([FromForm] string username)
        {
            bool valid = false;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            valid = await driveService.DeleteRide(username);

            return Ok(valid);
        }

        [Authorize(Roles = "regularUser")]
        [HttpPost]
        [Route("confirmRide")]
        public async Task<IActionResult> ConfirmRide([FromForm] string username)
        {
            bool valid = false;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            valid = await driveService.ConfirmRide(username);

            return Ok(valid);

        }

        [Authorize(Roles = "regularUser")]
        [HttpPost]
        [Route("block")]
        public async Task<IActionResult> Block([FromForm] string username)
        {
            RideStarts rideStarts;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            rideStarts = await driveService.BlockUser(username);

            return Ok(rideStarts);

        }

        [Authorize(Roles = "driver")]
        [HttpPost]
        [Route("blockD")]
        public async Task<IActionResult> BlockD([FromForm] string username)
        {
            RideStarts rideStarts;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            rideStarts = await driveService.BlockUserD(username);

            return Ok(rideStarts);

        }

        [Authorize(Roles = "driver")]
        [HttpPost]
        [Route("myRides")]
        public async Task<IActionResult> GetMyRides([FromForm] string username)
        {
            List<MyRide> myRides;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            myRides = await driveService.GetMyEndRides(username);

            return Ok(myRides);

        }

        [Authorize(Roles = "regularUser")]
        [HttpPost]
        [Route("previousRides")]
        public async Task<IActionResult> GetPreviousRides([FromForm] string username)
        {
            List<MyRide> myRides;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            myRides = await driveService.GetMyPreviousRides(username);

            return Ok(myRides);

        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("allRides")]
        public async Task<IActionResult> GetAllRides()
        {
            List<FullRide> rides;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            rides = await driveService.GetAllRides();

            return Ok(rides);

        }

        [Authorize(Roles = "driver, regularUser")]
        [HttpPost]
        [Route("setRating")]
        public async Task<IActionResult> SetRating([FromForm]Guid id)
        {
            bool valid;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            valid = await driveService.SetRating(id);

            return Ok(200);

        }


        [Authorize(Roles = "regularUser")]
        [HttpPost]
        [Route("isRating")]
        public async Task<IActionResult> IsRate([FromForm] string username)
        {
            Tuple<bool, Guid> data;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            data = await driveService.IsRate(username);

            return Ok(data);

        }


    }
}
