using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet]
        public IEnumerable<MenuItem> Get()
        {
            
            return _foodService.GetAll();
        }

        [HttpPost]
        public IEnumerable<MenuItem> Post([FromBody] StartUrlDTO value)
        {
            return _foodService.ScrapeProcess(value.MenuUrl);
        }
    }
}