using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // GET: PlanetProperties
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.PlanetProperties.ToListAsync());
        }

        // GET: PlanetProperties/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planetProperty = await _dbContext.PlanetProperties
                .Include(p => p.PossibleValues)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (planetProperty == null)
            {
                return NotFound();
            }

            return View(planetProperty);
        }

        // GET: PlanetProperties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlanetProperties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] PlanetProperty planetProperty)
        {
            if (ModelState.IsValid)
            {
                planetProperty.Id = Guid.NewGuid();
                _dbContext.Add(planetProperty);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(planetProperty);
        }

        // GET: PlanetProperties/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planetProperty = await _dbContext.PlanetProperties.FindAsync(id);
            if (planetProperty == null)
            {
                return NotFound();
            }
            return View(planetProperty);
        }

        // POST: PlanetProperties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                try
                {
                    _dbContext.Update(planetProperty);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanetPropertyExists(planetProperty.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id });
            }
            return View(planetProperty);
        }

        // GET: PlanetProperties/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planetProperty = await _dbContext.PlanetProperties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planetProperty == null)
            {
                return NotFound();
            }

            return View(planetProperty);
        }

        // POST: PlanetProperties/Delete/5
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

        // POST: PlanetProperties/AddValue/5
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

        // POST: PlanetProperties/Delete/5
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

        private bool PlanetPropertyExists(Guid id)
        {
            return _dbContext.PlanetProperties.Any(e => e.Id == id);
        }
    }
}
