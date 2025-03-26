using Microsoft.AspNetCore.Mvc;
using SEDEP.Models;
using Microsoft.AspNetCore.Http;

namespace SEDEP.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Simulación de autenticación con roles
            if (model.Usuario == "admin" && model.Password == "1234")
            {
                TempData["SuccessMessage"] = "Inicio de sesión exitoso. Bienvenido, Administrador.";
                HttpContext.Session.SetString("UserRole", "Administración");
                return RedirectToAction("DashboardAdmin", "Home"); // Redirigir a la vista de administrador
            }
            else if (model.Usuario == "user" && model.Password == "1234")
            {
                TempData["SuccessMessage"] = "Inicio de sesión exitoso. Bienvenido, Usuario.";
                HttpContext.Session.SetString("UserRole", "Jefatura");
                return RedirectToAction("DashboardJefatura", "Home"); // Redirigir a la vista de jefatura
            }
            else if (model.Usuario == "user" && model.Password == "1234")
            {
                TempData["SuccessMessage"] = "Inicio de sesión exitoso. Bienvenido, Usuario.";
                HttpContext.Session.SetString("UserRole", "SubAlterno");
                return RedirectToAction("DashboardUser", "Home"); // Redirigir a la vista de usuario
            }

            ModelState.AddModelError("", "Error, Datos incorrectos.");
            return View(model);
        }
    }
}
