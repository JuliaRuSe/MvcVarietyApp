using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCreditApp.Data;
using MvcCreditApp.Models;
using MvcCreditApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace MvcCreditApp.Controllers
{
    public class VarietyInfoesController : Controller
    {
        private readonly CreditContext _context;

        public VarietyInfoesController(CreditContext context)
        {
            _context = context;
        }

        // GET: VarietyInfoes
        public async Task<IActionResult> Index()
        {
            var varietyInfos = await _context.VarietyInfos
                .Include(v => v.Variety)
                .OrderByDescending(v => v.Year)
                .ThenBy(v => v.Variety.Name)
                .ToListAsync();
            return View(varietyInfos);
        }

        // GET: VarietyInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var varietyInfo = await _context.VarietyInfos
                .Include(v => v.Variety)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (varietyInfo == null)
            {
                return NotFound();
            }

            return View(varietyInfo);
        }

        // GET: VarietyInfoes/Create
        public IActionResult Create()
        {
            ViewData["VarietyId"] = new SelectList(_context.Varieties, "Id", "Name");
            return View();
        }

        // POST: VarietyInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VarietyId,Year,Germination,Productivity,Notes")] VarietyInfo varietyInfo)
        {

            var exists = await _context.VarietyInfos
                .AnyAsync(v => v.VarietyId == varietyInfo.VarietyId && v.Year == varietyInfo.Year);

            if (exists)
            {
                ModelState.AddModelError("Year", "Для этого сорта уже есть данные за указанный год");
            }

            if (ModelState.IsValid)
            {
                _context.Add(varietyInfo);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Характеристики для года {varietyInfo.Year} добавлены!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Varieties = _context.Varieties.OrderBy(v => v.Name).ToList();
            return View(varietyInfo);
        }

        // GET: VarietyInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var varietyInfo = await _context.VarietyInfos.FindAsync(id);
            if (varietyInfo == null)
            {
                return NotFound();
            }
            ViewData["VarietyId"] = new SelectList(_context.Varieties, "Id", "Crop", varietyInfo.VarietyId);
            return View(varietyInfo);

        }

        // POST: VarietyInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VarietyId,Year,Germination,Productivity,Notes")] VarietyInfo varietyInfo)
        {
            if (id != varietyInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(varietyInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VarietyInfoExists(varietyInfo.Id))
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
            ViewData["VarietyId"] = new SelectList(_context.Varieties, "Id", "Crop", varietyInfo.VarietyId);
            return View(varietyInfo);
        }

        // GET: VarietyInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var varietyInfo = await _context.VarietyInfos
                .Include(v => v.Variety)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (varietyInfo == null)
            {
                return NotFound();
            }

            return View(varietyInfo);
        }

        // POST: VarietyInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var varietyInfo = await _context.VarietyInfos.FindAsync(id);
            if (varietyInfo != null)
            {
                _context.VarietyInfos.Remove(varietyInfo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VarietyInfoExists(int id)
        {
            return _context.VarietyInfos.Any(e => e.Id == id);
        }

        //////////////////////
        // GET: VarietyInfo/TopByProductivity
        public async Task<IActionResult> TopByProductivity()
        {
            var topVarieties = await _context.VarietyInfos
                .Include(v => v.Variety)
                .OrderByDescending(v => v.Productivity)
                .ThenByDescending(v => v.Germination)
                .Take(5)
                .Select(v => new VarietySummaryViewModel
                {
                    VarietyId = v.VarietyId,
                    VarietyName = v.Variety.Name,
                    Crop = v.Variety.Crop,
                    Year = v.Year,
                    Germination = v.Germination,
                    Productivity = v.Productivity,
                    TotalScore = v.Germination + v.Productivity,
                    GerminationText = v.GerminationText,
                    ProductivityText = v.ProductivityText
                })
                .ToListAsync();

            ViewBag.Title = "Лучшие 5 сортов по урожайности";
            ViewBag.SortType = "top";
            return View("TopBottomVarieties", topVarieties);
        }

        // GET: VarietyInfo/BottomByProductivity
        public async Task<IActionResult> BottomByProductivity()
        {
            var bottomVarieties = await _context.VarietyInfos
                .Include(v => v.Variety)
                .OrderBy(v => v.Productivity)
                .ThenBy(v => v.Germination)
                .Take(5)
                .Select(v => new VarietySummaryViewModel
                {
                    VarietyId = v.VarietyId,
                    VarietyName = v.Variety.Name,
                    Crop = v.Variety.Crop,
                    Year = v.Year,
                    Germination = v.Germination,
                    Productivity = v.Productivity,
                    TotalScore = v.Germination + v.Productivity,
                    GerminationText = v.GerminationText,
                    ProductivityText = v.ProductivityText
                })
                .ToListAsync();

            ViewBag.Title = "Худшие 5 сортов по урожайности";
            ViewBag.SortType = "bottom";
            return View("TopBottomVarieties", bottomVarieties);
        }

        [HttpGet]
        public async Task<IActionResult> ExportToFile() 
        {
            // 1. 
           
            var topByProductivity = await _context.VarietyInfos
            .Include(v => v.Variety)
            .OrderByDescending(v => v.Productivity)
            .ThenByDescending(v => v.Germination)
            .Take(5)
            .Select(v => new
            {
                VarietyName = v.Variety.Name,
                Crop = v.Variety.Crop,
                Year = v.Year,
                Productivity = new { Value = v.Productivity, Text = v.ProductivityText },
                Germination = new { Value = v.Germination, Text = v.GerminationText },
                TotalScore = v.TotalScore
            })
            .ToListAsync();
            
            // 2. Настраиваем сериализатор, чтобы русские буквы не превращались в коды
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true // Красивый отступ для читаемости
            };

            // 3. Превращаем объект в JSON-строку
            string jsonString = JsonSerializer.Serialize(topByProductivity, options);

            // 4. Переводим строку в массив байт
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(jsonString);

            // 5. Отдаем файл в браузер (он автоматически попадет в папку "Загрузки")
            string fileName = $"topByProductivity.json";
            return File(fileBytes, "application/json", fileName);
        }
    }
}
