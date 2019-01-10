using Auth.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthServices _userService { get; }
        public AuthController(IAuthServices userService)
        {
            _userService = userService;
        }

        //[AllowAnonymous]
        [HttpPost("authenticate")]
        public ActionResult Authenticate([FromBody] UnicornUser userParam)
        {
            UnicornUser user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            _userService.DecryptToken(user.Token);

            return Ok(user);
        }


        [HttpGet("getall")]
        public ActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

    }
}
