using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class HolidayController : Controller
{
    private readonly ApplicationDbContext _context;

    public HolidayController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Holiday/
    public async Task<IActionResult> Index()
    {
        var holidays = await _context.Holidays.ToListAsync();
        return View("Index", holidays); // Ensure it explicitly looks for "Index"
    }

    // ✅ CREATE HOLIDAY (GET)
    public IActionResult Create()
    {
        return View();
    }

    // ✅ CREATE HOLIDAY (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Holiday holiday)
    {
        if (ModelState.IsValid)
        {
            _context.Holidays.Add(holiday);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(holiday);
    }

    // ✅ EDIT HOLIDAY (GET)
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var holiday = await _context.Holidays.FindAsync(id);
        if (holiday == null) return NotFound();

        return View(holiday);
    }

    // ✅ EDIT HOLIDAY (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Holiday holiday)
    {
        if (id != holiday.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(holiday);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(holiday);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var holiday = await _context.Holidays.FindAsync(id);
        if (holiday == null) return NotFound();

        return View(holiday);  // ✅ Ensures it looks for Details.cshtml
    }


    // ✅ DELETE HOLIDAY (GET)
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var holiday = await _context.Holidays.FindAsync(id);
        if (holiday == null) return NotFound();

        return View(holiday);
    }

    // ✅ DELETE HOLIDAY (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var holiday = await _context.Holidays.FindAsync(id);
        if (holiday != null)
        {
            _context.Holidays.Remove(holiday);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
