using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TEATRO.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdministradorDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // ✔️ Agregado: Mantenimiento de Obras
        public IActionResult GestionObras()
        {
            return RedirectToAction("Index", "Obras");
        }



        // ✔️ Agregado: Mantenimiento de Pagos
        public IActionResult GestionPagos()
        {
            return RedirectToAction("Index", "Pagos");
        }






        // ✔️ Agregado: Mantenimiento de Productos
        public IActionResult GestionProductos()
        {
            return RedirectToAction("Index", "Productos");
        }

        // ✔️ Agregado: Mantenimiento de Teatros
        public IActionResult GestionTeatros()
        {
            return RedirectToAction("Index", "Teatros");
        }

        // ✔️ Agregado: Mantenimiento de Asientos
        public IActionResult GestionAsientos()
        {
            return RedirectToAction("Index", "Acientos");
        }

        // ✔️ Agregado: Mantenimiento de Reservas
        public IActionResult GestionReservas()
        {
            return RedirectToAction("Index", "Reservas");
        }

        // ✔️ Agregado: Mantenimiento de Roles
        public IActionResult GestionRoles()
        {
            return RedirectToAction("Index", "Roles");
        }

        public IActionResult Reportes()
        {
            return View();
        }

        public IActionResult Configuracion()
        {
            return View();
        }

        public IActionResult GestionUsuarios()
        {
            return RedirectToAction("Index", "Usuarios");
        }

        public IActionResult CerrarSesion()
        {
            return RedirectToAction("Logout", "Usuarios");
        }
    }
}
