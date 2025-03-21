using Microsoft.AspNetCore.Mvc;
using Negocios;
using Modelos;
using System.Linq;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace SEDEP.Controllers
{
    public class EvaluacionController : Controller
    {
        //***********************************************************************************************
        //Objetos de la capa Negocios
        ConglomeradosNegocios objeto_ConglomeradosNegocios = new();
        FuncionarioNegocios objeto_FuncionarioNegocios = new();
        TiposObjetivosNegocios objeto_TiposObjetivoNegocios = new();
        TiposCompetenciasNegocios objeto_TiposCompenNegocis = new();
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
            return RedirectToAction("ConglomeradosPorFunc", new { cedula = cedulaSeleccionada });
        }

        /// <summary>
        /// Pantalla de evaluación (EVA3) - ya existente, 
        /// pero ahora le añadimos la capacidad de recibir la cédula del subalterno.
        /// </summary>
        [HttpGet]
        public IActionResult EvaluacionSubalterno(string cedula,int idConglomerado)
        {
            if (string.IsNullOrEmpty(cedula) || idConglomerado == 0)
            {
                TempData["Error"] = "Debe seleccionar un Conglomerado para el funcionario a evaluar.";
                return RedirectToAction("SeleccionarSubalterno");
            }

            if (!string.IsNullOrEmpty(cedula))
            {
                 var subalterno = objeto_FuncionarioNegocios.ConsultarFuncionarioID(cedula);

                var PesosConglomerados = objeto_ConglomeradosNegocios.ConsultarPesosXConglomerado(idConglomerado);

                ViewBag.PesosConglomerados = PesosConglomerados;
                ViewBag.IdConglomerado = idConglomerado;
                ViewData["ListaConglomerados"] = objeto_ConglomeradosNegocios.ListarConglomerados();
                ViewData["ListaTiposObjetivos"] = objeto_TiposObjetivoNegocios.ListarTiposObjetivos();
                ViewData["ListaTiposCompetencias"] = objeto_TiposCompenNegocis.ListarTiposCompetencias();

                return View(subalterno);
            }

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

        [HttpGet]
        public IActionResult ConglomeradosPorFunc(string cedula)
        {
            try
            {
                ViewData["ListaConglomerados"] = objeto_ConglomeradosNegocios.ListarConglomerados();

                return View(objeto_ConglomeradosNegocios.ConsultarConglomeradoXFuncionario(cedula));

            }
            catch (Exception)
            {
                return View();

            }
        }

        
    }//fin class
}//fin space