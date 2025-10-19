using Planets.Models;

namespace Planets.ViewModels
{
    /// <summary>
    /// View model for the 'Edit Planet' page.
    /// </summary>
    public class EditPlanetViewModel
    {
        /// <summary>
        /// The planet that is being edited.
        /// </summary>
        public required Planet Planet { get; init; }

        /// <summary>
        /// List of all property values that could be added to the planet.
        /// </summary>
        /// <remarks>
        /// NOTE: the already assigned property values aren't included in this list.
        /// </remarks>
        public List<PlanetPropertyValue> PropertyValuesToAdd { get; init; } = [];
    }
}
