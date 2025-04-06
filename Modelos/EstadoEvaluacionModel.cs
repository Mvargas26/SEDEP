using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class EstadoEvaluacionModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID del Estado")]
        public int IdEstado { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Estado de la Evaluación")]
        public string EstadoEvaluacion { get; set; }

    }//Fin clase 

}//Fin Space 
