using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Data.Repositories;
using System.Data.SqlClient;
using GestionFarmacia.Data;

public static class ReporteFactory
{
    public static IReporte CrearReporte()
    {
        return new ReporteRepository(DatabaseConnection.Instance.GetConnection());
    }
}

