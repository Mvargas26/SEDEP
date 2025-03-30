using Microsoft.AspNetCore.Mvc;
using Modelos;
using Negocios;

namespace SEDEP.Controllers
{
    public class MantenimientosController : Controller
    {
        //***********************************************************************************************
        //Objetos de la cap Negocios
        ConglomeradosNegocios objeto_ConglomeradosNegocios = new ConglomeradosNegocios();
        ObjetivoNegocios _objetivoNegocios = new ObjetivoNegocios();
        FuncionarioNegocios objeto_funcionario= new FuncionarioNegocios();

        //***********************************************************************************************
        #region FUNCIONARIOS

        public IActionResult FuncionariosManteni()
        {
            return View();
        }

        #endregion
        public IActionResult Index()
        {
            return View();
        }

        #region OBJETIVOS
        public IActionResult GestionObjetivos()
        {
            // Obtener la lista de objetivos desde la base de datos
            try
            {
                var objetivos = _objetivoNegocios.ListarObjetivos();
                return View(objetivos);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al obtener los objetivos: {ex.Message}";
                return View(new List<ObjetivoModel>());
            }
        }

        public IActionResult CrearNuevoObjetivo()
        {
            return View("CreaObjetivo", new ObjetivoModel());
        }

        [HttpPost]
        public IActionResult CrearObjetivo(ObjetivoModel nuevoObjetivo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _objetivoNegocios.CrearObjetivo(nuevoObjetivo);
                    return RedirectToAction(nameof(GestionObjetivos)); // Redirigir a la vista de gestión
                }
                else
                {
                    return View(nuevoObjetivo); // Mostrar errores de validación si los hay
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear el conglomerado: {ex.Message}";
                return View();
            }
        }

        public IActionResult EditaObjetivo(int id)
        {
            return View(_objetivoNegocios.ConsultarObjetivoID(id));

        }//fin EditaConglomerado

        [HttpPost]
        public IActionResult EditarObjetivo(ObjetivoModel objetivoEditado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _objetivoNegocios.ModificarObjetivo(objetivoEditado);
                    return RedirectToAction(nameof(GestionObjetivos)); // Redirigir a la lista de objetivos
                }
                else
                {
                    return View(objetivoEditado); // Mostrar errores de validación
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al actualizar el objetivo: {ex.Message}";
                return View(objetivoEditado);
            }
        }

        [HttpPost]
        public ActionResult BorraObjetivo(int id)
        {
            try
            {
                _objetivoNegocios.EliminarObjetivo(id);
                return RedirectToAction(nameof(GestionObjetivos));
            }
            catch
            {
                TempData["mensajeError"] = "No puede borrar este Conglomerado, verifique las relaciones.";
                return RedirectToAction(nameof(GestionObjetivos));
            }
        }
        #endregion

        #region DEPARTAMENTOS
        // Gestión de Departamentos (ya existente)
        public IActionResult ManteniDepartamentos()
        {
            var departamentos = new List<DepartamentoModel>
            {
                new DepartamentoModel { IdDepartamento = 1, Departamento = "Recursos Humanos" },
                new DepartamentoModel { IdDepartamento = 2, Departamento = "IT" }
            };
            return View(departamentos);
        }

        public IActionResult CreaDepartamento()
        {
            return View(new DepartamentoModel());
        }

        public IActionResult EditaDepartamento(int id)
        {
            var departamento = new DepartamentoModel { IdDepartamento = id, Departamento = "IT" };
            return View(departamento);
        }
        #endregion

        #region CONGLOMERADOS
        // Gestión de Conglomerados (Nuevo)
        public IActionResult ManteniConglomerados()
        {
            try
            {
                var conglomerados = objeto_ConglomeradosNegocios.ListarConglomerados();
                return View(conglomerados);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al obtener los conglomerados: {ex.Message}";
                return View(new List<ConglomeradoModel>());
            }
        }//fin ManteniConglomerados
         // Gestión de Funcionarios
        public IActionResult ManteniFuncionarios()
        {
            try
            {
               
                var funcionarios = objeto_funcionario.ListarFuncionarios();
                return View(funcionarios);
            }
            catch (Exception ex)
            {
                // Si ocurre un error, muestra un mensaje de error
                TempData["ErrorMessage"] = $"Error al obtener los funcionarios: {ex.Message}";
                return View(new List<FuncionarioModel>()); // Retorna una lista vacía si ocurre un error
            }
        }
        public IActionResult funcionario()
        {
            return View(new FuncionarioModel());
        }
        public IActionResult CrearNuevoConglomerado()
        {
            return View(new ConglomeradoModel());
        }

        [HttpPost]
        public IActionResult CrearNuevoConglomerado(IFormCollection collection)
        {
            try
            {
                ConglomeradoModel newConglomerado = new ConglomeradoModel

                {
                    IdConglomerado = Convert.ToInt32(collection["IdConglomerado"]),
                    NombreConglomerado = collection["NombreConglomerado"],
                    Descripcion = collection["Descripcion"]
                };

                objeto_ConglomeradosNegocios.CrearConglomerado(newConglomerado);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear el conglomerado: {ex.Message}";
                return View();
            }
        }


        public IActionResult EditaConglomerado(int id)
        {
            return View(objeto_ConglomeradosNegocios.ConsultarConglomeradoID(id));

        }//fin EditaConglomerado

        [HttpPost]
        public IActionResult EditaConglomerado(int id, IFormCollection collection)
        {
            try
            {
                ConglomeradoModel conglomeradoEditar = new()
                {
                    IdConglomerado = id,
                    NombreConglomerado = collection["NombreConglomerado"],
                    Descripcion = collection["Descripcion"]
                };

                objeto_ConglomeradosNegocios.ModificarConglomerado(conglomeradoEditar);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                return View();
            }


        }//fin EditaConglomerado

        public ActionResult BorrarConglomerado(int id)
        {
            return View(objeto_ConglomeradosNegocios.ConsultarConglomeradoID(id));
        }

        [HttpPost]
        public ActionResult BorrarConglomerado(int id, IFormCollection collection)
        {
            try
            {
                objeto_ConglomeradosNegocios.EliminarConglomerado(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["mensajeError"] = "No puede borrar este Conglomerado, verifique las relaciones.";
                return View();
            }
        }

        public IActionResult InformativaConglomerado()
        {
            try
            {
                // obtiene lista conglomerados
                var conglomerados = objeto_ConglomeradosNegocios.ListarConglomerados();
                return View(conglomerados);  // pasa los conglomerados a la vista
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al obtener los conglomerados: {ex.Message}";
                return View(new List<ConglomeradoModel>());
            }
        }
        #endregion

        #region Puestos

        // Datos "quemados"
        private static List<PuestoModel> puestos = new List<PuestoModel>
        {
            new PuestoModel { idPuesto = 1, Puesto = "Gerente de TI" },
            new PuestoModel { idPuesto = 2, Puesto = "Desarrollador" },
            new PuestoModel { idPuesto = 3, Puesto = "Analista de Sistemas" }
        };

        // Vista para listar puestos
        public IActionResult ManteniPuestos()
        {
            return View(puestos);
        }

        // Vista para crear un puesto
        public IActionResult CreaPuesto()
        {
            return View();
        }

        // Acción para crear un puesto
        [HttpPost]
        public IActionResult CreaPuestos(string nombrePuesto)
        {
            if (!string.IsNullOrEmpty(nombrePuesto) && !puestos.Any(p => p.Puesto == nombrePuesto))
            {
                var nuevoPuesto = new PuestoModel
                {
                    idPuesto = puestos.Max(p => p.idPuesto) + 1,
                    Puesto = nombrePuesto
                };
                puestos.Add(nuevoPuesto);
                TempData["Mensaje"] = $"Puesto {nombrePuesto} creado correctamente.";
                return RedirectToAction("GestionPuestos");
            }

            TempData["Error"] = "El nombre del puesto no puede estar vacío o duplicado.";
            return View();
        }

        // Vista para editar un puesto
        public IActionResult EditaPuesto(int id)
        {
            var puesto = puestos.FirstOrDefault(p => p.idPuesto == id);
            if (puesto == null)
            {
                return NotFound();
            }

            return View(puesto);
        }

        // Acción para modificar un puesto
        [HttpPost]
        public IActionResult EditaPuesto(int id, string nombrePuesto)
        {
            var puesto = puestos.FirstOrDefault(p => p.idPuesto == id);
            if (puesto == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(nombrePuesto) && !puestos.Any(p => p.Puesto == nombrePuesto))
            {
                puesto.Puesto = nombrePuesto;
                TempData["Mensaje"] = $"Puesto {nombrePuesto} modificado correctamente.";
                return RedirectToAction("GestionPuestos");
            }

            TempData["Error"] = "El nombre del puesto no puede estar vacío o duplicado.";
            return View(puesto);
        }

        // Acción para eliminar un puesto
        public IActionResult EliminarPuesto(int id)
        {
            var puesto = puestos.FirstOrDefault(p => p.idPuesto == id);
            if (puesto == null)
            {
                return NotFound();
            }

            puestos.Remove(puesto);
            TempData["Mensaje"] = $"Puesto {puesto.Puesto} eliminado correctamente.";
            return RedirectToAction("GestionPuestos");
        }


        #endregion


    }//fin class

    // lo ideal es crearlo en modelos, pero lo puse aqui solo para probar el front
    //public class ObjetivoModel
    //{
    //    public int Id { get; set; }
    //    public string Nombre { get; set; }
    //    public int Porcentaje { get; set; } // Entre 0 y 100
    //    public string Tipo { get; set; } // Estratégico, Operativo, Táctico
    //}

}//fin space
