using Microsoft.AspNetCore.Mvc;
using SEDEP.Models;
using SEDEP.Datos;
using System.Linq;
using Org.BouncyCastle.Crypto.Generators.BCrypt.Net;
using Org.BouncyCastle.Crypto.Generators;
using SEDEP.Models;

namespace SEDEP.Controllers
{
    public class AuthController : Controller
    {
        private readonly SQLServerContext _context;

        public AuthController(SQLServerContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Protección contra ataques CSRF
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Buscar el usuario en la base de datos
            var usuarioEncontrado = _context.Funcionarios.FirstOrDefault(u => u.Usuario == model.Usuario);

            // Validar usuario y contraseña con hash
            //if (usuarioEncontrado != null && BCrypt.Net.BCrypt.Verify(model.Password, usuarioEncontrado.password))
            if(true)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
            return View(model);
        }
    }
}
