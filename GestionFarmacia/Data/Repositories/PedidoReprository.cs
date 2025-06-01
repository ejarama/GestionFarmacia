using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Data.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly SqlConnection _connection;

        public PedidoRepository()
        {
            // Usamos siempre la misma instancia de SqlConnection proveniente del singleton
            _connection = DatabaseConnection.Instance.GetConnection();
        }

        public int CrearPedido(Pedido pedido)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_CrearPedido", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProveedorID", pedido.ProveedorID);
                    cmd.Parameters.AddWithValue("@FechaPedido", pedido.FechaPedido);

                    SqlParameter outputId = new SqlParameter("@PedidoID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputId);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    cmd.ExecuteNonQuery();
                    return Convert.ToInt32(outputId.Value);
                }
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public void InsertarDetallePedido(int pedidoId, DetallePedido detalle)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_InsertarDetallePedido", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PedidoID", pedidoId);
                    cmd.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                    cmd.Parameters.AddWithValue("@CantidadSolicitada", detalle.CantidadSolicitada);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public List<Pedido> ObtenerPedidosPendientes()
        {
            var lista = new List<Pedido>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_ObtenerPedidosPendientes", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Pedido
                            {
                                PedidoID = Convert.ToInt32(reader["PedidoID"]),
                                ProveedorID = Convert.ToInt32(reader["ProveedorID"]),
                                FechaPedido = Convert.ToDateTime(reader["FechaPedido"]),
                                Estado = reader["Estado"].ToString(),
                                ProveedorNombre = reader["ProveedorNombre"].ToString()
                            });
                        }
                    }
                }
                return lista;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public List<DetallePedido> ObtenerDetallePedido(int pedidoId)
        {
            var lista = new List<DetallePedido>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_ObtenerDetallePedido", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PedidoID", pedidoId);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new DetallePedido
                            {
                                PedidoID = pedidoId,
                                DetallePedidoID = Convert.ToInt32(reader["DetallePedidoID"]),
                                ProductoID = Convert.ToInt32(reader["ProductoID"]),
                                NombreProducto = reader["NombreProducto"].ToString(),
                                CantidadSolicitada = Convert.ToInt32(reader["CantidadSolicitada"]),
                                CantidadRecibida = reader["CantidadRecibida"] != DBNull.Value
                                                      ? Convert.ToInt32(reader["CantidadRecibida"])
                                                      : 0
                            });
                        }
                    }
                }
                return lista;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public bool RegistrarRecepcion(int pedidoId, List<DetallePedido> detallesRecibidos)
        {
            try
            {
                // Usamos directamente _connection sin envolverlo en using(var conn = _connection)
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (var transaction = _connection.BeginTransaction())
                {
                    // Actualizar recepción de cada producto
                    foreach (var detalle in detallesRecibidos)
                    {
                        using (var cmdDetalle = new SqlCommand("SP_ActualizarCantidadRecibida", _connection, transaction))
                        {
                            cmdDetalle.CommandType = CommandType.StoredProcedure;

                            // Aquí el SP debe esperar exactamente estos tres parámetros:
                            cmdDetalle.Parameters.AddWithValue("@PedidoID", detalle.PedidoID);
                            cmdDetalle.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                            cmdDetalle.Parameters.AddWithValue("@CantidadRecibida", detalle.CantidadRecibida);

                            cmdDetalle.ExecuteNonQuery();
                        }

                        // Actualizar stock
                        using (var cmdStock = new SqlCommand("SP_AumentarStockProducto", _connection, transaction))
                        {
                            cmdStock.CommandType = CommandType.StoredProcedure;
                            cmdStock.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                            cmdStock.Parameters.AddWithValue("@Cantidad", detalle.CantidadRecibida);
                            cmdStock.ExecuteNonQuery();
                        }
                    }

                    // Marcar el pedido como recibido
                    using (var cmdPedido = new SqlCommand("SP_ActualizarEstadoPedido", _connection, transaction))
                    {
                        cmdPedido.CommandType = CommandType.StoredProcedure;
                        cmdPedido.Parameters.AddWithValue("@PedidoID", pedidoId);
                        cmdPedido.Parameters.AddWithValue("@FechaRecepcion", DateTime.Now);
                        cmdPedido.Parameters.AddWithValue("@Estado", "Recibido");
                        cmdPedido.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Si falla algo, hacemos rollback y lanzamos la excepción
                try
                {
                    _connection?.BeginTransaction()?.Rollback();
                }
                catch { /* Ignoramos errores de rollback */ }

                throw new Exception("Error al registrar la recepción del pedido.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public void ActualizarCantidadRecibida(int detallePedidoId, int cantidadRecibida)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_ActualizarCantidadRecibida", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DetallePedidoID", detallePedidoId);
                    cmd.Parameters.AddWithValue("@CantidadRecibida", cantidadRecibida);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }
    }
}
