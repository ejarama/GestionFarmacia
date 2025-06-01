using GestionFarmacia.Entities;
using System;
using System.Collections.Generic;
using GestionFarmacia.Data;

namespace GestionFarmacia.Services
{
    public interface IVentaService
    {
        ResultadoVenta RegistrarVenta(Venta venta);
        Venta ObtenerPorId(int ventaId);
        List<Venta> ObtenerTodos();
        List<Venta> ObtenerPorRangoFecha(DateTime fechaInicio, DateTime fechaFin);
    }
}
