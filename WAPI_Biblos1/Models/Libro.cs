using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WAPI_Biblos1.Models
{
    public class Libro
    {
        public int Id { get; set;}
        [Required]
        [StringLength(maximumLength: 100)]        
        public string Titulo { get; set;}
        public int AutorId { get; set; }
        [ForeignKey("AutorId")]
        public Autor Autor { get; set; }
        public int EditorialId  { get; set; }
        [ForeignKey("EditorialId")]
        public Editorial Editorial{ get; set; }
        public int TemaId { get; set; }
        [ForeignKey("TemaId")]
        public Tema Tema { get; set; }
        [Range(0,10)]
        public int Calificacion { get; set; }
        public int Paginas { get; set; }
        [StringLength(maximumLength: 1024)]
        public string Comentario { get; set; }
        public DateTime Fecha  { get; set; }
    }
}
