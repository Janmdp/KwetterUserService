using Data;
using Logic;
using Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KwetterUserService.RabbitMQ;

namespace KwetterUserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserLogic logic;
        private readonly RabbitMqMessenger messenger;
        public UserController(UserDbContext context)
        {
            logic = new UserLogic(context);
            messenger = new RabbitMqMessenger();
        }

        [HttpGet("user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUser()
        {
            var userId = User
                .Claims
                .SingleOrDefault();

            if (userId == null)
                return Unauthorized("No valid user was supplied");

            var user = logic.GetUser(Convert.ToInt32(userId.Value));

            if (user == null)
                return NotFound("User data could not be found at this time");

            return Ok(user);
        }

        //[HttpPost("create")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> CreateUser([FromBody]User user)
        //{
        //    if (!logic.CheckExistingUser(user))
        //    {
        //        logic.CreateUser(user);
        //        return Ok();
        //    }
        //    return BadRequest();
           
        //}

        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userId = User
                .Claims
                .SingleOrDefault();

            if (Convert.ToInt32(userId.Value) == id)
            {
                logic.DeleteUser(id);
                messenger.SendDeleteMessage(Convert.ToString(id));
                return Ok();
            }
            return Unauthorized();
        }
    }
}
