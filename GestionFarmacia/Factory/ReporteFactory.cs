using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFarmacia.Reportes
{
    public static class ReporteFactory
    {
        public static IReporte CrearReporte(string tipo)
        {
            switch (tipo)
            {
                case "Ventas":
                    return new ReporteVentas();
                default:
                    throw new ArgumentException("Tipo de reporte no reconocido");
            }
        }
    }
}