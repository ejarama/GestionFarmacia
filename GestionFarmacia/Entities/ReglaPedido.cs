using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFarmacia.Entities
{
    public class ReglaPedido
    {
        public int ReglaID { get; set; }
        public int ProductoID { get; set; }
        public int ProveedorID { get; set; }
        public int CantidadSugerida { get; set; }
        public bool Activa { get; set; }
    }
}
