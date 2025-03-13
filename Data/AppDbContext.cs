using Microsoft.EntityFrameworkCore;
using TEATRO.Models;

namespace TEATRO.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; } = default!;
        public DbSet<Rol> Roles { get; set; } = default!;
        public DbSet<Teatro> Teatros { get; set; } = default!;
        public DbSet<Obra> Obras { get; set; } = default!;
        public DbSet<Reserva> Reservas { get; set; } = default!;
        public DbSet<Pago> Pagos { get; set; } = default!;

        // 🔹 Agregando las nuevas tablas
        public DbSet<Aciento> Aciento { get; set; } = default!;
        public DbSet<Producto> Productos { get; set; } = default!;
    }
}
