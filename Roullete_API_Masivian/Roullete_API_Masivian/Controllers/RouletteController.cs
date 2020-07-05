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
        public List<Roulette> roulettes;
        public List<Player> players;
        private IEasyCachingProvider cachingProvider;
        private IEasyCachingProviderFactory cachingProviderFactory;
        //private readonly ILogger<WeatherForecastController> _logger;
        public RouletteController(IEasyCachingProviderFactory pCachingProviderFactory)
        {
            // _logger = logger;
            cachingProviderFactory = pCachingProviderFactory;
            cachingProvider = this.cachingProviderFactory.GetCachingProvider(name: "Roulette_API_Masivian");
            roulettes = new List<Roulette>();
            players = new List<Player>();
            if (this.cachingProvider.Get<List<Roulette>>("Roulettes").Value == null)
            {
                this.cachingProvider.Set(cacheKey: "Roulettes", cacheValue: roulettes, expiration: TimeSpan.FromDays(5));
            }
            if (this.cachingProvider.Get<List<Player>>("Players").Value == null)
            {
                this.cachingProvider.Set(cacheKey: "Players", cacheValue: players, expiration: TimeSpan.FromDays(5));
            }
        }
        [HttpGet]
        public IActionResult getAllRoulettes()
        {
            var actualRoulettes = cachingProvider.Get<List<Roulette>>("Roulettes");
            String roulettesSummary = "";
            if (actualRoulettes.Value != null)
            {
                for (int i = 0; i < actualRoulettes.Value.Count; i++)
                {
                    Roulette actualRoulette = actualRoulettes.Value[i];
                    roulettesSummary += "\n id de ruleta: " + actualRoulette.rouletteId + "\n ¿la ruleta está activa?: " + actualRoulette.isActive;
                }
                return Ok("Bienvenido a Good Charm Casino, aquí puedes crear una ruleta y apostar. \n Ruletas actuales: " + roulettesSummary);
            }
            else
            {
                return Ok(roulettesSummary);
            }

        }
        [HttpGet("Ruleta{id}")]
        public IActionResult getRouletteById(long id)
        {
            var rouletteToFInd = findRoulette(id);
            return Ok("Bienvenido, te encuentras en la ruleta con Id: " + rouletteToFInd.rouletteId + "\n" + " Las apuestas actuales son: \n" + actualBetsInRoulette(rouletteToFInd));
        }
        [HttpGet("jugador{id}")]
        public IActionResult getPlayerById(long id)
        {
            Player playerToFind = findPlayer(id);
            return Ok("Jugador con Id: " + id + "\n Nombre de usuario: " + playerToFind.screenName + "\n Cantidad de créditos disponibles: $" + playerToFind.playerCredits);
        }
        [HttpGet("abrirRuleta{id}")]
        public IActionResult openRoulette(long id)
        {
            String response = "";
            List<Roulette> actualRoulettes = cachingProvider.Get<List<Roulette>>("Roulettes").Value;
            for (int i = 0; i < actualRoulettes.Count; i++)
            {
                Roulette actualRoulette = actualRoulettes[i];
                if (actualRoulette.rouletteId == id && actualRoulette.isActive == false)
                {
                    actualRoulette.isActive = true;
                    this.cachingProvider.Set(cacheKey: "Roulettes", cacheValue: actualRoulettes, expiration: TimeSpan.FromDays(5));
                    response += "La ruleta con id: " + id + " ha sido abierta exitosamente";
                }
                else
                {
                    if (actualRoulette.rouletteId == id && actualRoulette.isActive == true)
                    {
                        response += "La ruleta con id: " + id + " ya está abierta y lista para recibir apuestas";
                    }
                }
            }
            return Ok(response);
        }
        [HttpGet("cerrarRuleta{id}")]
        public IActionResult closeRoulette(long id)
        {
            String response = "";
            List<Roulette> actualRoulettes = cachingProvider.Get<List<Roulette>>("Roulettes").Value;
            response += "La ruleta con id: " + id + "Ha sido cerrada exitosamente, ya no se aceptarán más apuestas. Las apuestas realizadas son: \n";
            for (int i = 0; i < actualRoulettes.Count; i++)
            {
                Roulette actualRoulette = actualRoulettes[i];
                if (actualRoulette.rouletteId == id && actualRoulette.isActive == true)
                {
                    actualRoulette.isActive = false;
                    this.cachingProvider.Set(cacheKey: "Roulettes", cacheValue: actualRoulettes, expiration: TimeSpan.FromDays(5));
                    response += actualBetsInRoulette(actualRoulette);
                }
                else
                {
                    if (actualRoulette.rouletteId == id && actualRoulette.isActive == false)
                    {
                        response += "La ruleta con id: " + id + " ya está abierta y lista para recibir apuestas";
                        return Ok(response);
                    }
                }
            }
            return Ok(response);
        }
        [HttpPost("agregarRuleta")]
        public IActionResult addRoulette(Roulette pNewRoulette)
        {
            List<Roulette> actualListOfRoulettes = cachingProvider.Get<List<Roulette>>("Roulettes").Value;
            actualListOfRoulettes.Add(pNewRoulette);
            this.cachingProvider.Set(cacheKey: "Roulettes", cacheValue: actualListOfRoulettes, expiration: TimeSpan.FromDays(5));
            return Ok("Ruleta creada con id:" + pNewRoulette.rouletteId);
        }
        [HttpPost("agregarJugador")]
        public IActionResult addPlayer(Player pNewPlayer)
        {
            players = this.cachingProvider.Get<List<Player>>("Players").Value;
            players.Add(pNewPlayer);
            this.cachingProvider.Set(cacheKey: "Players", cacheValue: players, expiration: TimeSpan.FromDays(5));
            return Ok("Jugador creado con id:" + pNewPlayer.playerId + "\n" + "Nickname del jugador: " + pNewPlayer.screenName + "\n Cantidad de créditos: " + pNewPlayer.playerCredits);
        }
        [HttpPost("agregarApuesta")]
        public IActionResult addBet(Bet pNewBet)
        {
            Player actualPlayer = findPlayer(pNewBet.playerId);
            Roulette actualRoulette = findRoulette(pNewBet.rouletteId);
            roulettes = this.cachingProvider.Get<List<Roulette>>("Roulettes").Value;
            players = this.cachingProvider.Get<List<Player>>("Players").Value;
            if (pNewBet.quantityToBet <= 10000 && actualPlayer.playerCredits > 0 && actualPlayer.playerCredits >= pNewBet.quantityToBet && actualRoulette.isActive == true && pNewBet.numberToBet <= 36)
            {
                int newCredits = actualPlayer.playerCredits - pNewBet.quantityToBet;
                actualRoulette.bets.Add(pNewBet);
                roulettes.Add(actualRoulette);
                this.cachingProvider.Set(cacheKey: "Roulettes", cacheValue: roulettes, expiration: TimeSpan.FromDays(5));
                actualPlayer.playerCredits = newCredits;
                players.Add(actualPlayer);
                this.cachingProvider.Set(cacheKey: "Players", cacheValue: players, expiration: TimeSpan.FromDays(5));
                return Ok("Apuesta creada con id:" + pNewBet.betId + "\n" + "Cantidad de Dinero apostado: " + pNewBet.quantityToBet + " La cantidad fue debidata de sus créditos");
            }
            else
            {
                return Ok("La apuesta no es posible de hacerse debido a que haz realizado una apuesta mayor a 10000 us$, debido a que no tienes suficiente balance en tu cuenta, hiciste una apuesta a un número inválido o la ruleta está cerrada");
            }
        }
        public String actualBetsInRoulette(Roulette actualRoulette)
        {
            List<Bet> actualBets = actualRoulette.bets;
            String betsToDisplay = "";
            if (actualBets.Count > 0)
            {
                for (int i = 0; i < actualBets.Count; i++)
                {
                    Bet actualBet = actualBets[i];
                    betsToDisplay += "Id de apuesta " + actualBet.betId + "\n Id de ruleta: " + actualBet.rouletteId + "\n Id de jugardor que realizó la apuesta: " + actualBet.playerId + "\n Cantidad de Dinero apostado: " + actualBet.quantityToBet + "\n Color al cual se apostó: " + actualBet.colorToBet + "\n";
                }
            }
            return betsToDisplay;
        }
        public Player findPlayer(long id)
        {
            Player respose = null;
            List<Player> actualPlayers = this.cachingProvider.Get<List<Player>>("Players").Value;
            for (int i=0;i< actualPlayers.Count;i++)
            {
                Player actualPlayer = actualPlayers[i];
                if (actualPlayer.playerId == id)
                {
                    respose = actualPlayer;
                }
            }
            return respose;
        }
        public Roulette findRoulette(long id)
        {
            Roulette respose = null;
            List<Roulette> actualRoulettes = this.cachingProvider.Get<List<Roulette>>("Roulettes").Value;
            for (int i = 0; i < actualRoulettes.Count; i++)
            {
                Roulette actualRoulette = actualRoulettes[i];
                if (actualRoulette.rouletteId == id)
                {
                    respose = actualRoulette;
                }
            }
            return respose;
        }
    }
}
