using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFarmacia.Entities
{
    class ProveedorProducto
    {
        public int ProveedorID { get; set; }
        public int ProductoID { get; set; }
        public Proveedor Proveedor { get; set; }
        public Producto Producto { get; set; }
    }
}
