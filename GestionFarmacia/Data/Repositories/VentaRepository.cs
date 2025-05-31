using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Data
{
    public class VentaRepository : IVentaRepository
    {
        private readonly SqlConnection _connection;

        public VentaRepository()
        {
            _connection = DatabaseConnection.Instance.GetConnection();
        }

        public void Insertar(Venta venta)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_InsertarVenta", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UsuarioID", venta.UsuarioID);
                    cmd.Parameters.AddWithValue("@FechaVenta", venta.FechaVenta);
                    cmd.Parameters.AddWithValue("@TotalVenta", venta.TotalVenta);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar la venta.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public void Actualizar(Venta venta)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_ActualizarVenta", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VentaID", venta.VentaID);
                    cmd.Parameters.AddWithValue("@UsuarioID", venta.UsuarioID);
                    cmd.Parameters.AddWithValue("@FechaVenta", venta.FechaVenta);
                    cmd.Parameters.AddWithValue("@TotalVenta", venta.TotalVenta);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la venta.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public void Eliminar(int ventaId)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_EliminarVenta", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VentaID", ventaId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la venta.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public Venta ObtenerPorId(int ventaId)
        {
            try
            {
                Venta venta = null;

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_ConsultarVenta", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VentaID", ventaId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            venta = new Venta
                            {
                                VentaID = Convert.ToInt32(reader["VentaID"]),
                                UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                                FechaVenta = Convert.ToDateTime(reader["FechaVenta"]),
                                TotalVenta = Convert.ToDecimal(reader["TotalVenta"]),
                                Detalles = new List<DetalleVenta>()
                            };
                        }
                        reader.Close();
                    }
                }

                // Si la venta existe, consultar sus detalles
                if (venta != null)
                {
                    using (SqlCommand cmdDetalle = new SqlCommand("SP_ConsultarDetalleVenta", _connection))
                    {
                        cmdDetalle.CommandType = CommandType.StoredProcedure;
                        cmdDetalle.Parameters.AddWithValue("@VentaID", ventaId);

                        using (SqlDataReader reader = cmdDetalle.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                venta.Detalles.Add(new DetalleVenta
                                {
                                    ProductoID = Convert.ToInt32(reader["ProductoID"]),
                                    NombreProducto = reader["NombreProducto"].ToString(),
                                    Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                    PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"])
                                });
                            }
                            reader.Close();
                        }
                    }
                }

                return venta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la venta por ID con detalles.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public List<Venta> ObtenerTodos()
        {
            List<Venta> ventas = new List<Venta>();

            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_ConsultarVentas", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

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
                        reader.Close();
                    }
                }

                return ventas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las ventas.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public List<Venta> ObtenerPorRangoFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Venta> ventas = new List<Venta>();

            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (SqlCommand cmd = new SqlCommand("SP_ObtenerReporteVentas", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

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
                        reader.Close();
                    }
                }

                return ventas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las ventas por rango de fechas.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }
    }
}
