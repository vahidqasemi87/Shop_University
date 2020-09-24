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
    public class ProductAttributesController : Controller
    {
        private readonly SimpleShopDatabaseContext _context;

        public ProductAttributesController(SimpleShopDatabaseContext context)
        {
            _context = context;
        }
        [User]
        public async Task<IActionResult> Index()
        {
            var simpleShopDatabaseContext = _context.ProductAttribute.Include(p => p.Unit);
            return View(await simpleShopDatabaseContext.ToListAsync());
        }
        [User]
        public IActionResult Create()
        {
            ViewData["UnitId"] = new SelectList(_context.Unit, "UnitId", "Name");
            return View();
        }
        [User]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductAttributeId,Name,UnitId")] ProductAttribute productAttribute)
        {
            productAttribute.Name = productAttribute.Name.Trim();
            if (ModelState.IsValid)
            {
                _context.Add(productAttribute);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UnitId"] = new SelectList(_context.Unit, "UnitId", "Name", productAttribute.UnitId);
            return View(productAttribute);
        }
        [User]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productAttribute = await _context.ProductAttribute.FindAsync(id);
            if (productAttribute == null)
            {
                return NotFound();
            }
            ViewData["UnitId"] = new SelectList(_context.Unit, "UnitId", "Name", productAttribute.UnitId);
            return View(productAttribute);
        }
        [User]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductAttributeId,Name,UnitId")] ProductAttribute productAttribute)
        {
            if (id != productAttribute.ProductAttributeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productAttribute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductAttributeExists(productAttribute.ProductAttributeId))
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
            ViewData["UnitId"] = new SelectList(_context.Unit, "UnitId", "Name", productAttribute.UnitId);
            return View(productAttribute);
        }
        [User]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productAttribute = await _context.ProductAttribute.FindAsync(id);
            _context.ProductAttribute.Remove(productAttribute);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductAttributeExists(int id)
        {
            return _context.ProductAttribute.Any(e => e.ProductAttributeId == id);
        }
        //public JsonResult IsAttributeValueName(string name)
        //{
        //    return Json(!_context.ProductAttribute.Any(c => c.Name == name.Trim()));
        //}
    }
}
