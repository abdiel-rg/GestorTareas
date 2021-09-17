using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GestorTareas.Data
{
    public class Tarea
    {
        public Guid ID { get; init; } = Guid.NewGuid();

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now.AddSeconds(-DateTime.Now.Second);

        [Required]
        public string Materia { get; set; } = string.Empty;

        public string Comentarios { get; set; } = string.Empty;

        [JsonIgnore]
        public (TimeSpan Diferencia, bool Finalizo) TiempoRestante
        {
            get
            {
                TimeSpan diferencia = Fecha.Subtract(DateTime.Now);
                bool Finalizo = diferencia.TotalSeconds <= 0;
                return (Finalizo ? TimeSpan.Zero : diferencia, Finalizo);
            }
        }
    }
}
