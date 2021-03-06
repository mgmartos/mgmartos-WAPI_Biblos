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


        [HttpGet("inicio")]
        public async Task<ActionResult<List<int>>> GetDatosInicio()
        {

            int numAuth = await this.context.Autores.CountAsync();
            int numEdit = await this.context.Editoriales.CountAsync();
            int numLibros = await this.context.Libros.CountAsync();
            List<int> lista = new List<int>();
            lista.Add(numAuth);
            lista.Add(numEdit);
            lista.Add(numLibros);
            System.Threading.Thread.Sleep(500);
            return lista;
        }



        [HttpGet]
        [Route("todos2")]
        public ActionResult<string> GetAll()
        {
            // List<mlib> ooo = entidad.mlibs.ToList();
            //IEnumerable<mlib> oooo = entidad.mlibs.ToList();
 

            return "Salida del servicio";
        }

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

        [HttpGet("nomenclatoreditorial")]
        public async Task<ActionResult<List<Editorial>>> GetEditorialesletras(string semilla = "")
        {
            List<Editorial> editorialesNombre = await this.context.Editoriales.Where(a => a.NombreEditorial.StartsWith(semilla)).OrderBy(a => a.NombreEditorial).ToListAsync();
            return (List<Editorial>)editorialesNombre;
        }


        [HttpGet("temas")]
        public async Task<ActionResult<List<Tema>>> GetTemas()
        {
            return await this.context.Temas.AsQueryable().OrderBy(a => a.NombreTema).ToListAsync();
        }

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

    }
}
