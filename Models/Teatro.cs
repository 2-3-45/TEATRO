using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TEATRO.Models
{
    public class Teatro
    {
        // EF Core detecta automáticamente este campo como clave primaria
        public int Id { get; set; }

        [DisplayName("Nombre del Teatro")]
        public required string Nombre { get; set; }

        [DisplayName("Ubicación")]
        public required string Ubicacion { get; set; }

        // Constructor vacío (no es obligatorio en EF Core 7/8, pero se recomienda si hay otro constructor)
        public Teatro() { }

        // Constructor completo
        public Teatro(int id, string nombre, string ubicacion)
        {
            Id = id;
            Nombre = nombre;
            Ubicacion = ubicacion;
        }
    }
}
