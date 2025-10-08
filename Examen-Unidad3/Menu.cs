using Examen_Unidad3.Decorador;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Examen_Unidad3.ComedorLlevar;
using Timer = System.Windows.Forms.Timer;
using Examen_Unidad3.Prototype;
using Examen_Unidad3.Memento;
using Examen_Unidad3.MVC;
using Examen_Unidad3.Database;



namespace Examen_Unidad3
{
    public partial class Menu : Form, MenuView
    {
        private List<ProductoItem> ticketItems;
        private MenuController controller;
        private TicketCaretaker ticketCaretaker = new TicketCaretaker();
        private TicketOriginator ticketOriginator = new TicketOriginator();
        private PrototypeRegistry prototypeRegistry = new PrototypeRegistry();
        private Timer animacionTimer;
        public Menu()
        {

            InitializeComponent();
            this.Load += new System.EventHandler(this.Menu_Load);
            button8.Click += button8_Click;
            controller = new MenuController(this, listBoxTicket);
            // Aplicar mejoras de diseño
            AplicarTemaGeneral();
            MejorarPanelTicket();
            MejorarBotonesCategorias();
            MejorarPanelProductos();
            MejorarBotonesAccion();
            InicializarAnimaciones();

            // Iniciar timer de animaciones
            animacionTimer?.Start();
        }

        public void ActualizarTicket(List<string> items, decimal total)
        {
            listBoxTicket.Items.Clear();
            listBoxTicket.Items.Add(OrdenInfo.TipoOrden == "Llevar" ? "Orden PARA LLEVAR" : "Orden EN COMEDOR");
            if (!string.IsNullOrWhiteSpace(OrdenInfo.NombreCliente))
                listBoxTicket.Items.Add($"Cliente: {OrdenInfo.NombreCliente}");
            listBoxTicket.Items.Add(new string('-', 30));

            foreach (var item in items)
                listBoxTicket.Items.Add(item);

            listBoxTicket.Items.Add($"TOTAL               ${total,6:F2}");
        }
        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje);
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            // Agrega al inicio del ticket el tipo de orden
            string tipoOrden = OrdenInfo.TipoOrden == "Llevar" ? "Orden PARA LLEVAR" : "Orden EN COMEDOR";
            string nombreCliente = OrdenInfo.NombreCliente;

            listBoxTicket.Items.Add(tipoOrden);
            if (!string.IsNullOrWhiteSpace(nombreCliente))
            {
                listBoxTicket.Items.Add($"Cliente: {nombreCliente}");
            }
            listBoxTicket.Items.Add(new string('-', 30)); // Separador bonito

            // AGREGAR ESTA LÍNEA para cargar el último ticket
            CargarUltimoTicketEnTextBox();
        }


        private int lastTicketNumber = 1;  // Contador global para la numeración
        private int contadorTicket = 1;
        //Variable para el total de los productos del ticket -----------------------------------------------------------------
        private decimal total = 0.00m;
        //Lista para los productos del ticket -----------------------------------------------------------------
        private List<ProductoSeleccionado> productosSeleccionados = new List<ProductoSeleccionado>();
        //Variable para definir si estamos armando un combo-----------------------------------------------------------------
        private bool armandoCombo = false;
        //Lista para lo que va a llevar el combo -----------------------------------------------------------------
        private List<string> componentesCombo = new List<string>();
        //-----------------------------------------------------------------
        private enum EtapaCombo { Ninguna, Hamburguesa, Fritos, Bebida }

        private bool esperandoFritos = false;
        private bool esperandoBebida = false;

        private string comboActual = "";
        private enum EstadoCombo { Ninguno, EsperandoFritos, EsperandoBebida }
        private EstadoCombo estadoCombo = EstadoCombo.Ninguno;

        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                CargarProductosPorCategoria(btn.Text);
            }
        }

        private void CargarProductosPorCategoria(string categoria)
        {
            flowLayoutPanel2.Controls.Clear();

            List<string> productos = new List<string>();
            Dictionary<string, Color> coloresProductos = new Dictionary<string, Color>();

            switch (categoria)
            {
                case "Hamburguesas":
                    productos = new List<string> { "Clásica", "Famous Star", "Western", "Teriyaki" };
                    coloresProductos = productos.ToDictionary(p => p, p => Color.FromArgb(255, 107, 107));
                    break;
                case "Bebidas":
                    productos = new List<string> { "V Chico", "V Mediano", "V Grande", "Agua ciel" };
                    coloresProductos = productos.ToDictionary(p => p, p => Color.FromArgb(54, 162, 235));
                    break;
                case "Fritos":
                    productos = new List<string> { "F Chicas", "F Medianas", "F Grandes", "Aros" };
                    coloresProductos = productos.ToDictionary(p => p, p => Color.FromArgb(255, 193, 7));
                    break;
                case "Postres":
                    productos = new List<string> { "Nieve Vainilla", "Nieve Chocolate", "Nieve Fresa", "Galleta" };
                    coloresProductos = productos.ToDictionary(p => p, p => Color.FromArgb(255, 105, 180));
                    break;
                case "Extras":
                    productos = new List<string> { "Juguete" };
                    coloresProductos = productos.ToDictionary(p => p, p => Color.FromArgb(40, 167, 69));
                    break;
                case "Combo Hmbrg":
                    productos = new List<string> { "C Clásica", "C Famous", "C Western", "C Teriyaki" };
                    coloresProductos = productos.ToDictionary(p => p, p => Color.FromArgb(108, 117, 125));
                    break;
            }

            foreach (var nombre in productos)
            {
                Button btn = CrearBotonProducto(nombre, coloresProductos[nombre]);
                flowLayoutPanel2.Controls.Add(btn);
            }
        }

        private void Producto_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string nombreProducto = btn.Text;

                // ✅ MANEJAR COMBOS PRIMERO (tu lógica original):
                if (armandoCombo)
                {
                    // Tu código de combos SIN CAMBIOS...
                    if (estadoCombo == EstadoCombo.EsperandoFritos && EsFrito(nombreProducto))
                    {
                        componentesCombo.Add($"-- {nombreProducto}");
                        estadoCombo = EstadoCombo.EsperandoBebida;
                        CargarProductosPorCategoria("Bebidas");
                        return;
                    }
                    else if (estadoCombo == EstadoCombo.EsperandoBebida && EsBebida(nombreProducto))
                    {
                        componentesCombo.Add($"-- {nombreProducto}");
                        decimal precioCombo = ObtenerPrecioProducto(comboActual);

                        total += precioCombo;
                        listBoxTicket.Items.Add($"{lastTicketNumber}: {comboActual,-20} ${precioCombo,6:F2}");
                        lastTicketNumber++;

                        foreach (var comp in componentesCombo)
                            listBoxTicket.Items.Add(comp);

                        // Reset
                        armandoCombo = false;
                        comboActual = "";
                        estadoCombo = EstadoCombo.Ninguno;
                        componentesCombo.Clear();
                        ActualizarLineaTotal();
                        return;
                    }
                    // resto de tu lógica de combos...
                    return;
                }

                // ✅ INICIAR COMBO:
                if (EsCombo(nombreProducto))
                {
                    armandoCombo = true;
                    comboActual = nombreProducto;
                    componentesCombo.Clear();
                    componentesCombo.Add($"-- {ExtraerNombreBase(nombreProducto)}");
                    estadoCombo = EstadoCombo.EsperandoFritos;
                    CargarProductosPorCategoria("Fritos");
                    return;
                }

                // ✅ PRODUCTOS NORMALES - USAR CONTROLADOR CON TODOS LOS PATRONES:
                controller.AgregarProducto(nombreProducto, total, lastTicketNumber);

                // ✅ ACTUALIZAR VARIABLES LOCALES:
                decimal precio = ObtenerPrecioProducto(nombreProducto);
                var hamburguesaPrototype = prototypeRegistry.ObtenerHamburguesa(nombreProducto);
                if (hamburguesaPrototype != null)
                    precio = hamburguesaPrototype.ObtenerPrecio(); // ✅ PROTOTYPE usado aquí

                total += precio;
                listBoxTicket.Items.Add($"{lastTicketNumber}: {nombreProducto,-20} ${precio,6:F2}");
                lastTicketNumber++;
                ActualizarLineaTotal();
            }
        }



        private decimal ObtenerPrecioProducto(string nombre)
        {
            switch (nombre)
            {
                // Hamburguesas
                case "Clásica": return 60.00m;
                case "Famous Star": return 75.00m;
                case "Western": return 80.00m;
                case "Teriyaki": return 85.00m;

                // Bebidas
                case "V Chico": return 15.00m;
                case "V Mediano": return 18.00m;
                case "V Grande": return 21.00m;
                case "Agua ciel": return 12.00m;

                // Fritos
                case "F Chicas": return 25.00m;
                case "F Medianas": return 30.00m;
                case "F Grandes": return 35.00m;
                case "Aros": return 28.00m;

                // Postres
                case "Nieve Vainilla": return 20.00m;
                case "Nieve Chocolate": return 22.00m;
                case "Nieve Fresa": return 22.00m;
                case "Galleta": return 15.00m;

                // Extras
                case "Juguete": return 10.00m;

                // Combos
                case "C Clásica": return 90.00m;
                case "C Famous": return 105.00m;
                case "C Western": return 110.00m;
                case "C Teriyaki": return 115.00m;

                default: return 0.00m;
            }
        }

        private void btnEliminarSeleccionado_Click(object sender, EventArgs e)
        {
            int index = listBoxTicket.SelectedIndex;

            if (index >= 3 && index < listBoxTicket.Items.Count) // Evitar headers
            {
                // ✅ USAR CONTROLADOR (incluye MEMENTO):
                controller.EliminarProducto(index - 3, total, lastTicketNumber);

                // ✅ ACTUALIZAR VARIABLES LOCALES:
                string item = listBoxTicket.Items[index].ToString();

                if (item.StartsWith("TOTAL") || item.StartsWith("-")) return;

                decimal precioProducto = 0;
                int idx = item.LastIndexOf('$');
                if (idx != -1)
                {
                    string precioStr = item.Substring(idx + 1).Trim();
                    if (decimal.TryParse(precioStr, out precioProducto))
                    {
                        total -= precioProducto;
                    }
                }

                listBoxTicket.Items.RemoveAt(index);
                RenumerarTicket();
                ActualizarLineaTotal();
            }
        }

        private void DeshacerUltimaAccion()
        {
            // ✅ USAR CONTROLADOR (MEMENTO):
            bool deshecho = controller.DeshacerUltimaAccion();

            if (deshecho)
            {
                // Recalcular total desde el listBox
                total = 0;
                for (int i = 3; i < listBoxTicket.Items.Count; i++)
                {
                    string item = listBoxTicket.Items[i].ToString();
                    if (item.StartsWith("TOTAL")) break;

                    int idx = item.LastIndexOf('$');
                    if (idx != -1)
                    {
                        string precioStr = item.Substring(idx + 1).Trim();
                        if (decimal.TryParse(precioStr, out decimal precio))
                            total += precio;
                    }
                }
                ActualizarLineaTotal();
                MessageBox.Show("✅ Acción deshecha usando MEMENTO", "Patrón Memento");
            }
            else
            {
                MessageBox.Show("❌ No hay acciones que deshacer", "Memento");
            }
        }

        private void RenumerarTicket()
        {
            // Empieza desde el primer número que debe aparecer en el ticket (se salta las primeras 3 líneas)
            int contador = 1;

            // Recorre los ítems del ticket empezando desde la cuarta línea (índice 3)
            for (int i = 3; i < listBoxTicket.Items.Count; i++)
            {
                string item = listBoxTicket.Items[i].ToString();

                // Asegúrate de no renumerar líneas de encabezado, totales o separadores
                if (!item.StartsWith("TOTAL") && !item.StartsWith("-") && !item.StartsWith("----------------") && !string.IsNullOrWhiteSpace(item))
                {
                    // Si es un producto o ingrediente, actualiza la numeración
                    string itemTicket = item.Substring(item.IndexOf(':') + 1).Trim(); // Obtiene el texto después del número
                    listBoxTicket.Items[i] = $"{contador}: {itemTicket}";  // Actualiza la numeración
                    contador++;
                }
            }

            // Actualiza el contador global para seguir desde el número siguiente al último producto
            lastTicketNumber = contador;
        }



        private bool EsCombo(string nombre)
        {
            return nombre.StartsWith("C ");
        }

        private bool EsFrito(string nombre)
        {
            return nombre.StartsWith("F ") || nombre == "Aros";
        }

        private bool EsBebida(string nombre)
        {
            return nombre.StartsWith("V ") || nombre == "Agua ciel";
        }

        private string ExtraerNombreBase(string combo)
        {
            return combo.Replace("C ", "");
        }

        private void ActualizarLineaTotal()
        {
            // Elimina la línea anterior de total si existe
            var totalLines = listBoxTicket.Items.Cast<string>().Where(item => !item.StartsWith("TOTAL")).ToList();
            listBoxTicket.Items.Clear();
            foreach (var line in totalLines)
                listBoxTicket.Items.Add(line);

            listBoxTicket.Items.Add($"TOTAL               ${total,6:F2}");
        }

        private void listBoxTicket_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = listBoxTicket.SelectedItem?.ToString();

            // Lista de hamburguesas válidas (productos individuales)
            var hamburguesasValidas = new List<string> { "Clásica", "Famous Star", "Western", "Teriyaki" };

            // Verifica si el ítem seleccionado comienza con el nombre de una hamburguesa válida
            if (selectedItem != null &&
                (hamburguesasValidas.Any(h => selectedItem.Contains(h)) ||  // Verifica si contiene el nombre de una hamburguesa
                 (selectedItem.StartsWith("--") && hamburguesasValidas.Any(h => selectedItem.Substring(2).Trim().Contains(h)))))
            {
                button7.Enabled = true;  // Habilita el botón "Agregar"
                button8.Enabled = true;  // Habilita el botón "Quitar" - NUEVA LÍNEA
            }
            else
            {
                button7.Enabled = false;  // Deshabilita el botón "Agregar"
                button8.Enabled = false;  // Deshabilita el botón "Quitar" - NUEVA LÍNEA
            }
        }



        private void button7_Click(object sender, EventArgs e)
        {
            string productoSeleccionado = listBoxTicket.SelectedItem.ToString();
            string hamburguesaSeleccionada = ExtraerNombreHamburguesa(productoSeleccionado);

            if (string.IsNullOrEmpty(hamburguesaSeleccionada)) return;

            // ✅ LIMPIAR PANEL Y MOSTRAR OPCIONES DE INGREDIENTES:
            flowLayoutPanel2.Controls.Clear();

            // ✅ CREAR BOTONES PARA AGREGAR INGREDIENTES:
            Button btnQueso = new Button();
            btnQueso.Text = "Agregar Queso";
            btnQueso.Width = 280;
            btnQueso.Height = 112;
            btnQueso.BackColor = Color.FromArgb(34, 139, 34); // Verde
            btnQueso.ForeColor = Color.White;
            btnQueso.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnQueso.FlatStyle = FlatStyle.Flat;
            btnQueso.FlatAppearance.BorderSize = 0;
            btnQueso.Click += (s, e) => AgregarIngredienteConPatrones("Queso amarillo", hamburguesaSeleccionada);
            flowLayoutPanel2.Controls.Add(btnQueso);

            Button btnTocino = new Button();
            btnTocino.Text = "Agregar Tocino";
            btnTocino.Width = 280;
            btnTocino.Height = 112;
            btnTocino.BackColor = Color.FromArgb(34, 139, 34); // Verde
            btnTocino.ForeColor = Color.White;
            btnTocino.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnTocino.FlatStyle = FlatStyle.Flat;
            btnTocino.FlatAppearance.BorderSize = 0;
            btnTocino.Click += (s, e) => AgregarIngredienteConPatrones("Tocino", hamburguesaSeleccionada);
            flowLayoutPanel2.Controls.Add(btnTocino);
        }

        private void AgregarIngredienteConPatrones(string ingrediente, string hamburguesaSeleccionada)
        {
            // ✅ MEMENTO - Guardar estado antes del cambio:
            var memento = ticketOriginator.CrearMemento(listBoxTicket, total, lastTicketNumber);
            ticketCaretaker.GuardarEstado(memento);

            // ✅ DECORADOR - Crear hamburguesa base y decorarla:
            Hamburguesa hamburguesa = new HamburguesaBase(hamburguesaSeleccionada);
            Hamburguesa hamburguesaConIngrediente = hamburguesa;

            switch (ingrediente)
            {
                case "Queso amarillo":
                    hamburguesaConIngrediente = new QuesoExtra(hamburguesa);
                    break;
                case "Tocino":
                    hamburguesaConIngrediente = new BaconExtra(hamburguesa);
                    break;
            }

            // ✅ BUSCAR LA HAMBURGUESA EN EL TICKET:
            int indiceHamburguesa = -1;
            for (int i = 0; i < listBoxTicket.Items.Count; i++)
            {
                string item = listBoxTicket.Items[i].ToString();
                if (item.Contains(hamburguesaSeleccionada) && item.Contains(":"))
                {
                    indiceHamburguesa = i;
                    break;
                }
            }

            if (indiceHamburguesa >= 0)
            {
                // ✅ CALCULAR PRECIO DEL INGREDIENTE EXTRA:
                decimal precioExtra = hamburguesaConIngrediente.ObtenerPrecio() - hamburguesa.ObtenerPrecio();
                string itemExtra = $"-- {ingrediente} ${precioExtra,6:F2}";

                // ✅ INSERTAR INGREDIENTE DESPUÉS DE LA HAMBURGUESA:
                listBoxTicket.Items.Insert(indiceHamburguesa + 1, itemExtra);

                // ✅ ACTUALIZAR TOTAL:
                total += precioExtra;
                ActualizarLineaTotal();

                MessageBox.Show($"✅ {ingrediente} agregado usando DECORADOR", "Patrón Decorador");
            }
        }


        public void AgregarIngredienteExtra(string ingrediente, int indiceHamburguesa)
        {
            if (indiceHamburguesa >= 0 && indiceHamburguesa < ticketItems.Count)
            {
                var hamburguesa = ticketItems[indiceHamburguesa];
                if (hamburguesa.EsHamburguesa)
                {
                    decimal precioExtra = ObtenerPrecioIngredienteExtra(ingrediente);

                    // Agregar ingrediente extra como item separado
                    var ingredienteExtra = new ProductoItem
                    {
                        Nombre = $"-- {ingrediente}",
                        Precio = precioExtra,
                        EsIngredienteExtra = true
                    };

                    ticketItems.Insert(indiceHamburguesa + 1, ingredienteExtra);
                    total += precioExtra;
                }
            }
        }

        public void EliminarDelTicket(int indice)
        {
            if (indice >= 0 && indice < ticketItems.Count)
            {
                total -= ticketItems[indice].Precio;
                ticketItems.RemoveAt(indice);
            }
        }

        public List<string> ObtenerTicketItems()
        {
            var items = new List<string>();
            int contador = 1;

            foreach (var item in ticketItems)
            {
                if (item.EsIngredienteExtra)
                {
                    items.Add($"{item.Nombre,-20} ${item.Precio,6:F2}");
                }
                else
                {
                    items.Add($"{contador}: {item.Nombre,-20} ${item.Precio,6:F2}");
                    contador++;
                }
            }
            return items;
        }

        // ✅ Para sincronizar con Memento
        public void RestaurarEstado(List<string> items, decimal nuevoTotal, int nuevoLastTicketNumber)
        {
            ticketItems.Clear();
            total = nuevoTotal;
            lastTicketNumber = nuevoLastTicketNumber;

            // Reconstruir items desde las líneas del ticket
            // (Implementación simplificada)
        }

        private decimal ObtenerPrecioIngredienteExtra(string ingrediente)
        {
            switch (ingrediente)
            {
                case "Queso amarillo": return 8.00m;
                case "Tocino": return 12.00m;
                default: return 5.00m;
            }
        }


        public decimal ObtenerTotal() => total;
        public int ObtenerLastTicketNumber() => lastTicketNumber;



        private void AgregarIngrediente(string ingrediente, Hamburguesa hamburguesa)
        {
            // Crear un nuevo decorador basado en el ingrediente seleccionado
            Hamburguesa hamburguesaConIngrediente = hamburguesa;

            switch (ingrediente)
            {
                case "Queso amarillo":
                    hamburguesaConIngrediente = new QuesoExtra(hamburguesa); // Envolver con el decorador de Queso
                    break;
                case "Tocino":
                    hamburguesaConIngrediente = new BaconExtra(hamburguesa); // Envolver con el decorador de Tocino
                    break;
                default:
                    break;
            }

            // Obtener el nombre y precio actualizado de la hamburguesa con el ingrediente
            string itemTicket = $"{hamburguesa.Nombre} ({hamburguesa.ObtenerPrecio():C})";  // Formato original de la hamburguesa
            string itemExtra = $"-- {ingrediente} ({hamburguesaConIngrediente.ObtenerPrecio() - hamburguesa.ObtenerPrecio():C})";  // El extra agregado

            // Buscar la hamburguesa seleccionada en el ticket
            int index = -1;
            for (int i = 0; i < listBoxTicket.Items.Count; i++)
            {
                if (listBoxTicket.Items[i].ToString().StartsWith(hamburguesa.Nombre))
                {
                    index = i;
                    break;
                }
            }

            // Si la hamburguesa está en el ticket
            if (index >= 0)
            {
                // Insertar el extra justo después de la hamburguesa seleccionada
                listBoxTicket.Items.Insert(index + 1, itemExtra);
            }
            else
            {
                // Si no se encuentra la hamburguesa, agregarla y luego el extra
                listBoxTicket.Items.Add(itemTicket);  // Agregar la hamburguesa al ticket
                listBoxTicket.Items.Add(itemExtra);   // Agregar el extra justo debajo
            }

            // Actualizar el total con el precio del extra
            total += hamburguesaConIngrediente.ObtenerPrecio() - hamburguesa.ObtenerPrecio(); // Sumar el precio del ingrediente al total
            ActualizarLineaTotal();  // Actualizar el total en la interfaz
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Cargar el inventario desde el archivo
            Inventario inventario = new Inventario();
            inventario.CargarInventario("inventario.json"); // Cargar el inventario actual

            // Por cada producto en el ticket, descontamos el inventario
            for (int i = 3; i < listBoxTicket.Items.Count; i++)  // Empezamos desde la tercera línea (índice 3)
            {
                var item = listBoxTicket.Items[i];
                string itemString = item.ToString().Trim();

                // Si es el total, lo ignoramos
                if (itemString.Contains("TOTAL"))
                {
                    Debug.WriteLine($"Ignorando línea de total: {itemString}");
                    continue;
                }

                // Si el item empieza con "--" o contiene ":", lo tomamos para descontar
                if (itemString.StartsWith("--") || itemString.Contains(":"))
                {
                    string producto = itemString;

                    // Si tiene "--", eliminamos esos caracteres para que el producto sea válido
                    if (producto.StartsWith("--"))
                    {
                        producto = producto.Substring(2).Trim(); // Elimina los primeros dos caracteres "--"
                    }

                    // Si tiene ":", procesamos para extraer el producto
                    if (producto.Contains(":"))
                    {
                        var partes = producto.Split(':');
                        if (partes.Length > 1)
                        {
                            producto = partes[1].Trim(); // Extraemos el nombre y precio
                        }
                    }

                    // Eliminar cualquier texto después del precio (si hay un $)
                    int indexOfPrice = producto.IndexOf('$');
                    if (indexOfPrice != -1)
                    {
                        producto = producto.Substring(0, indexOfPrice).Trim(); // Solo tomar el nombre del producto
                    }

                    // Eliminar cualquier texto después de un paréntesis (si hay un '(')
                    int indexOfParenthesis = producto.IndexOf('(');
                    if (indexOfParenthesis != -1)
                    {
                        producto = producto.Substring(0, indexOfParenthesis).Trim(); // Solo tomar el nombre antes del paréntesis
                    }

                    // NUEVA VALIDACIÓN: No descontar si es un ingrediente quitado (contiene "sin")
                    if (producto.ToLower().Contains("sin "))
                    {
                        Debug.WriteLine($"Ignorando ingrediente quitado: {producto}");
                        continue;
                    }

                    // Solo descontar si no es un título de combo (que empieza con "C ")
                    if (!producto.StartsWith("C "))
                    {
                        Debug.WriteLine($"Descontando producto: {producto}");
                        DescontarInventario(producto, inventario);
                    }
                    else
                    {
                        Debug.WriteLine($"Ignorando título de combo: {producto}");
                    }
                }
                else
                {
                    // Si no contiene ":" o "--", lo ignoramos
                    Debug.WriteLine($"Formato incorrecto en el item: {itemString}");
                }
            }

            // Guardar el inventario actualizado
            inventario.GuardarInventario("inventario.json");

            // Luego de descontar el inventario, puedes guardar el ticket y finalizar
            Venta ventaForm = new Venta();
            ventaForm.productosDelTicket = listBoxTicket.Items.Cast<string>().ToList();
            ventaForm.textBox1.Text = total.ToString("F2"); // El total actual

            ventaForm.Show(); // o ShowDialog() si quieres que sea modal
            this.Hide();
        }


        public void DescontarInventario(string nombreProducto, Inventario inventario)
        {
            Debug.WriteLine($"Iniciando descuento para: {nombreProducto}");

            var ingredientesQuitados = ObtenerIngredientesQuitados(nombreProducto);

            switch (nombreProducto)
            {
                case "Clásica":
                    InventarioRepository.DescontarCantidad("Pan Kaiser", 1, "Venta - Hamburguesa Clásica");
                    if (!ingredientesQuitados.Contains("Lechuga"))
                        InventarioRepository.DescontarCantidad("Lechuga", 2, "Venta - Hamburguesa Clásica");
                    if (!ingredientesQuitados.Contains("Salsa clasica"))
                        InventarioRepository.DescontarCantidad("Salsa clasica", 1, "Venta - Hamburguesa Clásica");
                    if (!ingredientesQuitados.Contains("Queso amarillo"))
                        InventarioRepository.DescontarCantidad("Queso amarillo", 1, "Venta - Hamburguesa Clásica");
                    InventarioRepository.DescontarCantidad("Carne", 1, "Venta - Hamburguesa Clásica");
                    break;

                case "Famous":
                    InventarioRepository.DescontarCantidad("Pan Kaiser", 1, "Venta - Famous Star");
                    if (!ingredientesQuitados.Contains("Queso amarillo"))
                        InventarioRepository.DescontarCantidad("Queso amarillo", 1, "Venta - Famous Star");
                    if (!ingredientesQuitados.Contains("Lechuga"))
                        InventarioRepository.DescontarCantidad("Lechuga", 2, "Venta - Famous Star");
                    if (!ingredientesQuitados.Contains("Tomate"))
                        InventarioRepository.DescontarCantidad("Tomate", 1, "Venta - Famous Star");
                    if (!ingredientesQuitados.Contains("Cebolla"))
                        InventarioRepository.DescontarCantidad("Cebolla", 3, "Venta - Famous Star");
                    if (!ingredientesQuitados.Contains("Pepinillos"))
                        InventarioRepository.DescontarCantidad("Pepinillos", 3, "Venta - Famous Star");
                    InventarioRepository.DescontarCantidad("Carne", 1, "Venta - Famous Star");
                    if (!ingredientesQuitados.Contains("Salsa Especial"))
                        InventarioRepository.DescontarCantidad("Salsa Especial", 1, "Venta - Famous Star");
                    break;

                case "Western":
                    InventarioRepository.DescontarCantidad("Pan Kaiser", 1, "Venta - Western");
                    if (!ingredientesQuitados.Contains("Queso amarillo"))
                        InventarioRepository.DescontarCantidad("Queso amarillo", 1, "Venta - Western");
                    InventarioRepository.DescontarCantidad("Carne", 1, "Venta - Western");
                    if (!ingredientesQuitados.Contains("Tocino"))
                        InventarioRepository.DescontarCantidad("Tocino", 2, "Venta - Western");
                    if (!ingredientesQuitados.Contains("BBQ"))
                        InventarioRepository.DescontarCantidad("BBQ", 1, "Venta - Western");
                    break;

                case "Teriyaki":
                    InventarioRepository.DescontarCantidad("Pan Kaiser", 1, "Venta - Teriyaki");
                    InventarioRepository.DescontarCantidad("Carne", 1, "Venta - Teriyaki");
                    if (!ingredientesQuitados.Contains("Lechuga"))
                        InventarioRepository.DescontarCantidad("Lechuga", 2, "Venta - Teriyaki");
                    if (!ingredientesQuitados.Contains("Tomate"))
                        InventarioRepository.DescontarCantidad("Tomate", 1, "Venta - Teriyaki");
                    if (!ingredientesQuitados.Contains("Cebolla morada"))
                        InventarioRepository.DescontarCantidad("Cebolla morada", 2, "Venta - Teriyaki");
                    if (!ingredientesQuitados.Contains("Piña"))
                        InventarioRepository.DescontarCantidad("Piña", 1, "Venta - Teriyaki");
                    if (!ingredientesQuitados.Contains("Salsa Teriyaki"))
                        InventarioRepository.DescontarCantidad("Salsa Teriyaki", 1, "Venta - Teriyaki");
                    if (!ingredientesQuitados.Contains("Mayonesa"))
                        InventarioRepository.DescontarCantidad("Mayonesa", 1, "Venta - Teriyaki");
                    break;

                case "V Chico":
                    InventarioRepository.DescontarCantidad("Vasos chicos", 1, "Venta - Bebida");
                    break;

                case "V Mediano":
                    InventarioRepository.DescontarCantidad("Vasos medianos", 1, "Venta - Bebida");
                    break;

                case "V Grande":
                    InventarioRepository.DescontarCantidad("Vasos grandes", 1, "Venta - Bebida");
                    break;

                case "Agua ciel":
                    InventarioRepository.DescontarCantidad("Agua ciel", 1, "Venta - Bebida");
                    break;

                case "F Chicas":
                    InventarioRepository.DescontarCantidad("Papas", 4, "Venta - Papas");
                    break;

                case "F Medianas":
                    InventarioRepository.DescontarCantidad("Papas", 5, "Venta - Papas");
                    break;

                case "F Grandes":
                    InventarioRepository.DescontarCantidad("Papas", 6, "Venta - Papas");
                    break;

                case "Aros":
                    InventarioRepository.DescontarCantidad("Aros", 8, "Venta - Aros");
                    break;

                case "Nieve Vainilla":
                    InventarioRepository.DescontarCantidad("Nieve Vainilla", 6, "Venta - Postre");
                    break;

                case "Nieve Chocolate":
                    InventarioRepository.DescontarCantidad("Nieve Chocolate", 6, "Venta - Postre");
                    break;

                case "Nieve Fresa":
                    InventarioRepository.DescontarCantidad("Nieve Fresa", 6, "Venta - Postre");
                    break;

                case "Galleta":
                    InventarioRepository.DescontarCantidad("Galleta", 1, "Venta - Postre");
                    break;

                case "Juguete":
                    InventarioRepository.DescontarCantidad("Juguete", 1, "Venta - Extra");
                    break;

                default:
                    Debug.WriteLine($"Producto no encontrado: {nombreProducto}");
                    break;
            }
        }


        public void RestarIngrediente(List<Producto> categoria, string nombre, int cantidad)
        {
            var ingrediente = categoria.FirstOrDefault(i => i.Nombre == nombre);
            if (ingrediente != null)
            {
                if (ingrediente.Cantidad >= cantidad)
                {
                    ingrediente.Cantidad -= cantidad;
                    Debug.WriteLine($"Descontado {cantidad} de {nombre}. Cantidad restante: {ingrediente.Cantidad}");
                }
                else
                {
                    Debug.WriteLine($"No hay suficiente cantidad de {nombre}. Se necesita: {cantidad}, disponible: {ingrediente.Cantidad}");
                }
            }
            else
            {
                Debug.WriteLine($"Ingrediente no encontrado en inventario: {nombre}");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ComedorLlevar comedorllevar = new ComedorLlevar();
            comedorllevar.Show();

            // Ahora que ComedorLlevar se cerró, podemos cerrar el formulario de Venta
            this.Hide();
        }

        private void CargarUltimoTicketEnTextBox()
        {
            try
            {
                string carpetaTickets = Path.Combine(Directory.GetCurrentDirectory(), "Tickets");

                if (!Directory.Exists(carpetaTickets))
                {
                    textBox1.Text = "❌ No se encontró la carpeta de tickets.";
                    return;
                }

                string[] archivosTicket = Directory.GetFiles(carpetaTickets, "Ticket_*.txt");

                if (archivosTicket.Length == 0)
                {
                    textBox1.Text = "❌ No hay tickets disponibles.";
                    return;
                }

                // Ordenar por fecha de modificación (más reciente primero)
                var archivoMasReciente = archivosTicket
                    .OrderByDescending(f => File.GetLastWriteTime(f))
                    .First();

                // Leer el contenido del ticket más reciente
                string contenidoTicket = File.ReadAllText(archivoMasReciente, Encoding.UTF8);

                // Formatear para mostrar en el textBox
                string ticketFormateado = $"🎫 ÚLTIMO TICKET PROCESADO\n";
                ticketFormateado += $"📅 {File.GetLastWriteTime(archivoMasReciente):dd/MM/yyyy HH:mm:ss}\n";
                ticketFormateado += $"📄 {Path.GetFileName(archivoMasReciente)}\n";
                ticketFormateado += new string('=', 50) + "\n\n";
                ticketFormateado += contenidoTicket;

                textBox1.Text = ticketFormateado;

                // Configurar el textBox para mejor visualización
                textBox1.Font = new Font("Courier New", 9);
                textBox1.ReadOnly = true;
                textBox1.ScrollBars = ScrollBars.Vertical;
                textBox1.BackColor = Color.FromArgb(50, 50, 50); // FONDO OSCURO
                textBox1.ForeColor = Color.White; // TEXTO BLANCO

                // Posicionar el cursor al inicio
                textBox1.SelectionStart = 0;
                textBox1.ScrollToCaret();
            }
            catch (Exception ex)
            {
                textBox1.Text = $"❌ Error al cargar el último ticket: {ex.Message}";
            }
        }

        public void ActualizarUltimoTicket()
        {
            CargarUltimoTicketEnTextBox();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBoxTicket.SelectedItem == null) return;

            string productoSeleccionado = listBoxTicket.SelectedItem.ToString();
            string hamburguesaSeleccionada = ExtraerNombreHamburguesa(productoSeleccionado);

            if (string.IsNullOrEmpty(hamburguesaSeleccionada)) return;

            // ✅ LIMPIAR PANEL Y MOSTRAR OPCIONES DE INGREDIENTES A QUITAR:
            flowLayoutPanel2.Controls.Clear();

            var ingredientesDisponibles = ObtenerIngredientesParaQuitar(hamburguesaSeleccionada);

            foreach (var ingrediente in ingredientesDisponibles)
            {
                Button btnQuitar = new Button();
                btnQuitar.Text = $"Sin {ingrediente}";
                btnQuitar.Width = 280;
                btnQuitar.Height = 112;
                btnQuitar.BackColor = Color.FromArgb(220, 20, 60); // Rojo
                btnQuitar.ForeColor = Color.White;
                btnQuitar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btnQuitar.FlatStyle = FlatStyle.Flat;
                btnQuitar.FlatAppearance.BorderSize = 0;
                btnQuitar.Click += (s, e) => QuitarIngredienteConPatrones(ingrediente, hamburguesaSeleccionada);

                flowLayoutPanel2.Controls.Add(btnQuitar);
            }
        }

        private void QuitarIngredienteConPatrones(string ingrediente, string hamburguesaSeleccionada)
        {
            // ✅ MEMENTO - Guardar estado antes del cambio:
            var memento = ticketOriginator.CrearMemento(listBoxTicket, total, lastTicketNumber);
            ticketCaretaker.GuardarEstado(memento);

            // ✅ BUSCAR LA HAMBURGUESA EN EL TICKET:
            int indiceHamburguesa = -1;
            for (int i = 0; i < listBoxTicket.Items.Count; i++)
            {
                string item = listBoxTicket.Items[i].ToString();
                if (item.Contains(hamburguesaSeleccionada) && item.Contains(":"))
                {
                    indiceHamburguesa = i;
                    break;
                }
            }

            if (indiceHamburguesa == -1) return;

            // ✅ VERIFICAR SI YA EXISTE "SIN [INGREDIENTE]":
            bool yaQuitado = false;
            for (int i = indiceHamburguesa + 1; i < listBoxTicket.Items.Count; i++)
            {
                string item = listBoxTicket.Items[i].ToString();

                if ((item.Contains(":") && char.IsDigit(item[0])) || item.StartsWith("TOTAL"))
                    break;

                if (item.Contains($"sin {ingrediente}"))
                {
                    yaQuitado = true;
                    MessageBox.Show($"❌ Ya se quitó {ingrediente} de esta hamburguesa.", "Ingrediente ya quitado");
                    break;
                }
            }

            if (!yaQuitado)
            {
                // ✅ AGREGAR LÍNEA "SIN INGREDIENTE":
                string lineaSinIngrediente = $"-- sin {ingrediente}";
                listBoxTicket.Items.Insert(indiceHamburguesa + 1, lineaSinIngrediente);

                MessageBox.Show($"✅ {ingrediente} quitado (no se descontará del inventario)", "Ingrediente Quitado");
            }
        }

        private string ExtraerNombreHamburguesa(string itemSeleccionado)
        {
            var hamburguesasValidas = new List<string> { "Clásica", "Famous Star", "Western", "Teriyaki" };

            foreach (var hamburguesa in hamburguesasValidas)
            {
                if (itemSeleccionado.Contains(hamburguesa))
                {
                    return hamburguesa;
                }
            }

            return string.Empty;
        }

        private List<string> ObtenerIngredientesParaQuitar(string hamburguesa)
        {
            var ingredientes = new List<string>();

            switch (hamburguesa)
            {
                case "Clásica":
                    ingredientes.AddRange(new[] { "Lechuga", "Salsa clasica", "Queso amarillo" });
                    break;

                case "Famous Star":
                    ingredientes.AddRange(new[] { "Queso amarillo", "Lechuga", "Tomate", "Cebolla", "Pepinillos", "Salsa Especial" });
                    break;

                case "Western":
                    ingredientes.AddRange(new[] { "Queso amarillo", "Tocino", "BBQ" });
                    break;

                case "Teriyaki":
                    ingredientes.AddRange(new[] { "Lechuga", "Tomate", "Cebolla morada", "Piña", "Salsa Teriyaki", "Mayonesa" });
                    break;
            }

            return ingredientes;
        }

        private void QuitarIngrediente(string ingrediente, string hamburguesa)
        {
            // Buscar la hamburguesa en el ticket
            int indiceHamburguesa = -1;
            for (int i = 0; i < listBoxTicket.Items.Count; i++)
            {
                string item = listBoxTicket.Items[i].ToString();
                if (item.Contains(hamburguesa) && item.Contains(":"))
                {
                    indiceHamburguesa = i;
                    break;
                }
            }

            if (indiceHamburguesa == -1) return;

            // Verificar si ya existe una línea "sin [ingrediente]" para esta hamburguesa
            bool yaQuitado = false;
            for (int i = indiceHamburguesa + 1; i < listBoxTicket.Items.Count; i++)
            {
                string item = listBoxTicket.Items[i].ToString();

                // Si llegamos a otro producto numerado o al total, salimos
                if ((item.Contains(":") && char.IsDigit(item[0])) || item.StartsWith("TOTAL"))
                    break;

                // Si ya existe "sin [ingrediente]", no lo agregamos de nuevo
                if (item.Contains($"sin {ingrediente}"))
                {
                    yaQuitado = true;
                    MessageBox.Show($"Ya se quitó {ingrediente} de esta hamburguesa.", "Ingrediente ya quitado");
                    break;
                }
            }

            if (!yaQuitado)
            {
                // Agregar la línea "sin ingrediente" después de la hamburguesa
                string lineaSinIngrediente = $"-- sin {ingrediente}";
                listBoxTicket.Items.Insert(indiceHamburguesa + 1, lineaSinIngrediente);

                MessageBox.Show($"Se quitó {ingrediente} de la {hamburguesa}.", "Ingrediente quitado");
            }
        }

        private HashSet<string> ObtenerIngredientesQuitados(string nombreProducto)
        {
            var ingredientesQuitados = new HashSet<string>();

            // Buscar el producto en el ticket
            for (int i = 0; i < listBoxTicket.Items.Count; i++)
            {
                string item = listBoxTicket.Items[i].ToString();

                if (item.Contains(nombreProducto) && item.Contains(":"))
                {
                    // Revisar las líneas siguientes para buscar ingredientes quitados
                    for (int j = i + 1; j < listBoxTicket.Items.Count; j++)
                    {
                        string lineaSiguiente = listBoxTicket.Items[j].ToString();

                        // Si llegamos a otro producto o total, salimos
                        if ((lineaSiguiente.Contains(":") && char.IsDigit(lineaSiguiente[0])) ||
                            lineaSiguiente.StartsWith("TOTAL"))
                            break;

                        // Si encontramos una línea "sin [ingrediente]"
                        if (lineaSiguiente.Contains("sin "))
                        {
                            string ingrediente = lineaSiguiente.Replace("-- sin ", "").Trim();
                            ingredientesQuitados.Add(ingrediente);
                        }
                    }
                    break;
                }
            }

            return ingredientesQuitados;
        }


        //***************************************DISEÑO*************************************************
        private void AplicarTemaGeneral()
        {
            // Colores del tema Carl's Jr
            Color colorPrimario = Color.FromArgb(255, 204, 0);     // Amarillo Carl's Jr
            Color colorSecundario = Color.FromArgb(220, 53, 69);   // Rojo
            Color colorFondo = Color.FromArgb(45, 45, 48);         // Gris oscuro moderno
            Color colorTexto = Color.White;

            // Aplicar al formulario
            this.BackColor = colorFondo;
            this.ForeColor = colorTexto;
        }

        private void MejorarPanelTicket()
        {
            // Panel principal
            panel1.BackColor = Color.FromArgb(30, 30, 30);
            panel1.BorderStyle = BorderStyle.None;

            // Agregar sombra simulada con un panel adicional
            Panel sombra = new Panel();
            sombra.BackColor = Color.FromArgb(20, 20, 20);
            sombra.Size = new Size(panel1.Width + 6, panel1.Height + 6);
            sombra.Location = new Point(panel1.Location.X + 3, panel1.Location.Y + 3);
            this.Controls.Add(sombra);
            sombra.SendToBack();

            // ListBox del ticket
            listBoxTicket.BackColor = Color.FromArgb(40, 40, 40);
            listBoxTicket.ForeColor = Color.White;
            listBoxTicket.BorderStyle = BorderStyle.None;
            listBoxTicket.Font = new Font("Consolas", 12, FontStyle.Regular);

            // TextBox inferior
            textBox1.BackColor = Color.FromArgb(50, 50, 50);
            textBox1.ForeColor = Color.LightGray;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Courier New", 9);
        }

        private void MejorarBotonesCategorias()
        {
            flowLayoutPanel1.BackColor = Color.FromArgb(45, 45, 48);

            // Crear botones personalizados para categorías
            string[] categorias = { "Hamburguesas", "Bebidas", "Fritos", "Postres", "Extras", "Combo Hmbrg" };
            Color[] coloresCategorias = {
            Color.FromArgb(255, 107, 107), // Rojo para hamburguesas
            Color.FromArgb(54, 162, 235),  // Azul para bebidas
            Color.FromArgb(255, 193, 7),   // Amarillo para fritos
            Color.FromArgb(255, 105, 180), // Rosa para postres
            Color.FromArgb(40, 167, 69),   // Verde para extras
            Color.FromArgb(108, 117, 125)  // Gris para combos
            };

            Button[] botones = { button1, button2, button3, button5, button6, button4 };

            for (int i = 0; i < botones.Length; i++)
            {
                Button btn = botones[i];
                btn.BackColor = coloresCategorias[i];
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                btn.FlatAppearance.MouseOverBackColor = AjustarBrillo(coloresCategorias[i], 0.8f);

                // Efecto de sombra
                btn.FlatAppearance.MouseDownBackColor = AjustarBrillo(coloresCategorias[i], 0.6f);
            }
        }

        // Método auxiliar para ajustar brillo
        private Color AjustarBrillo(Color color, float factor)
        {
            int r = Math.Min(255, Math.Max(0, (int)(color.R * factor)));
            int g = Math.Min(255, Math.Max(0, (int)(color.G * factor)));
            int b = Math.Min(255, Math.Max(0, (int)(color.B * factor)));

            return Color.FromArgb(r, g, b);
        }

        private void MejorarPanelProductos()
        {
            flowLayoutPanel2.BackColor = Color.FromArgb(35, 35, 35);
            flowLayoutPanel2.Padding = new Padding(10);

            // Agregar borde redondeado
            flowLayoutPanel2.Region = CrearBordeRedondeado(flowLayoutPanel2.Width, flowLayoutPanel2.Height, 15);
        }

        // Método para crear bordes redondeados
        private Region CrearBordeRedondeado(int width, int height, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(width - radius, 0, radius, radius, 270, 90);
            path.AddArc(width - radius, height - radius, radius, radius, 0, 90);
            path.AddArc(0, height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return new Region(path);
        }

        private void MejorarBotonesAccion()
        {
            Button btnDeshacer = new Button();
            btnDeshacer.Text = "↶ Deshacer";
            btnDeshacer.Size = new Size(180, 60);
            btnDeshacer.Location = new Point(1570, 1141); 
            btnDeshacer.BackColor = Color.FromArgb(255, 193, 7);
            btnDeshacer.ForeColor = Color.White;
            btnDeshacer.FlatStyle = FlatStyle.Flat;
            btnDeshacer.FlatAppearance.BorderSize = 0;
            btnDeshacer.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnDeshacer.Click += (s, e) => DeshacerUltimaAccion(); 
            this.Controls.Add(btnDeshacer);
            btnDeshacer.BringToFront();

            // Botón Eliminar
            EstilizarBotonAccion(btnEliminarSeleccionado, Color.FromArgb(220, 53, 69), "🗑️ Eliminar");

            // Botón Agregar
            EstilizarBotonAccion(button7, Color.FromArgb(40, 167, 69), "➕ Agregar");

            // Botón Quitar
            EstilizarBotonAccion(button8, Color.FromArgb(255, 107, 107), "➖ Quitar");

            // Botón Finalizar
            EstilizarBotonAccion(button9, Color.FromArgb(23, 162, 184), "💳 Finalizar", true);

            // Botón Salir
            EstilizarBotonAccion(button10, Color.FromArgb(108, 117, 125), "🚪 Salir");
        }

        private void EstilizarBotonAccion(Button btn, Color colorBase, string texto, bool esImportante = false)
        {
            btn.Text = texto;
            btn.BackColor = colorBase;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", esImportante ? 14 : 12, FontStyle.Bold);
            btn.FlatAppearance.MouseOverBackColor = AjustarBrillo(colorBase, 0.8f);
            btn.FlatAppearance.MouseDownBackColor = AjustarBrillo(colorBase, 0.6f);

            if (esImportante)
            {
                btn.Size = new Size(btn.Width + 20, btn.Height + 10);
            }
        }

        

        private void InicializarAnimaciones()
        {
            animacionTimer = new Timer();
            animacionTimer.Interval = 16; // ~60 FPS
            animacionTimer.Tick += AnimacionTimer_Tick;
        }

        private void AnimacionTimer_Tick(object sender, EventArgs e)
        {
            // Efecto de respiración para el botón Finalizar
            if (button9.Enabled)
            {
                DateTime now = DateTime.Now;
                double respiracion = Math.Sin(now.Millisecond * 0.01) * 0.1 + 0.9;
                button9.BackColor = Color.FromArgb(
                    (int)(23 * respiracion),
                    (int)(162 * respiracion),
                    (int)(184 * respiracion)
                );
            }
        }

        private Button CrearBotonProducto(string nombre, Color colorBase)
        {
            Button btn = new Button();
            btn.Text = nombre;
            btn.Width = 280;
            btn.Height = 112;
            btn.BackColor = colorBase;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.FlatAppearance.MouseOverBackColor = AjustarBrillo(colorBase, 1.2f);
            btn.FlatAppearance.MouseDownBackColor = AjustarBrillo(colorBase, 0.8f);
            btn.Click += Producto_Click;

            // Efecto de sombra
            btn.Margin = new Padding(5);

            return btn;
        }
    }

    public class ProductoItem
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public bool EsHamburguesa { get; set; }
        public bool EsIngredienteExtra { get; set; }
    }
}
