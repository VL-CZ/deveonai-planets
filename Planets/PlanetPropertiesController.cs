using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Planets.Data;
using Planets.Models;

namespace Planets
{
    public class PlanetPropertiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlanetPropertiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlanetProperties
        public async Task<IActionResult> Index()
        {
            return View(await _context.PlanetProperties.ToListAsync());
        }

        // GET: PlanetProperties/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planetProperty = await _context.PlanetProperties
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
                _context.Add(planetProperty);
                await _context.SaveChangesAsync();
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

            var planetProperty = await _context.PlanetProperties.FindAsync(id);
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
                    _context.Update(planetProperty);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
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

            var planetProperty = await _context.PlanetProperties
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
            var planetProperty = await _context.PlanetProperties.FindAsync(id);
            if (planetProperty != null)
            {
                _context.PlanetProperties.Remove(planetProperty);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanetPropertyExists(Guid id)
        {
            return _context.PlanetProperties.Any(e => e.Id == id);
        }
    }
}
