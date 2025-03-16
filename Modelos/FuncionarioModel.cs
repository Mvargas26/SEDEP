using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class FuncionarioModel
    {
        [Key]
        [Column(TypeName = "VARCHAR(20)")]
        public string Cedula { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Nombre { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Apellido1 { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Apellido2 { get; set; }

        [Column(TypeName = "VARCHAR(150)")]
        public string Correo { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string Password { get; set; }

        public int IdDepartamento { get; set; }
        public int IdRol { get; set; }
        public int IdPuesto { get; set; }
        public int IdEstadoFuncionario { get; set; }

        // Relaciones
        //[ForeignKey("IdDepartamento")]
        //public virtual Departamento Departamento { get; set; }

        //[ForeignKey("IdRol")]
        //public virtual Rol Rol { get; set; }

        //[ForeignKey("IdPuesto")]
        //public virtual Puesto Puesto { get; set; }

        //[ForeignKey("IdEstadoFuncionario")]
        //public virtual EstadoFuncionario EstadoFuncionario { get; set; }

    }//public class
}//fin space
