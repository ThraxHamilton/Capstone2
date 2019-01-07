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
using Capstone2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

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
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            return View(await _context.Cons.Where(users => users.UserId == user.Id).ToListAsync());
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
        public async Task<IActionResult> Create(Cons cons)
        {
            ModelState.Remove("UserId");
            ModelState.Remove("User");


            if (ModelState.IsValid)
            {
                ApplicationUser user = await GetCurrentUserAsync();
                _context.Add(cons);
                cons.User = await
                    GetCurrentUserAsync();
                cons.UserId = cons.UserId;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetTotals));
            }
            ViewData["ConId"] = new SelectList(_context.ApplicationUsers, "ConEntry", "Date", cons.UserId);

            return View(cons);
        }
       
        public async Task<IActionResult> GetTotals()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            ProConViewModel proConViewModel = new ProConViewModel(_context, user);

            return View(proConViewModel);
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

            // Removes unneeded info from model state before passing it in
            ModelState.Remove("UserId");
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = await GetCurrentUserAsync();
                    _context.Update(cons);
                    cons.User = await
                  GetCurrentUserAsync();
                    cons.UserId = cons.UserId;
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
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", cons.UserId);
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
