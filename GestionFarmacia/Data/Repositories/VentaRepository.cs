using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Data
{
    public class VentaRepository : IVentaRepository
    {
        private readonly string _connectionString;

        public VentaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Insertar(Venta venta)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_InsertarVenta", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros para la venta
                    cmd.Parameters.AddWithValue("@UsuarioID", venta.UsuarioID);
                    cmd.Parameters.AddWithValue("@FechaVenta", venta.FechaVenta);
                    cmd.Parameters.AddWithValue("@TotalVenta", venta.TotalVenta);

                    // Abrir conexión y ejecutar
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar la venta.", ex);
            }
        }

        public void Actualizar(Venta venta)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_ActualizarVenta", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@VentaID", venta.VentaID);
                    cmd.Parameters.AddWithValue("@UsuarioID", venta.UsuarioID);
                    cmd.Parameters.AddWithValue("@FechaVenta", venta.FechaVenta);
                    cmd.Parameters.AddWithValue("@TotalVenta", venta.TotalVenta);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la venta.", ex);
            }
        }

        public void Eliminar(int ventaId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_EliminarVenta", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@VentaID", ventaId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la venta.", ex);
            }
        }

        public Venta ObtenerPorId(int ventaId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_ConsultarVenta", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VentaID", ventaId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Venta
                            {
                                VentaID = Convert.ToInt32(reader["VentaID"]),
                                UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                                FechaVenta = Convert.ToDateTime(reader["FechaVenta"]),
                                TotalVenta = Convert.ToDecimal(reader["TotalVenta"])
                            };
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la venta por ID.", ex);
            }
        }

        public List<Venta> ObtenerTodos()
        {
            List<Venta> ventas = new List<Venta>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_ConsultarVentas", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ventas.Add(new Venta
                            {
                                VentaID = Convert.ToInt32(reader["VentaID"]),
                                UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                                FechaVenta = Convert.ToDateTime(reader["FechaVenta"]),
                                TotalVenta = Convert.ToDecimal(reader["TotalVenta"])
                            });
                        }
                    }
                }
                return ventas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las ventas.", ex);
            }
        }

        public List<Venta> ObtenerPorRangoFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Venta> ventas = new List<Venta>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_ObtenerReporteVentas", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ventas.Add(new Venta
                            {
                                VentaID = Convert.ToInt32(reader["VentaID"]),
                                UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                                FechaVenta = Convert.ToDateTime(reader["FechaVenta"]),
                                TotalVenta = Convert.ToDecimal(reader["TotalVenta"])
                            });
                        }
                    }
                }
                return ventas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las ventas por rango de fechas.", ex);
            }
        }
    }
}
