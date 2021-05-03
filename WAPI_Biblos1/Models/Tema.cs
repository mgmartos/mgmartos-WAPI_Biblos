using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WAPI_Biblos1.Models
{
    public class Tema
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string NombreTema { get; set; }
    }
}
