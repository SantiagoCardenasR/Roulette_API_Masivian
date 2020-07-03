using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Roulette_API_Masivian.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using EasyCaching.Core;
using System.Runtime.InteropServices;
using ServiceStack.Text;

namespace Roullete_API_Masivian.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private Casino casino;
        private IEasyCachingProvider cachingProvider;
        private IEasyCachingProviderFactory cachingProviderFactory;
        //private readonly ILogger<WeatherForecastController> _logger;

        public RouletteController(IEasyCachingProviderFactory pCachingProviderFactory)
        {
            // _logger = logger;
            casino = new Casino();
            cachingProviderFactory = pCachingProviderFactory;
            cachingProvider = this.cachingProviderFactory.GetCachingProvider(name: "Roulette_API_Masivian");
        }
        [HttpGet(template:"Set")]
        public IActionResult SetItemInQueue()
        {
            this.cachingProvider.Set(cacheKey: "TestKey123", cacheValue: "Here is my value", expiration: TimeSpan.FromDays(1));
            return Ok();
        }
        [HttpGet]
        public IActionResult Get()
        {
            List<Roulette> allRoulettes = casino._roulettes;
            var result = this.cachingProvider.Get<String>(cacheKey: "TestKey123");
            return Ok(result.Value);
        }
        [HttpGet("buscar")]
        public IActionResult Get(int pId)
        {
            Roulette rouletteToFInd = casino.getRouletteById(pId);
            return Ok(rouletteToFInd);
        }
        [HttpPost("agregar")]
        public IActionResult addRoulette(Roulette pNewRoulette)
        {
            this.cachingProvider.Set(cacheKey: pNewRoulette.rouletteId.ToString(), cacheValue: pNewRoulette, expiration: TimeSpan.FromDays(5));
            return Ok("Ruleta creada con id:" + pNewRoulette.rouletteId);
        }
    }
}
