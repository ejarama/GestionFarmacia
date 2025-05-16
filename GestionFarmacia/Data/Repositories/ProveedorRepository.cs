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
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly SqlConnection _connection;

        public ProveedorRepository()
        {
            _connection = DatabaseConnection.Instance.GetConnection();
        }

        public bool Insertar(Proveedor proveedor)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertarProveedor", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                    cmd.Parameters.AddWithValue("@Contacto", (object)proveedor.Contacto ?? DBNull.Value);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar proveedor: " + ex.Message);
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public bool Actualizar(Proveedor proveedor)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarProveedor", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProveedorID", proveedor.ProveedorID);
                    cmd.Parameters.AddWithValue("@Nombre", proveedor.Nombre);
                    cmd.Parameters.AddWithValue("@Contacto", (object)proveedor.Contacto ?? DBNull.Value);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar proveedor: " + ex.Message);
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public bool Eliminar(int proveedorId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_BorrarProveedor", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProveedorID", proveedorId);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar proveedor: " + ex.Message);
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public List<Proveedor> Consultar(int? proveedorId = null)
        {
            List<Proveedor> lista = new List<Proveedor>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_ConsultarProveedores", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProveedorID", (object)proveedorId ?? DBNull.Value);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Proveedor
                            {
                                ProveedorID = Convert.ToInt32(reader["ProveedorID"]),
                                Nombre = reader["Nombre"].ToString(),
                                Contacto = reader["Contacto"]?.ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar proveedores: " + ex.Message);
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
