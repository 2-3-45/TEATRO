
using Microsoft.EntityFrameworkCore;
using TEATRO.Models;


namespace TEATRO.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TEATRO.Models.Usuario> Usuarios { get; set; } = default!;
        public DbSet<TEATRO.Models.Rol> Roles { get; set; } = default!;
        public DbSet<TEATRO.Models.Teatro> Teatros { get; set; } = default!;
        public DbSet<TEATRO.Models.Obra> Obras { get; set; } = default!;
        public DbSet<ProyectoProgramado_1.Models.Reserva> Reservas { get; set; } = default!;
        public DbSet<ProyectoProgramado_1.Models.Pago> Pagos { get; set; } = default!;

        public DbSet<Producto> Productos { get; set; }  // 🔹 Agregado para gestionar productos
    }
}

