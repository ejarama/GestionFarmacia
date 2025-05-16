using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GestionFarmacia.Data;
using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Utils;

namespace Farmacia.Data.Repositories
{
    public class ProveedorProductoRepository : IProveedorProductoRepository
    {
        private readonly SqlConnection _connection;

        public ProveedorProductoRepository()
        {
            _connection = DatabaseConnection.Instance.GetConnection();
        }

        public bool AsignarProveedorAProducto(int productoId, int proveedorId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_AsignarProveedorAProducto", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductoID", productoId);
                    cmd.Parameters.AddWithValue("@ProveedorID", proveedorId);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                // Loguear error o mostrar mensaje según se requiera
                Console.WriteLine("Error al asignar proveedor a producto: " + ex.Message);
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public bool EliminarProveedorDeProducto(int productoId, int proveedorId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_EliminarProveedorDeProducto", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductoID", productoId);
                    cmd.Parameters.AddWithValue("@ProveedorID", proveedorId);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar relación proveedor-producto: " + ex.Message);
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public List<int> ObtenerProveedoresPorProducto(int productoId)
        {
            List<int> proveedores = new List<int>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerProveedoresPorProducto", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductoID", productoId);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            proveedores.Add(Convert.ToInt32(reader["ProveedorID"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener proveedores del producto: " + ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return proveedores;
        }

        public List<int> ObtenerProductosPorProveedor(int proveedorId)
        {
            List<int> productos = new List<int>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerProductosPorProveedor", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProveedorID", proveedorId);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(Convert.ToInt32(reader["ProductoID"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener productos del proveedor: " + ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return productos;
        }
    }
}
