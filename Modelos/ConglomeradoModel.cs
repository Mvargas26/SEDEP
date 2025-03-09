using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class ConglomeradoModel
    {
        [Display(Name = "Id Conglomerado")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdConglomerado { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Nombre Conglomerado")]
        public string NombreConglomerado { get; set; }

        [Required]
        [Display(Name = "Descripcion Conglomerado")]
        public string Descripcion { get; set; }

    }//fin class
}//fin space
