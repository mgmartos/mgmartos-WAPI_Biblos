using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WAPI_Biblos1.Models
{
    public class BiblosDbContext2 : DbContext
    {
        private Func<object, object> p;
        //private string _conex;

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Editorial> Editoriales { get; set; }
        public DbSet<Tema> Temas { get; set; }
        public DbSet<Libro> Libros { get; set; }

        public BiblosDbContext2()// : base(options)
        {
            //_conex = options;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // string cadena = "Server=DESKTOP-6I4KAIB;Initial Catalog=Biblos2;Persist Security Info=False;User ID=sa;Password=trijaka;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
            optionsBuilder.UseSqlServer(this.GetConnectionString());
        }


        public static IConfiguration Configuration { get; set; }

        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            return Configuration.GetConnectionString("DefaultConnection");

        }

    }
}
