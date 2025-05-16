using System.Collections.Generic;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Data.Interfaces
{
    public interface IDetalleVentaRepository
    {
        bool Insertar(DetalleVenta detalle);
        bool Actualizar(DetalleVenta detalle);
        bool Eliminar(int detalleID);
        List<DetalleVenta> Consultar(int? detalleID = null);
        List<DetalleVenta> ConsultarPorVenta(int ventaID);
    }
}
