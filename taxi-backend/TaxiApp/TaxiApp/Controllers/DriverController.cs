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
    public class DriverController : ControllerBase
    {
        private readonly IDriverService driverService;

        public DriverController(IDriverService driverService)
        {
            this.driverService = driverService;
        }

        [Authorize(Roles = "driver")]
        [HttpPost]
        [Route("request")]
        public async Task<IActionResult> GetUserRequest([FromForm] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string request = await driverService.GetUserReq(username);

            return Ok(request);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("pendingDrivers")]
        public async Task<IActionResult> GetDrivers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Driver> drivers = await driverService.GetAllDrivers();

            return Ok(drivers);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("rejectDriver")]
        public async Task<IActionResult> RejectDriver([FromForm] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string email = await driverService.RejectDriver(username);

            if (email == "") return BadRequest();
            else if (email != "")
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential("app257438@gmail.com", "gvfb yavz spra mbet");
                MailMessage mail = new MailMessage(from: "app257438@gmail.com", to: email,
                    subject: "Verification request", body: "Your verification request has been unfortunately rejected.");


                smtpClient.Send(mail);
                return Ok(200);
            }
            else return Ok(200);

        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("acceptDriver")]
        public async Task<IActionResult> AcceptDriver([FromForm] string username)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string email = await driverService.AcceptDriver(username);

            if (email == "") return BadRequest();
            else if (email != "")
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential("app257438@gmail.com", "gvfb yavz spra mbet");
                MailMessage mail = new MailMessage(from: "app257438@gmail.com", to: email,
                    subject: "Verification request", body: "Your verification request has been successfully accepted!\nNow you can accept" +
                    " and receive rides.");


                smtpClient.Send(mail);
                return Ok(200);
            }
            else return Ok(200);
        }

        [Authorize(Roles = "regularUser")]
        [HttpPost]
        [Route("addRating")]
        public async Task<IActionResult> RateDriver([FromForm] Rating rating)
        {
            bool valid;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            valid = await driverService.AddRating(rating);

            return Ok(200);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("blockDriver")]
        public async Task<IActionResult> BDriver([FromForm] string username)
        {
            bool valid;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            valid = await driverService.BlockDriver(username);
 
            return Ok(valid);
           
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("unblockDriver")]
        public async Task<IActionResult> UBDriver([FromForm] string username)
        {
            bool valid;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            valid = await driverService.UnblockDriver(username);

            return Ok(valid);

        }

        [Authorize(Roles = "driver")]
        [HttpPost]
        [Route("isBlocked")]
        public async Task<IActionResult> IsBlocked([FromForm] string username)
        {
            bool valid;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            valid = await driverService.IsBlocked(username);

            return Ok(valid);

        }


    }
}
