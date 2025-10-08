using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Examen_Unidad3.Database;

namespace AdminConsoleApp.Utilidades
{
    // Clases auxiliares integradas
    public class ReporteCorteCaja
    {
        public decimal TotalVentas { get; set; }
        public decimal TotalPropinas { get; set; }
        public Dictionary<string, decimal> VentasPorCajero { get; set; }
        public Dictionary<string, decimal> PropinasPorCajero { get; set; }
        public int TotalTickets { get; set; }
        public DateTime FechaGeneracion { get; set; }

        public decimal TotalGeneral => TotalVentas + TotalPropinas;

        public ReporteCorteCaja()
        {
            VentasPorCajero = new Dictionary<string, decimal>();
            PropinasPorCajero = new Dictionary<string, decimal>();
        }
    }

    public class DatosTicket
    {
        public string Cajero { get; set; }
        public decimal Total { get; set; }
        public decimal Propina { get; set; }
        public DateTime Fecha { get; set; }  // Nueva propiedad
    }

    public class CorteCajaManager
    {
        private static readonly string carpetaTickets = Path.Combine(Directory.GetCurrentDirectory(), "Tickets");

        private static ReporteCorteCaja GenerarReporteCorteDeCaja(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                var reporte = new ReporteCorteCaja
                {
                    TotalVentas = 0,
                    TotalPropinas = 0,
                    TotalTickets = 0,
                    FechaGeneracion = DateTime.Now
                };

                // Obtener tickets de la base de datos
                List<TicketCompleto> tickets;

                if (fechaInicio.HasValue && fechaFin.HasValue)
                {
                    // Filtrar por rango de fechas
                    tickets = TicketsRepository.ObtenerTodos()
                        .Where(t => t.Fecha.Date >= fechaInicio.Value.Date && t.Fecha.Date <= fechaFin.Value.Date)
                        .ToList();
                }
                else if (fechaInicio.HasValue)
                {
                    tickets = TicketsRepository.ObtenerTodos()
                        .Where(t => t.Fecha.Date >= fechaInicio.Value.Date)
                        .ToList();
                }
                else if (fechaFin.HasValue)
                {
                    tickets = TicketsRepository.ObtenerTodos()
                        .Where(t => t.Fecha.Date <= fechaFin.Value.Date)
                        .ToList();
                }
                else
                {
                    // Obtener todos
                    tickets = TicketsRepository.ObtenerTodos();
                }

                // Procesar tickets
                foreach (var ticket in tickets)
                {
                    reporte.TotalVentas += ticket.Total;
                    reporte.TotalPropinas += ticket.Propina;
                    reporte.TotalTickets++;

                    // Agregar a la lista individual
                    reporte.TicketsIndividuales.Add(new DatosTicket
                    {
                        Cajero = ticket.Cajero,
                        Total = ticket.Total,
                        Propina = ticket.Propina,
                        Fecha = ticket.Fecha
                    });

                    // Acumular por cajero
                    if (!reporte.VentasPorCajero.ContainsKey(ticket.Cajero))
                    {
                        reporte.VentasPorCajero[ticket.Cajero] = 0;
                        reporte.PropinasPorCajero[ticket.Cajero] = 0;
                    }

                    reporte.VentasPorCajero[ticket.Cajero] += ticket.Total;
                    reporte.PropinasPorCajero[ticket.Cajero] += ticket.Propina;
                }

                return reporte;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar reporte de corte de caja: {ex.Message}");
            }
        }

        
        // Agregar nueva clase para almacenar tickets individuales
        public class ReporteCorteCaja
        {
            public decimal TotalVentas { get; set; }
            public decimal TotalPropinas { get; set; }
            public Dictionary<string, decimal> VentasPorCajero { get; set; }
            public Dictionary<string, decimal> PropinasPorCajero { get; set; }
            public int TotalTickets { get; set; }
            public DateTime FechaGeneracion { get; set; }
            public List<DatosTicket> TicketsIndividuales { get; set; } // Nueva propiedad

            public decimal TotalGeneral => TotalVentas + TotalPropinas;

            public ReporteCorteCaja()
            {
                VentasPorCajero = new Dictionary<string, decimal>();
                PropinasPorCajero = new Dictionary<string, decimal>();
                TicketsIndividuales = new List<DatosTicket>(); // Inicializar
            }
        }

        

        

        // Métodos públicos para la UI
        public static void GenerarReporte(DataGridView dgvReporte, Label lblTotalVentas, Label lblTotalPropinas, Label lblTotalGeneral, Label lblTotalTickets)
        {
            try
            {
                var reporte = GenerarReporteCorteDeCaja();

                CargarReporteEnTabla(dgvReporte, reporte);
                ActualizarLabelsResumen(lblTotalVentas, lblTotalPropinas, lblTotalGeneral, lblTotalTickets, reporte);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar reporte: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void GenerarReportePorFechas(DataGridView dgvReporte, Label lblTotalVentas, Label lblTotalPropinas, Label lblTotalGeneral, Label lblTotalTickets, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var reporte = GenerarReporteCorteDeCaja(fechaInicio, fechaFin);

                CargarReporteEnTabla(dgvReporte, reporte);
                ActualizarLabelsResumen(lblTotalVentas, lblTotalPropinas, lblTotalGeneral, lblTotalTickets, reporte);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar reporte por fechas: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void CargarReporteEnTabla(DataGridView dgv, ReporteCorteCaja reporte)
        {
            dgv.Rows.Clear();

            if (reporte.TicketsIndividuales.Count == 0)
            {
                dgv.Rows.Add("", "", "", "", "", "");
                dgv.Rows[0].Cells[1].Value = "No hay tickets para mostrar";
                dgv.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
                dgv.Rows[0].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Italic);
                return;
            }

            // Ordenar por fecha (más recientes primero)
            var ticketsOrdenados = reporte.TicketsIndividuales.OrderByDescending(t => t.Fecha).ToList();

            int id = 1;
            foreach (var ticket in ticketsOrdenados)
            {
                decimal total = ticket.Total + ticket.Propina;

                dgv.Rows.Add(
                    id,
                    ticket.Cajero,
                    ticket.Fecha.ToString("dd/MM/yyyy HH:mm"),
                    $"${ticket.Total:F2}",
                    $"${ticket.Propina:F2}",
                    $"${total:F2}"
                );

                // Aplicar colores alternados
                if (id % 2 == 0)
                {
                    dgv.Rows[dgv.Rows.Count - 1].DefaultCellStyle.BackColor = Color.AliceBlue;
                }

                // Resaltar tickets del día actual
                if (ticket.Fecha.Date == DateTime.Today)
                {
                    dgv.Rows[dgv.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightYellow;
                }

                id++;
            }
        }

        private static void ActualizarLabelsResumen(Label lblTotalVentas, Label lblTotalPropinas, Label lblTotalGeneral, Label lblTotalTickets, ReporteCorteCaja reporte)
        {
            if (lblTotalVentas != null)
                lblTotalVentas.Text = $"Total Ventas: ${reporte.TotalVentas:F2}";

            if (lblTotalPropinas != null)
                lblTotalPropinas.Text = $"Total Propinas: ${reporte.TotalPropinas:F2}";

            if (lblTotalGeneral != null)
                lblTotalGeneral.Text = $"Total General: ${reporte.TotalGeneral:F2}";

            if (lblTotalTickets != null)
                lblTotalTickets.Text = $"Total Tickets: {reporte.TotalTickets}";
        }

        public static void ExportarReporte(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                var reporte = fechaInicio.HasValue || fechaFin.HasValue
                    ? GenerarReporteCorteDeCaja(fechaInicio, fechaFin)
                    : GenerarReporteCorteDeCaja();

                string nombreArchivo = $"Corte_Caja_{DateTime.Now:yyyyMMdd_HHmm}.txt";
                string rutaCompleta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), nombreArchivo);

                using (StreamWriter writer = new StreamWriter(rutaCompleta, false, Encoding.UTF8))
                {
                    writer.WriteLine("═══════════════════════════════════════════════════════════════");
                    writer.WriteLine("                    REPORTE DE CORTE DE CAJA                    ");
                    writer.WriteLine("═══════════════════════════════════════════════════════════════");
                    writer.WriteLine($"Generado el: {DateTime.Now:dddd, dd 'de' MMMM 'de' yyyy 'a las' HH:mm:ss}");

                    if (fechaInicio.HasValue || fechaFin.HasValue)
                    {
                        writer.WriteLine($"Período: {fechaInicio?.ToString("dd/MM/yyyy") ?? "Inicio"} - {fechaFin?.ToString("dd/MM/yyyy") ?? "Fin"}");
                    }

                    writer.WriteLine($"Total de tickets procesados: {reporte.TotalTickets}");
                    writer.WriteLine();

                    writer.WriteLine("RESUMEN GENERAL:");
                    writer.WriteLine("───────────────────────────────────────────────────────────────");
                    writer.WriteLine($"Total Ventas:    ${reporte.TotalVentas:F2}");
                    writer.WriteLine($"Total Propinas:  ${reporte.TotalPropinas:F2}");
                    writer.WriteLine($"Total General:   ${reporte.TotalGeneral:F2}");
                    writer.WriteLine();

                    writer.WriteLine("DETALLE POR CAJERO:");
                    writer.WriteLine("───────────────────────────────────────────────────────────────");
                    writer.WriteLine($"{"Cajero",-20} {"Ventas",-12} {"Propinas",-12} {"Total",-12}");
                    writer.WriteLine(new string('-', 60));

                    foreach (var cajero in reporte.VentasPorCajero.Keys)
                    {
                        decimal ventas = reporte.VentasPorCajero[cajero];
                        decimal propinas = reporte.PropinasPorCajero.ContainsKey(cajero) ? reporte.PropinasPorCajero[cajero] : 0;
                        decimal total = ventas + propinas;

                        writer.WriteLine($"{cajero,-20} ${ventas,-11:F2} ${propinas,-11:F2} ${total,-11:F2}");
                    }

                    writer.WriteLine();
                    writer.WriteLine("═══════════════════════════════════════════════════════════════");
                    writer.WriteLine($"Archivo generado por: Sistema de Corte de Caja");
                    writer.WriteLine($"Fecha de generación: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine("═══════════════════════════════════════════════════════════════");
                }

                DialogResult resultado = MessageBox.Show(
                    $"Reporte exportado correctamente:\n\n{rutaCompleta}\n\n¿Desea abrir el archivo ahora?",
                    "Exportación Exitosa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (resultado == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("notepad.exe", rutaCompleta);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar reporte: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool ExistenTickets()
        {
            try
            {
                return TicketsRepository.ContarTickets() > 0;
            }
            catch
            {
                return false;
            }
        }

        public static int ContarTickets()
        {
            try
            {
                return TicketsRepository.ContarTickets();
            }
            catch
            {
                return 0;
            }
        }

        public static List<string> ObtenerCajerosConVentas()
        {
            try
            {
                var reporte = GenerarReporteCorteDeCaja();
                return reporte.VentasPorCajero.Keys.ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        // Método para obtener la ruta de la carpeta de tickets (útil para debugging)
        public static string ObtenerRutaCarpetaTickets()
        {
            return carpetaTickets;
        }
    }
}