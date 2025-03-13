using System.ComponentModel.DataAnnotations;

namespace TEATRO.Models
{
    public class RestablecerContraseñaViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string NuevaContraseña { get; set; }

        [Required(ErrorMessage = "Debe confirmar su contraseña.")]
        [Compare("NuevaContraseña", ErrorMessage = "Las contraseñas no coinciden.")]
        [DataType(DataType.Password)]
        public string ConfirmarContraseña { get; set; }
    }
}