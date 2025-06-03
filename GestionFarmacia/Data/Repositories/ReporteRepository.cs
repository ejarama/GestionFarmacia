using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace GestionFarmacia.Data.Repositories
{
    public class ReporteRepository : IReporte
    {
        private readonly SqlConnection _connection;

        public ReporteRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public List<ReporteVentas> ObtenerReporteVentas(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var reportes = new List<ReporteVentas>();

                using (var command = new SqlCommand("sp_ObtenerReporteVentas", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reportes.Add(new ReporteVentas
                            {
                                VentaID = reader["VentaID"] != DBNull.Value ? Convert.ToInt32(reader["VentaID"]) : 0,
                                FechaVenta = reader["FechaVenta"] != DBNull.Value ? Convert.ToDateTime(reader["FechaVenta"]) : DateTime.MinValue,
                                NombreUsuario = reader["NombreUsuario"] != DBNull.Value ? reader["NombreUsuario"].ToString() : string.Empty,
                                TotalVenta = reader["TotalVenta"] != DBNull.Value ? Convert.ToDecimal(reader["TotalVenta"]) : 0,
                                ProductoID = reader["ProductoID"] != DBNull.Value ? Convert.ToInt32(reader["ProductoID"]) : 0,
                                NombreProducto = reader["NombreProducto"] != DBNull.Value ? reader["NombreProducto"].ToString() : string.Empty,
                                Cantidad = reader["Cantidad"] != DBNull.Value ? Convert.ToInt32(reader["Cantidad"]) : 0,
                                PrecioUnitario = reader["PrecioUnitario"] != DBNull.Value ? Convert.ToDecimal(reader["PrecioUnitario"]) : 0,
                                PorcentajeDescuento = reader["PorcentajeDescuento"] != DBNull.Value ? Convert.ToDecimal(reader["PorcentajeDescuento"]) : 0,
                                ImporteDescuento = reader["ImporteDescuento"] != DBNull.Value ? Convert.ToDecimal(reader["ImporteDescuento"]) : 0,
                                TotalProducto = reader["TotalProducto"] != DBNull.Value ? Convert.ToDecimal(reader["TotalProducto"]) : 0,
                            });
                        }

                    }
                }

                return reportes;
            }
            catch (Exception ex)
            {

                throw new ArgumentException("Ocurrió un error al consultar la información del reporte", ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }
    }

}
