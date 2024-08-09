using Microsoft.AspNetCore.Mvc;
using Lignaris_Pizza_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Lignaris.DTO;

namespace Lignaris_Pizza_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriaPrimaController : Controller
    {
        private readonly LignarisPizzaContext _context;

        public MateriaPrimaController(LignarisPizzaContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ListaMateriaPrima")]
        public async Task<ActionResult<IEnumerable<object>>> GetMateriaPrimas()
        {
            var materiaPrimas = await _context.MateriaPrimas
                .Select(m => new
                {
                    m.IdMateriaPrima,
                    m.Nombre,
                    m.TipoMedida,
                    m.CantidadMinima,
                    Proveedores = m.MateriaProveedorIntermedia
                        .Select(mpi => new
                        {
                            mpi.IdProveedor
                        })
                        .ToList()
                })
                .ToListAsync();

            return Ok(materiaPrimas);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaPrima>> GetMateriaPrima(int id)
        {
            var materiaPrima = await _context.MateriaPrimas.Include(m => m.MateriaProveedorIntermedia).FirstOrDefaultAsync(m => m.IdMateriaPrima == id);
            if (materiaPrima == null)
            {
                return NotFound();
            }
            return materiaPrima;
        }

        [HttpPost]
        [Route("{idProveedor}/AgregarMateriaPrima")]
        public async Task<ActionResult> PostMateriaPrima(int idProveedor, List<MateriaPrimaDTO> materiaPrimaDTOs)
        {
            // Verificar si el proveedor existe
            var proveedor = await _context.Proveedors.FindAsync(idProveedor);
            if (proveedor == null)
            {
                return NotFound();
            }

            // Crear una lista para almacenar las entidades de MateriaProveedorIntermedium
            var materiaProveedorIntermedioms = new List<MateriaProveedorIntermedium>();

            foreach (var materiaPrimaDTO in materiaPrimaDTOs)
            {
                // Crear la nueva entidad de MateriaPrima
                var materiaPrima = new MateriaPrima
                {
                    Nombre = materiaPrimaDTO.Nombre,
                    TipoMedida = materiaPrimaDTO.TipoMedida,
                    CantidadMinima = materiaPrimaDTO.CantidadMinima
                };

                // Agregar la MateriaPrima al contexto
                _context.MateriaPrimas.Add(materiaPrima);
                await _context.SaveChangesAsync();  // Guardar los cambios para obtener el IdMateriaPrima

                // Crear la entrada en la tabla intermedia
                var materiaProveedorIntermedium = new MateriaProveedorIntermedium
                {
                    IdMateriaPrima = materiaPrima.IdMateriaPrima,
                    IdProveedor = idProveedor
                };

                materiaProveedorIntermedioms.Add(materiaProveedorIntermedium);
            }

            // Agregar todas las entradas en la tabla intermedia al contexto
            _context.MateriaProveedorIntermedia.AddRange(materiaProveedorIntermedioms);

            // Guardar todos los cambios en la base de datos
            await _context.SaveChangesAsync();

            return NoContent();
        }



        [HttpPut]
        [Route("ModificarMateriaPrima/{id:int}")]
        public async Task<IActionResult> PutMateriaPrima(int id, MateriaPrima materiaPrima)
        {
            if (id != materiaPrima.IdMateriaPrima)
            {
                return BadRequest();
            }

            _context.Entry(materiaPrima).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MateriaPrimaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("EliminarMateriaPrima/{id:int}")]
        public async Task<IActionResult> DeleteMateriaPrima(int id)
        {
            var materiaPrima = await _context.MateriaPrimas.FindAsync(id);
            if (materiaPrima == null)
            {
                return NotFound();
            }

            _context.MateriaPrimas.Remove(materiaPrima);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MateriaPrimaExists(int id)
        {
            return _context.MateriaPrimas.Any(e => e.IdMateriaPrima == id);
        }
    }
}
