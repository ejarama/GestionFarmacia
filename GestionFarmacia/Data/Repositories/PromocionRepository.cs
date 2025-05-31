using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Data.Repositories
{
    public class PromocionRepository : IPromocionRepository
    {
        private readonly SqlConnection _connection;

        public PromocionRepository()
        {
            _connection = DatabaseConnection.Instance.GetConnection();
        }

        public bool Insertar(Promocion promocion)
        {
            try
            {
                using (var cmd = new SqlCommand("SP_InsertarPromocion", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductoID", promocion.ProductoID);
                    cmd.Parameters.AddWithValue("@FechaInicio", promocion.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", promocion.FechaFin);
                    cmd.Parameters.AddWithValue("@PorcentajeDescuento", promocion.PorcentajeDescuento);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar promoción.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public bool Actualizar(Promocion promocion)
        {
            try
            {
                using (var cmd = new SqlCommand("SP_ActualizarPromocion", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PromocionID", promocion.PromocionID);
                    cmd.Parameters.AddWithValue("@ProductoID", promocion.ProductoID);
                    cmd.Parameters.AddWithValue("@FechaInicio", promocion.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", promocion.FechaFin);
                    cmd.Parameters.AddWithValue("@PorcentajeDescuento", promocion.PorcentajeDescuento);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar promoción.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public bool Eliminar(int promocionId)
        {
            try
            {
                using (var cmd = new SqlCommand("SP_EliminarPromocion", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PromocionID", promocionId);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar promoción.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public List<Promocion> ObtenerTodas()
        {
            var lista = new List<Promocion>();
            try
            {
                using (var cmd = new SqlCommand("SP_ObtenerTodasPromociones", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Promocion
                            {
                                PromocionID = Convert.ToInt32(reader["PromocionID"]),
                                ProductoID = Convert.ToInt32(reader["ProductoID"]),
                                FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                                FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                                PorcentajeDescuento = Convert.ToDecimal(reader["PorcentajeDescuento"])
                            });
                        }
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar promociones.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public Promocion ObtenerPromocionVigentePorProducto(int productoId, DateTime fecha)
        {
            Promocion promocion = null;

            try
            {
                using (var cmd = new SqlCommand("SP_ObtenerPromocionVigentePorProducto", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductoID", productoId);
                    cmd.Parameters.AddWithValue("@Fecha", fecha);

                    if (_connection.State != ConnectionState.Open)
                        _connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            promocion = new Promocion
                            {
                                PromocionID = Convert.ToInt32(reader["PromocionID"]),
                                ProductoID = productoId,
                                FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                                FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                                PorcentajeDescuento = Convert.ToDecimal(reader["PorcentajeDescuento"])
                            };
                        }
                    }
                }

                return promocion;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener promoción vigente.", ex);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }
    }
}
