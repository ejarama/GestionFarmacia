using System;
using System.Collections.Generic;

namespace GestionFarmacia.Entities
{
    public class Venta
    {
        public int VentaID { get; set; }
        public int UsuarioID { get; set; } 
        public DateTime FechaVenta { get; set; }
        public decimal TotalVenta { get; set; }

        public string TipoEntrega { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    }
}
