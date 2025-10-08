using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Examen_Unidad3.Database
{
    public class CajerosRepository
    {
        public static List<Cajero> ObtenerTodos()
        {
            var cajeros = new List<Cajero>();

            using (var conexion = DatabaseManager.ObtenerConexion())
            {
                conexion.Open();
                string sql = "SELECT Id, Nombre, Clave, Activo FROM Cajeros WHERE Activo = 1";

                using (var cmd = new SQLiteCommand(sql, conexion))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cajeros.Add(new Cajero
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Clave = reader.GetString(2)
                        });
                    }
                }
            }

            return cajeros;
        }

        public static bool ValidarClave(string clave)
        {
            using (var conexion = DatabaseManager.ObtenerConexion())
            {
                conexion.Open();
                string sql = "SELECT COUNT(*) FROM Cajeros WHERE Clave = @clave AND Activo = 1";

                using (var cmd = new SQLiteCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@clave", clave);
                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public static string ObtenerNombrePorClave(string clave)
        {
            using (var conexion = DatabaseManager.ObtenerConexion())
            {
                conexion.Open();
                string sql = "SELECT Nombre FROM Cajeros WHERE Clave = @clave AND Activo = 1";

                using (var cmd = new SQLiteCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@clave", clave);
                    object resultado = cmd.ExecuteScalar();
                    return resultado?.ToString() ?? "";
                }
            }
        }

        public static bool Agregar(string nombre, string clave)
        {
            try
            {
                using (var conexion = DatabaseManager.ObtenerConexion())
                {
                    conexion.Open();
                    string sql = "INSERT INTO Cajeros (Nombre, Clave) VALUES (@nombre, @clave)";

                    using (var cmd = new SQLiteCommand(sql, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@clave", clave);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ModificarClave(string claveActual, string nuevaClave)
        {
            try
            {
                using (var conexion = DatabaseManager.ObtenerConexion())
                {
                    conexion.Open();
                    string sql = "UPDATE Cajeros SET Clave = @nuevaClave WHERE Clave = @claveActual";

                    using (var cmd = new SQLiteCommand(sql, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nuevaClave", nuevaClave);
                        cmd.Parameters.AddWithValue("@claveActual", claveActual);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Eliminar(string clave)
        {
            try
            {
                using (var conexion = DatabaseManager.ObtenerConexion())
                {
                    conexion.Open();
                    string sql = "UPDATE Cajeros SET Activo = 0 WHERE Clave = @clave";

                    using (var cmd = new SQLiteCommand(sql, conexion))
                    {
                        cmd.Parameters.AddWithValue("@clave", clave);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static int ObtenerIdPorClave(string clave)
        {
            using (var conexion = DatabaseManager.ObtenerConexion())
            {
                conexion.Open();
                string sql = "SELECT Id FROM Cajeros WHERE Clave = @clave AND Activo = 1";

                using (var cmd = new SQLiteCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@clave", clave);
                    object resultado = cmd.ExecuteScalar();
                    return resultado != null ? Convert.ToInt32(resultado) : 0;
                }
            }
        }
    }
}