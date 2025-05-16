using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFarmacia.Entities
{
    class Proveedor
    {
        public int ProveedorID { get; set; }
        public string Nombre { get; set; }
        public string Contacto { get; set; }
        public List<ProveedorProducto> Productos { get; set; }
    }
}
