namespace Lignaris_Pizza_Backend.DTO
{
    public class InventarioDetalleDTO
    {
        public int IdInventario { get; set; }
        public string Nombre { get; set; }
        public decimal? CantidadDisponible { get; set; }
        public DateOnly FechaCompra { get; set; }
        public DateOnly? FechaCaducidad { get; set; }
        public string NumLote { get; set; }
        public int? Estatus { get; set; }
    }
}
