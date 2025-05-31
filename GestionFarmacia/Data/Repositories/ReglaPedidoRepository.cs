using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Data.Repositories
{
    public class ReglaPedidoRepository : IReglaPedidoRepository
    {
        private readonly SqlConnection _connection;

        public ReglaPedidoRepository()
        {
            _connection = DatabaseConnection.Instance.GetConnection();
        }

        public ReglaPedido ObtenerPorProducto(int productoId)
        {
            ReglaPedido regla = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_ObtenerReglaPedidoPorProducto", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductoID", productoId);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            regla = new ReglaPedido
                            {
                                ReglaID = Convert.ToInt32(reader["ReglaID"]),
                                ProductoID = Convert.ToInt32(reader["ProductoID"]),
                                ProveedorID = Convert.ToInt32(reader["ProveedorID"]),
                                CantidadSugerida = Convert.ToInt32(reader["CantidadSugerida"]),
                                Activa = Convert.ToBoolean(reader["Activa"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la regla de pedido por producto.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return regla;
        }

        public List<ReglaPedido> ObtenerTodas()
        {
            var lista = new List<ReglaPedido>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_ObtenerTodasReglasPedido", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ReglaPedido
                            {
                                ReglaID = Convert.ToInt32(reader["ReglaID"]),
                                ProductoID = Convert.ToInt32(reader["ProductoID"]),
                                ProveedorID = Convert.ToInt32(reader["ProveedorID"]),
                                CantidadSugerida = Convert.ToInt32(reader["CantidadSugerida"]),
                                Activa = Convert.ToBoolean(reader["Activa"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las reglas de pedido.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return lista;
        }

        public bool Insertar(ReglaPedido regla)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_InsertarReglaPedido", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductoID", regla.ProductoID);
                    cmd.Parameters.AddWithValue("@ProveedorID", regla.ProveedorID);
                    cmd.Parameters.AddWithValue("@CantidadSugerida", regla.CantidadSugerida);
                    cmd.Parameters.AddWithValue("@Activa", regla.Activa);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar regla de pedido.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public bool Actualizar(ReglaPedido regla)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_ActualizarReglaPedido", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReglaID", regla.ReglaID);
                    cmd.Parameters.AddWithValue("@ProductoID", regla.ProductoID);
                    cmd.Parameters.AddWithValue("@ProveedorID", regla.ProveedorID);
                    cmd.Parameters.AddWithValue("@CantidadSugerida", regla.CantidadSugerida);
                    cmd.Parameters.AddWithValue("@Activa", regla.Activa);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar regla de pedido.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public bool Eliminar(int reglaId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_EliminarReglaPedido", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReglaID", reglaId);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar regla de pedido.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }
    }
}
