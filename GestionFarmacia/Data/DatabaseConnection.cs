using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace GestionFarmacia.Data
{
    

    
        /// <summary>
        /// Se implementa el patrón Singleton para asegurar una única instancia de conexión a la base de datos.
        /// </summary>
        public sealed class DatabaseConnection
        {
            private static DatabaseConnection _instance = null;
            private static readonly object _lock = new object();
            private SqlConnection _connection;

            // Constructor privado para evitar instanciación externa
            private DatabaseConnection()
            {
                // Obtener cadena de conexión desde App.config
                string connectionString = ConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString;
                _connection = new SqlConnection(connectionString);
            }

            /// <summary>
            /// Devuelve la instancia única de DatabaseConnection.
            /// </summary>
            public static DatabaseConnection Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (_lock) // Hilo seguro
                        {
                            if (_instance == null)
                                _instance = new DatabaseConnection();
                        }
                    }
                    return _instance;
                }
            }

            /// <summary>
            /// Devuelve la conexión SQL activa.
            /// </summary>
            public SqlConnection GetConnection()
            {
                return _connection;
            }
        }
    
}
