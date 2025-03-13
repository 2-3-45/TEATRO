using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TEATRO.Data;
using TEATRO.Models;

namespace TEATRO.Controllers
{
    public class AcientosController : Controller
    {
        private readonly AppDbContext _context;

        public AcientosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Acientoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Aciento.ToListAsync());
        }

        // GET: Acientoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aciento = await _context.Aciento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aciento == null)
            {
                return NotFound();
            }

            return View(aciento);
        }

        // GET: Acientoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Acientoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Numero,Fila,Estado,Zona")] Aciento aciento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aciento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aciento);
        }

        // GET: Acientoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aciento = await _context.Aciento.FindAsync(id);
            if (aciento == null)
            {
                return NotFound();
            }
            return View(aciento);
        }

        // POST: Acientoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numero,Fila,Estado,Zona")] Aciento aciento)
        {
            if (id != aciento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aciento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcientoExists(aciento.Id))
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
            return View(aciento);
        }

        // GET: Acientoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aciento = await _context.Aciento
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aciento == null)
            {
                return NotFound();
            }

            return View(aciento);
        }

        // POST: Acientoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aciento = await _context.Aciento.FindAsync(id);
            if (aciento != null)
            {
                _context.Aciento.Remove(aciento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcientoExists(int id)
        {
            return _context.Aciento.Any(e => e.Id == id);
        }
    }
}
