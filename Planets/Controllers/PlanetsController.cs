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

        // GET: Planets
        public async Task<IActionResult> Index()
        {
            var stringIds = Request.Query.Select(x => x.Value.First());

            List<Guid> guids = stringIds
                .Select(s =>
                {
                    _ = Guid.TryParse(s, out var g);
                    return g;
                })
                .Where(g => g != Guid.Empty)
                .ToList();

            var planets = new List<Planet>();

            if (guids.Any())
            {
                planets = await _dbContext.Planets
                    .Where(p => guids.All(id => p.PropertyValues.Any(val => val.Id == id)))
                    .ToListAsync();
            }
            else
            {
                planets = await _dbContext.Planets.ToListAsync();
            }

            var viewModel = new PlanetListViewModel
            {
                Planets = planets,
                AllPlanetProperties = await _dbContext.PlanetProperties.Include(v => v.PossibleValues).OrderBy(v => v.Name).ToListAsync(),
                SelectedPropertyValueIds = guids
            };

            return View(viewModel);
        }

        // GET: Planets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planet = await _dbContext.Planets
                .FindAsync(id);

            if (planet == null)
            {
                return NotFound();
            }

            return View(planet);
        }

        // GET: Planets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Planets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Planet planet)
        {
            if (ModelState.IsValid)
            {
                planet.Id = Guid.NewGuid();
                _dbContext.Add(planet);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = planet.Id });
            }
            return View(planet);
        }

        // GET: Planets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planet = await _dbContext.Planets.FindAsync(id);
            if (planet == null)
            {
                return NotFound();
            }

            var viewModel = new EditPlanetViewModel
            {
                Planet = planet,
                PropertyValuesToAdd = await _dbContext.PlanetPropertyValues.Include(v => v.PlanetProperty).OrderBy(v => v.PlanetProperty.Name).ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Planets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Planets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planet = await _dbContext.Planets
                .FindAsync(id);

            if (planet == null)
            {
                return NotFound();
            }

            return View(planet);
        }

        // POST: Planets/Delete/5
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

        // POST: PlanetProperties/AddValue/5
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

        // POST: PlanetProperties/Delete/5
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