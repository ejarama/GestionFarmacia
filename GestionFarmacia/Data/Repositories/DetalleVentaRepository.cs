using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Entities;
using GestionFarmacia.Utils;


namespace GestionFarmacia.Data.Repositories
{
    public class DetalleVentaRepository : IDetalleVentaRepository
    {
        private readonly SqlConnection _connection;

        public DetalleVentaRepository()
        {
            _connection = DatabaseConnection.Instance.GetConnection();
        }

        public bool Insertar(DetalleVenta detalle)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertarDetalleVenta", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VentaID", detalle.VentaID);
                cmd.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                cmd.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                int filasAfectadas = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar detalle venta: " + ex.Message);
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public bool Actualizar(DetalleVenta detalle)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarDetalleVenta", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DetalleID", detalle.DetalleID);
                cmd.Parameters.AddWithValue("@VentaID", detalle.VentaID);
                cmd.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                cmd.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                int filasAfectadas = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar detalle venta: " + ex.Message);
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public bool Eliminar(int detalleID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_BorrarDetalleVenta", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DetalleID", detalleID);

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                int filasAfectadas = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar detalle venta: " + ex.Message);
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public List<DetalleVenta> Consultar(int? detalleID = null)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();

            try
            {
                SqlCommand cmd = new SqlCommand("sp_ConsultarDetalleVenta", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                if (detalleID.HasValue)
                    cmd.Parameters.AddWithValue("@DetalleID", detalleID.Value);
                else
                    cmd.Parameters.AddWithValue("@DetalleID", DBNull.Value);

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DetalleVenta detalle = new DetalleVenta
                    {
                        DetalleID = Convert.ToInt32(reader["DetalleID"]),
                        VentaID = Convert.ToInt32(reader["VentaID"]),
                        ProductoID = Convert.ToInt32(reader["ProductoID"]),
                        Cantidad = Convert.ToInt32(reader["Cantidad"]),
                        PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"])
                    };
                    lista.Add(detalle);
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar detalle venta: " + ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return lista;
        }

        public List<DetalleVenta> ConsultarPorVenta(int ventaID)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();

            try
            {
                SqlCommand cmd = new SqlCommand("sp_ConsultarDetalleVentaPorVenta", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VentaID", ventaID);

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DetalleVenta detalle = new DetalleVenta
                    {
                        DetalleID = Convert.ToInt32(reader["DetalleID"]),
                        VentaID = Convert.ToInt32(reader["VentaID"]),
                        ProductoID = Convert.ToInt32(reader["ProductoID"]),
                        Cantidad = Convert.ToInt32(reader["Cantidad"]),
                        PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"])
                    };
                    lista.Add(detalle);
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar detalle venta por venta: " + ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return lista;
        }
    }
}
