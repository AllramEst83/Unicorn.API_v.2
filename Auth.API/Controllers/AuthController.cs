using Auth.API.Auth.API.Models;
using Auth.API.Constants;
using Auth.API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthServices _userService { get; }
        public AuthController(IAuthServices userService)
        {
            _userService = userService;
        }
        

        [HttpPost("authenticate")]
        public ActionResult Authenticate([FromBody] UnicornUser userParam)
        {
            AuthAPIUser _user = _userService.Auth(userParam);
            if (_user == null)
                return BadRequest(new { message = "Username or password is incorrect" });


            return Ok(_user);
        }

        [HttpPost("validate")]
        public ActionResult Validate([FromBody] UserValidation validation)
        {
            var response = _userService.DecodeToken(validation.Token);
            if (response.IsAuthenticated == false)
                return BadRequest(response);


            return Ok(response);
        }

        [HttpPost("getall")]
        public ActionResult GetAll([FromBody] AuthAPIUser requestUser)
        {
            UserClaimsModel response = _userService.DecodeToken(requestUser.Token);
            if (!_userService.UserExistsWithRole(AuthServiceConstants.Admin ,response))
            {
                return BadRequest(new { message = "You do not have permission to access this content" });
            }

            var users = _userService.GetAll();
            return Ok(users);
        }

    }
}
