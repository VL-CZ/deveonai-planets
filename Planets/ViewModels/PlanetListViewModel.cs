using Planets.Models;

namespace Planets.ViewModels
{
    /// <summary>
    /// View model for the 'Planet List' page
    /// </summary>
    public class PlanetListViewModel
    {
        /// <summary>
        /// List of all planets.
        /// </summary>
        public List<Planet> Planets { get; init; } = [];

        /// <summary>
        /// List of all available planet properties.
        /// </summary>
        public List<PlanetProperty> AllPlanetProperties { get; init; } = [];

        /// <summary>
        /// List of the property value IDs (<see cref="PlanetPropertyValue.Id"/> ) that are currently selected in the filter.
        /// </summary>
        public List<Guid> SelectedPropertyValueIds { get; init; } = [];
    }
}
