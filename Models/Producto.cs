using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // <-- Agrega esta librería

namespace TEATRO.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(300, ErrorMessage = "La descripción no puede tener más de 300 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(1, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        [Column(TypeName = "decimal(18, 2)")]  // 🔹 Definición explícita de precisión
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La imagen es obligatoria.")]
        public string Imagen { get; set; }

        [Required(ErrorMessage = "El estado del producto es obligatorio.")]
        public bool Estado { get; set; }
    }
}
