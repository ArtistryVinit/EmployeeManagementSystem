﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Controllers
{
    public class SystemProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SystemProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SystemProfiles
        public async Task<IActionResult> Index()
        {
            var systemProfiles = await _context.systemProfiles.Include(s => s.Profile).ToListAsync();
            return View(systemProfiles);
        }

        // GET: SystemProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemProfile = await _context.systemProfiles
                .Include(s => s.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (systemProfile == null)
            {
                return NotFound();
            }

            return View(systemProfile);
        }

        // GET: SystemProfiles/Create
        public IActionResult Create()
        {
            ViewData["ProfileId"] = new SelectList(_context.systemProfiles, "Id", "Name");
            return View();
        }

        // POST: SystemProfiles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemProfile systemProfile)
        {

            systemProfile.CreatedById = "Vinit Ahir";
            systemProfile.CreatedOn = DateTime.Now;

            _context.Add(systemProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


            ViewData["ProfileId"] = new SelectList(_context.systemProfiles, "Id", "Name", systemProfile.ProfileId);
            return View(systemProfile);
        }


        //// GET: SystemProfiles/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var systemProfile = await _context.systemProfiles.FindAsync(id);
        //    if (systemProfile == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewData["ProfileId"] = new SelectList(_context.systemProfiles, "Id", "Name", systemProfile.ProfileId);
        //    return View(systemProfile);
        //}

        // GET: SystemProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemProfile = await _context.systemProfiles.FindAsync(id);
            if (systemProfile == null)
            {
                return NotFound();
            }

            // Populate dropdown
            ViewData["ProfileId"] = new SelectList(_context.systemProfiles, "Id", "Name", systemProfile.ProfileId);

            return View(systemProfile);
        }


        // POST: SystemProfiles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ProfileId")] SystemProfile systemProfile)
        {
            if (id != systemProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(systemProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemProfileExists(systemProfile.Id))
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

            ViewData["ProfileId"] = new SelectList(_context.systemProfiles, "Id", "Name", systemProfile.ProfileId);
            return View(systemProfile);
        }

        // GET: SystemProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemProfile = await _context.systemProfiles
                .Include(s => s.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (systemProfile == null)
            {
                return NotFound();
            }

            return View(systemProfile);
        }

        // POST: SystemProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var systemProfile = await _context.systemProfiles.FindAsync(id);
            if (systemProfile != null)
            {
                _context.systemProfiles.Remove(systemProfile);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SystemProfileExists(int id)
        {
            return _context.systemProfiles.Any(e => e.Id == id);
        }
    }
}
