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

namespace KwetterUserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserLogic logic;
        public UserController(UserDbContext context)
        {
            logic = new UserLogic(context);
        }
        [HttpGet("user")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUser()
        {
            var username = User
                .Claims
                .SingleOrDefault();

            if (username == null)
                return Unauthorized("No valid user was supplied");

            var user = logic.GetUser(username.Value);

            if (user == null)
                return NotFound("User data could not be found at this time");

            return Ok(user);
        }
    }
}
