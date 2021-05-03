using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WAPI_Biblos1.Models
{
    public class Autor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string NombreAutor { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Apellidos { get; set; }

    }

    //public class OldAuthor
    //{
    //    public string id { get; set; }
    //    public int idAutor { get; set; }
    //    public string NombreAutor { get; set; }
    //}
}
