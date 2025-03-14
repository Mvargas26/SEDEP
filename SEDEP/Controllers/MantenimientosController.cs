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

    // lo ideal es crearlo en modelos, pero lo puse aqui solo para probar el front
    public class ObjetivoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Porcentaje { get; set; } // Entre 0 y 100
        public string Tipo { get; set; } // Estratégico, Operativo, Táctico
    }

}
