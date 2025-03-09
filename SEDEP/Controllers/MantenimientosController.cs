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
    }
}
