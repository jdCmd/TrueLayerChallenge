namespace TrueLayerChallenge.WebApi.Dtos
{
    /// <summary>
    /// Data transfer object containing the Shakespearean description of a given pokemon.
    /// </summary>
    public class ShakespeareanPokemonDescriptionDto
    {
        /// <summary>
        /// Creates a new <see cref="ShakespeareanPokemonDescriptionDto"/>.
        /// </summary>
        /// <param name="name">Name of the pokemon for which the <see cref="ShakespeareanPokemonDescriptionDto"/> corresponds to.</param>
        /// <param name="description">Shakespearean description of the pokemon with <paramref name="name"/>.</param>
        public ShakespeareanPokemonDescriptionDto(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Name of the pokemon for which the <see cref="ShakespeareanPokemonDescriptionDto"/> corresponds to.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Shakespearean description of the pokemon.
        /// </summary>
        public string Description { get; set; }
    }
}
