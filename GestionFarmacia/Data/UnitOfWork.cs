using GestionFarmacia.Entities;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GestionFarmacia.Data
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

                // Insertar venta principal
                SqlCommand cmdVenta = new SqlCommand("SP_InsertarVenta", _connection, transaction);
                cmdVenta.CommandType = CommandType.StoredProcedure;
                cmdVenta.Parameters.AddWithValue("@UsuarioID", venta.UsuarioID);
                cmdVenta.Parameters.AddWithValue("@FechaVenta", venta.FechaVenta);
                cmdVenta.Parameters.AddWithValue("@TotalVenta", venta.TotalVenta);

                // Salida con el nuevo ID generado
                SqlParameter outputId = new SqlParameter("@VentaID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmdVenta.Parameters.Add(outputId);

                cmdVenta.ExecuteNonQuery();
                venta.VentaID = Convert.ToInt32(outputId.Value);

                // Insertar detalles
                foreach (var detalle in venta.Detalles)
                {
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
                }

                transaction.Commit();
                return true;
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