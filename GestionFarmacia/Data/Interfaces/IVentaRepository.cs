using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Data
{
    public interface IVentaRepository
    {

        void Insertar(Venta venta);
        void Actualizar(Venta venta);
        void Eliminar(int ventaId);
        Venta ObtenerPorId(int ventaId);
        List<Venta> ObtenerTodos();
        List<Venta> ObtenerPorRangoFecha(DateTime fechaInicio, DateTime fechaFin);
    }
}
