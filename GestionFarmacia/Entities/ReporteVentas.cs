using System;
using System.Collections.Generic;
using System.Linq;
using GestionFarmacia.Data;
using GestionFarmacia.Data.Repositories;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Reportes
{
    public class ReporteVentas : IReporte
    {
        public List<object> Generar(DateTime fechaInicio, DateTime fechaFin, string filtroEntrega)
        {
            var repo = new VentaRepository();
            var ventas = repo.ObtenerPorRangoFecha(fechaInicio, fechaFin);

            if (filtroEntrega != "Todos")
            {
                ventas = ventas.Where(v => v.TipoEntrega == filtroEntrega).ToList();
            }

            return ventas.Cast<object>().ToList(); // Lo devolvemos como objeto genérico
        }
    }
}