using Microsoft.AspNetCore.Mvc;
using Negocios;

namespace SEDEP.Controllers
{
    public class EvaluacionController : Controller
    {
        //***********************************************************************************************
        //Objetos de la cap Negocios
        ConglomeradosNegocios objeto_ConglomeradosNegocios = new();
        FuncionarioNegocios objeto_FuncionarioNegocios = new();
        //***********************************************************************************************
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EvaluacionSubalterno()
        {
            //el id Departamento va a venir despues de la jefatura que hace login
            var ListaFuncionariosPorDepartamento = objeto_FuncionarioNegocios.ObtenerFuncionariosPorDepartamento(1);

            return View();
        }
    }//fin class
}//fin space
