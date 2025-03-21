using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class AuditoriaFuncionarioModel
    {
        [Display(Name = "ID de Auditoría")]
        [Key]
        [StringLength(20)]
        public string IdAuditoria { get; set; }

        [Required]
        [ForeignKey("Funcionario")]
        [StringLength(100)]
        [Display(Name = "ID del Funcionario")]
        public string IdFuncionario { get; set; }
        public virtual FuncionarioModel Funcionario { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Acción")]
        public string Accion { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Fecha y Hora")]
        public string FechaHora { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Usuario que Realiza la Acción")]
        public string UsuarioAccion { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Campo Modificado")]
        public string CampoModificado { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Valor Anterior")]
        public string ValorAnterior { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Valor Nuevo")]
        public string ValorNuevo { get; set; }

    }//Fin de la clase 
} // Fin space 
