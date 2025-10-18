using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Planets.Data;
using Planets.Models;

namespace Planets.Controllers
{
    public class PlanetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlanetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Planets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Planets.ToListAsync());
        }

        // GET: Planets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planet = await _context.Planets
                .FirstOrDefaultAsync(m => m.Id == id);
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
                _context.Add(planet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

            var planet = await _context.Planets.FindAsync(id);
            if (planet == null)
            {
                return NotFound();
            }

            ViewData["Properties"] = await _context.PlanetPropertyValues.Include(v => v.PlanetProperty).OrderBy(v => v.PlanetProperty.Name).ToListAsync();

            return View(planet);
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
                try
                {
                    _context.Update(planet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanetExists(planet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
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

            var planet = await _context.Planets
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var planet = await _context.Planets.FindAsync(id);
            if (planet != null)
            {
                _context.Planets.Remove(planet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: PlanetProperties/AddValue/5
        [HttpPost, ActionName("AddProperty")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProperty(Guid id, [FromForm] Guid propertyValueId)
        {
            var planet = await _context.Planets.FindAsync(id);
            var propertyValue = await _context.PlanetPropertyValues.FindAsync(propertyValueId);

            if (planet is null || propertyValue is null)
            {
                return NotFound();
            }

            planet.PropertyValues.Add(propertyValue);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: PlanetProperties/Delete/5
        [HttpPost, ActionName("DeleteProperty")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProperty(Guid id, [FromForm] Guid propertyValueId)
        {
            var planet = await _context.Planets.FindAsync(id);
            var propertyValue = await _context.PlanetPropertyValues.FindAsync(propertyValueId);

            if (planet is null || propertyValue is null)
            {
                return NotFound();
            }

            planet.PropertyValues.Remove(propertyValue);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        private bool PlanetExists(Guid id)
        {
            return _context.Planets.Any(e => e.Id == id);
        }
    }
}