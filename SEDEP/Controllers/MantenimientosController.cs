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
        DepartamentosNegocio objeto_departamento = new DepartamentosNegocio();

        //***********************************************************************************************
        #region FUNCIONARIOS
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
        [HttpGet]
        // pantalla para nuevo funcionario{
        public IActionResult CrearNuevoFuncionario()
        {
            return View(new FuncionarioModel());
        }
        // Crear nuevo funcionario
        [HttpPost]
        public IActionResult CrearNuevoFuncionario(IFormCollection collectionn)
        {
            try
            {
                FuncionarioModel newFuncionario = new FuncionarioModel
                {
                    Cedula = collectionn["Cedula"],
                    Nombre = collectionn["Nombre"],
                    Apellido1 = collectionn["Apellido1"],
                    Apellido2 = collectionn["Apellido2"],
                    Correo = collectionn["Correo"],
                    Password = collectionn["Password"],
                    IdDepartamento = Convert.ToInt32(collectionn["IdDepartamento"]),
                    IdRol = Convert.ToInt32(collectionn["IdRol"]),
                    IdPuesto = Convert.ToInt32(collectionn["IdPuesto"]),
                    IdEstadoFuncionario = Convert.ToInt32(collectionn["IdEstadoFuncionario"])
                };
                objeto_funcionario.CrearFuncionario(newFuncionario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear el funcionario: {ex.Message}";
                return View();
            }
        }
        // Editar funcionario
        [HttpGet("Mantenimientos/EditaFuncionario/{cedula}")]
        public IActionResult EditaFuncionario(string cedula)
        {
            return View(objeto_funcionario.ConsultarFuncionarioID(cedula));
        }
        [HttpPost]
        public IActionResult EditaFuncionario(string cedula, IFormCollection collection)
        {
            try
            {
                FuncionarioModel funcionarioEditar = new FuncionarioModel
                {
                    Cedula = cedula,
                    Nombre = collection["Nombre"],
                    Apellido1 = collection["Apellido1"],
                    Apellido2 = collection["Apellido2"],
                    Correo = collection["Correo"],
                    Password = collection["Password"],
                    Departamento = collection["Departamento"],
                    Rol = collection["Rol"],
                    Puesto = collection["Puesto"],
                    Estado = collection["EstadoFuncionario"]
                };
                objeto_funcionario.ModificarFuncionario(funcionarioEditar);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al editar el funcionario: {ex.Message}";
                return View();
            }
        }
        // Borrar funcionario
        [HttpGet("Mantenimientos/BorrarFuncionario/{cedula}")]
        public IActionResult BorrarFuncionario(string cedula)
        {
            return View(objeto_funcionario.ConsultarFuncionarioID(cedula));
        }
        [HttpPost("Mantenimientos/BorrarFuncionario/{cedula}")]
        public IActionResult BorrarFuncionario(string cedula, IFormCollection collection)
        {
            try
            {
                objeto_funcionario.EliminarFuncionario(cedula);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al borrar el funcionario: {ex.Message}";
                return View();
            }
        }


        #endregion
        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion
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
            try { 

            var departamentos = objeto_departamento.ListarDepartamentos();
            return View(departamentos);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al obtener los departamentos: {ex.Message}";
                return View(new List<DepartamentoModel>());
            }
        }
        
        //crear departamento

        public IActionResult CreaDepartamento()
        {
            return View(new DepartamentoModel());
        }
        [HttpPost]
        public IActionResult CreaDepartamento(IFormCollection collection)
        {
            try
            {
                DepartamentoModel newDepartamento = new DepartamentoModel
                {
                    Departamento = collection["Departamento"]
                };
                objeto_departamento.CrearDepartamento(newDepartamento);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear el departamento: {ex.Message}";
                return View();
            }
        }
        //editar departamento
        [HttpGet]
        public IActionResult EditaDepartamento(int id)
        {
            return View(objeto_departamento.ConsultarDepartamentoID(id));
        }
        [HttpPost]
        public IActionResult EditaDepartamento(int id, IFormCollection collection)
        {
            try
            {
                DepartamentoModel departamentoEditar = new DepartamentoModel
                {
                    IdDepartamento = id,
                    Departamento = collection["Departamento"]
                };
                objeto_departamento.ModificarDepartamento(departamentoEditar);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al editar el departamento: {ex.Message}";
                return View();
            }
        }
        //borrar departamento
        [HttpGet]
        public IActionResult BorrarDepartamento(int id)
        {
            return View(objeto_departamento.ConsultarDepartamentoID(id));
        }
        [HttpPost]
        public IActionResult BorrarDepartamento(int id, IFormCollection collection)
        {
            try
            {
                objeto_departamento.EliminarDepartamento(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al borrar el departamento: {ex.Message}";
                return View();
            }
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
