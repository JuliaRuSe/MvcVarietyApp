using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCreditApp.Data;
using MvcCreditApp.Models;

namespace MvcCreditApp.Controllers
{
    public class VarietiesController : Controller
    {
        private readonly CreditContext _context;

        public VarietiesController(CreditContext context)
        {
            _context = context;
        }

        // GET: Varieties
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Varieties.ToListAsync());
            var varieties = await _context.Varieties
                .Include(v => v.VarietyInfos)
                .OrderBy(v => v.Name)
                .ToListAsync();
            return View(varieties);
        }

        // GET: Varieties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variety = await _context.Varieties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variety == null)
            {
                return NotFound();
            }

            return View(variety);
        }

        // GET: Varieties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Varieties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Crop,Breeder,Description")] Variety variety)
        {
            if (ModelState.IsValid)
            {
                _context.Add(variety);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(variety);
        }

        // GET: Varieties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variety = await _context.Varieties.FindAsync(id);
            if (variety == null)
            {
                return NotFound();
            }
            return View(variety);
        }

        // POST: Varieties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Crop,Breeder,Description")] Variety variety)
        {
            if (id != variety.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(variety);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VarietyExists(variety.Id))
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
            return View(variety);
        }

        // GET: Varieties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variety = await _context.Varieties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variety == null)
            {
                return NotFound();
            }

            return View(variety);
        }

        // POST: Varieties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var variety = await _context.Varieties.FindAsync(id);
            if (variety != null)
            {
                _context.Varieties.Remove(variety);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VarietyExists(int id)
        {
            return _context.Varieties.Any(e => e.Id == id);
        }
    }
}
