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
    public class OrdersController : Controller
    {
        private readonly SimpleShopDatabaseContext _context;

        public OrdersController(SimpleShopDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var simpleShopDatabaseContext = _context.Order.Include(o => o.Customer);
            return View(await simpleShopDatabaseContext.ToListAsync());
        }
        [User]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [User]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [User]
        public IActionResult EditSent(int id)
        {
            var order = _context.Order.Find(id);
            order.IsSent = !order.IsSent;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [User]
        public IActionResult EditPayed(int orderId,string paymentCode)
        {
            var order = _context.Order.Find(orderId);
            order.IsPayed = !order.IsPayed;
            order.PaymentCode = paymentCode==null?"":paymentCode;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
