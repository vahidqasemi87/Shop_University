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
	public class StatesController : Controller
	{
		private readonly SimpleShopDatabaseContext _context;

		public StatesController(SimpleShopDatabaseContext context)
		{
			_context = context;
		}
		[User]
		public async Task<IActionResult> Index()
		{
			return View(await _context.State.ToListAsync());
		}
		[User]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("StateId,Name")] State state)
		{
			state.Name = state.Name.Trim();
			if (ModelState.IsValid)
			{
				_context.Add(state);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(state);
		}
		[User]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var state = await _context.State.FindAsync(id);
			if (state == null)
			{
				return NotFound();
			}
			return View(state);
		}
		[User]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("StateId,Name")] State state)
		{
			if (id != state.StateId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(state);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!StateExists(state.StateId))
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
			return View(state);
		}
		[User]
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var state = await _context.State.FindAsync(id);
			_context.State.Remove(state);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool StateExists(int id)
		{
			return _context.State.Any(e => e.StateId == id);
		}
		public JsonResult IsStateExists(string name)
		{
			return Json(!_context.State.Any(c => c.Name == name.Trim()));
		}
	}
}
