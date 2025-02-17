using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using EmployeeManagementSystem.Models; // Ensure this namespace matches your project
using EmployeeManagementSystem.Data; // Ensure you have the DbContext imported
using System.Linq;

namespace EmployeeManagementSystem.Controllers
{
    public class AuditController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuditController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Audit
        public IActionResult Index()
        {
            var audits = _context.auditLogs.ToList();
            return View(audits);
        }

        // GET: Audit/Details/5
        public IActionResult Details(int id)
        {
            var audit = _context.auditLogs.Find(id);
            if (audit == null)
            {
                return NotFound();
            }
            return View(audit);
        }

        // GET: Audit/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Audit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Audit audit)
        {
            if (ModelState.IsValid)
            {
                _context.auditLogs.Add(audit);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(audit);
        }

        // GET: Audit/Edit/5
        public IActionResult Edit(int id)
        {
            var audit = _context.auditLogs.Find(id);
            if (audit == null)
            {
                return NotFound();
            }
            return View(audit);
        }

        // POST: Audit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Audit audit)
        {
            if (id != audit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.auditLogs.Update(audit);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(audit);
        }

        // GET: Audit/Delete/5
        public IActionResult Delete(int id)
        {
            var audit = _context.auditLogs.Find(id);
            if (audit == null)
            {
                return NotFound();
            }
            return View(audit);
        }

        // POST: Audit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var audit = _context.auditLogs.Find(id);
            if (audit != null)
            {
                _context.auditLogs.Remove(audit);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
