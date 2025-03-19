using Microsoft.AspNetCore.Mvc;
using Negocios;
using Modelos;
using System.Linq;
using System.Collections.Generic;

namespace SEDEP.Controllers
{
    public class EvaluacionController : Controller
    {
        //***********************************************************************************************
        //Objetos de la capa Negocios
        ConglomeradosNegocios objeto_ConglomeradosNegocios = new();
        FuncionarioNegocios objeto_FuncionarioNegocios = new();
        //***********************************************************************************************

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Pantalla para que la jefatura seleccione al subalterno a evaluar (EVA2).
        /// </summary>
        [HttpGet]
        public IActionResult SeleccionarSubalterno()
        {
            // Supongamos que la jefatura pertenece al Departamento ID = 1 (harcoded)
            // En un caso real, obtendrías el idDepartamento desde el login o sesión.
            int idDepartamento = 1;

            var listaSubalternos = objeto_FuncionarioNegocios.ObtenerFuncionariosPorDepartamento(idDepartamento);

            // Pasamos la lista de subalternos a la vista
            return View(listaSubalternos);
        }

        /// <summary>
        /// Acción que procesa la selección del subalterno y redirige a la evaluación (EVA3).
        /// </summary>
        [HttpPost]
        public IActionResult SeleccionarSubalterno(string cedulaSeleccionada)
        {
            if (string.IsNullOrEmpty(cedulaSeleccionada))
            {
                // Manejo simple: si no selecciona nada, podemos volver a la vista con un mensaje
                TempData["Error"] = "Debe seleccionar un subalterno.";
                return RedirectToAction("SeleccionarSubalterno");
            }

            // Redirige a la pantalla de evaluación (EVA3), pasando la cédula como parámetro
            return RedirectToAction("EvaluacionSubalterno", new { cedula = cedulaSeleccionada });
        }

        /// <summary>
        /// Pantalla de evaluación (EVA3) - ya existente, 
        /// pero ahora le añadimos la capacidad de recibir la cédula del subalterno.
        /// </summary>
        [HttpGet]
        public IActionResult EvaluacionSubalterno(string cedula)
        {
            if (!string.IsNullOrEmpty(cedula))
            {
                // Aquí puedes cargar datos del subalterno si lo deseas
                // var subalterno = objeto_FuncionarioNegocios.ConsultarFuncionarioID(cedula);

                // También podrías pasar subalterno a la vista
                // return View(subalterno);
            }

            // Si no hay cédula o no la usas, simplemente retornas la vista
            return View();
        }

        [HttpGet]
        public IActionResult ModificarPeso(string tipo, int obj, string objName, string pesoActual)
        {
            // Podrías guardar estos valores en un ViewBag, ViewData o en un modelo
            // Por simplicidad, usaremos ViewBag
            ViewBag.Tipo = tipo;
            ViewBag.Obj = obj;
            ViewBag.ObjName = objName;
            ViewBag.PesoActual = pesoActual;

            return View();
        }
    }
}