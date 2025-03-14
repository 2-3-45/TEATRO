using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TEATRO.Data;
using TEATRO.Models;

namespace ProyectoProgramado_1.Controllers
{
    public class TeatrosController : Controller
    {
        private readonly AppDbContext _context;

        public TeatrosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Teatroes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teatros.ToListAsync());
        }

        // GET: Teatroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teatro = await _context.Teatros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teatro == null)
            {
                return NotFound();
            }

            return View(teatro);
        }

        // GET: Teatroes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teatroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Ubicacion")] Teatro teatro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teatro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teatro);
        }

        // GET: Teatroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teatro = await _context.Teatros.FindAsync(id);
            if (teatro == null)
            {
                return NotFound();
            }
            return View(teatro);
        }

        // POST: Teatroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Ubicacion")] Teatro teatro)
        {
            if (id != teatro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teatro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeatroExists(teatro.Id))
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
            return View(teatro);
        }

        // GET: Teatroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teatro = await _context.Teatros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teatro == null)
            {
                return NotFound();
            }

            return View(teatro);
        }

        // POST: Teatroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teatro = await _context.Teatros.FindAsync(id);
            if (teatro != null)
            {
                _context.Teatros.Remove(teatro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeatroExists(int id)
        {
            return _context.Teatros.Any(e => e.Id == id);
        }
    }
}