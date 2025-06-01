using GestionFarmacia.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GestionFarmacia.Data
{
    public class ResultadoVenta
    {
        public bool Exito { get; set; }
        public int VentaID { get; set; }
        public bool PedidoAutomaticoGenerado { get; set; }
    }

    public class UnitOfWork
    {
        private readonly SqlConnection _connection;

        public UnitOfWork()
        {
            _connection = DatabaseConnection.Instance.GetConnection();
        }

        public ResultadoVenta RegistrarVenta(Venta venta)
        {
            SqlTransaction transaction = null;
            var resultado = new ResultadoVenta { Exito = false, VentaID = 0, PedidoAutomaticoGenerado = false };

            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                transaction = _connection.BeginTransaction();

                // Insertar venta principal
                SqlCommand cmdVenta = new SqlCommand("SP_InsertarVenta", _connection, transaction);
                cmdVenta.CommandType = CommandType.StoredProcedure;
                cmdVenta.Parameters.AddWithValue("@UsuarioID", venta.UsuarioID);
                cmdVenta.Parameters.AddWithValue("@FechaVenta", venta.FechaVenta);
                cmdVenta.Parameters.AddWithValue("@TotalVenta", venta.TotalVenta);

                SqlParameter outputId = new SqlParameter("@VentaID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmdVenta.Parameters.Add(outputId);
                cmdVenta.ExecuteNonQuery();
                venta.VentaID = Convert.ToInt32(outputId.Value);

                var productosConStockMinimo = new List<int>();

                foreach (var detalle in venta.Detalles)
                {
                    // Insertar detalle
                    SqlCommand cmdDetalle = new SqlCommand("SP_InsertarDetalleVenta", _connection, transaction);
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.AddWithValue("@VentaID", venta.VentaID);
                    cmdDetalle.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                    cmdDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                    cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                    cmdDetalle.Parameters.AddWithValue("@PorcentajeDescuento", detalle.PorcentajeDescuento);
                    cmdDetalle.ExecuteNonQuery();

                    // Actualizar stock
                    SqlCommand cmdStock = new SqlCommand("SP_ActualizarStock", _connection, transaction);
                    cmdStock.CommandType = CommandType.StoredProcedure;
                    cmdStock.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                    cmdStock.Parameters.AddWithValue("@CantidadVendida", detalle.Cantidad);
                    cmdStock.ExecuteNonQuery();

                    // Consultar stock actual y mínimo
                    SqlCommand cmdConsultaStock = new SqlCommand(
                        "SELECT CantidadStock, StockMinimo FROM Productos WHERE ProductoID = @ProductoID",
                        _connection, transaction);
                    cmdConsultaStock.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);

                    using (var reader = cmdConsultaStock.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int stockActual = Convert.ToInt32(reader["CantidadStock"]);
                            int stockMinimo = Convert.ToInt32(reader["StockMinimo"]);

                            if (stockActual <= stockMinimo)
                            {
                                productosConStockMinimo.Add(detalle.ProductoID);
                            }
                        }
                    }
                }

                bool seGeneroPedido = false;

                foreach (int productoID in productosConStockMinimo)
                {
                    SqlCommand cmdRegla = new SqlCommand("SP_ObtenerReglaPedidoPorProducto", _connection, transaction);
                    cmdRegla.CommandType = CommandType.StoredProcedure;
                    cmdRegla.Parameters.AddWithValue("@ProductoID", productoID);

                    using (var reader = cmdRegla.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int proveedorId = Convert.ToInt32(reader["ProveedorID"]);
                            int cantidadSugerida = Convert.ToInt32(reader["CantidadSugerida"]);

                            reader.Close(); // IMPORTANTE antes de ejecutar otro comando

                            SqlCommand cmdPedido = new SqlCommand("SP_InsertarPedidoAutomatico", _connection, transaction);
                            cmdPedido.CommandType = CommandType.StoredProcedure;
                            cmdPedido.Parameters.AddWithValue("@ProductoID", productoID);
                            cmdPedido.Parameters.AddWithValue("@ProveedorID", proveedorId);
                            cmdPedido.Parameters.AddWithValue("@Cantidad", cantidadSugerida);
                            cmdPedido.ExecuteNonQuery();

                            seGeneroPedido = true;
                        }
                    }
                }

                transaction.Commit();

                resultado.Exito = true;
                resultado.VentaID = venta.VentaID;
                resultado.PedidoAutomaticoGenerado = seGeneroPedido;
                return resultado;
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                throw new Exception("Error al registrar la venta con transacción.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }
    }
}
