using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planets.Models
{
    public class PlanetProperty
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public virtual ICollection<PlanetPropertyValue> PossibleValues { get; } = [];
    }
}
