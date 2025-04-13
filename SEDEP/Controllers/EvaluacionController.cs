using Microsoft.AspNetCore.Mvc;
using Negocios;
using Modelos;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SEDEP.Controllers
{
    public class EvaluacionController : Controller
    {
        // Objetos de la capa Negocios
        ConglomeradosNegocios objeto_ConglomeradosNegocios = new();
        FuncionarioNegocios objeto_FuncionarioNegocios = new();
        TiposObjetivosNegocios objeto_TiposObjetivoNegocios = new();
        TiposCompetenciasNegocios objeto_TiposCompenNegocis = new();
        EvaluacionesNegocio objeto_Evaluaciones = new();
        EvaluacionXObjetivosNegocio objeto_EvaXObjetivo = new();
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
            int idDepartamento = 1; // Harcoded para ejemplo
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

                //Traemos la listas de obj y comp relacionadas a este conglomerado
                var (listaObjetivos, listaCompetencias) = objeto_Evaluaciones.ListarObjYCompetenciasXConglomerado(idConglomerado);
                ViewBag.ListaObjetivos = listaObjetivos;
                ViewBag.ListaCompetencias = listaCompetencias;

                ViewBag.PesosConglomerados = PesosConglomerados;
                ViewBag.IdConglomerado = idConglomerado;
                ViewData["ListaConglomerados"] = objeto_ConglomeradosNegocios.ListarConglomerados();
                ViewData["ListaTiposObjetivos"] = objeto_TiposObjetivoNegocios.ListarTiposObjetivos();
                ViewData["ListaTiposCompetencias"] = objeto_TiposCompenNegocis.ListarTiposCompetencias();
                return View(subalterno);
            }
            return View();
        }

        [HttpPost]
        public IActionResult EvaluacionSubalterno([FromBody] dynamic evaluacionData)
        {
            try
            {
                // Convertir a JObject para manipular más fácilmente
                var jsonData = JObject.Parse(evaluacionData.ToString());

                // Acceder a los arrays
                var objetivos = jsonData["objetivos"];
                var competencias = jsonData["competencias"];
                string observaciones = (string)jsonData["observaciones"] ?? string.Empty;
                string cedulaFuncionario = (string)jsonData["cedFuncionario"] ?? string.Empty;
                int idConglo = ((JObject)jsonData)["idConglo"]?.Value<int>() ?? 0;


                // Obtener la zona horaria de Costa Rica
                TimeZoneInfo costaRicaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time");

                // Convertir UTC a hora de Costa Rica y obtener solo la fecha
                DateTime fechaCR = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, costaRicaTimeZone).Date;

                //cargamos la data a un objeto nuevo
                EvaluacionModel newEvaluacion = new() { 
                IdFuncionario = cedulaFuncionario,
                EstadoEvaluacion = 1, //1 = estado planificacion
                FechaCreacion = fechaCR,
                Observaciones = observaciones
                };

                //Guardamos y obtenemos el nuevo registro para sacar el id
                EvaluacionModel evaluacionGuardada = objeto_Evaluaciones.CrearEvaluacion(newEvaluacion);

                // con el idEvaluacion Cargamos los objetivos
                List<EvaluacionXObjetivoModel> listaObjetivos = new List<EvaluacionXObjetivoModel>();

                foreach (var objetivo in objetivos)
                {
                    listaObjetivos.Add(new EvaluacionXObjetivoModel
                    {
                        IdEvaluacion = evaluacionGuardada.IdEvaluacion,
                        IdObjetivo = Convert.ToInt32( objetivo["id"]),
                        ValorObtenido = Convert.ToDecimal(objetivo["actual"]),
                        peso = Convert.ToDecimal(objetivo["peso"]),
                        meta = objetivo["meta"].ToString()

                    });
                }

                //Guardamos los objetivos de la tabla
                foreach (EvaluacionXObjetivoModel new_EvaXObj in listaObjetivos)
                {
                    objeto_EvaXObjetivo.CrearEvaluacionXObjetivo(new_EvaXObj);
                }




                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
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

        [HttpGet]
        public IActionResult CrearSeguimiento()
        {
            // En un escenario real, aquí cargarías la lista de objetivos
            // que el Encargado definió, y se los pasas a la vista
            // Por ahora, simplemente retornamos la vista con datos quemados
            return View();
        }

        [HttpGet]
        public IActionResult ModificarActualSeguimiento(string tipo, string obj, string objName, string actualValue, string metaValue)
        {
            ViewBag.Tipo = tipo;
            ViewBag.Obj = obj;
            ViewBag.ObjName = objName;
            ViewBag.ActualValue = actualValue;
            ViewBag.MetaValue = metaValue;
            return View();
        }

        [HttpGet]
        public IActionResult RealizarEvaluacionComoFuncionario()
        {
            // En un escenario real, aquí cargarías la lista de objetivos
            // que el Encargado definió, y se los pasas a la vista
            // Por ahora, simplemente retornamos la vista con datos quemados
            return View();
        }

        [HttpGet]
        public IActionResult ModificarActual(string tipo, string obj, string objName, string actualValue, string metaValue)
        {
            ViewBag.Tipo = tipo;
            ViewBag.Obj = obj;
            ViewBag.ObjName = objName;
            ViewBag.ActualValue = actualValue;
            ViewBag.MetaValue = metaValue;
            return View();
        }
    }
}
