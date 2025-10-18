using System.ComponentModel.DataAnnotations.Schema;

namespace Planets.Models
{
    public class Planet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public virtual ICollection<PlanetPropertyValue> PropertyValues { get; set; } = [];
    }
}
