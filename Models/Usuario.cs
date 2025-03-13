using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEATRO.Models
{
    public class Usuario
    {
        [Key] // Se agrega esta anotación para que EF Core reconozca que es clave primaria
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [DisplayName("Nombre completo")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [DisplayName("Correo Electrónico")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; }  // 🔹 Campo agregado

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [DisplayName("Rol")]
        public int RolId { get; set; }

        // 🔴 Constructor vacío necesario para EF Core
        public Usuario() { }

        public string? TokenRecuperacion { get; set; }


        public Usuario(int id, string nombre, string correo, string password, int rolId)
        {
            Id = id;
            Nombre = nombre;
            Correo = correo;
            Password = password;
            RolId = rolId;
        }
    }
}
