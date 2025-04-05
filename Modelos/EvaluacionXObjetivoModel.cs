using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class EvaluacionXObjetivoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Evaluación x Objetivo")]
        public int IdEvaxObj { get; set; }

        [Required]
        [Display(Name = "Evaluación Asociada")]
        [ForeignKey("Evaluacion")]
        public int IdEvaluacion { get; set; }
        public virtual EvaluacionModel Evaluacion { get; set; }

        [Required]
        [Display(Name = "Objetivo Evaluado")]
        [ForeignKey("Objetivo")]
        public int IdObjetivo { get; set; }
        public virtual ObjetivoModel Objetivo { get; set; }

        [Required]
        [Display(Name = "Valor Obtenido")]
        public decimal ValorObtenido { get; set; } // Decimal(5,2)

    }//Fin Clase 
} // Fin Space 
