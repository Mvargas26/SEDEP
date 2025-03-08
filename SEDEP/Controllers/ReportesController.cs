using Microsoft.AspNetCore.Mvc;

namespace SEDEP.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
