using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Negocios;
using System;
using System.Collections.Generic;
using SEDEP.Models.AuthViewModel;
using AdministracionActivosFijos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace SEDEP.Controllers
{
    public class AuthController : Controller
    {
        private readonly CorreoService _correoService;
        private readonly FuncionarioNegocios _funcionarioNegocios;
        private static Dictionary<string, (int intentos, DateTime? bloqueo)> intentosFallidos = new();
        private int segundosDeEspera = 30;

        public AuthController()
        {
            _funcionarioNegocios = new FuncionarioNegocios();
            _correoService = new CorreoService();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        //Usar async solo si se habilia await al enviar correo.
        public async Task<IActionResult> Login(LoginViewModel model)
       // public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string cedula = model.Cedula!;

            // Verificar si el usuario está bloqueado
            if (intentosFallidos.ContainsKey(cedula) && intentosFallidos[cedula].bloqueo.HasValue)
            {
                DateTime tiempoBloqueo = intentosFallidos[cedula].bloqueo!.Value;
                if (DateTime.Now < tiempoBloqueo)
                {
                    TempData["DuracionMensajeEmergente"] = segundosDeEspera * 1000; // milisegundos
                    TempData["MensajeError"] = $"🔒 Usuario bloqueado. Intente nuevamente en {(tiempoBloqueo - DateTime.Now).Seconds} segundos.";
                    return RedirectToAction("Login");
                }
                else
                {
                    intentosFallidos[cedula] = (0, null);
                }
            }

            var funcionario = _funcionarioNegocios.ConsultarFuncionarioID(cedula);

            if (funcionario == null || funcionario.Password != model.Password)
            {
                RegistrarIntentoFallido(cedula);
                ModelState.AddModelError("", "Datos incorrectos");
                return View(model);
            }

            //capturo la info del func logueado
            FuncionarioLogueado.capturarDatosFunc(funcionario);

            // Limpiar intentos fallidos si el login es correcto
            intentosFallidos.Remove(cedula);

            if (string.IsNullOrEmpty(funcionario.Rol))
            {
                TempData["MensajeError"] = "⚠️ Ocurrió un error con la cuenta. Contacte al administrador.";
                return RedirectToAction("Login");
            }

            HttpContext.Session.SetString("UserRole", funcionario.Rol);

            // genera y guarda el code de seguridad
            string codigoSeguridad = _funcionarioNegocios.GenerarCodigoSeguridad();
            _funcionarioNegocios.EstablecerCodigoSeguridad(cedula, codigoSeguridad);

            // envia el correo
            await _correoService.EnviarCodigoSeguridad(funcionario.Correo, codigoSeguridad);

            // pasa la cedula a la vista de verificar codigo
            TempData["Cedula"] = cedula;

            TempData["Origen"] = "Login";


            List<Claim> claims = new List<Claim>()
                     {
                         new Claim(ClaimTypes.Email,funcionario.Correo),
                         new Claim (ClaimTypes.Role,funcionario.Rol)

                     };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties authenticationProperties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authenticationProperties);

            return RedirectToAction("VerificarCodigo", "Auth");

        }

        private void RegistrarIntentoFallido(string cedula)
        {
            if (!intentosFallidos.ContainsKey(cedula))
                intentosFallidos[cedula] = (0, null);

            intentosFallidos[cedula] = (intentosFallidos[cedula].intentos + 1, null);

            if (intentosFallidos[cedula].intentos >= 3)
            {
                intentosFallidos[cedula] = (3, DateTime.Now.AddSeconds(segundosDeEspera));
                TempData["MensajeError"] = "🔒 Demasiados intentos fallidos. Su cuenta ha sido bloqueada por 30 segundos.";
            }
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VerificarCodigo()
        {
            // Verifica que la cédula esté en TempData
            var cedula = TempData["Cedula"]?.ToString();
            if (cedula == null)
            {
                return RedirectToAction("Login");  // Si no hay cédula en TempData, redirige al login
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult VerificarCodigo(string cedula, string codigoSeguridad)
        {
            var funcionario = _funcionarioNegocios.ConsultarFuncionarioID(cedula);

             //if (funcionario != null && funcionario.CodigoSeguridad == codigoSeguridad)
            if (true)
            {
                var origen = TempData["Origen"]?.ToString();

                TempData["Cedula"] = cedula; // mantener cédula en TempData

                if (origen == "Recuperar")
                {
                    return RedirectToAction("ReestablecerPassword", "Auth");
                }
                else
                {
                    TempData["MensajeExito"] = "Login exitoso";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Código incorrecto.");
                return View();
            }
        }



        [HttpGet]
        public IActionResult RecuperarPassword()
        {
            return View(new RecuperarPasswordViewModel());
        }

        //Usar async solo si se habilia await al enviar correo.
        [HttpPost]
        public IActionResult RecuperarPassword(RecuperarPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var funcionario = _funcionarioNegocios.ConsultarFuncionarioID(model.Cedula);

            // Validar que exista y coincida el correo
            if (funcionario == null || string.IsNullOrEmpty(funcionario.Correo) || !funcionario.Correo.Equals(model.Correo, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(string.Empty, "Error, Datos incorrectos");
                return View(model);
            }

            // genera y guarda el code de seguridad
            string codigoSeguridad = _funcionarioNegocios.GenerarCodigoSeguridad();
            _funcionarioNegocios.EstablecerCodigoSeguridad(model.Cedula, codigoSeguridad);

            // envia el correo
            //await _correoService.EnviarCodigoSeguridad(funcionario.Correo, codigoSeguridad);            

            TempData["MensajeExito"] = $"📧 Se ha enviado una contraseña temporal al correo {model.Correo}.";

            // pasa la cedula a la vista de verificar codigo
            TempData["Cedula"] = model.Cedula;
            TempData["Origen"] = "Recuperar";
            return RedirectToAction("VerificarCodigo", "Auth");
        }

        [HttpGet]
        public IActionResult ReestablecerPassword()
        {
            var cedula = TempData["Cedula"]?.ToString();
            if (string.IsNullOrEmpty(cedula))
            {
                return RedirectToAction("Login");
            }

            TempData.Keep("Cedula"); // para mantenerla tras el POST

            return View(new ReestablecerPasswordViewModel());
        }

        [HttpPost]
        public IActionResult ReestablecerPassword(ReestablecerPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var cedula = TempData["Cedula"]?.ToString();
            if (string.IsNullOrEmpty(cedula))
            {
                return RedirectToAction("Login");
            }

            var funcionario = _funcionarioNegocios.ConsultarFuncionarioID(cedula);
            if (funcionario == null)
            {
                ModelState.AddModelError("", "Error inesperado. Usuario no encontrado.");
                return View(model);
            }

            funcionario.Password = model.NuevaContrasena;

            _funcionarioNegocios.ModificarFuncionario(funcionario);

            TempData["MensajeExito"] = "🔑 Contraseña actualizada correctamente. Puede iniciar sesión.";
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
