using GestionFarmacia.Entities;
using System;
using System.Collections.Generic;

namespace GestionFarmacia.Data.Interfaces
{
    public interface IReporte
    {
        List<ReporteVentas> ObtenerReporteVentas(DateTime fechaInicio, DateTime fechaFin);
      
    }
}
