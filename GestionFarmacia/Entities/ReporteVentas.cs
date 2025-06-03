using System;

namespace GestionFarmacia.Entities
{
    public class ReporteVentas
    {
        public int VentaID { get; set; }
        public DateTime FechaVenta { get; set; }
        public string NombreUsuario { get; set; }
        public decimal TotalVenta { get; set; }
        public int ProductoID { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal ImporteDescuento { get; set; }
        public decimal TotalProducto { get; set; }
    }
}
