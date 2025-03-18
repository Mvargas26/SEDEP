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

        //***********************************************************************************************

        public IActionResult Index()
        {
            return View();
        }

        #region OBJETIVOS
        public IActionResult GestionObjetivos()
        {
            var objetivos = new List<ObjetivoModel>
        {
            new ObjetivoModel { Id = 1, Nombre = "Mejorar productividad", Porcentaje = 80, Tipo = "Estratégico" },
            new ObjetivoModel { Id = 2, Nombre = "Aumentar satisfacción del cliente", Porcentaje = 90, Tipo = "Operativo" }
        };
            return View(objetivos);
        }

        public IActionResult CreaObjetivo()
        {
            return View(new ObjetivoModel());
        }

        public IActionResult EditaObjetivo(int id)
        {
            var objetivo = new ObjetivoModel { Id = id, Nombre = "Mejorar productividad", Porcentaje = 80, Tipo = "Estratégico" };
            return View(objetivo);
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
        #endregion


    }//fin class

    // lo ideal es crearlo en modelos, pero lo puse aqui solo para probar el front
    public class ObjetivoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Porcentaje { get; set; } // Entre 0 y 100
        public string Tipo { get; set; } // Estratégico, Operativo, Táctico
    }

}//fin space
