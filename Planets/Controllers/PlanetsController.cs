using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planets.Data;
using Planets.Models;
using Planets.ViewModels;

namespace Planets.Controllers
{
    public class PlanetsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public PlanetsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 'GET: /Planets' - Displays list of all planets. The query params serve for filtering, 
        /// the following format is used: (key - <see cref="PlanetProperty.Name"/>, value - <see cref="PlanetPropertyValue.Id"/>)
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var propertyValueIds = Request.Query.SelectMany(param => param.Value);

            var propertyValueGuids = propertyValueIds
                .Select(s =>
                {
                    _ = Guid.TryParse(s, out var g);
                    return g;
                })
                .Where(g => g != Guid.Empty)
                .ToList();

            var planets = new List<Planet>();

            if (propertyValueGuids.Any()) // filtering -> select only matching planets
            {
                planets = await _dbContext.Planets
                    .Where(p => propertyValueGuids.All(valueId => p.PropertyValues.Any(val => val.Id == valueId)))
                    .ToListAsync();
            }
            else // no filtering -> return all planets
            {
                planets = await _dbContext.Planets.ToListAsync();
            }

            var viewModel = new PlanetListViewModel
            {
                Planets = planets,
                AllPlanetProperties = await _dbContext.PlanetProperties.Include(v => v.PossibleValues).OrderBy(v => v.Name).ToListAsync(),
                SelectedPropertyValueIds = propertyValueGuids
            };

            return View(viewModel);
        }

        /// <summary>
        /// 'GET: Planets/Details/{id}' - Displays planet detail.
        /// </summary>
        /// <param name="id">ID of the planet to get.</param>
        public async Task<IActionResult> Details(Guid id)
        {
            var planet = await _dbContext.Planets
                .FindAsync(id);

            if (planet == null)
            {
                return NotFound();
            }

            return View(planet);
        }

        /// <summary>
        /// 'GET: Planets/Create' - Displays the 'create planet' page.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 'POST: Planets/Create' - Creates a planet and stores it into the DB.
        /// </summary>
        /// <param name="planet">The planet data to store.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Planet planet)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(planet);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = planet.Id });
            }
            return View(planet);
        }

        /// <summary>
        /// 'GET: Planets/Edit/5' - Displays the 'edit planet' page.
        /// </summary>
        /// <param name="id">ID of the planet to edit.</param>
        public async Task<IActionResult> Edit(Guid id)
        {
            var planet = await _dbContext.Planets.FindAsync(id);

            if (planet == null)
            {
                return NotFound();
            }

            var addedPropertyValues = planet.PropertyValues.Select(v => v.Id).ToList();

            var propertyValuesToAdd = await _dbContext.PlanetPropertyValues
                .Include(v => v.PlanetProperty)
                .Where(v => !addedPropertyValues.Contains(v.Id)) // exclude the already applied property values
                .OrderBy(v => v.PlanetProperty.Name)
                .ToListAsync();

            var viewModel = new EditPlanetViewModel
            {
                Planet = planet,
                PropertyValuesToAdd = propertyValuesToAdd
            };

            return View(viewModel);
        }

        /// <summary>
        /// 'POST: Planets/Edit/{id}' - Updates an existing planet and saves the changes to the DB.
        /// </summary>
        /// <param name="id">ID of the planet to edit.</param>
        /// <param name="planet">The updated planet data.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Planet planet)
        {
            if (id != planet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _dbContext.Update(planet);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id });
            }
            return View(planet);
        }


        /// <summary>
        /// 'GET: Planets/Delete/{id}' - Displays the 'delete planet' confirmation page.
        /// </summary>
        /// <param name="id">ID of the planet to delete.</param>
        public async Task<IActionResult> Delete(Guid id)
        {
            var planet = await _dbContext.Planets
                .FindAsync(id);

            if (planet == null)
            {
                return NotFound();
            }

            return View(planet);
        }


        /// <summary>
        /// 'POST: Planets/Delete/{id}' - Confirms and performs deletion of the specified planet from the DB.
        /// </summary>
        /// <param name="id">ID of the planet to delete.</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var planet = await _dbContext.Planets.FindAsync(id);
            if (planet != null)
            {
                _dbContext.Planets.Remove(planet);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// 'POST: PlanetProperties/AddProperty/{id}' - adds a property value to the given planet.
        /// </summary>
        /// <param name="id">ID of the planet, to which the property value is added.</param>
        /// <param name="propertyValueId">ID of the property value being added.</param>
        [HttpPost, ActionName("AddProperty")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProperty(Guid id, [FromForm] Guid propertyValueId)
        {
            var planet = await _dbContext.Planets.FindAsync(id);
            var propertyValue = await _dbContext.PlanetPropertyValues.FindAsync(propertyValueId);

            if (planet is null || propertyValue is null)
            {
                return NotFound();
            }

            planet.PropertyValues.Add(propertyValue);

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        /// <summary>
        /// 'POST: PlanetProperties/Delete/{id}' - removes a property value from the given planet.
        /// </summary>
        /// <param name="id">ID of the planet, from which the property value is removed.</param>
        /// <param name="propertyValueId">ID of the property value being removed.</param>
        [HttpPost, ActionName("DeleteProperty")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProperty(Guid id, [FromForm] Guid propertyValueId)
        {
            var planet = await _dbContext.Planets.FindAsync(id);
            var propertyValue = await _dbContext.PlanetPropertyValues.FindAsync(propertyValueId);

            if (planet is null || propertyValue is null)
            {
                return NotFound();
            }

            planet.PropertyValues.Remove(propertyValue);

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}