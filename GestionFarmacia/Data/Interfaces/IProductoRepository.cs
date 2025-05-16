using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Data
{
    public interface IProductoRepository
    {
        void Insertar(Producto producto);
        void Actualizar(Producto producto);
        void Eliminar(int productoID);
        Producto ObtenerPorID(int productoID);
        List<Producto> ObtenerTodos();
    }
}
