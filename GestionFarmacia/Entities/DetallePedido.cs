using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFarmacia.Entities
{
    public class DetallePedido
    {
        public int DetallePedidoID { get; set; }
        public int PedidoID { get; set; }
        public int ProductoID { get; set; }
        public string NombreProducto { get; set; } // Para mostrar en la UI
        public int CantidadSolicitada { get; set; }
        public int? CantidadRecibida { get; set; }
    }
}