using AdministracionActivosFijos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Negocios;
using SEDEP.Models;
using System.Diagnostics;

namespace SEDEP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult pruebaConexion()
        {
            ConglomeradosNegocios obj1 = new ConglomeradosNegocios();
            FuncionarioNegocios objFuncionarios = new FuncionarioNegocios();


           TempData["Mensaje"] = obj1.pruebaConexion(); ;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //destruimos tambien el objeto capturado en el login
            FuncionarioLogueado.CerrarSesion();

            return RedirectToAction("Login", "Auth");
        }

    }//fin class
}//fin space
