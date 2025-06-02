using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFarmacia.Reportes
{
    public interface IReporte
    {
        List<object> Generar(DateTime fechaInicio, DateTime fechaFin, string filtroEntrega);
    }
}