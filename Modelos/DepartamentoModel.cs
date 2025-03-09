using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class DepartamentoModel
    {
        [Display(Name = "Id Departamento")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDepartamento { get; set; }  

        [Required]
        [StringLength(255)]
        [Display(Name = "Nombre del Departamento")]
        public string Departamento { get; set; }

    } //Fin clase

} // Fin space
