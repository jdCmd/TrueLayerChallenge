using Microsoft.AspNetCore.Mvc;
using TrueLayerChallenge.WebApi.Dtos;
using TrueLayerChallenge.WebApi.Resources;
using TrueLayerChallenge.WebApi.Services.Interfaces;

namespace TrueLayerChallenge.WebApi.Controllers
{
    /// <summary>
    /// Controller for performing Pokemon related actions.
    /// </summary>
    public class PokemonController : ApiController<PokemonController>
    {
        private readonly IPokemonService _pokemonService;

        /// <summary>
        /// Creates a new <see cref="PokemonController"/>.
        /// </summary>
        /// <param name="logger"><see cref="ILogger{PokemonController}"/> providing logging functionality.</param>
        /// <param name="pokemonService"><see cref="IPokemonService"/> used for performing pokemon related operations.</param>
        /// <inheritdoc cref="ApiController{TController}"/>.
        public PokemonController(ILogger<PokemonController> logger, IPokemonService pokemonService) : base(logger)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet]
        [Route("{pokemonName}")]
        public async Task<ActionResult<ShakespeareanPokemonDescriptionDto>> GetShakespeareanDescription(string pokemonName)
        {
            return await PerformFuncAsync<ShakespeareanPokemonDescriptionDto>(nameof(GetShakespeareanDescription), async () =>
            {
                if (string.IsNullOrWhiteSpace(pokemonName)) throw new ArgumentException(UserMessages.Pokemon_Get_NameNotProvided, nameof(pokemonName));

                var dto = await _pokemonService.GetShakespeareanDescriptionAsync(pokemonName);

                if (dto == null) return NotFound(UserMessages.Pokemon_Get_NotFound);

                return Ok(dto);
            });
        }
    }
}