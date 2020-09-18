using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SimpleShop.ActionFilters;
using SimpleShop.Models;

namespace SimpleShop.Controllers
{
    public class ProductPhotoesController : Controller
    {
        private readonly SimpleShopDatabaseContext _context;
        private readonly IHostEnvironment _hostEnvironment;

        public ProductPhotoesController(SimpleShopDatabaseContext context, IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
        }
        [User]
        [Route("ProductPhotoes/{id}")]
        public IActionResult Index(int id)
        {
            if (id!=0)
            {
                HttpContext.Session.SetInt32("Id", id);
            }
            else if(HttpContext.Session.Keys.Contains("Id"))
            {
                id = (int)HttpContext.Session.GetInt32("Id");
            }
            var product = _context.Product.Include(p => p.ProductPhoto).Where(p => p.ProductId == id).First();
            return View(product);
        }
        [User]
        [HttpPost]
        public void Index(int productId, IFormFile[] photoFiles)
        {
            if (productId != 0)
            {
                HttpContext.Session.SetInt32("Id", productId);
            }
            else if (HttpContext.Session.Keys.Contains("Id"))
            {
                productId = (int)HttpContext.Session.GetInt32("Id");
            }
            if (photoFiles != null)
            {
                foreach (var photoFile in photoFiles)
                {
                    var productPhoto = new ProductPhoto();
                    string photoFilename = $"{Guid.NewGuid().ToString()}.jpg";
                    MemoryStream stream = new MemoryStream();
                    photoFile.CopyTo(stream);
                    new ImageFactory().Load(stream.GetBuffer())
                        .Resize(new Size(721, 466))
                        .Format(new JpegFormat())
                        .Save($"{_hostEnvironment.ContentRootPath}\\wwwroot\\files\\productphotos\\{photoFilename}");
                    productPhoto.PhotoFilename = photoFilename;
                    productPhoto.ProductId = productId;
                    _context.ProductPhoto.Add(productPhoto);
                }
                _context.SaveChanges();
            }
            HttpContext.Response.Redirect($"/productphotoes/{productId}");
        }
        [User]
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name");
            return View();
        }
        [User]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductPhotoId,ProductId,PhotoFilename")] ProductPhoto productPhoto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productPhoto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name", productPhoto.ProductId);
            return View(productPhoto);
        }
        [User]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productPhoto = await _context.ProductPhoto.FindAsync(id);
            int productId = productPhoto.ProductId;
            _context.ProductPhoto.Remove(productPhoto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = productId });
        }

        private bool ProductPhotoExists(int id)
        {
            return _context.ProductPhoto.Any(e => e.ProductPhotoId == id);
        }
    }
}
