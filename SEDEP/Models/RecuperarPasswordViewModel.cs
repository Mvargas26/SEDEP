using System.ComponentModel.DataAnnotations;

namespace SEDEP.Models
{
    public class RecuperarPasswordViewModel
    {
        [Required(ErrorMessage = "La cédula es obligatoria")]
        [CustomCedulaValidation(ErrorMessage = "La cédula debe tener 9 dígitos si es nacional o el formato correcto si es extranjera")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido")]
        public string Correo { get; set; }
    }
}
