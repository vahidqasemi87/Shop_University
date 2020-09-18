using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleShop.ActionFilters;
using SimpleShop.Models;

namespace SimpleShop.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly SimpleShopDatabaseContext _context;

        public OrderDetailsController(SimpleShopDatabaseContext context)
        {
            _context = context;
        }
        [User]
        [Route("OrderDetails/{id}")]
        public IActionResult Index(int id)
        {
            if (id != 0)
            {
                HttpContext.Session.SetInt32("Id", id);
            }
            else if (HttpContext.Session.Keys.Contains("Id"))
            {
                id = (int)HttpContext.Session.GetInt32("Id");
            }
            var orderDetails = _context.Order.Include(o=>o.OrderDetail).Where(o=>o.OrderId==id).First().OrderDetail.ToList();
            foreach (var orderDetail in orderDetails)
            {
                _context.Entry(orderDetail).Reference(o => o.Product).Load();
            }
            return View(orderDetails);
        }
        [User]
        public void Delete(int id)
        {
            var orderDetail =_context.OrderDetail.Find(id);
            int orderId = orderDetail.OrderId;
            _context.OrderDetail.Remove(orderDetail);
            _context.SaveChanges();
            HttpContext.Response.Redirect($"/orderdetails/{orderId}");
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetail.Any(e => e.OrderDetailId == id);
        }
    }
}
