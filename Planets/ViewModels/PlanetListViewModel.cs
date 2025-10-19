using Planets.Models;

namespace Planets.ViewModels
{
    public class PlanetListViewModel
    {
        public List<Planet> Planets { get; init; } = [];

        public List<PlanetProperty> AllPlanetProperties { get; init; } = [];

        public List<Guid> SelectedPropertyValueIds { get; init; } = [];
    }
}
