using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TEATRO.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [DisplayName("Nombre completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
        [DisplayName("Correo Electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El captcha es obligatorio.")]
        [DisplayName("Captcha")]
        public string CaptchaCode { get; set; }
    }
}
