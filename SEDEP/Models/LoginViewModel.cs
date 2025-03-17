using System.ComponentModel.DataAnnotations;

namespace SEDEP.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [MinLength(3, ErrorMessage = "El usuario debe tener al menos 3 caracteres")]
        [MaxLength(50, ErrorMessage = "El usuario no puede superar los 50 caracteres")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [MaxLength(20, ErrorMessage = "La contraseña no puede superar los 20 caracteres")]
        public string Password { get; set; }
    }
}
