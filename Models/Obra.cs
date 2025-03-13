using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEATRO.Models
{
    public class Obra
    {
        [Key]  // ✅ Corrección: Marcamos el campo 'Id' como clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 🔹 Para que sea autoincremental
        public int Id { get; set; }

        [DisplayName("Título de la Obra")]
        [Required(ErrorMessage = "El título es obligatorio.")]
        public string Titulo { get; set; }

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        [Required]
        public int TeatroId { get; set; }

        // 🔹 Relación con la tabla de Teatros
        [ForeignKey("TeatroId")]
        public Teatro Teatro { get; set; }

        // 🔹 Constructor vacío requerido por EF Core
        public Obra() { }

        public Obra(int id, string titulo, string descripcion, int teatroId)
        {
            Id = id;
            Titulo = titulo;
            Descripcion = descripcion;
            TeatroId = teatroId;
        }
    }
}
