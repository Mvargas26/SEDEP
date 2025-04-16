using Microsoft.AspNetCore.Mvc;
using Negocios;
using Modelos;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using AdministracionActivosFijos;

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
        EvaluacionXcompetenciaNegocios objeto_EvaXcompetencia = new();

        // Propiedad que devuelve la fecha actual en CR (se calcula cada vez que se accede)
        private DateTime FechaCostaRica =>
       TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
       TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time")).Date;

        public IActionResult Index()
        {
            return View();
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
        public IActionResult ModificarActual(string tipo, string obj, string objName, string actualValue, string metaValue)
        {
            ViewBag.Tipo = tipo;
            ViewBag.Obj = obj;
            ViewBag.ObjName = objName;
            ViewBag.ActualValue = actualValue;
            ViewBag.MetaValue = metaValue;
            return View();
        }

        #region Planificacion

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

                //cargamos la data a un objeto nuevo
                EvaluacionModel newEvaluacion = new()
                {
                    IdFuncionario = cedulaFuncionario,
                    EstadoEvaluacion = 1, //1 = estado planificacion
                    FechaCreacion = FechaCostaRica,
                    Observaciones = observaciones,
                    idConglomerado = idConglo
                };

                //Guardamos y obtenemos el nuevo registro para sacar el id
                EvaluacionModel evaluacionGuardada = objeto_Evaluaciones.CrearEvaluacion(newEvaluacion);

                // con el idEvaluacion Cargamos los objetivos
                List<EvaluacionXObjetivoModel> listaObjetivosXEvaluacion = new List<EvaluacionXObjetivoModel>();

                foreach (var objetivo in objetivos)
                {
                    listaObjetivosXEvaluacion.Add(new EvaluacionXObjetivoModel
                    {
                        IdEvaluacion = evaluacionGuardada.IdEvaluacion,
                        IdObjetivo = Convert.ToInt32(objetivo["id"]),
                        ValorObtenido = Convert.ToDecimal(objetivo["actual"]),
                        Peso = Convert.ToDecimal(objetivo["peso"]),
                        Meta = objetivo["meta"].ToString()

                    });
                }

                //Guardamos los objetivos de la tabla
                foreach (EvaluacionXObjetivoModel new_EvaXObj in listaObjetivosXEvaluacion)
                {
                    objeto_EvaXObjetivo.CrearEvaluacionXObjetivo(new_EvaXObj);
                }

                //Despues las competencias
                List<EvaluacionXcompetenciaModel> listaCompetencias = new List<EvaluacionXcompetenciaModel>();

                foreach (var competencia in competencias)
                {
                    listaCompetencias.Add(new EvaluacionXcompetenciaModel
                    {
                        IdEvaluacion = evaluacionGuardada.IdEvaluacion,
                        IdCompetencia = Convert.ToInt32(competencia["id"]),
                        ValorObtenido = Convert.ToDecimal(competencia["actual"]),
                        Peso = Convert.ToDecimal(competencia["peso"]),
                        Meta = competencia["meta"].ToString()

                    });
                }

                //Guardamos las competencias de la tabla
                foreach (EvaluacionXcompetenciaModel new_EvaXCompe in listaCompetencias)
                {
                    objeto_EvaXcompetencia.CrearEvaluacionXCompetencia(new_EvaXCompe);
                }

                return Json(new { success = true, redirectUrl = Url.Action("Index", "Evaluacion") });
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
        
        #endregion

        #region EvaluacionComoFuncionario

        [HttpGet]
        public IActionResult ElegirCongloComoFuncionario()
        {
            try
            {
                //Esto se debe capturar en el login
                FuncionarioModel newFuncionarioLogin = new();
                //newFuncionarioLogin = FuncionarioLogueado.retornarDatosFunc();

                //Eliminar cuando el login este activo
                newFuncionarioLogin.Cedula = "123456789";


                newFuncionarioLogin.IdDepartamento = 1;
                ViewData["ListaConglomerados"] = objeto_ConglomeradosNegocios.ListarConglomerados();
                return View(objeto_ConglomeradosNegocios.ConsultarConglomeradoXFuncionario(newFuncionarioLogin.Cedula));
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult RealizarEvaluacionComoFuncionario(string cedula, int idConglomerado)
        {

            try
            {
                if (string.IsNullOrEmpty(cedula) || idConglomerado == 0)
                {
                    TempData["Error"] = "Debe seleccionar un Conglomerado para el funcionario a evaluar.";
                    return RedirectToAction("SeleccionarSubalterno");
                }
                if (!string.IsNullOrEmpty(cedula))
                {
                    //Traemos la info necesaria para la vista
                    var subalterno = objeto_FuncionarioNegocios.ConsultarFuncionarioID(cedula);
                    var PesosConglomerados = objeto_ConglomeradosNegocios.ConsultarPesosXConglomerado(idConglomerado);
                    ViewBag.anioActual = FechaCostaRica.Year;
                    ViewBag.PesosConglomerados = PesosConglomerados;
                    ViewBag.IdConglomerado = idConglomerado;
                    ViewData["ListaConglomerados"] = objeto_ConglomeradosNegocios.ListarConglomerados();
                    ViewData["ListaTiposObjetivos"] = objeto_TiposObjetivoNegocios.ListarTiposObjetivos();
                    ViewData["ListaTiposCompetencias"] = objeto_TiposCompenNegocis.ListarTiposCompetencias();


                    //Traemos la Evaluacion
                    EvaluacionModel ultimaEvaluacion = new();
                    ultimaEvaluacion = objeto_Evaluaciones.ConsultarEvaluacionComoFuncionario(cedula, idConglomerado);

                    //Validamos que tenga una Evaluacion
                    if (ultimaEvaluacion == null)
                    {
                        TempData["AlertMessage"] = "No hay una evaluación para usted en este conglomerado.Por favor contacte a su Jefatura para planificarla.";
                        return RedirectToAction("Index");
                    }

                    //Traemos la listas de obj y comp relacionadas a este conglomerado
                    var (listaObjetivos, listaCompetencias) = objeto_Evaluaciones.Listar_objetivosYCompetenciasXEvaluacion(ultimaEvaluacion.IdEvaluacion);
                    ViewBag.ListaObjetivos = listaObjetivos;
                    ViewBag.ListaCompetencias = listaCompetencias;
                    ViewBag.idEvaluacion = ultimaEvaluacion.IdEvaluacion;

                    return View(subalterno);
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = $"Error al cargar la evaluación: {ex.Message}";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public IActionResult RealizarEvaluacionComoFuncionario([FromBody] dynamic evaluacionData)
        {
            try
            {
                // Convertir a JObject para manipular más fácilmente
                var jsonData = JObject.Parse(evaluacionData.ToString());

                // Acceder a los arrays
                var objetivosAsignados = jsonData["objetivosAsignados"];
                var competenciasAsignadas = jsonData["competenciasAsignadas"];
                int idEvaluacion = ((JObject)jsonData)["idEvaluacion"]?.Value<int>() ?? 0;
                string cedulaFuncionario = (string)jsonData["cedFuncionario"] ?? string.Empty;
                int idConglo = ((JObject)jsonData)["idConglo"]?.Value<int>() ?? 0;

                //Traemos la Evaluacion
                EvaluacionModel ultimaEvaluacion = new();
                ultimaEvaluacion = objeto_Evaluaciones.ConsultarEvaluacionComoFuncionario(cedulaFuncionario, idConglo);

                //lista de las evaluaciones a Modificar
                var objetivosAModificar = new List<EvaluacionXObjetivoModel>();

                foreach (var objetivo in objetivosAsignados)
                {
                    objetivosAModificar.Add(new EvaluacionXObjetivoModel
                    {
                        IdEvaxObj = Convert.ToInt32(objetivo.id),
                        ValorObtenido = Convert.ToDecimal(objetivo.actual)
                    });
                }

                //ista de las competencias a Evaluar
                var competenciasAModificar = new List<EvaluacionXcompetenciaModel>();

                foreach (var competencia in competenciasAsignadas)
                {
                    competenciasAModificar.Add(new EvaluacionXcompetenciaModel
                    {
                        IdEvaxComp = Convert.ToInt32(competencia.id),
                        ValorObtenido = Convert.ToDecimal(competencia.actual)
                    });
                }

                //modificamos los valores de objetivos en la BD
                foreach (var objetivo in objetivosAModificar)
                {
                    //traemos el objetivo que esta en la db
                    EvaluacionXObjetivoModel objetivoAlterado = objeto_EvaXObjetivo.ConsultarEvaluacionXObjetivoPorID(objetivo.IdEvaxObj);
                    //solo le cambiamos el valor actual
                    objetivoAlterado.ValorObtenido = objetivo.ValorObtenido;
                    //lo guardamos nuevamente
                    objeto_EvaXObjetivo.ModificarEvaluacionXObjetivo(objetivoAlterado);

                }

                //modificamos los valores de Competencias en la BD
                foreach (var competencia in competenciasAModificar)
                {
                    //traemos el  que esta en la db
                    EvaluacionXcompetenciaModel CompetenciaAlterada = objeto_EvaXcompetencia.consultarEvaXCompPorID(competencia.IdEvaxComp);
                    //solo le cambiamos el valor actual
                    CompetenciaAlterada.ValorObtenido = competencia.ValorObtenido;
                    //lo guardamos nuevamente
                    objeto_EvaXcompetencia.ActualizarEvaluacionXCompetencia(CompetenciaAlterada);

                }




                return Json(new { success = true, redirectUrl = Url.Action("Index", "Evaluacion") });

            }
            catch (Exception ex)
            {

                return Json(new { success = false, error = ex.Message });
            }

        }


        #endregion
    }
}
