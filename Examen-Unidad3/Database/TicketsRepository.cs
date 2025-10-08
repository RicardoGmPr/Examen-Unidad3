using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Examen_Unidad3.Database
{
    public class TicketsRepository
    {
        // Guardar un ticket completo
        public static bool GuardarTicket(
            string numeroTicket,
            string claveCajero,
            string tipoOrden,
            string nombreCliente,
            decimal total,
            decimal propina,
            decimal pago,
            decimal cambio,
            List<DetalleTicketItem> productos)
        {
            try
            {
                // Obtener ID del cajero
                int cajeroId = CajerosRepository.ObtenerIdPorClave(claveCajero);
                if (cajeroId == 0)
                {
                    return false;
                }

                using (var conexion = DatabaseManager.ObtenerConexion())
                {
                    conexion.Open();

                    // Iniciar transacción
                    using (var transaction = conexion.BeginTransaction())
                    {
                        try
                        {
                            // Insertar ticket principal
                            string sqlTicket = @"
                                INSERT INTO Tickets (NumeroTicket, CajeroId, TipoOrden, NombreCliente, Total, Propina, Pago, Cambio)
                                VALUES (@numero, @cajeroId, @tipo, @cliente, @total, @propina, @pago, @cambio);
                                SELECT last_insert_rowid();";

                            long ticketId;
                            using (var cmd = new SQLiteCommand(sqlTicket, conexion, transaction))
                            {
                                cmd.Parameters.AddWithValue("@numero", numeroTicket);
                                cmd.Parameters.AddWithValue("@cajeroId", cajeroId);
                                cmd.Parameters.AddWithValue("@tipo", tipoOrden);
                                cmd.Parameters.AddWithValue("@cliente", nombreCliente ?? "");
                                cmd.Parameters.AddWithValue("@total", total);
                                cmd.Parameters.AddWithValue("@propina", propina);
                                cmd.Parameters.AddWithValue("@pago", pago);
                                cmd.Parameters.AddWithValue("@cambio", cambio);

                                ticketId = (long)cmd.ExecuteScalar();
                            }

                            // Insertar detalles del ticket
                            string sqlDetalle = @"
                                INSERT INTO DetalleTicket (TicketId, NumeroLinea, Producto, Precio, EsExtra)
                                VALUES (@ticketId, @numLinea, @producto, @precio, @esExtra)";

                            int numLinea = 1;
                            foreach (var item in productos)
                            {
                                using (var cmd = new SQLiteCommand(sqlDetalle, conexion, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@ticketId", ticketId);
                                    cmd.Parameters.AddWithValue("@numLinea", numLinea);
                                    cmd.Parameters.AddWithValue("@producto", item.Producto);
                                    cmd.Parameters.AddWithValue("@precio", item.Precio);
                                    cmd.Parameters.AddWithValue("@esExtra", item.EsExtra ? 1 : 0);
                                    cmd.ExecuteNonQuery();
                                }
                                numLinea++;
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

        // Obtener todos los tickets
        public static List<TicketCompleto> ObtenerTodos()
        {
            var tickets = new List<TicketCompleto>();

            using (var conexion = DatabaseManager.ObtenerConexion())
            {
                conexion.Open();
                string sql = @"
                    SELECT t.Id, t.NumeroTicket, c.Nombre as Cajero, t.TipoOrden, 
                           t.NombreCliente, t.Total, t.Propina, t.Pago, t.Cambio, t.Fecha
                    FROM Tickets t
                    INNER JOIN Cajeros c ON t.CajeroId = c.Id
                    ORDER BY t.Fecha DESC";

                using (var cmd = new SQLiteCommand(sql, conexion))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tickets.Add(new TicketCompleto
                        {
                            Id = reader.GetInt32(0),
                            NumeroTicket = reader.GetString(1),
                            Cajero = reader.GetString(2),
                            TipoOrden = reader.GetString(3),
                            NombreCliente = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            Total = reader.GetDecimal(5),
                            Propina = reader.GetDecimal(6),
                            Pago = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                            Cambio = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                            Fecha = DateTime.Parse(reader.GetString(9))
                        });
                    }
                }
            }

            return tickets;
        }

        // Obtener detalle de un ticket
        public static List<DetalleTicketItem> ObtenerDetalleTicket(int ticketId)
        {
            var detalles = new List<DetalleTicketItem>();

            using (var conexion = DatabaseManager.ObtenerConexion())
            {
                conexion.Open();
                string sql = @"
                    SELECT NumeroLinea, Producto, Precio, EsExtra
                    FROM DetalleTicket
                    WHERE TicketId = @ticketId
                    ORDER BY NumeroLinea";

                using (var cmd = new SQLiteCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@ticketId", ticketId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            detalles.Add(new DetalleTicketItem
                            {
                                NumeroLinea = reader.GetInt32(0),
                                Producto = reader.GetString(1),
                                Precio = reader.GetDecimal(2),
                                EsExtra = reader.GetInt32(3) == 1
                            });
                        }
                    }
                }
            }

            return detalles;
        }

        // Obtener tickets por fecha
        public static List<TicketCompleto> ObtenerPorFecha(DateTime fecha)
        {
            var tickets = new List<TicketCompleto>();

            using (var conexion = DatabaseManager.ObtenerConexion())
            {
                conexion.Open();
                string sql = @"
                    SELECT t.Id, t.NumeroTicket, c.Nombre as Cajero, t.TipoOrden, 
                           t.NombreCliente, t.Total, t.Propina, t.Pago, t.Cambio, t.Fecha
                    FROM Tickets t
                    INNER JOIN Cajeros c ON t.CajeroId = c.Id
                    WHERE DATE(t.Fecha) = DATE(@fecha)
                    ORDER BY t.Fecha DESC";

                using (var cmd = new SQLiteCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@fecha", fecha.ToString("yyyy-MM-dd"));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tickets.Add(new TicketCompleto
                            {
                                Id = reader.GetInt32(0),
                                NumeroTicket = reader.GetString(1),
                                Cajero = reader.GetString(2),
                                TipoOrden = reader.GetString(3),
                                NombreCliente = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                Total = reader.GetDecimal(5),
                                Propina = reader.GetDecimal(6),
                                Pago = reader.IsDBNull(7) ? 0 : reader.GetDecimal(7),
                                Cambio = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8),
                                Fecha = DateTime.Parse(reader.GetString(9))
                            });
                        }
                    }
                }
            }

            return tickets;
        }

        // Contar tickets
        public static int ContarTickets()
        {
            using (var conexion = DatabaseManager.ObtenerConexion())
            {
                conexion.Open();
                string sql = "SELECT COUNT(*) FROM Tickets";

                using (var cmd = new SQLiteCommand(sql, conexion))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }

    // Clases auxiliares
    public class TicketCompleto
    {
        public int Id { get; set; }
        public string NumeroTicket { get; set; }
        public string Cajero { get; set; }
        public string TipoOrden { get; set; }
        public string NombreCliente { get; set; }
        public decimal Total { get; set; }
        public decimal Propina { get; set; }
        public decimal Pago { get; set; }
        public decimal Cambio { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class DetalleTicketItem
    {
        public int NumeroLinea { get; set; }
        public string Producto { get; set; }
        public decimal Precio { get; set; }
        public bool EsExtra { get; set; }
    }
}