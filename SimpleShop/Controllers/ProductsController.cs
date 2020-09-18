using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleShop.Models;
using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using ImageProcessor.Common;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SimpleShop.ActionFilters;

namespace SimpleShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly SimpleShopDatabaseContext _context;
        private readonly IHostEnvironment _hostEnvironment;
        public ProductsController(SimpleShopDatabaseContext context,IHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        [User]
        public async Task<IActionResult> Index()
        {
            var simpleShopDatabaseContext = _context.Product.Include(p => p.Subcategory);
            return View(await simpleShopDatabaseContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategory, "SubcategoryId", "Name");
            return View();
        }
        [User]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,SubcategoryId,Name,UnitPrice")] Product product,IFormFile photoFile)
        {
            if (photoFile!=null)
            {
                string photoFilename = $"{Guid.NewGuid().ToString()}.jpg";
                MemoryStream stream = new MemoryStream();
                photoFile.CopyTo(stream);
                new ImageFactory().Load(stream.GetBuffer())
                    .Resize(new Size(640,640))
                    .Format(new JpegFormat())
                    .Save($"{_hostEnvironment.ContentRootPath}\\wwwroot\\files\\productphotos\\{photoFilename}");
                product.PhotoFilename = photoFilename;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategory, "SubcategoryId", "Name", product.SubcategoryId);
            return View(product);
        }
        [User]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategory, "SubcategoryId", "Name", product.SubcategoryId);
            return View(product);
        }
        [User]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,SubcategoryId,Name,UnitPrice,PhotoFilename")] Product product,IFormFile photoFile)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (photoFile!=null)
                    {
                        string photoFilename = $"{Guid.NewGuid().ToString()}.jpg";
                        MemoryStream stream = new MemoryStream();
                        photoFile.CopyTo(stream);
                        new ImageFactory().Load(stream.GetBuffer())
                            .Resize(new Size(800, 0))
                            .Format(new JpegFormat())
                            .Save($"{_hostEnvironment.ContentRootPath}\\wwwroot\\files\\productphotos\\{photoFilename}");
                        product.PhotoFilename = photoFilename;
                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["SubcategoryId"] = new SelectList(_context.Subcategory, "SubcategoryId", "Name", product.SubcategoryId);
            return View(product);
        }
        [User]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
