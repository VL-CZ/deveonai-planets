using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planets.Data;
using Planets.Models;

namespace Planets.Controllers
{
    public class PlanetPropertiesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public PlanetPropertiesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 'GET: /PlanetProperties' - Displays a list of all planet properties.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.PlanetProperties.ToListAsync());
        }


        /// <summary>
        /// 'GET: PlanetProperties/Details/{id}' - Displays details of a specific planet property, including its possible values.
        /// </summary>
        /// <param name="id">ID of the planet property to display.</param>
        public async Task<IActionResult> Details(Guid id)
        {
            var planetProperty = await _dbContext.PlanetProperties
                .Include(p => p.PossibleValues)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (planetProperty == null)
            {
                return NotFound();
            }

            return View(planetProperty);
        }


        /// <summary>
        /// 'GET: PlanetProperties/Create' - Displays the 'create planet property' page.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// 'POST: PlanetProperties/Create' - Creates a new planet property and stores it in the DB.
        /// </summary>
        /// <param name="planetProperty">The planet property data to store.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] PlanetProperty planetProperty)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(planetProperty);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = planetProperty.Id });
            }
            return View(planetProperty);
        }


        /// <summary>
        /// 'GET: PlanetProperties/Edit/{id}' - Displays the 'edit planet property' page for the specified property.
        /// </summary>
        /// <param name="id">ID of the planet property to edit.</param>
        public async Task<IActionResult> Edit(Guid id)
        {
            var planetProperty = await _dbContext.PlanetProperties.FindAsync(id);
            if (planetProperty == null)
            {
                return NotFound();
            }
            return View(planetProperty);
        }

        /// <summary>
        /// 'POST: PlanetProperties/Edit/{id}' - Updates an existing planet property and saves the changes to the DB.
        /// </summary>
        /// <param name="id">ID of the planet property to edit.</param>
        /// <param name="planetProperty">The updated planet property data.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] PlanetProperty planetProperty)
        {
            if (id != planetProperty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _dbContext.Update(planetProperty);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id });
            }

            return View(planetProperty);
        }


        /// <summary>
        /// 'GET: PlanetProperties/Delete/{id}' - Displays the 'delete planet property' confirmation page.
        /// </summary>
        /// <param name="id">ID of the planet property to delete.</param>
        public async Task<IActionResult> Delete(Guid id)
        {
            var planetProperty = await _dbContext.PlanetProperties
                .FindAsync(id);

            if (planetProperty == null)
            {
                return NotFound();
            }

            return View(planetProperty);
        }


        /// <summary>
        /// 'POST: PlanetProperties/Delete/{id}' - Confirms and performs deletion of the specified planet property from the DB.
        /// </summary>
        /// <param name="id">ID of the planet property to delete.</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var planetProperty = await _dbContext.PlanetProperties.FindAsync(id);
            if (planetProperty != null)
            {
                _dbContext.PlanetProperties.Remove(planetProperty);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// 'POST: PlanetProperties/AddValue/{id}' - Adds a new property value to the specified planet property.
        /// </summary>
        /// <param name="id">ID of the planet property, to which the property value is added.</param>
        /// <param name="propertyValue">The property value to add.</param>
        [HttpPost, ActionName("AddValue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPropertyValue(Guid id, [Bind("Value")] PlanetPropertyValue propertyValue)
        {
            var planetProperty = await _dbContext.PlanetProperties.FindAsync(id);

            if (planetProperty is null)
            {
                return NotFound();
            }

            planetProperty.PossibleValues.Add(propertyValue);

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        /// <summary>
        /// 'POST: PlanetProperties/DeleteValue/{id}' - Deletes the specified planet property value.
        /// </summary>
        /// <param name="id">ID of the planet property value to delete.</param>
        [HttpPost, ActionName("DeleteValue")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePropertyValue(Guid id)
        {
            var propertyValue = await _dbContext.PlanetPropertyValues.FindAsync(id);

            if (propertyValue is null)
            {
                return NotFound();
            }

            var propertyId = propertyValue.PlanetProperty.Id;
            _dbContext.PlanetPropertyValues.Remove(propertyValue);

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = propertyId });
        }
    }
}
