using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthService.Api.Commands;
using AuthService.Api.Queries;
using AuthService.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator bus;

        public UserController(IMediator bus)
        {
            this.bus = bus;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] CreateUserCommand cmd)
        {
            var registerResult = await bus.Send(cmd);
            if(registerResult.UserId == null)
            {
                throw new Exception();
            }
            var result = await bus.Send(new GetUserQuery
            {
                Password = cmd.Password,
                Username = cmd.Username
            });
            return new JsonResult(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] GetUserQuery cmd)
        {
            var result = await bus.Send(cmd);
            return new JsonResult(result);
        }

        public ActionResult GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return new JsonResult(new UserDto()
            {
                NameIdentifier = identity.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value,
                Email = identity.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault().Value,
                Role = identity.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value
            }); ;
        }
    }
}
