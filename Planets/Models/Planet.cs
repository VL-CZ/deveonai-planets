using System.ComponentModel.DataAnnotations.Schema;

namespace Planets.Models
{
    /// <summary>
    /// Represents a planet entity.
    /// </summary>
    public class Planet
    {
        /// <summary>
        /// Planet identifier.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the planet.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Property values associated with the planet.
        /// </summary>
        public virtual ICollection<PlanetPropertyValue> PropertyValues { get; set; } = [];
    }
}
