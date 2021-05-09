using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAPI_Biblos1.Models;

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
            return await (this.context.Libros.Where(x => x.Id == id)
                        .Include(x => x.Autor)
                            .Include(x => x.Editorial)
                            .Include(x => x.Tema)
                            .FirstOrDefaultAsync());
                      
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

        [HttpGet("libros-autor")]
        public async Task<ActionResult<List<Libro>>> GetLibros(int idautor)
        {
            return await this.context.Libros.Where(x => x.AutorId == idautor).ToListAsync();
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

    }
}
