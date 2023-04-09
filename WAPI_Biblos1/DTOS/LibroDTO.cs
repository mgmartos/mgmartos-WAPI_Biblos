using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WAPI_Biblos1.Models
{
    public class LibroDTO
    {
        public string Titulo { get; set; }
        public int AutorId { get; set; }
        public int EditorialId { get; set; }
        public int TemaId { get; set; }
        public int Calificacion { get; set; }
        public int Paginas { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class LecturasDTO
    {
        public string titulo { get; set; }
        public string autor { get; set; }
        public int CodAutor { get; set; }
        public DateTime fecha { get; set; }       
        public int calificacion { get; set; }
        public string comentario { get; set; }
        public Boolean Ebook { get; set; }
        public DateTime fecha_Inicio { get; set; }
        public int paginas { get; set; }
    }
}
//$id: "839",
//idLibro: 1852,
//titulo: "MUERTE EN LA FENICE ",
//autor: "DONNA LEON ",
//editorial: "CÍRCULO DE LECTORES ",
//coleccion: " ",
//fecha: "2006-10-06T00:00:00",
//tema: "NOVELA POLICÍACA ",
//calificacion: 6,
//paginas: 296,
//comentario: "Primer libro de la autora que leí, curiosa y evocadora por la ciudad en que transcurre. ",
//prestamo: " ",
//fecha_prestamo: null,
//numobras: null,
//origen: " ",
//CodAutor: 4516