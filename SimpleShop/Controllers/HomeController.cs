using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleShop.ActionFilters;
using SimpleShop.Helpers;
using SimpleShop.Models;

namespace SimpleShop.Controllers
{
	public class HomeController : Controller
	{
		private readonly SimpleShopDatabaseContext _db;
		private readonly HashHelper _hashHelper;
		private readonly BinaryHelper _binaryHelper;

		public HomeController(SimpleShopDatabaseContext db, HashHelper hashHelper, BinaryHelper binaryHelper)
		{
			_db = db;
			_hashHelper = hashHelper;
			_binaryHelper = binaryHelper;
		}
		[User]
		public IActionResult UserIndex()
		{
			return View();
		}

		public IActionResult UserLogin()
		{
			if (HttpContext.Session.Keys.Contains("User"))
			{
				return RedirectToAction("UserIndex");
			}
			return View();
		}

		[HttpPost]
		public IActionResult UserLogin(string username, string password, string code)
		{
			if (HttpContext.Session.GetString("Code") == code.ToLower())
			{
				string hash = _hashHelper.GetMD5(password);
				var user = _db.User.Where(u => u.Username == username && u.Password == hash).FirstOrDefault();
				if (user != null)
				{
					HttpContext.Session.Set("User", _binaryHelper.ToBinary(user));
					return RedirectToAction("UserIndex");
				}
				else
				{
					ViewData["Message"] = "شناسه یا رمز نادرست است !";
					return View();
				}
			}
			else
			{
				ViewData["Message"] = "کد امنیتی نادرست است !";
				return View();
			}
		}

		public IActionResult CustomerIndex()
		{
			ViewData["Categories"] = _db.Category.OrderBy(c => c.Name).Include(c => c.Subcategory).ToList();
			ViewData["Products"] = _db.Product.Include(p => p.ProductPhoto).Include(p => p.Subcategory).OrderByDescending(p => p.ProductId).ToList();
			return View();
		}

		[HttpPost]
		public IActionResult CustomerLogin(string username, string password)
		{
			string hash = _hashHelper.GetMD5(password);
			var customer = _db.Customer.Where(u => u.Username == username && u.Password == hash).FirstOrDefault();
			if (customer != null)
			{
				HttpContext.Session.Set("Customer", _binaryHelper.ToBinary(customer));
				return RedirectToAction("CustomerIndex");
			}
			else
			{
				ViewData["Categories"] = _db.Category.OrderBy(c => c.Name).Include(c => c.Subcategory).ToList();
				ViewData["Products"] = _db.Product.Include(p => p.ProductPhoto).Include(p => p.Subcategory).OrderByDescending(p => p.ProductId).ToList();
				ViewData["Message"] = "شناسه یا رمز نادرست است !";
				return View("CustomerIndex");
			}
		}
		[User]
		public IActionResult UserLogout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("UserLogin");
		}

		[HttpPost]
		[User]
		public IActionResult ChangeUserPassword(string password)
		{
			var user = _db.User.Find((_binaryHelper.FromBinary<User>(HttpContext.Session.Get("User"))).UserId);
			user.Password = _hashHelper.GetMD5(password);
			_db.SaveChanges();
			HttpContext.Session.Clear();
			return RedirectToAction("UserLogin");
		}

		public IActionResult Signup()
		{
			ViewData["State"] = _db.State.Include(s => s.City).OrderBy(s => s.Name).ToList();
			return View();
		}

		[HttpPost]
		public IActionResult Signup(Customer customer)
		{
			try
			{
				customer.Password = _hashHelper.GetMD5(customer.Password);
				_db.Customer.Add(customer);
				_db.SaveChanges();
				return RedirectToAction("CustomerIndex");
			}
			catch (Exception)
			{
				ViewData["State"] = _db.State.Include(s => s.City).OrderBy(s => s.Name).ToList();
				return View();
			}
		}
		[Customer]
		public IActionResult Product(int? id)
		{
			//var product = _db.Product.Find(id);

			var product = _db.Product
				.Include(c => c.ProductPhoto)
				.Include(c => c.ProductAttributeValue)
				.ThenInclude(c => c.ProductAttribute)
				.Where(w => w.ProductId == id).FirstOrDefault();
			return View(product);
		}
		public IActionResult Products()
		{
			return View(_db.Product.OrderBy(p => p.Name).ToList());
		}

		public IActionResult Search(string name)
		{
			return View("Products", _db.Product.Where(p => p.Name.Contains(name)).OrderBy(p => p.Name).ToList());
		}
		[Customer]
		public IActionResult AddToOrder(int id)
		{
			var product = _db.Product.Find(id);
			var curomer = _db.Customer.Find(_binaryHelper.FromBinary<Customer>(HttpContext.Session.Get("Customer")).CustomerId);
			var order = curomer.Order.Where(o => o.IsPayed == false).FirstOrDefault();
			if (order == null)
			{
				order = new Order();
				order.IsPayed = false;
				order.IsSent = false;
				order.OrderDate = DateTime.Now;
				order.PaymentCode = "";
				order.CustomerId = curomer.CustomerId;
				_db.Order.Add(order);
				_db.SaveChanges();
			}
			var orderDetail = new OrderDetail();
			orderDetail.ProductId = id;
			orderDetail.Quantity = 1;
			orderDetail.UnitPrice = product.UnitPrice;
			order.OrderDetail.Add(orderDetail);
			_db.SaveChanges();
			return RedirectToAction("Order", order);
		}
		[Customer]
		public IActionResult Order()
		{
				var curomer = _db.Customer.Find(_binaryHelper.FromBinary<Customer>(HttpContext.Session.Get("Customer")).CustomerId);
				_db.Entry(curomer).Collection(c => c.Order).Load();
				var order = curomer.Order.Where(o => o.IsPayed == false).ToList();
				return View(order);
		}
		[Customer]
		public IActionResult OrderDetails(int id)
		{
			var orderDetails = _db.OrderDetail.Include(c => c.Product).Where(w => w.OrderId == id).FirstOrDefault();
			return View(orderDetails);
		}
		[Customer]
		public void Pay(int id)
		{
			var order = _db.Order.Find(id);
			order.IsPayed = true;
			_db.SaveChanges();
			HttpContext.Response.Redirect("https://sadad.shaparak.ir/billpayment");
		}
		public IActionResult Captcha()
		{
			//string code = new Random().Next(1000, 9999).ToString();
			string code = GenerateCoupon(6, new Random());
			HttpContext.Session.SetString("Code", code);
			Bitmap bitmap = new Bitmap(300, 150);
			var graphics = Graphics.FromImage(bitmap);
			graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, 300, 150));
			graphics.FillRectangle(new HatchBrush(HatchStyle.Cross, Color.Gray), new Rectangle(0, 0, 300, 150));
			graphics.DrawString(code, new Font("Consolas", 50), Brushes.Gray, new PointF(20, 20));
			MemoryStream stream = new MemoryStream();
			graphics.Save();
			bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
			return new FileContentResult(stream.GetBuffer(), "image/jpg");
		}
		public static string GenerateCoupon(int length, Random random)
		{
			string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			StringBuilder result = new StringBuilder(length);
			for (int i = 0; i < length; i++)
			{
				result.Append(characters[random.Next(characters.Length)]);
			}
			return result.ToString().ToLower();
		}
	}
}
