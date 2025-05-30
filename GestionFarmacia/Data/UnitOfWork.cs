using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using GestionFarmacia.Data;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Utils
{
    public class UnitOfWork
    {
        private readonly SqlConnection _connection;

        public UnitOfWork()
        {
            _connection = DatabaseConnection.Instance.GetConnection();
        }

        public bool RegistrarVenta(Venta venta)
        {
            SqlTransaction transaction = null;

            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                transaction = _connection.BeginTransaction();

                // 1. Insertar la venta y obtener el ID generado
                SqlCommand cmdVenta = new SqlCommand("sp_InsertarVenta", _connection, transaction);
                cmdVenta.CommandType = CommandType.StoredProcedure;
                cmdVenta.Parameters.AddWithValue("@UsuarioID", venta.UsuarioID);
                cmdVenta.Parameters.AddWithValue("@FechaVenta", venta.FechaVenta);
                cmdVenta.Parameters.AddWithValue("@TotalVenta", venta.TotalVenta);

                SqlParameter outVentaID = new SqlParameter("@VentaID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmdVenta.Parameters.Add(outVentaID);
                cmdVenta.ExecuteNonQuery();
                venta.VentaID = (int)outVentaID.Value;

                // 2. Procesar cada detalle
                foreach (var detalle in venta.Detalles)
                {
                    // Insertar detalle de venta
                    SqlCommand cmdDetalle = new SqlCommand("sp_InsertarDetalleVenta", _connection, transaction);
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.AddWithValue("@VentaID", venta.VentaID);
                    cmdDetalle.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                    cmdDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                    cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                    cmdDetalle.ExecuteNonQuery();

                    // Actualizar stock y obtener nuevo stock
                    SqlCommand cmdStock = new SqlCommand("sp_ActualizarStockProducto", _connection, transaction);
                    cmdStock.CommandType = CommandType.StoredProcedure;
                    cmdStock.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                    cmdStock.Parameters.AddWithValue("@CantidadVendida", detalle.Cantidad);

                    SqlParameter outStock = new SqlParameter("@NuevoStock", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmdStock.Parameters.Add(outStock);
                    cmdStock.ExecuteNonQuery();

                    int nuevoStock = (int)outStock.Value;

                    // Verificar stock mínimo
                    int stockMinimo = ObtenerStockMinimo(detalle.ProductoID, transaction);
                    if (nuevoStock < stockMinimo)
                    {
                        string nombre = ObtenerNombreProducto(detalle.ProductoID, transaction);
                        MessageBox.Show(
                            $"El producto '{detalle.NombreProducto}' (ID: {detalle.ProductoID}) tiene un stock actual de {nuevoStock}, por debajo del mínimo establecido ({stockMinimo}).",
                            "Alerta de stock bajo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }

                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                Console.WriteLine("Error en transacción de venta: " + ex.Message);
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        private int ObtenerStockMinimo(int productoID, SqlTransaction transaction)
        {
            SqlCommand cmd = new SqlCommand("SELECT StockMinimo FROM Productos WHERE ProductoID = @ProductoID", _connection, transaction);
            cmd.Parameters.AddWithValue("@ProductoID", productoID);
            return (int)cmd.ExecuteScalar();
        }

        private string ObtenerNombreProducto(int productoID, SqlTransaction transaction)
        {
            SqlCommand cmd = new SqlCommand("SELECT Nombre FROM Productos WHERE ProductoID = @ProductoID", _connection, transaction);
            cmd.Parameters.AddWithValue("@ProductoID", productoID);
            return cmd.ExecuteScalar().ToString();
        }

    }
}
