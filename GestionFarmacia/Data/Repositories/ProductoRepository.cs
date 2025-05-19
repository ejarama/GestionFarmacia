using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GestionFarmacia.Entities;
using GestionFarmacia.Data;

namespace Data.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
       
        private readonly SqlConnection _connectionString;

        public ProductoRepository()
        {
            _connectionString = DatabaseConnection.Instance.GetConnection();
        }

        public void Insertar(Producto producto)
        {
            try
            {
               
                using (SqlCommand cmd = new SqlCommand("SP_InsertarProducto", _connectionString))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@CantidadStock", producto.CantidadStock);
                    cmd.Parameters.AddWithValue("@StockMinimo", producto.StockMinimo);

                    if (_connectionString.State != ConnectionState.Open)
                        _connectionString.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el producto.", ex);
            }
        }

        public void Actualizar(Producto producto)
        {
            try
            {
                
                using (SqlCommand cmd = new SqlCommand("SP_ActualizarProducto", _connectionString))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProductoID", producto.ProductoID);
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@CantidadStock", producto.CantidadStock);
                    cmd.Parameters.AddWithValue("@StockMinimo", producto.StockMinimo);

                    if (_connectionString.State != ConnectionState.Open)
                        _connectionString.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el producto.", ex);
            }
        }

        public void Eliminar(int productoId)
        {
            try
            {
                
                using (SqlCommand cmd = new SqlCommand("SP_BorrarProducto", _connectionString))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProductoID", productoId);

                    if (_connectionString.State != ConnectionState.Open)
                        _connectionString.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el producto.", ex);
            }
        }


        public Producto ObtenerPorID(int productoId)
        {
            try
            {
            
                using (SqlCommand cmd = new SqlCommand("SP_ConsultarProducto", _connectionString))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductoID", productoId);

                    if (_connectionString.State != ConnectionState.Open)
                        _connectionString.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Producto
                            {
                                ProductoID = Convert.ToInt32(reader["ProductoID"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                Precio = Convert.ToDecimal(reader["Precio"]),
                                CantidadStock = Convert.ToInt32(reader["CantidadStock"]),
                                StockMinimo = Convert.ToInt32(reader["StockMinimo"])
                            };
                        }
                    }
                }
                return null; // No encontró el producto
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el producto por ID.", ex);
            }
        }

        public List<Producto> ObtenerTodos()
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                
                using (SqlCommand cmd = new SqlCommand("SP_ConsultarProducto", _connectionString))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Para obtener todos, suponemos que el SP acepta NULL para ProductoID
                    cmd.Parameters.AddWithValue("@ProductoID", DBNull.Value);

                    if (_connectionString.State != ConnectionState.Open)
                        _connectionString.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(new Producto
                            {
                                ProductoID = Convert.ToInt32(reader["ProductoID"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                Precio = Convert.ToDecimal(reader["Precio"]),
                                CantidadStock = Convert.ToInt32(reader["CantidadStock"]),
                                StockMinimo = Convert.ToInt32(reader["StockMinimo"])
                            });
                        }
                    }
                }
                return productos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de productos.", ex);
            }
        }

        
    }
}
