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
    public class CustomersController : Controller
    {
        private readonly SimpleShopDatabaseContext _context;

        public CustomersController(SimpleShopDatabaseContext context)
        {
            _context = context;
        }
        [User]
        public async Task<IActionResult> Index()
        {
            var simpleShopDatabaseContext = _context.Customer.Include(c => c.City).Include(c=>c.City.State);
            return View(await simpleShopDatabaseContext.ToListAsync());
        }
    }
}
