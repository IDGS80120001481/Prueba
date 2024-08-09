using Lignaris.DTO;
using Lignaris_Pizza_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lignaris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecetasController : ControllerBase
    {
        private readonly LignarisPizzaContext _context;

        public RecetasController(LignarisPizzaContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ListaRecetas")]
        public async Task<ActionResult<IEnumerable<RecetaDTO>>> GetRecetas()
        {
            var recetas = await _context.Receta
                .Include(r => r.RecetaDetalles)
                .ThenInclude(rd => rd.IdMateriaPrimaNavigation)
                .ToListAsync();

            var recetaDTOs = recetas.Select(r => new RecetaDTO
            {
                IdReceta = r.IdReceta,
                Nombre = r.Nombre,
                Foto = r.Foto,
                Tamanio = r.Tamanio,
                PrecioUnitario = r.PrecioUnitario,
                Estatus = r.Estatus,
                RecetaDetalles = r.RecetaDetalles.Select(rd => new RecetaDetalleDTO
                {
                    IdRecetaDetalle = rd.IdRecetaDetalle,
                    IdMateriaPrima = rd.IdMateriaPrima,
                    Cantidad = rd.Cantidad
                }).ToList()
            }).ToList();

            return recetaDTOs;
        }

        [HttpGet]
        [Route("ListaRecetas/{id:int}")]
        public async Task<ActionResult<RecetaDTO>> GetReceta(int id)
        {
            var receta = await _context.Receta
                .Include(r => r.RecetaDetalles)
                .ThenInclude(rd => rd.IdMateriaPrimaNavigation)
                .FirstOrDefaultAsync(r => r.IdReceta == id);

            if (receta == null)
            {
                return NotFound();
            }

            var recetaDTO = new RecetaDTO
            {
                IdReceta = receta.IdReceta,
                Nombre = receta.Nombre,
                Foto = receta.Foto,
                Tamanio = receta.Tamanio,
                PrecioUnitario = receta.PrecioUnitario,
                Estatus = receta.Estatus,
                RecetaDetalles = receta.RecetaDetalles.Select(rd => new RecetaDetalleDTO
                {
                    IdRecetaDetalle = rd.IdRecetaDetalle,
                    IdMateriaPrima = rd.IdMateriaPrima,
                    Cantidad = rd.Cantidad
                }).ToList()
            };

            return recetaDTO;
        }

        [HttpPost]
        [Route("AgregarReceta")]
        public async Task<ActionResult<RecetaDTO>> PostReceta(RecetaDTO recetaDTO)
        {
            var receta = new Recetum
            {
                Nombre = recetaDTO.Nombre,
                Foto = recetaDTO.Foto,
                Tamanio = recetaDTO.Tamanio,
                PrecioUnitario = recetaDTO.PrecioUnitario,
                Estatus = recetaDTO.Estatus,
                RecetaDetalles = recetaDTO.RecetaDetalles.Select(rd => new RecetaDetalle
                {
                    IdMateriaPrima = rd.IdMateriaPrima,
                    Cantidad = rd.Cantidad
                }).ToList()
            };

            _context.Receta.Add(receta);
            await _context.SaveChangesAsync();

            recetaDTO.IdReceta = receta.IdReceta;

            return CreatedAtAction(nameof(GetReceta), new { id = receta.IdReceta }, recetaDTO);
        }

        [HttpPut]
        [Route("ModificarReceta/{id:int}")]
        public async Task<IActionResult> PutReceta(int id, RecetaDTO recetaDTO)
        {
            if (id != recetaDTO.IdReceta)
            {
                return BadRequest();
            }

            var receta = await _context.Receta
                .Include(r => r.RecetaDetalles)
                .FirstOrDefaultAsync(r => r.IdReceta == id);

            if (receta == null)
            {
                return NotFound();
            }

            receta.Nombre = recetaDTO.Nombre;
            receta.Foto = recetaDTO.Foto;
            receta.Tamanio = recetaDTO.Tamanio;
            receta.PrecioUnitario = recetaDTO.PrecioUnitario;
            receta.Estatus = recetaDTO.Estatus;

            // Update RecetaDetalles
            _context.RecetaDetalles.RemoveRange(receta.RecetaDetalles);
            receta.RecetaDetalles = recetaDTO.RecetaDetalles.Select(rd => new RecetaDetalle
            {
                IdMateriaPrima = rd.IdMateriaPrima,
                Cantidad = rd.Cantidad
            }).ToList();

            _context.Entry(receta).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("EliminarReceta/{id:int}")]
        public async Task<IActionResult> DeleteReceta(int id)
        {
            var receta = await _context.Receta
                .Include(r => r.RecetaDetalles)
                .FirstOrDefaultAsync(r => r.IdReceta == id);

            if (receta == null)
            {
                return NotFound();
            }

            _context.RecetaDetalles.RemoveRange(receta.RecetaDetalles);
            _context.Receta.Remove(receta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}