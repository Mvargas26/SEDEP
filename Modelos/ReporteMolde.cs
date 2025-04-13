using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class ReporteMolde
    {
        //Ajustar en base al sp a futuro, si hace falta 
        public string Titulo { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public List<string> Contenido { get; set; } = new();
    }
}
