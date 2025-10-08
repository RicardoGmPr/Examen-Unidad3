using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace Examen_Unidad3.Database
{
    public class InventarioRepository
    {
        // Obtener todos los productos
        public static List<Producto> ObtenerTodos()
        {
            var productos = new List<Producto>();

            using (var conexion = DatabaseManager.ObtenerConexion())
            {
                conexion.Open();
                string sql = "SELECT Id, Nombre, Cantidad, Unidad, Categoria FROM Inventario ORDER BY Categoria, Nombre";

                using (var cmd = new SQLiteCommand(sql, conexion))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(new Producto(
                            reader.GetString(1),  // Nombre
                            reader.GetInt32(2),   // Cantidad
                            reader.GetString(3)   // Unidad
                        ));
                    }
                }
            }

            return productos;
        }

        // Obtener productos por categoría
        public static List<Producto> ObtenerPorCategoria(string categoria)
        {
            var productos = new List<Producto>();

            using (var conexion = DatabaseManager.ObtenerConexion())
            {
                conexion.Open();
                string sql = "SELECT Nombre, Cantidad, Unidad FROM Inventario WHERE Categoria = @categoria ORDER BY Nombre";

                using (var cmd = new SQLiteCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@categoria", categoria);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(new Producto(
                                reader.GetString(0),
                                reader.GetInt32(1),
                                reader.GetString(2)
                            ));
                        }
                    }
                }
            }

            return productos;
        }

        // Actualizar cantidad de un producto
        public static bool ActualizarCantidad(string nombreProducto, int nuevaCantidad, string motivo = "")
        {
            try
            {
                using (var conexion = DatabaseManager.ObtenerConexion())
                {
                    conexion.Open();

                    using (var transaction = conexion.BeginTransaction())
                    {
                        try
                        {
                            // Obtener cantidad anterior
                            string sqlSelect = "SELECT Id, Cantidad FROM Inventario WHERE Nombre = @nombre";
                            int productoId = 0;
                            int cantidadAnterior = 0;

                            using (var cmdSelect = new SQLiteCommand(sqlSelect, conexion, transaction))
                            {
                                cmdSelect.Parameters.AddWithValue("@nombre", nombreProducto);
                                using (var reader = cmdSelect.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        productoId = reader.GetInt32(0);
                                        cantidadAnterior = reader.GetInt32(1);
                                    }
                                }
                            }

                            if (productoId == 0)
                                return false;

                            // Actualizar cantidad
                            string sqlUpdate = @"
                                UPDATE Inventario 
                                SET Cantidad = @cantidad, UltimaActualizacion = CURRENT_TIMESTAMP 
                                WHERE Id = @id";

                            using (var cmdUpdate = new SQLiteCommand(sqlUpdate, conexion, transaction))
                            {
                                cmdUpdate.Parameters.AddWithValue("@cantidad", nuevaCantidad);
                                cmdUpdate.Parameters.AddWithValue("@id", productoId);
                                cmdUpdate.ExecuteNonQuery();
                            }

                            // Registrar en historial
                            string sqlHistorial = @"
                                INSERT INTO HistorialInventario (ProductoId, CantidadAnterior, CantidadNueva, Motivo)
                                VALUES (@productoId, @anterior, @nueva, @motivo)";

                            using (var cmdHistorial = new SQLiteCommand(sqlHistorial, conexion, transaction))
                            {
                                cmdHistorial.Parameters.AddWithValue("@productoId", productoId);
                                cmdHistorial.Parameters.AddWithValue("@anterior", cantidadAnterior);
                                cmdHistorial.Parameters.AddWithValue("@nueva", nuevaCantidad);
                                cmdHistorial.Parameters.AddWithValue("@motivo", motivo);
                                cmdHistorial.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // Descontar cantidad (para cuando se vende un producto)
        public static bool DescontarCantidad(string nombreProducto, int cantidad, string motivo = "Venta")
        {
            try
            {
                using (var conexion = DatabaseManager.ObtenerConexion())
                {
                    conexion.Open();

                    // Obtener cantidad actual
                    string sqlSelect = "SELECT Cantidad FROM Inventario WHERE Nombre = @nombre";
                    int cantidadActual = 0;

                    using (var cmd = new SQLiteCommand(sqlSelect, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombreProducto);
                        object resultado = cmd.ExecuteScalar();
                        if (resultado != null)
                            cantidadActual = Convert.ToInt32(resultado);
                    }

                    // Calcular nueva cantidad
                    int nuevaCantidad = cantidadActual - cantidad;
                    if (nuevaCantidad < 0)
                        nuevaCantidad = 0;

                    // Actualizar
                    return ActualizarCantidad(nombreProducto, nuevaCantidad, motivo);
                }
            }
            catch
            {
                return false;
            }
        }

        // Agregar cantidad (para pedidos)
        public static bool AgregarCantidad(string nombreProducto, int cantidad, string motivo = "Pedido")
        {
            try
            {
                using (var conexion = DatabaseManager.ObtenerConexion())
                {
                    conexion.Open();

                    // Obtener cantidad actual
                    string sqlSelect = "SELECT Cantidad FROM Inventario WHERE Nombre = @nombre";
                    int cantidadActual = 0;

                    using (var cmd = new SQLiteCommand(sqlSelect, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombreProducto);
                        object resultado = cmd.ExecuteScalar();
                        if (resultado != null)
                            cantidadActual = Convert.ToInt32(resultado);
                    }

                    // Calcular nueva cantidad
                    int nuevaCantidad = cantidadActual + cantidad;

                    // Actualizar
                    return ActualizarCantidad(nombreProducto, nuevaCantidad, motivo);
                }
            }
            catch
            {
                return false;
            }
        }

        // Obtener cantidad actual de un producto
        public static int ObtenerCantidad(string nombreProducto)
        {
            using (var conexion = DatabaseManager.ObtenerConexion())
            {
                conexion.Open();
                string sql = "SELECT Cantidad FROM Inventario WHERE Nombre = @nombre";

                using (var cmd = new SQLiteCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombreProducto);
                    object resultado = cmd.ExecuteScalar();
                    return resultado != null ? Convert.ToInt32(resultado) : 0;
                }
            }
        }

        // Obtener productos con stock bajo
        public static List<Producto> ObtenerStockBajo(int cantidadMinima = 5)
        {
            var productos = new List<Producto>();

            using (var conexion = DatabaseManager.ObtenerConexion())
            {
                conexion.Open();
                string sql = "SELECT Nombre, Cantidad, Unidad FROM Inventario WHERE Cantidad <= @minimo ORDER BY Cantidad";

                using (var cmd = new SQLiteCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@minimo", cantidadMinima);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(new Producto(
                                reader.GetString(0),
                                reader.GetInt32(1),
                                reader.GetString(2)
                            ));
                        }
                    }
                }
            }

            return productos;
        }
    }
}