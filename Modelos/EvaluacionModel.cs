using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Modelos
{
    public class EvaluacionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID de Evaluación")]
        public int IdEvaluacion { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Funcionario Evaluado")]
        public string IdFuncionario { get; set; }

        [ForeignKey("IdFuncionario")]
        public virtual FuncionarioModel Funcionario { get; set; }

        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; } // Permite nulos

        [Required]
        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [Display(Name = "Estado de la Evaluación")]
        public int EstadoEvaluacion { get; set; }
        public int idConglomerado { get; set; }

        [ForeignKey("EstadoEvaluacion")]
        public virtual EstadoEvaluacionModel EstadoEvaluacionObj { get; set; }
    }//Fin Clase
}//Fin Space 
