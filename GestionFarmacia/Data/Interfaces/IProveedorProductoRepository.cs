using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFarmacia.Data.Interfaces
{
    public interface IProveedorProductoRepository
    {
        bool AsignarProveedorAProducto(int productoId, int proveedorId);
        bool EliminarProveedorDeProducto(int productoId, int proveedorId);
        List<int> ObtenerProveedoresPorProducto(int productoId);
        List<int> ObtenerProductosPorProveedor(int proveedorId);
    }
}
