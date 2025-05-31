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
                                FechaPedido = Convert.ToDateTime(reader["FechaPedido"]),
                                Estado = reader["Estado"].ToString(),
                                ProveedorID = 0, // Se puede omitir o mapear si se requiere
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
                using (var conn = _connection)
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        // Actualizar recepción de cada producto
                        foreach (var detalle in detallesRecibidos)
                        {
                            using (var cmdDetalle = new SqlCommand("SP_ActualizarCantidadRecibida", conn, transaction))
                            {
                                cmdDetalle.CommandType = CommandType.StoredProcedure;
                                cmdDetalle.Parameters.AddWithValue("@PedidoID", detalle.PedidoID);
                                cmdDetalle.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                                cmdDetalle.Parameters.AddWithValue("@CantidadRecibida", detalle.CantidadRecibida);
                                cmdDetalle.ExecuteNonQuery();
                            }

                            // Actualizar stock
                            using (var cmdStock = new SqlCommand("SP_AumentarStockProducto", conn, transaction))
                            {
                                cmdStock.CommandType = CommandType.StoredProcedure;
                                cmdStock.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                                cmdStock.Parameters.AddWithValue("@Cantidad", detalle.CantidadRecibida);
                                cmdStock.ExecuteNonQuery();
                            }
                        }

                        // Marcar el pedido como recibido
                        using (var cmdPedido = new SqlCommand("SP_ActualizarEstadoPedido", conn, transaction))
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
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar la recepción del pedido.", ex);
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