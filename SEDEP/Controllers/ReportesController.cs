using Microsoft.AspNetCore.Mvc;
using Modelos;

namespace SEDEP.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ReporteJefaturas()
        {
            return View();
        }

        public IActionResult ReportesRRHH()
        {
            return View();
        }

        public IActionResult ReportePersonal()
        {
            var evaluacion = new EvaluacionModel
            {
                IdEvaluacion = 1,
                FechaCreacion = DateTime.Now.AddDays(-30),
                EstadoEvaluacion = 2,
                Observaciones = "Excelente compromiso y cumplimiento de metas.",
                Funcionario = new FuncionarioModel
                {
                    Cedula = "12345678",
                    Nombre = "Carlos",
                    Apellido1 = "Ramírez",
                    Apellido2 = "Mora",
                    Departamento = "Recursos Humanos"
                }
            };

            return View(evaluacion);
        }

    }
}
