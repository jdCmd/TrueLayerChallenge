using Microsoft.AspNetCore.Mvc;
using TrueLayerChallenge.WebApi.Dtos;
using TrueLayerChallenge.WebApi.Resources;

namespace TrueLayerChallenge.WebApi.Controllers
{
    /// <summary>
    /// Controller for performing Pokemon related actions.
    /// </summary>
    public class PokemonController : ApiController<PokemonController>
    {
        public PokemonController(ILogger<PokemonController> logger) : base(logger) { }

        [HttpGet]
        [Route("{pokemonName}")]
        public ActionResult<ShakespeareanPokemonDescriptionDto> GetShakespeareanDescription(string pokemonName)
        {
            return PerformFunc<ShakespeareanPokemonDescriptionDto>(nameof(GetShakespeareanDescription), () =>
            {
                if (string.IsNullOrWhiteSpace(pokemonName)) throw new ArgumentException(UserMessages.Pokemon_Get_NameNotProvided, nameof(pokemonName));

                // for now just respond with something to enable setting up initial tests. 
                var dto = new ShakespeareanPokemonDescriptionDto(pokemonName, "some description");

                return Ok(dto);
            });
        }
    }
}