using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WAPI_Biblos1.Models
{
    public class Editorial
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string NombreEditorial { get; set; }
        public string UrlEditorial { get; set; }

    }
}
