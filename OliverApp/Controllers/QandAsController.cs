using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OliverApp.Data;
using OliverApp.Models;

namespace OliverApp.Controllers
{
    public class QandAsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QandAsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: QandAs
        public async Task<IActionResult> Index()
        {
            return View(await _context.QandA.ToListAsync());
        }

        // GET: QandAs/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: QandAs/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.QandA.Where( q => q.Question.Contains(SearchPhrase)).ToListAsync());
        }


        // GET: QandAs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qandA = await _context.QandA
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qandA == null)
            {
                return NotFound();
            }

            return View(qandA);
        }

        // GET: QandAs/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: QandAs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer")] QandA qandA)
        {
            if (ModelState.IsValid)
            {
                _context.Add(qandA);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(qandA);
        }

        // GET: QandAs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qandA = await _context.QandA.FindAsync(id);
            if (qandA == null)
            {
                return NotFound();
            }
            return View(qandA);
        }

        // POST: QandAs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer")] QandA qandA)
        {
            if (id != qandA.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(qandA);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QandAExists(qandA.Id))
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
            return View(qandA);
        }

        // GET: QandAs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qandA = await _context.QandA
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qandA == null)
            {
                return NotFound();
            }

            return View(qandA);
        }

        // POST: QandAs/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var qandA = await _context.QandA.FindAsync(id);
            _context.QandA.Remove(qandA);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QandAExists(int id)
        {
            return _context.QandA.Any(e => e.Id == id);
        }
    }
}
