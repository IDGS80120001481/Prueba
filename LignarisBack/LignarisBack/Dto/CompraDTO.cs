namespace Lignaris_Pizza_Backend.DTO
{
    public class CompraDTO
    {
        public int IdEmpleado { get; set; }
        public int IdProveedor { get; set; }
        public DateTime FechaCompra { get; set; }
        public List<CompraDetalleDTO> DetallesCompra { get; set; } = new List<CompraDetalleDTO>();
        public List<InventarioDTO> Inventarios { get; set; } = new List<InventarioDTO>();
    }
}
