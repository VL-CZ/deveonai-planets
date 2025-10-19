using System.ComponentModel.DataAnnotations.Schema;

namespace Planets.Models
{
    /// <summary>
    /// Represents a value corresponding to a specific <see cref="PlanetProperty"/>.
    /// </summary>
    public class PlanetPropertyValue
    {
        /// <summary>
        /// Identifier of the planet property value.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Value of the planet property.
        /// </summary>
        public required string Value { get; set; }

        /// <summary>
        /// The associated planet property.
        /// </summary>
        public virtual PlanetProperty PlanetProperty { get; set; } = null!;

        /// <summary>
        /// List of planets that have this property value.
        /// </summary>
        public virtual ICollection<Planet> Planets { get; set; } = [];
    }
}
