using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GestionFarmacia.Entities;
using GestionFarmacia.Data.Interfaces;


namespace GestionFarmacia.Data.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly SqlConnection _connection;

        public ProductoRepository()
        {
            _connection = DatabaseConnection.Instance.GetConnection();
        }

        public void Insertar(Producto producto)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertarProducto", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@CantidadStock", producto.CantidadStock);
                    cmd.Parameters.AddWithValue("@StockMinimo", producto.StockMinimo);

                    AbrirConexion();
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                CerrarConexion();
            }
        }

        public void Actualizar(Producto producto)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarProducto", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductoID", producto.ProductoID);
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@CantidadStock", producto.CantidadStock);
                    cmd.Parameters.AddWithValue("@StockMinimo", producto.StockMinimo);

                    AbrirConexion();
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                CerrarConexion();
            }
        }

        public void Eliminar(int productoID)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_EliminarProducto", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductoID", productoID);

                    AbrirConexion();
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                CerrarConexion();
            }
        }

        public Producto ObtenerPorID(int productoID)
        {
            Producto producto = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerProductoPorID", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductoID", productoID);

                    AbrirConexion();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto = MapearProducto(reader);
                        }
                    }
                }
            }
            finally
            {
                CerrarConexion();
            }

            return producto;
        }

        public Producto ObtenerPorNombre(string nombre)
        {
            Producto producto = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerProductoPorNombre", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", nombre);

                    AbrirConexion();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto = MapearProducto(reader);
                        }
                    }
                }
            }
            finally
            {
                CerrarConexion();
            }

            return producto;
        }

        public List<Producto> ObtenerTodos()
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_ObtenerTodosProductos", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    AbrirConexion();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(MapearProducto(reader));
                        }
                    }
                }
            }
            finally
            {
                CerrarConexion();
            }

            return productos;
        }

        private Producto MapearProducto(SqlDataReader reader)
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

        private void AbrirConexion()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }

        private void CerrarConexion()
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
        }
    }
}
