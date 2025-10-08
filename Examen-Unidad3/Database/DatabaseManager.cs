using System;
using System.Data.SQLite;
using System.IO;

namespace Examen_Unidad3.Database
{
    public class DatabaseManager
    {
        private static string dbPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "CarlsJr.db"
        );

        private static string connectionString = $"Data Source={dbPath};Version=3;";

        public static void InicializarBaseDatos()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
                CrearTablas();
                InsertarDatosIniciales();
            }
        }

        public static SQLiteConnection ObtenerConexion()
        {
            return new SQLiteConnection(connectionString);
        }

        private static void CrearTablas()
        {
            using (var conexion = ObtenerConexion())
            {
                conexion.Open();

                string sqlCajeros = @"
                    CREATE TABLE IF NOT EXISTS Cajeros (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL,
                        Clave TEXT NOT NULL UNIQUE,
                        Activo INTEGER DEFAULT 1,
                        FechaCreacion TEXT DEFAULT CURRENT_TIMESTAMP
                    )";

                string sqlTickets = @"
                    CREATE TABLE IF NOT EXISTS Tickets (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        NumeroTicket TEXT NOT NULL UNIQUE,
                        CajeroId INTEGER NOT NULL,
                        TipoOrden TEXT NOT NULL,
                        NombreCliente TEXT,
                        Total REAL NOT NULL,
                        Propina REAL DEFAULT 0,
                        Pago REAL,
                        Cambio REAL,
                        Fecha TEXT DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (CajeroId) REFERENCES Cajeros(Id)
                    )";

                string sqlDetalleTicket = @"
                    CREATE TABLE IF NOT EXISTS DetalleTicket (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        TicketId INTEGER NOT NULL,
                        NumeroLinea INTEGER NOT NULL,
                        Producto TEXT NOT NULL,
                        Precio REAL NOT NULL,
                        EsExtra INTEGER DEFAULT 0,
                        FOREIGN KEY (TicketId) REFERENCES Tickets(Id)
                    )";

                string sqlInventario = @"
                    CREATE TABLE IF NOT EXISTS Inventario (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL UNIQUE,
                        Cantidad INTEGER NOT NULL,
                        Unidad TEXT NOT NULL,
                        Categoria TEXT NOT NULL,
                        UltimaActualizacion TEXT DEFAULT CURRENT_TIMESTAMP
                    )";

                string sqlHistorial = @"
                    CREATE TABLE IF NOT EXISTS HistorialInventario (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProductoId INTEGER NOT NULL,
                        CantidadAnterior INTEGER,
                        CantidadNueva INTEGER,
                        Motivo TEXT,
                        Fecha TEXT DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (ProductoId) REFERENCES Inventario(Id)
                    )";

                ExecuteNonQuery(conexion, sqlCajeros);
                ExecuteNonQuery(conexion, sqlTickets);
                ExecuteNonQuery(conexion, sqlDetalleTicket);
                ExecuteNonQuery(conexion, sqlInventario);
                ExecuteNonQuery(conexion, sqlHistorial);
            }
        }

        private static void ExecuteNonQuery(SQLiteConnection conexion, string sql)
        {
            using (var cmd = new SQLiteCommand(sql, conexion))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void InsertarDatosIniciales()
        {
            MigrarInventarioDesdeJSON();
            MigrarCajerosDesdeTXT();
        }

        private static void MigrarInventarioDesdeJSON()
        {
            try
            {
                Inventario inv = new Inventario();
                inv.InicializarInventario();
                inv.CargarInventario("inventario.json");

                using (var conexion = ObtenerConexion())
                {
                    conexion.Open();

                    foreach (var producto in inv.congelado)
                    {
                        InsertarProducto(conexion, producto, "Congelado");
                    }

                    foreach (var producto in inv.refrigerado)
                    {
                        InsertarProducto(conexion, producto, "Refrigerado");
                    }

                    foreach (var producto in inv.secos)
                    {
                        InsertarProducto(conexion, producto, "Seco");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error migrando inventario: {ex.Message}");
            }
        }

        private static void InsertarProducto(SQLiteConnection conexion, Producto producto, string categoria)
        {
            string sql = @"
                INSERT OR IGNORE INTO Inventario (Nombre, Cantidad, Unidad, Categoria) 
                VALUES (@nombre, @cantidad, @unidad, @categoria)";

            using (var cmd = new SQLiteCommand(sql, conexion))
            {
                cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@cantidad", producto.Cantidad);
                cmd.Parameters.AddWithValue("@unidad", producto.Unidad);
                cmd.Parameters.AddWithValue("@categoria", categoria);
                cmd.ExecuteNonQuery();
            }
        }

        private static void MigrarCajerosDesdeTXT()
        {
            try
            {
                string archivo = "cajeros.txt";
                if (File.Exists(archivo))
                {
                    using (var conexion = ObtenerConexion())
                    {
                        conexion.Open();

                        string[] lineas = File.ReadAllLines(archivo);
                        foreach (var linea in lineas)
                        {
                            string[] partes = linea.Split(';');
                            if (partes.Length == 2)
                            {
                                string sql = @"
                                    INSERT OR IGNORE INTO Cajeros (Nombre, Clave) 
                                    VALUES (@nombre, @clave)";

                                using (var cmd = new SQLiteCommand(sql, conexion))
                                {
                                    cmd.Parameters.AddWithValue("@nombre", partes[0]);
                                    cmd.Parameters.AddWithValue("@clave", partes[1]);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error migrando cajeros: {ex.Message}");
            }
        }
    }
}