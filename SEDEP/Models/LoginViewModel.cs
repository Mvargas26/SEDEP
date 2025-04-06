using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SEDEP.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "La cédula es obligatoria")]
        [CustomCedulaValidation(ErrorMessage = "La cédula debe tener 9 dígitos si es nacional o el formato correcto si es extranjera")]
        public string? Cedula { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? Rol { get; set; }
    }
    public class CustomCedulaValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult("La cédula es obligatoria.");

            string cedula = value.ToString()!;

            // Nacional: 9 dígitos exactos
            if (Regex.IsMatch(cedula, @"^\d{9}$"))
            {
                return ValidationResult.Success!;
            }

            // Extranjero: Ejemplo de validación personalizada (ajustar según el formato requerido)
            if (Regex.IsMatch(cedula, @"^[A-Z]{1,3}-\d{6,9}$"))
            {
                return ValidationResult.Success!;
            }

            return new ValidationResult("Formato de cédula incorrecto.");
        }
    }
}
