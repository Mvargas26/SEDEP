using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

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
        public string Departamento { get; set; }
        public int IdRol { get; set; }
        public string Rol { get; set; }
       
        public int IdPuesto { get; set; }
        public string Puesto { get; set; }

        public int IdConglomerado { get; set; }
        public string NombreConglomerado { get; set; }

        public int IdEstadoFuncionario { get; set; }

        public string Estado { get; set; }
        public string CodigoSeguridad { get; set; }
    }//public class
}//fin space
