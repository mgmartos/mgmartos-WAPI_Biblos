using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WAPI_Biblos1.Models
{
    public class Autor_Des
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Apellidos { get; set; }
        public int AutorId { get; set; }
        [ForeignKey("AutorId")]
        public Autor Autor { get; set; }


    }
}
