using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AnimalDistributorService.Api.Commands;
using AnimalDistributorService.Api.Commands.Dtos;
using AnimalDistributorService.Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimalDistributorService.Controller
{
    [Route("api/[controller]")]
    public class AnimalController : ControllerBase
    {
        private readonly IMediator bus;

        public AnimalController(IMediator bus)
        {
            this.bus = bus;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> Post(Guid id)
        {
            var result = await bus.Send(new GetAnimalQuery { Id = id });
            return new JsonResult(result);
        }

        [Authorize(Policy = "IsSuperUser")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateAnimalCommandDto cmd)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var createAnimalCommand = new CreateAnimalCommand
            {
                Alias = cmd.Alias,
                AnimalType = cmd.AnimalType,
                Localization = cmd.Localization,
                CreatorId = Guid.Parse(identity.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value)
            };
            var result = await bus.Send(createAnimalCommand);
            return new JsonResult(result);
        }
    }
}
