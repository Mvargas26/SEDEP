using Microsoft.AspNetCore.Mvc;
using Modelos;

namespace SEDEP.Controllers
{
    public class MantenimientosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

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

        // Gestión de Conglomerados (Nuevo)
        public IActionResult ManteniConglomerados()
        {
            var conglomerados = new List<ConglomeradoModel>
            {
                new ConglomeradoModel { IdConglomerado = 1, Nombre = "Profesional", Descripcion = "Nivel Profesional" },
                new ConglomeradoModel { IdConglomerado = 2, Nombre = "Técnico", Descripcion = "Nivel Técnico" }
            };
            return View(conglomerados);
        }

        public IActionResult CreaConglomerado()
        {
            return View(new ConglomeradoModel());
        }

        public IActionResult EditaConglomerado(int id)
        {
            var conglomerado = new ConglomeradoModel { IdConglomerado = id, Nombre = "Técnico", Descripcion = "Nivel Técnico" };
            return View(conglomerado);
        }
    }
}
