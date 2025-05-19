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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly SqlConnection _connection;

        public UsuarioRepository()
        {
            _connection = DatabaseConnection.Instance.GetConnection();
        }

        public bool Insertar(Usuario usuario)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertarUsuario", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                cmd.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                cmd.Parameters.AddWithValue("@Rol", usuario.Rol);

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                int filas = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return filas > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar usuario: " + ex.Message);
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public bool Actualizar(Usuario usuario)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarUsuario", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsuarioID", usuario.UsuarioID);
                cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                cmd.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                cmd.Parameters.AddWithValue("@Rol", usuario.Rol);

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                int filas = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return filas > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar usuario: " + ex.Message);
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public bool Eliminar(int usuarioID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_BorrarUsuario", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                int filas = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return filas > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar usuario: " + ex.Message);
                return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public List<Usuario> Consultar(int? usuarioID = null)
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                SqlCommand cmd = new SqlCommand("sp_ConsultarUsuario", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                if (usuarioID.HasValue)
                    cmd.Parameters.AddWithValue("@UsuarioID", usuarioID.Value);
                else
                    cmd.Parameters.AddWithValue("@UsuarioID", DBNull.Value);

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                        NombreUsuario = reader["NombreUsuario"].ToString(),
                        Contraseña = reader["Contraseña"].ToString(),
                        Rol = reader["Rol"].ToString()
                    };
                    lista.Add(usuario);
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar usuarios: " + ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return lista;
        }

        public Usuario ObtenerPorNombreUsuario(string nombreUsuario)
        {
            Usuario usuario = null;

            try
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerUsuarioPorNombre", _connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                        NombreUsuario = reader["NombreUsuario"].ToString(),
                        Contraseña = reader["Contraseña"].ToString(),
                        Rol = reader["Rol"].ToString()
                    };
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener usuario por nombre: " + ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return usuario;
        }
    }
}
