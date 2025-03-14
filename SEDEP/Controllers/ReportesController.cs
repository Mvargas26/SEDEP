using Microsoft.AspNetCore.Mvc;

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
            return View();
        }
    }
}
