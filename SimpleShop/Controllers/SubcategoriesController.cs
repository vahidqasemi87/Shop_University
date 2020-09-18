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
    public class SubcategoriesController : Controller
    {
        private readonly SimpleShopDatabaseContext _context;

        public SubcategoriesController(SimpleShopDatabaseContext context)
        {
            _context = context;
        }
        [User]
        public async Task<IActionResult> Index()
        {
            var simpleShopDatabaseContext = _context.Subcategory.Include(c => c.Category);
            return View(await simpleShopDatabaseContext.ToListAsync());
        }
        [User]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name");
            return View();
        }
        [User]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubcategoryId,CategoryId,Name")] Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subcategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", subcategory.CategoryId);
            return View(subcategory);
        }
        [User]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subcategory = await _context.Subcategory.FindAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", subcategory.CategoryId);
            return View(subcategory);
        }
        [User]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubcategoryId,CategoryId,Name")] Subcategory subcategory)
        {
            if (id != subcategory.SubcategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subcategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubcategoryExists(subcategory.SubcategoryId))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name", subcategory.CategoryId);
            return View(subcategory);
        }
        [User]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var subcategory = await _context.Subcategory.FindAsync(id);
            _context.Subcategory.Remove(subcategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubcategoryExists(int id)
        {
            return _context.Subcategory.Any(e => e.SubcategoryId == id);
        }
    }
}
