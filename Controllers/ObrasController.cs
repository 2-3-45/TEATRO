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
    public class ObrasController : Controller
    {
        private readonly AppDbContext _context;

        public ObrasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Obras
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Obras.Include(o => o.Teatro);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Obras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var obra = await _context.Obras.Include(o => o.Teatro).FirstOrDefaultAsync(m => m.Id == id);
            if (obra == null) return NotFound();

            return View(obra);
        }

        // GET: Obras/Create
        public IActionResult Create()
        {
            ViewData["TeatroId"] = new SelectList(_context.Teatros, "Id", "Nombre");
            return View();
        }

        // POST: Obras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Descripcion,TeatroId")] Obra obra)
        {
            if (ModelState.IsValid) // ✅ Cambio en la validación para corregir el flujo
            {
                try
                {
                    _context.Add(obra);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = $"Ocurrió un error al guardar: {ex.Message}";
                }
            }
            else
            {
                ViewData["Error"] = "Error en la validación del modelo.";
            }

            ViewData["TeatroId"] = new SelectList(_context.Teatros, "Id", "Nombre", obra.TeatroId);
            return View(obra);
        }


        // GET: Obras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var obra = await _context.Obras.FindAsync(id);
            if (obra == null) return NotFound();

            ViewData["TeatroId"] = new SelectList(_context.Teatros, "Id", "Nombre", obra.TeatroId);
            return View(obra);
        }

        // POST: Obras/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descripcion,TeatroId")] Obra obra)
        {
            if (id != obra.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(obra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObraExists(obra.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["TeatroId"] = new SelectList(_context.Teatros, "Id", "Nombre", obra.TeatroId);
            return View(obra);
        }

        // GET: Obras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var obra = await _context.Obras.Include(o => o.Teatro).FirstOrDefaultAsync(m => m.Id == id);
            if (obra == null) return NotFound();

            return View(obra);
        }

        // POST: Obras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obra = await _context.Obras.FindAsync(id);
            if (obra != null)
            {
                _context.Obras.Remove(obra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObraExists(int id)
        {
            return _context.Obras.Any(e => e.Id == id);
        }
    }
}