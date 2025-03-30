using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SEDEP.Models;
using Negocios;
using System;
using System.Collections.Generic;

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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string cedula = model.Cedula;

            // Verificar si el usuario está bloqueado
            if (intentosFallidos.ContainsKey(cedula) && intentosFallidos[cedula].bloqueo.HasValue)
            {
                DateTime tiempoBloqueo = intentosFallidos[cedula].bloqueo.Value;
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

            // Consultar el funcionario en la base de datos
            var funcionario = _funcionarioNegocios.ConsultarFuncionarioID(cedula);

            if (funcionario == null || funcionario.Password != model.Password)
            {
                RegistrarIntentoFallido(cedula);
                ModelState.AddModelError("", "Datos incorrectos");
                return View(model);
            }



            // Limpiar intentos fallidos si el login es correcto
            intentosFallidos.Remove(cedula);

            // Verificación extra: si no tiene rol definido, mostrar error genérico
            if (string.IsNullOrEmpty(funcionario.Rol))
            {
                TempData["MensajeError"] = "⚠️ Ocurrió un error con la cuenta. Contacte al administrador.";
                return RedirectToAction("Login");
            }

            TempData["MensajeExito"] = $"✅ Inicio de sesión exitoso. Bienvenido, {funcionario.Rol}.";
            HttpContext.Session.SetString("UserRole", funcionario.Rol);

            // genera y guarda el code de seguridad
            string codigoSeguridad = _funcionarioNegocios.GenerarCodigoSeguridad();
            _funcionarioNegocios.EstablecerCodigoSeguridad(cedula, codigoSeguridad);

            // envia el correo
            await _correoService.EnviarCodigoSeguridad(funcionario.Correo, codigoSeguridad);

            // pasa la cedula a la vista de verificar codigo
            TempData["Cedula"] = cedula;

            // redirigfe a la vista
            return RedirectToAction("VerificarCodigo", "Auth");


            // En producción podrías usar esto:
            /*
            return funcionario.Rol switch
            {
                "Administración" => RedirectToAction("DashboardAdmin", "Home"),
                "Jefatura" => RedirectToAction("DashboardJefatura", "Home"),
                "SubAlterno" => RedirectToAction("DashboardUser", "Home"),
                _ => RedirectToAction("Login")
            };
            */
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

            return View();
        }

        [HttpPost]
        public IActionResult VerificarCodigo(string cedula, string codigoSeguridad)
        {
            // consultar al funcionario utilizando la cedula
            var funcionario = _funcionarioNegocios.ConsultarFuncionarioID(cedula);

            // compara el codigo de seguridad del correo y el ingresado
            if (funcionario != null && funcionario.CodigoSeguridad == codigoSeguridad)
            {
                //si el codigo es correcto redirige al index
                return RedirectToAction("Index", "Home");
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

            // Simulación
            string passwordTemporal = GenerarPasswordTemporal();
            
            
            // Llamar a un servicio real de correo

            TempData["MensajeExito"] = $"📧 Se ha enviado una contraseña temporal al correo {model.Correo}.";
            TempData["DuracionMensajeEmergente"] = 8000;
            ModelState.AddModelError(string.Empty, "Se ha enviado un correo");

            return RedirectToAction("Login");
        }

        [HttpGet]
        private string GenerarPasswordTemporal()
        {
            var caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var random = new Random();
            var clave = new char[8];
            for (int i = 0; i < clave.Length; i++)
            {
                clave[i] = caracteres[random.Next(caracteres.Length)];
            }
            return new string(clave);
        }

    }
}
