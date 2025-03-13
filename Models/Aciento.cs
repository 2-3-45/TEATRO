using System.ComponentModel.DataAnnotations;

namespace TEATRO.Models
{
    public class Aciento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de asiento es obligatorio.")]
        [Display(Name = "Número de Asiento")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "La fila es obligatoria.")]
        [Display(Name = "Fila del Asiento")]
        public string Fila { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Display(Name = "Estado del Asiento")]
        public string Estado { get; set; }  // Ejemplo: Disponible, Ocupado, Reservado

        [Display(Name = "Zona del Teatro")]
        public string Zona { get; set; }  // Opcional: VIP, Preferencial, General
    }
}
