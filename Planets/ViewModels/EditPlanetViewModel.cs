using Planets.Models;

namespace Planets.ViewModels
{
    public class EditPlanetViewModel
    {
        public required Planet Planet { get; init; }

        public List<PlanetPropertyValue> PropertyValuesToAdd { get; init; } = [];
    }
}
