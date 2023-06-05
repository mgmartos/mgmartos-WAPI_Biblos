using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAPI_Biblos1.DTOS;
using WAPI_Biblos1.Models;
using WAPI_Biblos1.Utils;

namespace WAPI_Biblos1.Controllers
{
   // [Route("api/[controller]")]
    [Route("api/Libros")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly BiblosDbContext context;
        private readonly IMapper mapper;


        public LibrosController(BiblosDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }



        [HttpGet("autores")]
        public async Task<ActionResult<List<Autor>>> GetAutores(string tipo = "n")
        {
            if (tipo.ToLower() == "n")
                return await this.context.Autores.AsQueryable().OrderBy(a => a.NombreAutor).ToListAsync();
            else
                return await this.context.Autores.AsQueryable().OrderBy(a => a.Apellidos).ToListAsync();
        }

        [HttpGet("autorespag")]
        public async Task<ActionResult<List<Autor>>> GetAutorespag([FromQuery] PaginacionDTO paginacionDTO, string tipo = "n")
        {
            var queryable = context.Autores.AsQueryable();
            await HttpContext.InsertarParametroEnCabecera(queryable);
            if (tipo.ToLower() == "n")
                return await (this.context.Autores.AsQueryable().OrderBy(a => a.NombreAutor)).Paginar(paginacionDTO).ToListAsync();
            else
                return await (this.context.Autores.AsQueryable().OrderBy(a => a.Apellidos)).Paginar(paginacionDTO).ToListAsync();
        }

        [HttpGet("autorespagsinc")]
        public ActionResult<List<Autor>> GetAutorespagsinc([FromQuery] PaginacionDTO paginacionDTO, string tipo = "n")
        {
            var queryable = context.Autores.AsQueryable();
            HttpContext.InsertarParametroEnCabecera2(queryable);
            if (tipo.ToLower() == "n")
                return  (this.context.Autores.AsQueryable().OrderBy(a => a.NombreAutor)).Paginar(paginacionDTO).ToList();
            else
                return (this.context.Autores.AsQueryable().OrderBy(a => a.Apellidos)).Paginar(paginacionDTO).ToList();
        }


        [HttpGet("autoresLetra")]
        public async Task<ActionResult<List<Autor>>> GetAutoresletra([FromQuery] PaginacionDTO paginacionDTO, string tipo = "n", string letra = "A")
        {
            var queryable = context.Autores.AsQueryable().Where(a => a.Nombre.StartsWith(letra));
            var queryable2 = context.Autores.AsQueryable().Where(a => a.Apellidos.StartsWith(letra));
            //var queryable = context.Autores.AsQueryable();
            //await HttpContext.InsertarParametroEnCabecera(queryable);

            if (tipo.ToLower() == "n")
            {
                /// var  queryable = context.Autores.AsQueryable();//.Where(a => a.Nombre.StartsWith(letra));
                await HttpContext.InsertarParametroEnCabecera(queryable);
                return await (this.context.Autores.AsQueryable().Where(a => a.Nombre.StartsWith(letra)).OrderBy(a => a.NombreAutor)).Paginar(paginacionDTO).ToListAsync();
            }
            else
            {
                await HttpContext.InsertarParametroEnCabecera(queryable2);
                return await (this.context.Autores.AsQueryable().Where(a => a.Apellidos.StartsWith(letra)).OrderBy(a => a.Apellidos)).Paginar(paginacionDTO).ToListAsync();
            }
        }

        /// <summary>
        /// Devuelve la cantidad de autores que existen con la inicial dada.
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="letra"></param>
        /// <returns></returns>
        [HttpGet("cantAutoresLetra")]
        public int  GetCantAutoresletra([FromQuery]  string tipo = "n", string letra = "A")
        {
            var queryable = context.Autores.AsQueryable().Where(a => a.Nombre.StartsWith(letra));
            var queryable2 = context.Autores.AsQueryable().Where(a => a.Apellidos.StartsWith(letra));
            //var queryable = context.Autores.AsQueryable();
            //await HttpContext.InsertarParametroEnCabecera(queryable);

            if (tipo.ToLower() == "n")
            {
                /// var  queryable = context.Autores.AsQueryable();//.Where(a => a.Nombre.StartsWith(letra));
                return queryable.Count();
            }
            else
            {
                return queryable2.Count();
            }
        }


        [HttpGet("autoresLetrasinc")]
        public ActionResult<List<Autor>> GetAutoresletrasinc([FromQuery] PaginacionDTO paginacionDTO, string tipo = "n", string letra = "A")
        {
            var queryable = context.Autores.AsQueryable().Where(a => a.Nombre.StartsWith(letra));
            var queryable2 = context.Autores.AsQueryable().Where(a => a.Apellidos.StartsWith(letra));
            //var queryable = context.Autores.AsQueryable();
            //await HttpContext.InsertarParametroEnCabecera(queryable);

            if (tipo.ToLower() == "n")
            {
                /// var  queryable = context.Autores.AsQueryable();//.Where(a => a.Nombre.StartsWith(letra));
                HttpContext.InsertarParametroEnCabecera2(queryable);
                return (this.context.Autores.AsQueryable().Where(a => a.Nombre.StartsWith(letra)).OrderBy(a => a.NombreAutor)).Paginar(paginacionDTO).ToList();
            }
            else
            {
                HttpContext.InsertarParametroEnCabecera2(queryable2);
                return (this.context.Autores.AsQueryable().Where(a => a.Apellidos.StartsWith(letra)).OrderBy(a => a.Apellidos)).Paginar(paginacionDTO).ToList();
            }
        }


        /// <summary>
        /// Obtiene el total de objetos bibliográficos en la BBDD
        /// de forma síncrona
        /// </summary>
        /// <returns></returns>
        [HttpGet("inicio_sinc")]
        //public async Task<ActionResult<List<int>>> GetDatosInicio()
        public  ActionResult<List<int>> GetDatosInicio2()
        {

            int numAuth = this.context.Autores.Count();
            int numEdit = this.context.Editoriales.Count();
            int numLibros = this.context.Libros.Count();
            int numlecturas=  this.context.Lecturas.Count();
            List<int> lista = new List<int>();
            lista.Add(numAuth);
            lista.Add(numEdit);
            lista.Add(numLibros);
            lista.Add(numlecturas);
            System.Threading.Thread.Sleep(500);
            return lista;
        }

        /// <summary>
        /// Obtiene el total de objetos bibliográficos en la BBDD
        /// de forma Asíncrona
        /// </summary>
        /// <returns></returns>
        [HttpGet("inicio")]
        public async Task<ActionResult<List<int>>> GetDatosInicio()
        {

            int numAuth = await this.context.Autores.CountAsync();
            int numEdit = await this.context.Editoriales.CountAsync();
            int numLibros = await this.context.Libros.CountAsync();
            int numlecturas = await this.context.Lecturas.CountAsync();
            List<int> lista = new List<int>();
            lista.Add(numAuth);
            lista.Add(numEdit);
            lista.Add(numLibros);
            lista.Add(numlecturas);
            System.Threading.Thread.Sleep(500);
            return lista;
        }




        [HttpGet]
        [Route("todos2")]
        public ActionResult<string> GetAll()
        {
            // List<mlib> ooo = entidad.mlibs.ToList();
            //IEnumerable<mlib> oooo = entidad.mlibs.ToList();
 

            return "Salida del servicio WebApi";
        }

        /// <summary>
        /// Obtiene una lista paginada de los libros (Sin filtros)
        /// </summary>
        /// <param name="paginacionDTO"></param>
        /// <returns></returns>
        [HttpGet("libros")]

        public async Task<ActionResult<List<Libro>>> GetLibros([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Libros.AsQueryable();
            await HttpContext.InsertarParametroEnCabecera(queryable);
            var libros =  await (this.context.Libros.AsQueryable().OrderBy(x => x.Titulo)
                            .Include(x => x.Autor)
                            .Include(x => x.Editorial)
                            .Include(x => x.Tema)).Paginar(paginacionDTO).ToListAsync();
            return libros;

        }

        /// <summary>
        /// Obtiene un libro de la BBDD por id.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// parámetro ID . Rem
        /// 
        /// </remarks>
        /// <returns></returns>
        [HttpGet("libro")]
        //[Route("libro/{Id:int}")]
        public async Task<ActionResult<Libro>> GetLibroP(int id)
        {
            var lib =  await (this.context.Libros.Where(x => x.Id == id)
                        .Include(x => x.Autor)
                            .Include(x => x.Editorial)
                            .Include(x => x.Tema)
                            .FirstOrDefaultAsync());
            return lib;      
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Ejemplo anterior con Post
        /// </remarks>
        /// 
        [HttpPost("libro2")]
        //[Route("libro/{Id:int}")]
        public async Task<ActionResult<Libro>> GetLibro(int id)
        {
            return await (this.context.Libros.Where(x => x.Id == id)
                        .Include(x => x.Autor)
                            .Include(x => x.Editorial)
                            .Include(x => x.Tema)
                            .FirstOrDefaultAsync());

        }

        [HttpGet("librosautor")]
        public async Task<ActionResult<List<Libro>>> GetLibros(int idautor)
        {
            return await this.context.Libros.Where(x => x.AutorId == idautor)
                            .Include(x => x.Autor)
                            .Include(x => x.Editorial)
                            .Include(x => x.Tema).ToListAsync();
        }

        [HttpGet("librostema")]
        public async Task<ActionResult<List<Libro>>> GetLibrosTema([FromQuery] PaginacionDTO paginacionDTO, int idtema)
        {
            var queryable = this.context.Libros.Where(x => x.TemaId == idtema).AsQueryable();
            await HttpContext.InsertarParametroEnCabecera(queryable);
            //return  await (this.context.Libros.AsQueryable().OrderBy(x => x.Titulo)
            //                .Include(x => x.Autor)
            //                .Include(x => x.Editorial)
            //                .Include(x => x.Tema)).Paginar(paginacionDTO).ToListAsync();
            return await (this.context.Libros.Where(x => x.TemaId == idtema)
                          .Include(x => x.Autor)
                          .Include(x => x.Editorial)
                          .Include(x => x.Tema)).Paginar(paginacionDTO).ToListAsync();
        }

        [HttpGet("libroseditorial")]
        public async Task<ActionResult<List<Libro>>> GetLibrosEditorial([FromQuery] PaginacionDTO paginacionDTO, int ideditorial)
        {
            var queryable = this.context.Libros.Where(x => x.EditorialId == ideditorial).AsQueryable();
            await HttpContext.InsertarParametroEnCabecera(queryable);

            return await (this.context.Libros.Where(x => x.EditorialId == ideditorial)
                          .Include(x => x.Autor)
                          .Include(x => x.Editorial)
                          .Include(x => x.Tema)).Paginar(paginacionDTO).ToListAsync();
        }



        [HttpPut("altal/{id:int}")]
        public async Task<ActionResult> PutLibro(int id, [FromBody] LibroDTO libroDTO)
        {
            var libro = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);
            if (libro == null)
            {
                libro = new Libro() { Titulo =  libroDTO.Titulo , AutorId = libroDTO.AutorId , EditorialId = libroDTO.EditorialId , 
                                      TemaId = libroDTO.TemaId, Calificacion = libroDTO.Calificacion, Paginas = libroDTO.Paginas, Comentario = libroDTO.Comentario, 
                                      Fecha = libroDTO.Fecha};
                context.Libros.Add(libro);
            }
            else
            {
               // libro = mapper.Map<Libro>(libroDTO);
                libro.Titulo = libroDTO.Titulo;
                libro.AutorId = libroDTO.AutorId;
                libro.Calificacion = libroDTO.Calificacion;
                libro.Comentario = libroDTO.Comentario;
                libro.EditorialId = libroDTO.EditorialId;
                libro.Fecha = libroDTO.Fecha;
                libro.Paginas = libroDTO.Paginas;
                libro.TemaId = libroDTO.TemaId;
            }
            await context.SaveChangesAsync();
            return Ok(libro);
        }



        /// <summary>
        /// Obtiene lista de libros cuyo título comienza por una determinada letra (con Paginación)
        /// </summary>
        /// <param name="paginacionDTO"></param>
        /// <param name="letra"></param>
        /// <returns></returns>
        [HttpGet("librosLetra")]
        public async Task<ActionResult<List<Libro>>> GetLibrosLetra([FromQuery] PaginacionDTO paginacionDTO ,string letra="#")
        {
            if ((letra == "#") || (string.IsNullOrEmpty(letra)))
            {
                var queryable = this.context.Libros.AsQueryable().Where(x => x.Titulo.CompareTo("A") < 0 || x.Titulo.CompareTo("ZZZZZZZ") > 0);
                await HttpContext.InsertarParametroEnCabecera(queryable);
                
                return await  (this.context.Libros.AsQueryable().Where(x => x.Titulo.CompareTo("A") < 0 || x.Titulo.CompareTo("ZZZZZZZ") > 0).OrderBy(x => x.Titulo)
                        .Include(x => x.Autor)
                        .Include(x => x.Editorial)
                        .Include(x => x.Tema)).Paginar(paginacionDTO).ToListAsync();
                //   return queryable.Skip((paginacionDTO.Pagina - 1) * paginacionDTO.RecordsPorPagina).
                // Take(paginacionDTO.RecordsPorPagina);

            }
            else
            {
                var queryable2 = this.context.Libros.AsQueryable().Where(x => x.Titulo.StartsWith(letra));
                await HttpContext.InsertarParametroEnCabecera(queryable2);
                return await this.context.Libros.AsQueryable().Where(x => x.Titulo.StartsWith(letra)).OrderBy(x=>x.Titulo)
                            .Include(x => x.Autor)
                            .Include(x => x.Editorial)
                            .Include(x => x.Tema).Paginar(paginacionDTO).ToListAsync();
            }
        }

        /// <summary>
        /// Libros cuyo título contiene la cadena pasada como parámetro
        /// </summary>
        /// <param name="semilla"></param>
        /// <returns></returns>
        [HttpGet("nomenclatorlibros")]
        public async Task<ActionResult<List<Libro>>> GetLibrosNomenclator(/*[FromQuery] PaginacionDTO paginacionDTO,*/ string semilla = "sueñan")
        {
            var libros = await this.context.Libros.AsQueryable().Where(x => x.Titulo.Contains(semilla)).OrderBy(x => x.Titulo)
                            .Include(x => x.Autor)
                            .Include(x => x.Editorial)
                            .Include(x => x.Tema).ToListAsync();

            //var libros = await this.context.Libros.AsQueryable().Where(x => x.Titulo.Contains(semilla)).OrderBy(x => x.Titulo)
            //                    .Include(x => x.Autor)
            //                    .Include(x => x.Editorial)
            //                    .Include(x => x.Tema).Paginar(paginacionDTO).ToListAsync();

            //await HttpContext.InsertarParametroEnCabecera(libros);


            return libros;
        }

        /// <summary>
        /// Lista completa de las editoriales existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet("editoriales")]
        public async Task<ActionResult<List<Editorial>>> GetEditoriales()
        {
            //var categoriasProd = (from categorias in _context.Categoria
            //                      join productos in _context.Producto
            //                         on categorias.Id equals
            //                                        productos.CategoriaId
            //                             into grupo
            //                      select new
            //                      {
            //                          Categoria = categorias.Nombre,
            //                          TotalProductos = grupo.Count()
            //                      })
            //                                       .ToList();
            //foreach (var categoria in categoriasProd)
            //{
            //    Console.WriteLine(categoria.Categoria + ": " +
            //                 categoria.TotalProductos);
            //}

            //var editorialesList = (from editoriales in this.context.Editoriales join libros in this.context.Libros on editoriales.Id equals libros.EditorialId into lista 
            //                       select new { nombre = editoriales.NombreEditorial,
            //                                   numlibros = lista.Count() 
            //                                  }
            //                       ).ToList();





            return await this.context.Editoriales.AsQueryable().OrderBy(a => a.NombreEditorial).ToListAsync();
        }
        /// <summary>
        /// Búsqueda tipo Nomenclator dentro de la lista de editoriales
        /// </summary>
        /// <param name="semilla"></param>
        /// <returns></returns>
        [HttpGet("nomenclatoreditorial")]
        public async Task<ActionResult<List<Editorial>>> GetEditorialesletras(string semilla = "")
        {
            List<Editorial> editorialesNombre = await this.context.Editoriales.Where(a => a.NombreEditorial.StartsWith(semilla)).OrderBy(a => a.NombreEditorial).ToListAsync();
            return (List<Editorial>)editorialesNombre;
        }

        /// <summary>
        ///  TEMAS lista de los temas existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet("temas")]
        public async Task<ActionResult<List<Tema>>> GetTemas()
        {
            return await this.context.Temas.AsQueryable().OrderBy(a => a.NombreTema).ToListAsync();
        }

        /// <summary>
        /// Creación de un nuevo tema
        /// </summary>
        /// <param name="id"></param>
        /// <param name="temaDTO"></param>
        /// <returns></returns>
        // Alta Tema
        [HttpPut("altat/{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] TemaCrearDTO temaDTO)
        {
            var tema = await context.Temas.FirstOrDefaultAsync(x => x.Id == id);
            if (tema == null)
            {
                tema = new Tema() { NombreTema = temaDTO.nombreTema};
                context.Temas.Add(tema);
            }
            else
            {
                tema.NombreTema = temaDTO.nombreTema;
            }
            await context.SaveChangesAsync();
            //            return NoContent();
            return Ok(tema);
        }

        /// <summary>
        /// Eliminación de un tema de la lista de temas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // Delete Tema
        [HttpDelete("delTema/{id:int}")]
        public async Task<ActionResult> DeleteT(int id)
        {
            var tema = await this.context.Temas.FirstOrDefaultAsync(x => x.Id == id);
            if (tema == null)
            {
                return NoContent();
            }
            this.context.Remove(tema);
            await this.context.SaveChangesAsync();
            return NoContent();

        }

        /// <summary>
        /// Búsqueda tipo Nomenclator de temas
        /// </summary>
        /// <param name="semilla"></param>
        /// <returns></returns>
        [HttpGet("nomenclatortema")]
        public async Task<ActionResult<List<Tema>>> GetTemasletras(string semilla = "")
        {
             List<Tema> temasNombre = await this.context.Temas.Where(a => a.NombreTema.StartsWith(semilla)).OrderBy(a => a.NombreTema).ToListAsync();
            return (List<Tema>)temasNombre;

        }

        [HttpGet("traspaso")]
        public void MetodoDePrueba()
        {
            //new WAPI_Biblos1.Utils.Importaciones(this.context).Traspaso_Autores();
            //new WAPI_Biblos1.Utils.Importaciones(this.context).Traspaso_Temas();
            //new WAPI_Biblos1.Utils.Importaciones(this.context).Traspaso_Libros();
        }


        //[HttpPut("{id:int}")]
        //public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        //{
        //    var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);
        //    if (genero == null)
        //    {
        //        return NotFound();
        //    }

        //    genero = mapper.Map(generoCreacionDTO, genero);
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}

        /// <summary>
        ///  AUTORES
        ///  Obtención de un autor por su ID
        /// </summary>
        /// <param name="idautor"></param>
        /// <returns></returns>
        [HttpGet("autor")]
        public async Task<ActionResult<Autor>> GetAutor(int idautor)
        {
            var auth = await this.context.Autores.Where(a => a.Id == idautor).FirstOrDefaultAsync();
            if (auth == null)
            {
                return NotFound();
            }
            return auth;
        }

        /// <summary>
        /// Creación de un nuevo autor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="autorDTO"></param>
        /// <returns></returns>
        [HttpPut("altaa/{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] AutorCrearDTO autorDTO)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
            {
                autor = new Autor() { NombreAutor = autorDTO.nombre + " " + autorDTO.apellidos, Nombre = autorDTO.nombre, Apellidos = autorDTO.apellidos };
                context.Autores.Add(autor);
            }
            else
            {
                autor.NombreAutor = autorDTO.nombre + " " + autorDTO.apellidos;
                autor.Nombre = autorDTO.nombre;
                autor.Apellidos = autorDTO.apellidos;
                //autor = mapper.Map<Autor>(autorDTO);

            }
            await context.SaveChangesAsync();
            //            return NoContent();
            return Ok(autor);

        }
        /// <summary>
        /// Eliminación de un autor de la lista de los mismos por su ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delAutor/{id:int}")]
        public async Task<ActionResult> DeleteA(int id)
        {
            var autor =  await this.context.Autores.FirstOrDefaultAsync(x=> x.Id == id);
            if (autor == null)
            {
                return NoContent();
            }
            this.context.Remove(autor);
            await this.context.SaveChangesAsync();
            return NoContent();

        }

        /// <summary>
        /// Búsqueda tipo nomenclator de autores.
        /// Se buscan coincidencias en nombre y en apellidos
        /// </summary>
        /// <param name="semilla"></param>
        /// <returns></returns>
        [HttpGet("nomenclatorautor")]
        public async Task<ActionResult<List<Autor>>> GetAutoresletras(string semilla = "")
        {
            // IQueryable<List<Autor>> autoresNombre = (IQueryable<List<Autor>>)await this.context.Autores.AsQueryable().Where(a => a.Nombre.StartsWith(semilla)).OrderBy(a => a.NombreAutor).ToListAsync();
            // IQueryable<List<Autor>> autoresApellido = (IQueryable<List<Autor>>)await this.context.Autores.AsQueryable().Where(a => a.Apellidos.StartsWith(semilla)).OrderBy(a => a.Apellidos).ToListAsync();
            // autoresNombre = autoresNombre.Union(autoresApellido);
            List<Autor> autoresNombre = await this.context.Autores.Where(a => a.Nombre.Contains(semilla)).OrderBy(a => a.Nombre).ToListAsync();
            List<Autor> autoresApellido = await this.context.Autores.Where(a => a.Apellidos.Contains(semilla)).OrderBy(a => a.Apellidos).ToListAsync();

            //var autoresNombre2 =  (List<Autor>)autoresNombre.Union(autoresApellido);
            autoresNombre.AddRange(autoresApellido);
            return (List<Autor>)autoresNombre;

        }

        /// <summary>
        ///   Obtiene la lista de lecturas paginada
        ///   Se pasa como parámetro la página y cantidad de registros por página
        /// </summary>
        /// <returns></returns>

        [HttpGet("lecturas")]

        public async Task<ActionResult<List<Lecturas>>> GetLecturas([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Lecturas.AsQueryable();
            await HttpContext.InsertarParametroEnCabecera(queryable);
            //  public async Task<ActionResult<List<Libro>>> GetLibrosLetra([FromQuery] PaginacionDTO paginacionDTO, string letra = "#")
           return  await this.context.Lecturas.AsQueryable().OrderByDescending(a => a.fecha).Paginar(paginacionDTO).ToListAsync();
  }

        




[HttpPut("altalect")]
        public async Task<ActionResult> Put([FromBody] LecturasDTO lecDTO)
        {

            var lectura = await context.Lecturas.FirstOrDefaultAsync(x => x.titulo.Trim() == lecDTO.titulo.Trim());
            try
            {
            if (lectura == null)
            {

                lectura = new Lecturas()
                {
                    titulo = lecDTO.titulo,
                    autor = lecDTO.autor,
                    CodAutor = lecDTO.CodAutor,
                    fecha = lecDTO.fecha,
                    calificacion = lecDTO.calificacion,
                    comentario = lecDTO.comentario,
                    //Ebook = lecDTO.Ebook == true ? 1 : 0,
                    Ebook = lecDTO.Ebook,
                    fecha_Inicio = lecDTO.fecha_Inicio,
                    paginas = lecDTO.paginas
                };
                context.Lecturas.Add(lectura);
            }
            else
            {
                lectura.titulo = lecDTO.titulo;
                lectura.autor = lecDTO.autor;
                lectura.CodAutor = lecDTO.CodAutor;
                lectura.fecha = lecDTO.fecha;
                lectura.calificacion = lecDTO.calificacion;
                lectura.comentario = lecDTO.comentario;
                //lectura.Ebook = lecDTO.Ebook == true ? 1 : 0;
                lectura.Ebook = lecDTO.Ebook;
                lectura.fecha_Inicio = lecDTO.fecha_Inicio;
                lectura.paginas = lecDTO.paginas;

            }
            await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //            return NoContent();
            return Ok(lectura);
        }
        /// <summary>
        /// Obtiene el registro de una lectura a partir del título
        /// </summary>
        /// <param name="titulo"></param>
        /// <returns></returns>
        [HttpGet("lectura")]
        public async Task<ActionResult<Lecturas>> GetLectura(string titulo)
        {

            var lect = await (this.context.Lecturas.Where(x => x.titulo.Trim() == titulo.Trim())
                            .FirstOrDefaultAsync());
            return lect;
        }




    }
}
