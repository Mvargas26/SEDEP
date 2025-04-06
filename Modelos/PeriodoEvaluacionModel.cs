using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class PeriodoEvaluacionModel
    {
        [Key]
        [Display(Name = "Año de Evaluación")]
        public int Anio { get; set; }

        [Required]
        [Display(Name = "Fecha Máxima para el Período")]
        public DateTime FechaMaxima { get; set; }


    }//Fin clase
} //Fin Space 
