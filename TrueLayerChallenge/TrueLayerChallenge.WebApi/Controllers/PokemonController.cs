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

        /// <summary>
        /// Gets the Shakespearean description for the pokemon with the given <paramref name="pokemonName"/>.
        /// </summary>
        /// <param name="pokemonName">Name of the pokemon for which to obtain the Shakespearean description for.</param>
        /// <returns>
        /// <see cref="OkObjectResult"/> on successfully determining the Shakespearean description for the given pokemon.
        /// -or-
        /// <see cref="BadRequestResult"/> if the given <paramref name="pokemonName"/> is null, empty or whitespace.
        /// -or-
        /// <see cref="NotFoundObjectResult"/> if failed to determine Shakespearean description for the given pokemon.
        /// Reasons include the given <paramref name="pokemonName"/> does not correspond to a real pokemon.
        /// -or-
        /// <see cref="StatusCodeResult"/> corresponding to an internal server error.
        /// </returns>
        [HttpGet]
        [Route("{pokemonName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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