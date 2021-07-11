using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WAPI_Biblos1.DTOS
{
    public class TemaDTO
    {
        public int Id { get; set; }
        public string NombreTema { get; set; }
    }

    public class TemaCrearDTO
    {
        public string nombreTema { get; set; }
    }
}
