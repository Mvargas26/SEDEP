using Microsoft.AspNetCore.Mvc;
using SEDEP.Models;

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

            // Simulación de autenticación
            if (model.Usuario == "admin" && model.Password == "1234")
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
            return View(model);
        }
    }
}
