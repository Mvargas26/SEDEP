using Microsoft.AspNetCore.Mvc;
using Negocios;
using Modelos;
using System.Linq;
using System.Collections.Generic;

namespace SEDEP.Controllers
{
    public class EvaluacionController : Controller
    {
        // Objetos de la capa Negocios
        ConglomeradosNegocios objeto_ConglomeradosNegocios = new();
        FuncionarioNegocios objeto_FuncionarioNegocios = new();
        TiposObjetivosNegocios objeto_TiposObjetivoNegocios = new();
        TiposCompetenciasNegocios objeto_TiposCompenNegocis = new();

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
            int idDepartamento = 1; // Simulación: id de departamento harcoded
            var listaSubalternos = objeto_FuncionarioNegocios.ObtenerFuncionariosPorDepartamento(idDepartamento);
            return View(listaSubalternos);
        }

        [HttpPost]
        public IActionResult SeleccionarSubalterno(string cedulaSeleccionada)
        {
            if (string.IsNullOrEmpty(cedulaSeleccionada))
            {
                TempData["Error"] = "Debe seleccionar un subalterno.";
                return RedirectToAction("SeleccionarSubalterno");
            }
            return RedirectToAction("ConglomeradosPorFunc", new { cedula = cedulaSeleccionada });
        }

        /// <summary>
        /// Pantalla de evaluación (EVA3) – carga datos según la cédula y el id de conglomerado.
        /// </summary>
        [HttpGet]
        public IActionResult EvaluacionSubalterno(string cedula, int idConglomerado)
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
    }
}
