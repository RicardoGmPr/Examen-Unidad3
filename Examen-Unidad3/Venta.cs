using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Examen_Unidad3.Database;


namespace Examen_Unidad3
{
    public partial class Venta : Form
    {
        public Venta()
        {
            InitializeComponent();
        }
        public List<string> productosDelTicket = new List<string>();
        public void textBox2_TextChanged(object sender, EventArgs e)
        {

        }



        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal total = 0;
            decimal pago = 0;

            // Calcular cambio
            if (decimal.TryParse(textBox1.Text, out total) && decimal.TryParse(textBox2.Text, out pago))
            {
                decimal cambio = pago - total;
                textBox3.Text = cambio.ToString("F2");
            }
            else
            {
                MessageBox.Show("Por favor ingrese valores válidos para total y pago.", "Error");
                return;
            }

            try
            {
                // Obtener propina
                decimal propina = 0;
                if (!string.IsNullOrEmpty(textBox4.Text))
                {
                    decimal.TryParse(textBox4.Text, out propina);
                }

                // Generar número de ticket
                string numeroTicket = "Ticket_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

                // Preparar productos para BD
                var productosDetalle = new List<DetalleTicketItem>();
                int numLinea = 1;

                foreach (var item in productosDelTicket)
                {
                    string itemStr = item.ToString();

                    // Saltar encabezados y separadores
                    if (itemStr.StartsWith("Orden") || itemStr.StartsWith("Cliente:") ||
                        itemStr.StartsWith("---") || itemStr.StartsWith("TOTAL"))
                        continue;

                    // Extraer nombre del producto y precio
                    bool esExtra = itemStr.StartsWith("--");
                    string productoNombre = "";
                    decimal precio = 0;

                    if (itemStr.Contains("$"))
                    {
                        int indexPrecio = itemStr.LastIndexOf('$');
                        string precioStr = itemStr.Substring(indexPrecio + 1).Trim();
                        decimal.TryParse(precioStr, out precio);

                        productoNombre = itemStr.Substring(0, indexPrecio).Trim();
                        // Limpiar numeración si existe
                        if (productoNombre.Contains(":"))
                        {
                            int indexDosPuntos = productoNombre.IndexOf(':');
                            productoNombre = productoNombre.Substring(indexDosPuntos + 1).Trim();
                        }
                    }

                    if (!string.IsNullOrEmpty(productoNombre))
                    {
                        productosDetalle.Add(new DetalleTicketItem
                        {
                            NumeroLinea = numLinea,
                            Producto = productoNombre,
                            Precio = precio,
                            EsExtra = esExtra
                        });
                        numLinea++;
                    }
                }

                // Guardar en base de datos - AQUÍ ESTÁ LA CORRECCIÓN
                bool guardadoBD = TicketsRepository.GuardarTicket(
                    numeroTicket,
                    POSLogin.claveIngresada,
                    ComedorLlevar.OrdenInfo.TipoOrden,
                    ComedorLlevar.OrdenInfo.NombreCliente,
                    total,
                    propina,
                    pago,
                    decimal.Parse(textBox3.Text),
                    productosDetalle  // <-- ESTE ES EL PARÁMETRO QUE FALTABA
                );

                // También guardar archivo de texto (opcional, para respaldo)
                string carpetaTickets = Path.Combine(Directory.GetCurrentDirectory(), "Tickets");
                if (!Directory.Exists(carpetaTickets))
                {
                    Directory.CreateDirectory(carpetaTickets);
                }

                string nombreArchivo = numeroTicket + ".txt";
                string rutaCompleta = Path.Combine(carpetaTickets, nombreArchivo);

                using (StreamWriter writer = new StreamWriter(rutaCompleta))
                {
                    writer.WriteLine("========= TICKET =========");
                    writer.WriteLine($"Fecha: {DateTime.Now}");
                    writer.WriteLine($"Cajero: {POSLogin.nombre}");
                    writer.WriteLine("-------------------------------");

                    foreach (var item in productosDelTicket)
                    {
                        writer.WriteLine(item);
                    }

                    writer.WriteLine("-------------------------------");
                    writer.WriteLine($"TOTAL: ${textBox1.Text}");
                    writer.WriteLine($"PROPINA: ${textBox4.Text}");
                    writer.WriteLine($"PAGO: ${textBox2.Text}");
                    writer.WriteLine($"CAMBIO: ${textBox3.Text}");
                    writer.WriteLine("===============================");
                    writer.WriteLine("¡Gracias por su compra!");
                }

                string mensaje = guardadoBD
                    ? "Ticket guardado exitosamente en base de datos y archivo de respaldo"
                    : "Ticket guardado en archivo (advertencia: no se pudo guardar en BD)";

                MessageBox.Show(mensaje);

                ComedorLlevar formComedorLlevar = new ComedorLlevar();
                formComedorLlevar.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el ticket: " + ex.Message);
            }
        }

        private void Venta_Load(object sender, EventArgs e)
        {

        }
    }


}
