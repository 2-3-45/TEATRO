using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BCrypt.Net;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using TEATRO.Data;
using TEATRO.Models;

namespace ProyectoProgramado_1.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================
        // 🟠 MÉTODOS DE REGISTRO
        // ==========================
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View("Create"); // Muestra directamente la vista del formulario
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Correo,Password")] Usuario usuario, string CaptchaCode)
        {
            // Validación del Captcha
            if (!ValidarCaptcha(CaptchaCode))
            {
                ModelState.AddModelError(string.Empty, "Captcha inválido.");
                return View("Create", usuario);
            }

            // Si el modelo es válido, creamos al nuevo usuario
            if (ModelState.IsValid)
            {
                usuario.RolId = 2; // Rol por defecto: Comprador
                usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);  // Hasheo de la contraseña

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");  // Redirige al Home
            }

            return View("Create", usuario);
        }

        // ==========================
        // 🔵 MÉTODOS DE LOGIN
        // ==========================
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Validación del Captcha
            if (!ValidarCaptcha(model.CaptchaCode))
            {
                ModelState.AddModelError(string.Empty, "Captcha inválido.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Buscamos al usuario en la base de datos
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == model.Correo);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuario.Password))
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                return View(model);
            }

            // Crear las claims para la autenticación
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.RolId == 1 ? "Administrador" : "Comprador")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Iniciar sesión
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (usuario.RolId == 1)
                return RedirectToAction("Index", "AdministradorDashboard");

            return RedirectToAction("Index", "Home");
        }

        // ==========================
        // 🟣 MÉTODO PARA CERRAR SESIÓN
        // ==========================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuarios");
        }

        // ==========================
        // 🔒 MÉTODO PARA GENERAR CAPTCHA
        // ==========================
        public IActionResult GenerarCaptcha()
        {
            var random = new Random();
            var captchaCode = random.Next(1000, 9999).ToString();

            HttpContext.Session.SetString("CaptchaCode", captchaCode);

            using var bitmap = new Bitmap(100, 40);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            using var font = new Font("Arial", 18, FontStyle.Bold);
            graphics.DrawString(captchaCode, font, Brushes.Black, 10, 5);

            using var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            return File(stream.ToArray(), "image/png");
        }

        private bool ValidarCaptcha(string captchaCode)
        {
            var captchaCorrecto = HttpContext.Session.GetString("CaptchaCode");
            return captchaCode == captchaCorrecto;
        }

        // ==========================
        // 🔐 MÉTODO DE RECUPERACIÓN DE CONTRASEÑA
        // ==========================
        [HttpGet]
        public IActionResult RecuperarContraseña() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnviarCorreoRecuperacion(string Correo)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == Correo);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Correo no registrado.");
                return View("RecuperarContraseña");
            }

            string token = Guid.NewGuid().ToString();
            usuario.TokenRecuperacion = token;
            await _context.SaveChangesAsync();

            string enlaceRecuperacion = Url.Action("RestablecerContraseña", "Usuarios",
                new { token = token }, Request.Scheme);

            await EnviarCorreo(usuario.Correo, "Recuperación de Contraseña",
                $"Haz clic en el siguiente enlace para restablecer tu contraseña: {enlaceRecuperacion}");

            ViewBag.Mensaje = "Se ha enviado un correo con las instrucciones para restablecer la contraseña.";
            return View("RecuperarContraseña");
        }

        private async Task EnviarCorreo(string destinatario, string asunto, string mensaje)
        {
            var remitente = "nicole.diaz.t@gmail.com";
            var clave = "ywfp wbpu pkzb niof";
            var smtpServer = "smtp.gmail.com";

            using (var cliente = new SmtpClient(smtpServer, 587))
            {
                cliente.Credentials = new NetworkCredential(remitente, clave);
                cliente.EnableSsl = true;

                var correo = new MailMessage(remitente, destinatario, asunto, mensaje);
                await cliente.SendMailAsync(correo);
            }
        }

        // ==========================
        // ⚙️ CRUD (Protegido por Roles)
        // ==========================
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ListaUsuarios()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null) return NotFound();

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListaUsuarios));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}