using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WAPI_Biblos1.Models
{
    public class BiblosDbContext : DbContext
    {
        //private Func<object, object> p;

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Editorial> Editoriales { get; set; }
        public DbSet<Tema> Temas { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Lecturas> Lecturas { get; set; }

        public BiblosDbContext( DbContextOptions options) : base(options)
        {
            
        }

    }
}

