using System.ComponentModel.DataAnnotations;

namespace SEDEP.Models.AuthViewModel
{
    public class ReestablecerPasswordViewModel
    {
        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string? NuevaContrasena { get; set; }

        [Required(ErrorMessage = "Debe confirmar la contraseña")]
        [DataType(DataType.Password)]
        [Compare("NuevaContrasena", ErrorMessage = "Las contraseñas no coinciden")]
        public string? ConfirmarContrasena { get; set; }
    }
}
