using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Capstone2.Data;
using Capstone2.Models;
using Microsoft.AspNetCore.Identity;

namespace Capstone2.Controllers
{
    public class ConsController : Controller
    {
        private readonly ApplicationDbContext _context;
        /* Represents user data */
        private readonly UserManager<ApplicationUser> _userManager;

        /* Retrieves the data for the current user from _userManager*/
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public ConsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Cons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cons.ToListAsync());
        }

        // GET: Cons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cons = await _context.Cons
                .FirstOrDefaultAsync(m => m.ConId == id);
            if (cons == null)
            {
                return NotFound();
            }

            return View(cons);
        }

        // GET: Cons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConId,UserId,Date,ConEntry")] Cons cons)
        {
            ModelState.Remove("UserId");
            ModelState.Remove("User");


            if (ModelState.IsValid)
            {
                _context.Add(cons);
                cons.User = await
                    GetCurrentUserAsync();
                cons.UserId = cons.UserId;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConId"] = new SelectList(_context.ApplicationUsers, "ConEntry", "Date", cons.UserId);

            return View(cons);
        }

        // GET: Cons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cons = await _context.Cons.FindAsync(id);
            if (cons == null)
            {
                return NotFound();
            }
            return View(cons);
        }

        // POST: Cons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConId,UserId,Date,ConEntry")] Cons cons)
        {
            if (id != cons.ConId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsExists(cons.ConId))
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
            return View(cons);
        }

        // GET: Cons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cons = await _context.Cons
                .FirstOrDefaultAsync(m => m.ConId == id);
            if (cons == null)
            {
                return NotFound();
            }

            return View(cons);
        }

        // POST: Cons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cons = await _context.Cons.FindAsync(id);
            _context.Cons.Remove(cons);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsExists(int id)
        {
            return _context.Cons.Any(e => e.ConId == id);
        }
    }
}
