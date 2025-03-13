using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TEATRO.Models
{


    public class Teatro
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Nombre del Teatro")]
        public string Nombre { get; set; }

        [DisplayName("Ubicación")]
        public string Ubicacion { get; set; }



        // 🔴 Este constructor vacío es NECESARIO
        public Teatro() { }





        public Teatro(int id, string nombre, string ubicacion)
        {
            Id = id;
            Nombre = nombre;
            Ubicacion = ubicacion;
        }
    }

}