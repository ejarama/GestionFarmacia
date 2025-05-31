using GestionFarmacia.Entities;
using System;
using System.Collections.Generic;

namespace GestionFarmacia.Services
{
    public interface IVentaService
    {
        bool RegistrarVenta(Venta venta);
        Venta ObtenerPorId(int ventaId);
        List<Venta> ObtenerTodos();
        List<Venta> ObtenerPorRangoFecha(DateTime fechaInicio, DateTime fechaFin);
    }
}
