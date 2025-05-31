namespace GestionFarmacia.Entities
{
    public class DetalleVenta
    {
        public int DetalleID { get; set; }
        public int VentaID { get; set; }
        public int ProductoID { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PorcentajeDescuento { get; set; }

        public decimal Subtotal
        {
            get
            {
                var descuento = PrecioUnitario * (PorcentajeDescuento / 100);
                return (PrecioUnitario - descuento) * Cantidad;
            }
        }

    }
}
