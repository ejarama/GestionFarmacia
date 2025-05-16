using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFarmacia.Entities
{
    class Venta
    {
        public int VentaID { get; set; }
        public int UsuarioID { get; set; }
        public DateTime FechaVenta { get; set; } 
        public decimal TotalVenta { get; set; }
        public List<DetalleVenta> Detalles { get; set; }
        public Usuario Usuario { get; set; }
    }
}
