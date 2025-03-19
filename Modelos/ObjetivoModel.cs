using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class ObjetivoModel
    {
        public int IdObjetivo { get; set; }
        public required string Objetivo { get; set; }
        public decimal Porcentaje { get; set; }
        public int? IdTipoObjetivo { get; set; }

    }//fin class
}//fin space
