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
        public async Task<ActionResult<List<Autor>>> GetAutores(string tipo="n")
        {
            if (tipo.ToLower() == "n")
                return await this.context.Autores.AsQueryable().OrderBy(a => a.NombreAutor).ToListAsync();
            else
                return await this.context.Autores.AsQueryable().OrderBy(a => a.Apellidos).ToListAsync();
        }


        [HttpGet("autoresLetra")]
        public async Task<ActionResult<List<Autor>>> GetAutoresletra(string tipo="n", string letra="A")
        {
            if (tipo.ToLower() == "n")
                return await this.context.Autores.AsQueryable().Where(a => a.Nombre.StartsWith(letra)).OrderBy(a => a.NombreAutor).ToListAsync();
            else
                return await this.context.Autores.AsQueryable().Where(a => a.Apellidos.StartsWith(letra)).OrderBy(a => a.Apellidos).ToListAsync();
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
            return lista;
        }


        //[HttpGet("inicio")]
        //public ActionResult<List<int>> GetDatosInicio()
        //{
        //    int numAuth = this.context.Autores.Count();
        //    int numEdit = this.context.Editoriales.Count();
        //    int numLibros = this.context.Libros.Count();
        //    List<int> lista = new List<int>();
        //    lista.Add(numAuth);
        //    lista.Add(numEdit);
        //    lista.Add(numLibros);
        //    return lista;
        //}


        [HttpGet]
        [Route("todos2")]
        public ActionResult<string> GetAll()
        {
            // List<mlib> ooo = entidad.mlibs.ToList();
            //IEnumerable<mlib> oooo = entidad.mlibs.ToList();

            return "Salida del servicio";
        }

        [HttpGet("libros")]
        public async Task<ActionResult<List<Libro>>> GetLibros()
        {
            return await (this.context.Libros.Include(x => x.Autor)
                            .Include(x => x.Editorial)
                            .Include(x => x.Tema))
                            .ToListAsync();
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







        [HttpGet("librosLetra")]
        public async Task<ActionResult<List<Libro>>> GetLibrosLetra([FromQuery] PaginacionDTO paginacionDTO ,string letra="#")
        {
            if ((letra == "#") || (string.IsNullOrEmpty(letra)))
            {
               // var queryable = this.context.Libros.AsQueryable().Where(x => x.Titulo.CompareTo("A") < 0 || x.Titulo.CompareTo("ZZZZZZZ") > 0).ToListAsync();
                var queryable = this.context.Libros.AsQueryable().Where(x => x.Titulo.CompareTo("A") < 0 || x.Titulo.CompareTo("ZZZZZZZ") > 0).OrderBy(x => x.Titulo)
                        .Include(x => x.Autor)
                        .Include(x => x.Editorial)
                        .Include(x => x.Tema).ToListAsync();
                //await this.HttpContext.InsertarParametroEnCabecera(queryable);

             //   return queryable.Skip((paginacionDTO.Pagina - 1) * paginacionDTO.RecordsPorPagina).
             // Take(paginacionDTO.RecordsPorPagina);

                return await queryable;

            }
            else
            { 
            return await this.context.Libros.AsQueryable().Where(x => x.Titulo.StartsWith(letra)).OrderBy(x=>x.Titulo)
                            .Include(x => x.Autor)
                            .Include(x => x.Editorial)
                            .Include(x => x.Tema).ToListAsync();
            }
        }


        [HttpGet("editoriales")]
        public async Task<ActionResult<List<Editorial>>> GetEditoriales()
        {
            return await this.context.Editoriales.AsQueryable().OrderBy(a => a.NombreEditorial).ToListAsync();
        }

        [HttpGet("temas")]
        public async Task<ActionResult<List<Tema>>> GetTemas()
        {
            return await this.context.Temas.AsQueryable().OrderBy(a => a.NombreTema).ToListAsync();
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
        public async Task<ActionResult<Autor>> GetAutor(int id)
        {
            var auth = await this.context.Autores.Where(a => a.Id == id).FirstOrDefaultAsync();
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


    }
}
