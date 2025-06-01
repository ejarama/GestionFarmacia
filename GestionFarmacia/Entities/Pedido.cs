using System;
using System.Collections.Generic;

namespace GestionFarmacia.Entities
{
    public class Pedido
    {
        public int PedidoID { get; set; }
        public int ProveedorID { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public string Estado { get; set; }

        // Propiedad adicional para mostrar el nombre del proveedor
        public string ProveedorNombre { get; set; }

        // Navegación a detalles, si la necesitas
        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }
}
