using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimpleShop.ActionFilters;
using SimpleShop.Helpers;
using SimpleShop.Models;

namespace SimpleShop.Controllers
{
    public class UsersController : Controller
    {
        private readonly SimpleShopDatabaseContext _context;
        private readonly HashHelper _hashHelper;

        public UsersController(SimpleShopDatabaseContext context, HashHelper hashHelper)
        {
            _context = context;
            _hashHelper = hashHelper;
        }
        [User]
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }
        [User]
        public IActionResult Create()
        {
            return View();
        }
        [User]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Password,Family,Name,Mobile,IsAdmin")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = _hashHelper.GetMD5(user.Password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        [User]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [User]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Family,Name,Mobile,IsAdmin")] User user,string password,string oldPassword)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            try
            {
                if (password!=null&&password.Length>0)
                {
                    user.Password = _hashHelper.GetMD5(password);
                }
                else
                {
                    user.Password = oldPassword;
                }
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserId))
                {
                    return NotFound();
                }
                else
                {
                    
                }
            }

            return View(user);
        }
        [User]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
