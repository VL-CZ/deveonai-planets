using System.ComponentModel.DataAnnotations.Schema;

namespace Planets.Models
{
    public class PlanetPropertyValue
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public required string Value { get; set; }

        public virtual PlanetProperty PlanetProperty { get; set; } = null!;
    }
}
