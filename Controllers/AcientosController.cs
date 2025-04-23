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
            var asientos = await _context.Aciento.ToListAsync();

            // Ordenar los asientos de la fila 1 a la 10 de manera estricta
            var asientosPorFila = asientos
                .OrderBy(a => ParsearFila(a.Fila))
                .ThenBy(a => a.Numero)
                .GroupBy(a => a.Fila)
                .ToList();

            return View(asientosPorFila);
        }

        // Método para parsear filas, asegurando orden de 1 a 10
        private int ParsearFila(string fila)
        {
            // Intenta convertir a número, priorizando filas de 1 a 10
            if (int.TryParse(fila, out int numero))
            {
                // Asegura que solo los números del 1 al 10 se ordenen primero
                return numero >= 1 && numero <= 10 ? numero : int.MaxValue;
            }

            // Cualquier otra fila (no numérica o fuera del rango 1-10) se coloca al final
            return int.MaxValue;
        }


        //Metodo para reservar asientos
        [HttpPost]
        public async Task<IActionResult> Reservar(List<int> asientosSeleccionados)
        {
            if (asientosSeleccionados != null && asientosSeleccionados.Any())
            {
                var asientos = await _context.Aciento
                    .Where(a => asientosSeleccionados.Contains(a.Id))
                    .ToListAsync();

                foreach (var asiento in asientos)
                {
                    asiento.Estado = "Ocupado";  // Cambiamos el estado a "Ocupado"
                    _context.Update(asiento);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
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
