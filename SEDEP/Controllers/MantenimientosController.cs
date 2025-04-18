using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using Negocios;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SEDEP.Controllers
{
    public class MantenimientosController : Controller
    {
        //***********************************************************************************************
        //Objetos de la cap Negocios
        ConglomeradosNegocios objeto_ConglomeradosNegocios = new ConglomeradosNegocios();
        ObjetivoNegocios _objetivoNegocios = new ObjetivoNegocios();
        FuncionarioNegocios objeto_funcionario= new FuncionarioNegocios();
        MetasNegocios _objetoMeta = new MetasNegocios();
        CompetenciasNegocio _objetoCompe = new CompetenciasNegocio();
        DepartamentosNegocio objeto_departamento = new DepartamentosNegocio();
        PuestosNegocio _objetoPuesto = new PuestosNegocio();
        FuncionarioXConglomeradoNegocios objeto_funcionarioXConglomerado = new FuncionarioXConglomeradoNegocios();
        PeriodosEvaluacionNegocio _objetoPeriodo = new PeriodosEvaluacionNegocio();
        RolesNegocios objeto_rol = new RolesNegocios();
        TiposCompetenciasNegocios objeto_TiposCompenNegocis = new();

        //***********************************************************************************************
        #region FUNCIONARIOS
        // Gestión de Funcionarios
        [Authorize(Roles = "Administración")]

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
                TempData["MensajeError"] = $"Error al obtener los funcionarios: {ex.Message}";
                return View(new List<FuncionarioModel>()); // Retorna una lista vacía si ocurre un error
            }
        }

        [Authorize(Roles = "Administración")]

        [HttpGet]
        public IActionResult CrearNuevoFuncionario()
        {
            var puestos = _objetoPuesto.ObtenerPuestos();
            var conglomerados = objeto_ConglomeradosNegocios.ListarConglomerados();
            var departamentos = objeto_departamento.ListarDepartamentos();
            var roles = objeto_rol.ListarRoles(); // Obtener la lista de roles
            var funcionario = new FuncionarioModel(); // Inicializa un nuevo objeto FuncionarioModel
            FuncionarioViewModel viewModel = new FuncionarioViewModel
            {
                Funcionario = funcionario, 
                Puestos = puestos,
                Conglomerados = conglomerados,
                Departamentos = departamentos,
                Roles = roles // Asigna la lista de roles al ViewModel
            };

            return View(viewModel);
        }

        // Crear nuevo funcionario
        [HttpPost]
        public IActionResult CrearNuevoFuncionario(IFormCollection collectionn)
        {
            try
            {
                FuncionarioModel newFuncionario = new FuncionarioModel
                {
                    Cedula = collectionn["Funcionario.Cedula"]!,
                    Nombre = collectionn["Funcionario.Nombre"]!,
                    Apellido1 = collectionn["Funcionario.Apellido1"]!,
                    Apellido2 = collectionn["Funcionario.Apellido2"]!,
                    Correo = collectionn["Funcionario.Correo"]!,
                    Password = collectionn["Funcionario.Password"]!,
                    IdDepartamento = Convert.ToInt32(collectionn["Funcionario.IdDepartamento"]),
                    IdRol = Convert.ToInt32(collectionn["Funcionario.IdRol"]),
                    IdPuesto = Convert.ToInt32(collectionn["Funcionario.IdPuesto"]),
                    IdEstadoFuncionario = Convert.ToInt32(collectionn["Funcionario.IdEstadoFuncionario"])
                };

                objeto_funcionario.CrearFuncionario(newFuncionario);
                string idFuncionario = newFuncionario.Cedula;
                // Se obtienen los conglomerados seleccionados y puende ser multiples 

                var conglomeradosSeleccionados = collectionn["IdConglomeradosSeleccionados"];

                foreach (var id in conglomeradosSeleccionados)
                {
                    FuncionarioXConglomeradoModel relacion = new FuncionarioXConglomeradoModel
                    {
                        IdFuncionario = idFuncionario,
                        IdConglomerado = Convert.ToInt32(id)
                    };

                    objeto_funcionarioXConglomerado.CrearFuncionarioXConglomerado(relacion);
                }

                TempData["MensajeExito"] = $"Funcionario {newFuncionario.Nombre} {newFuncionario.Apellido1} {newFuncionario.Apellido2} creado correctamente.";
                return RedirectToAction(nameof(ManteniFuncionarios));
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al crear el funcionario: {ex.Message}";

                // Recargar listas
                var puestos = _objetoPuesto.ObtenerPuestos();
                var conglomerados = objeto_ConglomeradosNegocios.ListarConglomerados();

                FuncionarioViewModel viewModel = new FuncionarioViewModel
                {
                    Funcionario = new FuncionarioModel(), 
                    Puestos = puestos,
                    Conglomerados = conglomerados
                };

                return View(viewModel); 
            }
        }
       
        [Authorize(Roles = "Administración")]

        // Editar funcionario
        [HttpGet("Mantenimientos/EditaFuncionario/{cedula}")]
        public IActionResult EditaFuncionario(string cedula)
        {

            // Consulta el funcionario por cédula
            var funcionario = objeto_funcionario.ConsultarFuncionarioID(cedula);

            // Consulta los conglomerados asociados al funcionario
            var conglomerados = objeto_ConglomeradosNegocios.ListarConglomerados();

            // Obtiene la lista de puestos de la capa de negocios
            var puestos = _objetoPuesto.ObtenerPuestos();
            var departamentos = objeto_departamento.ListarDepartamentos();
            var roles = objeto_rol.ListarRoles(); // Obtener la lista de roles

            // Crea el ViewModel
            FuncionarioViewModel viewModel = new FuncionarioViewModel
            {
                Funcionario = funcionario,
                Puestos = puestos,
                Conglomerados = conglomerados,
                Departamentos = departamentos,
                Roles = roles // Asigna la lista de roles al ViewModel
            };

            return View(viewModel);
        }

        [HttpPost("Mantenimientos/EditaFuncionario/{cedula}")]
        public IActionResult EditaFuncionario(string cedula, IFormCollection collection)
        {
            try
            {
                FuncionarioModel funcionarioEditar = new FuncionarioModel
                {
                    Cedula = cedula,

                    Nombre = collection["Funcionario.Nombre"],
                    Apellido1 = collection["Funcionario.Apellido1"],
                    Apellido2 = collection["Funcionario.Apellido2"],
                    Correo = collection["Funcionario.Correo"],
                    Password = collection["Funcionario.Password"],
                    IdDepartamento = Convert.ToInt32(collection["Funcionario.IdDepartamento"]),
                    IdRol = Convert.ToInt32(collection["Funcionario.IdRol"]),
                    IdPuesto = Convert.ToInt32(collection["Funcionario.IdPuesto"]),
                    IdEstadoFuncionario = Convert.ToInt32(collection["Funcionario.IdEstadoFuncionario"])

                };

                objeto_funcionario.ModificarFuncionario(funcionarioEditar);
                // 🔁 Eliminar relaciones actuales
                objeto_funcionarioXConglomerado.EliminarPorFuncionario(cedula);

                // 🔁 Insertar las nuevas relaciones
                var conglomeradosSeleccionados = collection["IdConglomeradosSeleccionados"];
                foreach (var id in conglomeradosSeleccionados)
                {
                    FuncionarioXConglomeradoModel relacion = new FuncionarioXConglomeradoModel
                    {
                        IdFuncionario = cedula,
                        IdConglomerado = Convert.ToInt32(id)
                    };
                    objeto_funcionarioXConglomerado.CrearFuncionarioXConglomerado(relacion);
                }

                TempData["MensajeExito"] = $"Funcionario {funcionarioEditar.Nombre} {funcionarioEditar.Apellido1} {funcionarioEditar.Apellido2} modificado correctamente.";
                return RedirectToAction(nameof(ManteniFuncionarios));
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al editar el funcionario: {ex.Message}";
                return View();
            }
        }

        // Borrar funcionario
        [Authorize(Roles = "Administración")]

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
                var funcionario = objeto_funcionario.ConsultarFuncionarioID(cedula);
                objeto_funcionario.EliminarFuncionario(cedula);
                TempData["MensajeExito"] = $"Funcionario {funcionario.Nombre} {funcionario.Apellido1} {funcionario.Apellido2} eliminado correctamente.";
                return RedirectToAction(nameof(ManteniFuncionarios));
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return RedirectToAction(nameof(ManteniFuncionarios));
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

        [Authorize(Roles = "Administración,Jefatura")]

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
                TempData["MensajeError"] = $"Error al obtener los objetivos: {ex.Message}";
                return View(new List<ObjetivoModel>());
            }
        }

        [Authorize(Roles = "Administración,Jefatura")]

        public IActionResult CrearNuevoObjetivo()
        {
            return View("CreaObjetivo", new ObjetivoModel());
        }

        [Authorize(Roles = "Administración,Jefatura")]

        [HttpPost]
        public IActionResult CrearObjetivo(ObjetivoModel nuevoObjetivo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _objetivoNegocios.CrearObjetivo(nuevoObjetivo);
                    TempData["MensajeExito"] = $"Objetivo {nuevoObjetivo.Objetivo} creado correctamente.";
                    return RedirectToAction(nameof(GestionObjetivos)); // Redirigir a la vista de gestión
                }
                else
                {
                    return View(nuevoObjetivo); // Mostrar errores de validación si los hay
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al crear el objetivo: {ex.Message}";
                return View();
            }
        }

        [Authorize(Roles = "Administración,Jefatura")]

        public IActionResult EditaObjetivo(int id)
        {
            return View(_objetivoNegocios.ConsultarObjetivoID(id));
        } // fin EditaConglomerado

        [Authorize(Roles = "Administración,Jefatura")]

        [HttpPost]
        public IActionResult EditarObjetivo(ObjetivoModel objetivoEditado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _objetivoNegocios.ModificarObjetivo(objetivoEditado);
                    TempData["MensajeExito"] = $"Objetivo {objetivoEditado.Objetivo} modificado correctamente.";
                    return RedirectToAction(nameof(GestionObjetivos)); // Redirigir a la lista de objetivos
                }
                else
                {
                    return View(objetivoEditado); // Mostrar errores de validación
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al actualizar el objetivo: {ex.Message}";
                return View(objetivoEditado);
            }
        }

        [Authorize(Roles = "Administración,Jefatura")]

        [HttpPost]
        public ActionResult BorraObjetivo(int id)
        {
            try
            {
                var objetivo = _objetivoNegocios.ConsultarObjetivoID(id);
                if (objetivo == null)
                {
                    TempData["MensajeError"] = $"El objetivo con ID {id} no fue encontrado.";
                }
                else
                {
                    _objetivoNegocios.EliminarObjetivo(id);
                    TempData["MensajeExito"] = $"Objetivo {objetivo.Objetivo} eliminado correctamente.";
                }
                return RedirectToAction(nameof(GestionObjetivos));
            }
            catch
            {
                TempData["MensajeError"] = "No puede borrar este Conglomerado, verifique las relaciones.";
                return RedirectToAction(nameof(GestionObjetivos));
            }
        }
        #endregion

        #region DEPARTAMENTOS
        // Gestión de Departamentos (ya existente)
        [Authorize(Roles = "Administración")]

        public IActionResult ManteniDepartamentos()
        {
            try
            {
                var departamentos = objeto_departamento.ListarDepartamentos();
                return View(departamentos);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al obtener los departamentos: {ex.Message}";
                return View(new List<DepartamentoModel>());
            }
        }

        // crear departamento
        [Authorize(Roles = "Administración")]

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
                    Departamento = collection["Departamento"]!
                };
                objeto_departamento.CrearDepartamento(newDepartamento);
                TempData["MensajeExito"] = $"Departamento {newDepartamento.Departamento} creado correctamente.";
                return RedirectToAction(nameof(ManteniDepartamentos));
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al crear el departamento: {ex.Message}";
                return View();
            }
        }

        // editar departamento
        [Authorize(Roles = "Administración")]
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
                    Departamento = collection["Departamento"]!
                };
                objeto_departamento.ModificarDepartamento(departamentoEditar);
                TempData["MensajeExito"] = $"Departamento {departamentoEditar.Departamento} modificado correctamente.";
                return RedirectToAction(nameof(ManteniDepartamentos));
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al editar el departamento: {ex.Message}";
                return View();
            }
        }

        // borrar departamento
        [Authorize(Roles = "Administración")]
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
                var departamento = objeto_departamento.ConsultarDepartamentoID(id);

                if (departamento == null)
                {
                    TempData["MensajeError"] = $"El departamento con ID {id} no fue encontrado.";
                }
                else
                {
                    objeto_departamento.EliminarDepartamento(id);
                    TempData["MensajeExito"] = $"Departamento {departamento.Departamento} eliminado correctamente.";
                }

                return RedirectToAction(nameof(ManteniDepartamentos));
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al borrar el departamento: {ex.Message}";
                return View();
            }
        }

        #endregion

        #region CONGLOMERADOS
        // Gestión de Conglomerados (Nuevo)
        [Authorize(Roles = "Administración")]
        public IActionResult ManteniConglomerados()
        {
            try
            {
                var conglomerados = objeto_ConglomeradosNegocios.ListarConglomerados();
                return View(conglomerados);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al obtener los conglomerados: {ex.Message}";
                return View(new List<ConglomeradoModel>());
            }
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
                string nombre = collection["NombreConglomerado"]!;
                string descripcion = collection["Descripcion"]!;

                // Validación: que no estén vacíos o solo espacios
                if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(descripcion))
                {
                    TempData["MensajeError"] = "El nombre y la descripción no pueden estar vacíos ni contener solo espacios.";
                    return View(new ConglomeradoModel
                    {
                        IdConglomerado = Convert.ToInt32(collection["IdConglomerado"]!),
                        NombreConglomerado = nombre,
                        Descripcion = descripcion
                    });
                }
                ConglomeradoModel newConglomerado = new ConglomeradoModel
                {
                    IdConglomerado = Convert.ToInt32(collection["IdConglomerado"]!),
                    NombreConglomerado = collection["NombreConglomerado"]!,
                    Descripcion = collection["Descripcion"]!
                };

                objeto_ConglomeradosNegocios.CrearConglomerado(newConglomerado);
                TempData["MensajeExito"] = $"Conglomerado {newConglomerado.NombreConglomerado} creado correctamente.";
                return RedirectToAction(nameof(ManteniConglomerados));
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al crear el conglomerado: {ex.Message}";
                return View();
            }
        }

        //modelo "dummy" para cada "peso"
        public class PesoDummy
        {
            public string Nombre { get; set; }
            public decimal Porcentaje { get; set; }
        }

        //modelo "dummy" para EditaConglomerado: 
        public class EditaConglomeradoDummyModel
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public List<PesoDummy> Pesos { get; set; }
        }


        [Authorize(Roles = "Administración")]
        [HttpGet]
        public IActionResult EditaConglomerado(int id)
        {
            //datos de ejemplo
            var model = new EditaConglomeradoDummyModel
            {
                Id = id,
                Nombre = "Conglomerado Demo " + id,
                Descripcion = "Texto descriptivo de prueba",
                Pesos = new List<PesoDummy>
        {
            new PesoDummy { Nombre = "Objetivo 1", Porcentaje = 30 },
            new PesoDummy { Nombre = "Objetivo 2", Porcentaje = 20 },
            new PesoDummy { Nombre = "Competencia A", Porcentaje = 25 }
        }
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditaConglomerado(EditaConglomeradoDummyModel model)
        {
            decimal suma = model.Pesos.Sum(x => x.Porcentaje);
            if (suma > 100)
            {
                TempData["MensajeError"] = "La sumatoria de porcentajes excede el 100% (validación back-end).";
                return View(model);
            }

            //si es valido, “simula” guardado
            TempData["MensajeExito"] = "Cambios guardados (datos quemados, sin DB).";
            return RedirectToAction("ManteniConglomerados");
        }


        [Authorize(Roles = "Administración")]
        public ActionResult BorrarConglomerado(int id)
        {
            return View(objeto_ConglomeradosNegocios.ConsultarConglomeradoID(id));
        }

        [HttpPost]
        public ActionResult BorrarConglomerado(int id, IFormCollection collection)
        {
            try
            {
                var conglomerado = objeto_ConglomeradosNegocios.ConsultarConglomeradoID(id);
                if (conglomerado == null)
                {
                    TempData["MensajeError"] = $"El conglomerado con ID {id} no fue encontrado.";
                }
                else
                {
                    objeto_ConglomeradosNegocios.EliminarConglomerado(id);
                    TempData["MensajeExito"] = $"Conglomerado {conglomerado.NombreConglomerado} eliminado correctamente.";
                }

                return RedirectToAction(nameof(ManteniConglomerados));
            }
            catch
            {
                TempData["MensajeError"] = "No puede borrar este Conglomerado, verifique las relaciones.";
                return View();
            }
        }

        public IActionResult InformativaConglomerado()
        {
            try
            {
                var conglomerados = objeto_ConglomeradosNegocios.ListarConglomerados();
                return View(conglomerados);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al obtener los conglomerados: {ex.Message}";
                return View(new List<ConglomeradoModel>());
            }
        }

        #endregion

        #region Metas
        public IActionResult ManteniMetas()
        {
            try
            {
                var objetivos = _objetoMeta.ListarMetas();
                return View(objetivos);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al obtener las metas: {ex.Message}";
                return View(new List<MetaModel>());
            }
        }

        public IActionResult CrearNuevaMeta()
        {
            return View("CrearNuevaMeta", new MetaModel());
        }

        [HttpPost]
        public IActionResult CrearMeta(MetaModel nuevaMeta)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _objetoMeta.CrearMeta(nuevaMeta);
                    TempData["MensajeExito"] = $"Meta {nuevaMeta.Meta} creada correctamente.";
                    return RedirectToAction(nameof(ManteniMetas)); // Redirigir a la vista de gestión
                }
                else
                {
                    return View(nuevaMeta); // Mostrar errores de validación si los hay
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al crear la meta: {ex.Message}";
                return View();
            }
        }

        public IActionResult EditaMeta(int id)
        {
            return View(_objetoMeta.ConsultarMetaID(id));
        }

        [HttpPost]
        public IActionResult EditarMeta(MetaModel metaEditada)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _objetoMeta.ModificarMeta(metaEditada);
                    TempData["MensajeExito"] = $"Meta {metaEditada.Meta} modificada correctamente.";
                    return RedirectToAction(nameof(ManteniMetas)); // Redirigir a la lista de metas
                }
                else
                {
                    return View(metaEditada); // Mostrar errores de validación
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al actualizar la meta: {ex.Message}";
                return View(metaEditada);
            }
        }

        [HttpPost]
        public ActionResult BorraMeta(int id)
        {
            try
            {
                var meta = _objetoMeta.ConsultarMetaID(id);
                if (meta == null)
                {
                    TempData["MensajeError"] = $"La meta con ID {id} no fue encontrada.";
                }
                else
                {
                    _objetoMeta.EliminarMeta(id);
                    TempData["MensajeExito"] = $"Meta {meta.Meta} eliminada correctamente.";
                }
                return RedirectToAction(nameof(ManteniMetas));
            }
            catch
            {
                TempData["MensajeError"] = "No puede borrar esta meta, verifique las relaciones.";
                return RedirectToAction(nameof(ManteniMetas));
            }
        }

        #endregion

        #region Competencias

        public IActionResult ManteniCompetencias()
        {
            try
            {
                var competencias = _objetoCompe.ListarCompetencias();
                return View(competencias);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al obtener las competencias: {ex.Message}";
                return View(new List<CompetenciasModel>());
            }
        }

        public IActionResult CrearNuevaCompetencia()
        {
            ViewData["ListaTiposCompetencias"] = objeto_TiposCompenNegocis.ListarTiposCompetencias();
            return View("CrearNuevaCompetencia", new CompetenciasModel());
        }

        [HttpPost]
        public IActionResult CrearCompetencia(CompetenciasModel nuevaCompe)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _objetoCompe.CrearCompetencia(nuevaCompe);
                    TempData["MensajeExito"] = $"Competencia {nuevaCompe.Competencia} creada correctamente.";
                    return RedirectToAction(nameof(ManteniCompetencias));
                }
                else
                {
                    return View(nuevaCompe);
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al crear la competencia: {ex.Message}";
                return View();
            }
        }

        public IActionResult EditaCompetencia(int id)
        {
            return View(_objetoCompe.ConsultarCompetenciaID(id));
        }

        [HttpPost]
        public IActionResult EditarCompetencia(CompetenciasModel competenciaEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _objetoCompe.ModificarCompetencia(competenciaEdit);
                    TempData["MensajeExito"] = $"Competencia {competenciaEdit.Competencia} modificada correctamente.";
                    return RedirectToAction(nameof(ManteniCompetencias));
                }
                else
                {
                    return View(competenciaEdit);
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al actualizar la competencia: {ex.Message}";
                return View(competenciaEdit);
            }
        }

        [HttpPost]
        public ActionResult BorraCompetencia(int id)
        {
            try
            {
                var competencia = _objetoCompe.ConsultarCompetenciaID(id);
                if (competencia == null)
                {
                    TempData["MensajeError"] = $"La competencia con ID {id} no fue encontrada.";
                }
                else
                {
                    _objetoCompe.EliminarCompetencia(id);
                    TempData["MensajeExito"] = $"Competencia {competencia.Competencia} eliminada correctamente.";
                }
                return RedirectToAction(nameof(ManteniCompetencias));
            }
            catch
            {
                TempData["MensajeError"] = "No puede borrar esta competencia, verifique las relaciones.";
                return RedirectToAction(nameof(ManteniCompetencias));
            }
        }



        #endregion

        #region Puestos
        // Vista para listar puestos
        public IActionResult ManteniPuestos()
        {
            try
            {
                var puestos = _objetoPuesto.ObtenerPuestos();
                return View(puestos);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al obtener los puestos: {ex.Message}";
                return View(new List<PuestoModel>());
            }
        }

        // Vista para crear un puesto
        // Acción GET para mostrar el formulario de creación
        public IActionResult CreaPuesto()
        {
            return View("CreaPuesto", new PuestoModel());
        }

        // Acción POST para procesar el formulario y crear el puesto
        [HttpPost]
        public IActionResult CreaPuestos(PuestoModel nuevoPuesto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Aquí llamas al método para agregar el puesto a la base de datos
                    _objetoPuesto.AgregarPuesto(nuevoPuesto);

                    // Si la inserción fue exitosa, muestras el mensaje de éxito
                    TempData["MensajeExito"] = $"Puesto {nuevoPuesto.Puesto} creado correctamente.";
                    return RedirectToAction(nameof(ManteniPuestos));
                }
                else
                {
                    // Si el modelo no es válido, vuelve a la vista original con los datos
                    return View("CreaPuesto", nuevoPuesto); // Usa el mismo nombre de vista aquí
                }
            }
            catch (Exception ex)
            {
                // Aquí atrapas el error y lo muestras en la vista usando TempData
                TempData["MensajeError"] = $"Error al crear el puesto: {ex.Message}";
                return View("CreaPuesto", nuevoPuesto); // Usa el mismo nombre de vista aquí
            }
        }


        // Vista para editar un puesto
        public IActionResult EditaPuesto(int id)
        {
            return View(_objetoPuesto.ObtenerPuestoId(id));
        }

        // Acción para modificar un puesto
        [HttpPost]
        public IActionResult EditarPuesto(PuestoModel puestoModificado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _objetoPuesto.ActualizarPuesto(puestoModificado);
                    TempData["MensajeExito"] = $"Puesto {puestoModificado.Puesto} modificado correctamente.";
                    return RedirectToAction(nameof(ManteniPuestos));
                }
                else
                {
                    return View(puestoModificado);
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al actualizar el puesto: {ex.Message}";
                return View(puestoModificado);
            }
        }

        // Acción para eliminar un puesto
        public IActionResult EliminarPuesto(int id)
        {
            try
            {
                var puesto = _objetoPuesto.ObtenerPuestoId(id);
                if (puesto == null)
                {
                    TempData["MensajeError"] = $"El puesto con ID {id} no fue encontrado.";
                }
                else
                {
                    _objetoPuesto.EliminarPuesto(id);
                    TempData["MensajeExito"] = $"Puesto {puesto.Puesto} eliminado correctamente.";
                }
                return RedirectToAction(nameof(ManteniPuestos));
            }
            catch
            {
                TempData["MensajeError"] = "No puede borrar este puesto, verifique las relaciones.";
                return RedirectToAction(nameof(ManteniPuestos));
            }
        }


        #endregion

        #region Periodos

        [Authorize(Roles = "Administración")]
        public IActionResult ManteniPeriodo()
        {
            try
            {
                var periodos = _objetoPeriodo.ListarPeriodos();
                return View(periodos);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al obtener los períodos: {ex.Message}";
                return View(new List<PeriodoEvaluacionModel>());
            }
        }

        [Authorize(Roles = "Administración")]
        public IActionResult CreaPeriodo()
        {
            return View(new PeriodoEvaluacionModel());
        }

        [HttpPost]
        public IActionResult CreaPeriodo(PeriodoEvaluacionModel nuevoPeriodo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _objetoPeriodo.CrearPeriodo(nuevoPeriodo);
                    TempData["MensajeExito"] = "Período creado correctamente.";
                    return RedirectToAction(nameof(ManteniPeriodo));
                }
                return View(nuevoPeriodo);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al crear el período: {ex.Message}";
                return View(nuevoPeriodo);
            }
        }

        [Authorize(Roles = "Administración")]
        [HttpGet("Mantenimientos/EditaPeriodo/{anio}")]
        public IActionResult EditaPeriodo(int anio)
        {
            return View(_objetoPeriodo.ConsultarPeriodoPorAnio(anio));
        }

        [HttpPost]
        public IActionResult EditarPeriodo(PeriodoEvaluacionModel periodo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _objetoPeriodo.ModificarPeriodo(periodo);
                    TempData["MensajeExito"] = "Período modificado correctamente.";
                    return RedirectToAction(nameof(ManteniPeriodo));
                }
                else
                {
                    return View(periodo);
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al modificar el período: {ex.Message}";
                return View(periodo);
            }
        }

        [Authorize(Roles = "Administración")]
        public IActionResult EliminarPeriodo(int anio)
        {
            try
            {
                var periodo = _objetoPeriodo.ConsultarPeriodoPorAnio(anio);
                if (periodo == null)
                {
                    TempData["MensajeError"] = $"El período con ID {anio} no fue encontrado.";
                }
                else
                {
                    _objetoPeriodo.EliminarPeriodo(anio);
                    TempData["MensajeExito"] = $"Período {periodo.Anio} eliminado correctamente.";
                }
                return RedirectToAction(nameof(ManteniPeriodo));
            }
            catch
            {
                TempData["MensajeError"] = "No puede borrar este período, verifique las relaciones.";
                return RedirectToAction(nameof(ManteniPeriodo));
            }
        }

        #endregion
    }//fin class

}//fin space
