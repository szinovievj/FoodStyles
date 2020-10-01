using System.Collections.Generic;
using FoodStyles.DTO;
using FoodStyles.Services;
using FoodStyles.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodStyles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScrapeController : ControllerBase
    {
        private readonly ILogger<ScrapeController> _logger;
        private readonly FoodService _foodService;

        public ScrapeController(ILogger<ScrapeController> logger, FoodService foodService)
        {
            _logger = logger;
            _foodService = foodService;
        }

        [HttpPost]
        public ActionResult<IEnumerable<MenuItem>> Post([FromBody] StartUrlDTO value)
        {
            _logger.LogInformation("Accepted request");
            return Ok (_foodService.ScrapeProcess(value.MenuUrl));
        }
    }
}