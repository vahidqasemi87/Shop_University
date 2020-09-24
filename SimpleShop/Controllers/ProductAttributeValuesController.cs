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
    public class ProductAttributeValuesController : Controller
    {
        private readonly SimpleShopDatabaseContext _context;

        public ProductAttributeValuesController(SimpleShopDatabaseContext context)
        {
            _context = context;
        }
        [User]
        [Route("ProductAttributeValues/{id}")]
        public IActionResult Index(int id)
        {
            ViewData["ProductAttributes"] = _context.ProductAttribute.OrderBy(p=>p.Name).ToList();
            var product = _context.Product.Include(p => p.ProductAttributeValue).Where(p=>p.ProductId==id).First();
            foreach (var productAttribute in product.ProductAttributeValue.Select(p=>p.ProductAttribute))
            {
                _context.Entry(productAttribute).Reference(p => p.Unit).Load();
            }
            return View(product);
        }
        [User]
        [HttpPost]
        public void Index(int productId,int productAttributeId,string value)
        {
            var productAttributeValue = new ProductAttributeValue();
            productAttributeValue.ProductId = productId;
            productAttributeValue.ProductAttributeId = productAttributeId;
            productAttributeValue.Value = value;
            _context.ProductAttributeValue.Add(productAttributeValue);
            _context.SaveChanges();
            HttpContext.Response.Redirect($"/productattributevalues/{productId}");
        }

        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name");
            ViewData["ProductAttributeId"] = new SelectList(_context.ProductAttribute, "ProductAttributeId", "Name");
            return View();
        }
        [User]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductAttributeValueId,ProductId,ProductAttributeId,Value")] ProductAttributeValue productAttributeValue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productAttributeValue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name", productAttributeValue.ProductId);
            ViewData["ProductAttributeId"] = new SelectList(_context.ProductAttribute, "ProductAttributeId", "Name", productAttributeValue.ProductAttributeId);
            return View(productAttributeValue);
        }
        [User]
        [HttpPost]
        public void Edit(int productAttributeValueId,int productAttributeId,string value)
        {
            var productAttributeValue = _context.ProductAttributeValue.Find(productAttributeValueId);
            productAttributeValue.ProductAttributeId = productAttributeId;
            productAttributeValue.Value = value;
            _context.SaveChanges();
            HttpContext.Response.Redirect($"/productattributevalues/{productAttributeValue.ProductId}");
        }
        [User]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productAttributeValue = await _context.ProductAttributeValue.FindAsync(id);
            int productId = productAttributeValue.ProductId;
            _context.ProductAttributeValue.Remove(productAttributeValue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index),new { id=productId});
        }

        private bool ProductAttributeValueExists(int id)
        {
            return _context.ProductAttributeValue.Any(e => e.ProductAttributeValueId == id);
        }

    }
}
