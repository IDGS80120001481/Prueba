using Lignaris_Pizza_Backend.Models;
using LignarisBack.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace LignarisBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class VentaController : ControllerBase
    {

        private readonly LignarisPizzaContext _basedatos;

        public VentaController(LignarisPizzaContext basedatos)
        {
            _basedatos = basedatos;
        }

        [HttpPost]
        public async Task<ActionResult<VentaDto>> InsertVenta(VentaDto _venta)
        {
            var venta = new Ventum
            {
                IdEmpleado = _venta.IdEmpleado,
                IdCliente = _venta.IdCliente,
                Estatus = _venta.Estatus,
                FechaVenta = _venta.FechaVenta,
                Total = _venta.Total
            };

            _basedatos.Venta.Add(venta);
            await _basedatos.SaveChangesAsync();

            int idVenta = venta.IdVenta;

            for(int i = 0; i < _venta.DetalleVenta!.Length; i++) 
            {
                var detalleVenta = new VentaDetalle
                {
                    IdVenta = idVenta,
                    IdReceta = _venta.DetalleVenta[i].IdReceta,
                    Cantidad = _venta.DetalleVenta[i].Cantidad
                };

                _basedatos.VentaDetalles.Add(detalleVenta);
                await _basedatos.SaveChangesAsync();
            }

            return Ok("Se ha realizado correctamente la venta: " + idVenta);
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecetaVentasDto>>> GetRecetasVenta()
        {
            return await _basedatos.Receta
                .Select(x => mapRecetaDto(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecetaVentasDto>> GetRecetaIDVenta(int id)
        {
            var receta = await _basedatos.Receta.FindAsync(id);

            if (receta == null)
            {
                return NotFound();
            }

            return mapRecetaDto(receta);
        }

        private static RecetaVentasDto mapRecetaDto(Recetum receta) =>
           new RecetaVentasDto
           {
               IdReceta = receta.IdReceta,
               Nombre = receta.Nombre,
               Descripcion = receta.descripcion,
               Foto = receta.Foto,
               Tamanio = receta.Tamanio,
               PrecioUnitario = receta.PrecioUnitario,
               Estatus = receta.Estatus
           };
    }
}
