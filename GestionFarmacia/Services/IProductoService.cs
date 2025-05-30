using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Services.Interfaces
{
    public interface IProductoService
    {
        bool RegistrarProducto(Producto producto);
        bool ActualizarProducto(Producto producto);
        bool EliminarProducto(int productoId);
        void AsignarProveedoresAProducto(int productoId, List<int> proveedorIds);
        List<int> ObtenerProveedoresPorProducto(int productoId);
        List<Producto> ObtenerProductos();
        Producto ObtenerPorNombre(string nombre);
    }
}
