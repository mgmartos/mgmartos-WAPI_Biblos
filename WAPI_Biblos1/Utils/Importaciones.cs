using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WAPI_Biblos1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;


namespace WAPI_Biblos1.Utils
{
    public class Importaciones 
    {
        private readonly BiblosDbContext context;
  //      public IConfiguration Configuration { get; }

       // string cadena = "Server=DESKTOP-6I4KAIB;Initial Catalog=Biblos2;Persist Security Info=False;User ID=sa;Password=trijaka;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";

        public Importaciones(BiblosDbContext context)
        {
            this.context = context;
        }

        public void Traspaso_Temas()
        {
            string fileName = "D:\\Pruebas\\WAPI_Biblos1\\temas.txt";
            string streamLista = System.IO.File.ReadAllText(fileName);
            string[] sTemas = streamLista.Split(',');
            for (int i=0;i<sTemas.Length;i++)
            {
                context.Add(new Tema() { NombreTema = sTemas[i].Trim() });
                context.SaveChanges();
                System.Diagnostics.Debug.WriteLine("Tema : " + sTemas[i]);
            }

            fileName = "D:\\Pruebas\\WAPI_Biblos1\\editoriales.txt";
            string streameditorial = System.IO.File.ReadAllText(fileName);
            string[] sEditorial = streameditorial.Split(',');
            for (int i = 0; i < sEditorial.Length; i++)
            {
                context.Add(new Editorial() { NombreEditorial = sEditorial[i].Trim() });
                context.SaveChanges();
                System.Diagnostics.Debug.WriteLine("Editorial : " + sEditorial[i]);
            }


        }
        public void Traspaso_Autores()
        {


            OldAuthor autor = new OldAuthor()
            {
                id = "9999",
                idAutor = 3999,
                NombreAutor = "Autor Ansurez"
            };

            string fileName = "D:\\Pruebas\\WAPI_Biblos1\\autores.json";
            string jsonString = System.IO.File.ReadAllText(fileName);

            jsonString = jsonString.Replace('[', ' ');
            jsonString = jsonString.Replace(']', ' ');
            jsonString = jsonString.Replace("\r\n", "");
            jsonString = jsonString.Replace("},{", "};{");


            List<string> jsonAutores = jsonString.Trim().Split(';').ToList();
            List<OldAuthor> ListaAutores = new List<OldAuthor>();
            foreach (string item in jsonAutores)
            {
                OldAuthor itemAutor = Newtonsoft.Json.JsonConvert.DeserializeObject<OldAuthor>(item);
                System.Diagnostics.Debug.WriteLine(itemAutor.NombreAutor);
                ListaAutores.Add(itemAutor);
                Autor rautor = new Autor();
                rautor.NombreAutor = itemAutor.NombreAutor;
                string[] partes = itemAutor.NombreAutor.Split(' ');
                rautor.Nombre = rautor.Apellidos = " ";
                if (partes.Length > 0)
                {
                    rautor.Nombre = partes[0];
                    for (int i = 1; i < partes.Length; i++)
                    rautor.Apellidos += partes[i] + " ";
                }        
                context.Add(rautor);
                context.SaveChanges();
            }
            //System.Diagnostics.Debug.WriteLine(ListaAutores.ToString());

        }




        public void Traspaso_Libros()
        {
            DateTime inicio = DateTime.Now;
            string fileName = "D:\\Pruebas\\WAPI_Biblos1\\Libros.json";
            string jsonString = System.IO.File.ReadAllText(fileName);

            jsonString = jsonString.Replace('$', ' ');
            jsonString = jsonString.Replace('[', ' ');
            jsonString = jsonString.Replace(']', ' ');
            jsonString = jsonString.Replace("\r\n", "");
            jsonString = jsonString.Replace("},{", "};{");

            // Obtenemos la lista de temas
           // List<Tema> ListaTemas = this.context.Temas.ToList();
            // Obtenemos lista de editoriales
           // List<Editorial> ListaEditoriales = this.context.Editoriales.ToList();
            // obtenemos la lista de autores
           // List<Autor> ListaAutores = this.context.Autores.ToList();


            List<string> jsonLibros = jsonString.Trim().Split(';').ToList();
            List<OldLibro> ListaLibros = new List<OldLibro>();

            foreach (string item in jsonLibros)
            {
                try
                {
                    OldLibro itemLibro = Newtonsoft.Json.JsonConvert.DeserializeObject<OldLibro>(item);                                
                   // System.Diagnostics.Debug.WriteLine(itemLibro.titulo);
                   // ListaLibros.Add(itemLibro);
                    Libro rlibro = new Libro();
                    rlibro.Titulo = itemLibro.titulo;
                    int calificacion = rlibro.Calificacion = 0;
                    if (int.TryParse(itemLibro.calificacion, out calificacion))
                        rlibro.Calificacion = calificacion;
                    int paginas = rlibro.Paginas = 0;
                    if (int.TryParse(itemLibro.paginas, out paginas))
                        rlibro.Paginas = paginas;
                    rlibro.Comentario = "";                    
                    rlibro.Comentario = itemLibro.comentario;

                    //var tema = from m in ListaTemas where m.NombreTema.Contains(itemLibro.tema.Trim())  select m;
                    //var editorial = from m in ListaEditoriales
                    //                where m.NombreEditorial.Contains(itemLibro.editorial.Trim())
                    //                select m;
                    //var autor = from m in ListaAutores
                    //            where m.NombreAutor.Contains(itemLibro.autor.Trim())    select m;

                        var tema = from m in this.context.Temas where m.NombreTema.Contains(itemLibro.tema.Trim()) select m;
                   
                    var editorial = from m in this.context.Editoriales
                                    where m.NombreEditorial.Contains(itemLibro.editorial.Trim())
                                    select m;
                    var autor = from m in this.context.Autores
                                where m.NombreAutor.Contains(itemLibro.autor.Trim())
                                select m;
                    rlibro.TemaId = 1;
                    rlibro.EditorialId = 1;
                    rlibro.AutorId = 27;
                    if (tema.Count() > 0)
                        rlibro.TemaId = tema.FirstOrDefault().Id;
                    if (editorial.Count() > 0)
                        rlibro.EditorialId = editorial.FirstOrDefault().Id;
                    if (autor.Count() > 0 )
                    rlibro.AutorId = autor.FirstOrDefault().Id;
                    DateTime fecha;
                    try 
                    {
                    fecha  = DateTime.Parse(itemLibro.fecha);
                    }
                    catch(Exception ex)
                    {
                        fecha = new DateTime(1970, 01, 01);
                        System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
                    }

                    rlibro.Fecha = fecha;

                    this.context.Add(rlibro);

                    System.Diagnostics.Debug.WriteLine(itemLibro.titulo);

                }           
                catch (Exception ex ) 
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(item);
                }
                //  context.Add(rlibro);
                
            }
            //this.context.SaveChanges();
            
            
            DateTime fin = DateTime.Now;
            long intervalo = fin.Ticks - inicio.Ticks;
            DateTime fIntervalo = new DateTime(intervalo);

            System.Diagnostics.Debug.WriteLine(intervalo.ToString());
            System.Diagnostics.Debug.WriteLine(fIntervalo.ToLongDateString());
            System.Diagnostics.Debug.WriteLine(fIntervalo.ToLongTimeString());



        }



    }

    public class OldAuthor
    {
        public string id { get; set; }
        public int idAutor { get; set; }
        public string NombreAutor { get; set; }
    }

    public class OldLibro
    {
        public string id { get; set; }
        public string idLibro { get; set; }
        public string titulo { get; set; }
        public string autor { get; set; }
        public string editorial { get; set; }
        public string coleccion { get; set; }
        public string fecha { get; set; }
        public string tema { get; set; }
        public string calificacion { get; set; }
        public string paginas { get; set; }
        public string comentario { get; set; }
        public string prestamo { get; set; }
        public string fecha_prestamo { get; set; }
        public string numobras { get; set; }
        public string origen { get; set; }
        public string CodAutor { get; set; }
    }
}
