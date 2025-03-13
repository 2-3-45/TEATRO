

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TEATRO.Models
{


    public class Rol
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Nombre del Rol")]
        public string Nombre { get; set; }



        // 🔴 Este constructor vacío es NECESARIO
        public Rol() { }

        public Rol(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }


    }
}