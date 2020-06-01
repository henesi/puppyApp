using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AnimalSearchService.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AnimalSearchService.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IAnimalRepository _animalrepository;

        public SearchController(IAnimalRepository repository)
        {
            this._animalrepository = repository;
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<ActionResult> GetAll([FromQuery] string alias, [FromQuery] string country, [FromQuery] string city, [FromQuery] int animalType)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var result = await _animalrepository.Find(alias, country, city, animalType, "");
            return new JsonResult(result);
        }

        [Authorize]
        [HttpGet("mine")]
        public async Task<ActionResult> GetMine([FromQuery] string alias, [FromQuery] string country, [FromQuery] string city, [FromQuery] int animalType)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var creatorId = identity.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var result = await _animalrepository.Find(alias, country, city, animalType, creatorId);
            return new JsonResult(result);
        }
    }
}
