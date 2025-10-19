using System.ComponentModel.DataAnnotations.Schema;

namespace Planets.Models
{
    /// <summary>
    /// Represents a property that a planet can have.
    /// </summary>
    public class PlanetProperty
    {
        /// <summary>
        /// Planet property identifier.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the planet property (e.g. HasAtmosphere).
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Possible values for this planet property.
        /// </summary>
        public virtual ICollection<PlanetPropertyValue> PossibleValues { get; } = [];
    }
}
