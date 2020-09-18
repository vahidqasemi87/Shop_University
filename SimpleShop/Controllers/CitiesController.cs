using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleShop.ActionFilters;
using SimpleShop.Models;

namespace SimpleShop.Controllers
{
    public class CitiesController : Controller
    {
        private readonly SimpleShopDatabaseContext _context;

        public CitiesController(SimpleShopDatabaseContext context)
        {
            _context = context;
        }
        [User]
        public async Task<IActionResult> Index()
        {
            var simpleShopDatabaseContext = _context.City.Include(c => c.State);
            return View(await simpleShopDatabaseContext.ToListAsync());
        }
        [User]
        public IActionResult Create()
        {
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "Name");
            return View();
        }
        [User]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CityId,StateId,Name")] City city)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "Name", city.StateId);
            return View(city);
        }
        [User]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "Name", city.StateId);
            return View(city);
        }
        [User]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CityId,StateId,Name")] City city)
        {
            if (id != city.CityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.CityId))
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
            ViewData["StateId"] = new SelectList(_context.State, "StateId", "Name", city.StateId);
            return View(city);
        }
        [User]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var city = await _context.City.FindAsync(id);
            _context.City.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
            return _context.City.Any(e => e.CityId == id);
        }
    }
}
