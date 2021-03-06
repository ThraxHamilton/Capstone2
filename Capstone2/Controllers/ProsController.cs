﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Capstone2.Data;
using Capstone2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Capstone2.Controllers
{
    public class ProsController : Controller
    {
        private readonly ApplicationDbContext _context;
        /* Represents user data */
        private readonly UserManager<ApplicationUser> _userManager;

        /* Retrieves the data for the current user from _userManager*/
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public ProsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
       
            _context = context;
            _userManager = userManager;
        }

        // GET: Pros
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            return View(await _context.Pros.Where(users => users.UserId == user.Id).ToListAsync());
        }

        // GET: Pros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pros = await _context.Pros
                .FirstOrDefaultAsync(m => m.ProId == id);
            if (pros == null)
            {
                return NotFound();
            }

            return View(pros);
        }

        // GET: Pros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pros/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pros pros)
            
        {

            // Removes unneeded info from model state before passing it in
            ModelState.Remove("UserId");
            ModelState.Remove("User");
           
           
            if (ModelState.IsValid)
            {
                ApplicationUser user = await GetCurrentUserAsync();
                _context.Add(pros);
                // Adds info back in
                pros.User = await
                    GetCurrentUserAsync();
                pros.UserId = pros.UserId;
                await _context.SaveChangesAsync();
                return RedirectToAction("Create","Cons");
            }
            ViewData["ProId"] = new SelectList(_context.ApplicationUsers, "ProEntry", "Date", pros.UserId);

            return View(pros);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Continue(Pros pros)

        {
            // Removes unneeded info from model state before passing it in
            ModelState.Remove("UserId");
            ModelState.Remove("User");


            if (ModelState.IsValid)
            {
                _context.Add(pros);
                // Adds info back in
                pros.User = await
                    GetCurrentUserAsync();
                pros.UserId = pros.UserId;
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Pros");
            }
            ViewData["ProId"] = new SelectList(_context.ApplicationUsers, "ProEntry", "Date", pros.UserId);

            return View("Create");
        }

        // GET: Pros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pros = await _context.Pros.FindAsync(id);
            if (pros == null)
            {
                return NotFound();
            }
            return View(pros);
        }

        // POST: Pros/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProId,UserId,Date,ProEntry")] Pros pros)
        {
            if (id != pros.ProId)
            {
                return NotFound();
            }

            // Removes unneeded info from model state before passing it in
            ModelState.Remove("UserId");
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pros);
                    pros.User = await
                   GetCurrentUserAsync();
                    pros.UserId = pros.UserId;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProsExists(pros.ProId))
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
            //ViewData["ProId"] = new SelectList(_context.ApplicationUsers, "ProEntry", "Date", pros.ProId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", pros.UserId);


            return View(pros);
        }

        // GET: Pros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pros = await _context.Pros
                .FirstOrDefaultAsync(m => m.ProId == id);
            if (pros == null)
            {
                return NotFound();
            }

            return View(pros);
        }

        // POST: Pros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pros = await _context.Pros.FindAsync(id);
            _context.Pros.Remove(pros);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProsExists(int id)
        {
            return _context.Pros.Any(e => e.ProId == id);
        }
    }
}
