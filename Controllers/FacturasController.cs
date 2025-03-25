using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TEATRO.Data;
using TEATRO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEATRO.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FacturasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/facturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFacturas([FromQuery] int? clienteId, [FromQuery] int? eventoId, [FromQuery] DateTime? fecha)
        {
            var query = _context.Facturas.AsQueryable();

            if (clienteId.HasValue)
                query = query.Where(f => f.ClienteId == clienteId.Value);

            if (eventoId.HasValue)
                query = query.Where(f => f.EventoId == eventoId.Value);

            if (fecha.HasValue)
                query = query.Where(f => f.Fecha.Date == fecha.Value.Date);

            var facturas = await query.Include(f => f.Entradas).ToListAsync();
            return Ok(facturas);
        }

        // GET: api/facturas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetFactura(int id)
        {
            var factura = await _context.Facturas
                                        .Include(f => f.Entradas)
                                        .FirstOrDefaultAsync(f => f.Id == id);

            if (factura == null)
                return NotFound();

            return Ok(factura);
        }

        // POST: api/facturas
        [HttpPost]
        public async Task<ActionResult<Factura>> PostFactura(Factura factura)
        {
            // Calcula el total sumando cada entrada (Cantidad x PrecioUnitario)
            factura.Total = factura.Entradas.Sum(e => e.Cantidad * e.PrecioUnitario);
            factura.Fecha = DateTime.Now;

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFactura), new { id = factura.Id }, factura);
        }

        // PUT: api/facturas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactura(int id, Factura factura)
        {
            if (id != factura.Id)
                return BadRequest();

            // Recalcular total en caso de modificación
            factura.Total = factura.Entradas.Sum(e => e.Cantidad * e.PrecioUnitario);

            _context.Entry(factura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Facturas.Any(f => f.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/facturas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactura(int id)
        {
            var factura = await _context.Facturas
                                        .Include(f => f.Entradas)
                                        .FirstOrDefaultAsync(f => f.Id == id);
            if (factura == null)
                return NotFound();

            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = $"Factura {id} eliminada exitosamente." });
        }
    }
}

