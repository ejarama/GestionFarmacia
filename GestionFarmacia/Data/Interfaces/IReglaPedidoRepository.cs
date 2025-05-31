using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Data.Interfaces
{
    public interface IReglaPedidoRepository
    {
        ReglaPedido ObtenerPorProducto(int productoId);
        List<ReglaPedido> ObtenerTodas();
        bool Insertar(ReglaPedido regla);
        bool Actualizar(ReglaPedido regla);
        bool Eliminar(int reglaId);
    }
}
