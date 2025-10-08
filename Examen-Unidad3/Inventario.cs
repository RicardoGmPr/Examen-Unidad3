using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;
using Examen_Unidad3.Database;

namespace Examen_Unidad3
{
    public class Inventario
    {
        public List<Producto> congelado = new List<Producto>();
        public List<Producto> refrigerado = new List<Producto>();
        public List<Producto> secos = new List<Producto>();

        // Método para inicializar el inventario con los productos solicitados
        public void InicializarInventario()
        {
            // Congelado
            congelado.Add(new Producto("Carne", 20, "piezas"));
            congelado.Add(new Producto("Papas", 100, "onzas"));
            congelado.Add(new Producto("Aros", 100, "piezas"));
            congelado.Add(new Producto("Galletas", 20, "piezas"));
            congelado.Add(new Producto("Nieve Vainilla", 20, "onzas"));
            congelado.Add(new Producto("Nieve Chocolate", 20, "onzas"));
            congelado.Add(new Producto("Nieve Fresa", 20, "onzas"));

            // Refrigerado
            refrigerado.Add(new Producto("Queso amarillo", 20, "piezas"));
            refrigerado.Add(new Producto("Lechuga", 20, "hojas"));
            refrigerado.Add(new Producto("Tomate", 20, "rebanadas"));
            refrigerado.Add(new Producto("Cebolla", 20, "aros"));
            refrigerado.Add(new Producto("Cebolla morada", 20, "piezas"));
            refrigerado.Add(new Producto("Tocino", 20, "piezas"));
            refrigerado.Add(new Producto("Pepinillos", 20, "piezas"));

            // Secos
            secos.Add(new Producto("Pan Kaiser", 20, "piezas"));
            secos.Add(new Producto("Salsa clasica", 20, "onzas"));
            secos.Add(new Producto("Salsa Especial", 20, "onzas"));
            secos.Add(new Producto("Mayonesa", 20, "onzas"));
            secos.Add(new Producto("BBQ", 20, "onzas"));
            secos.Add(new Producto("Salsa Teriyaki", 20, "onzas"));
            secos.Add(new Producto("Vasos chicos", 20, "piezas"));
            secos.Add(new Producto("Vasos medianos", 20, "piezas"));
            secos.Add(new Producto("Vasos grandes", 20, "piezas"));
            secos.Add(new Producto("Agua ciel", 20, "piezas"));
            secos.Add(new Producto("Juguete", 20, "piezas"));
            secos.Add(new Producto("Piña", 20, "piezas"));
        }

        // Método para mostrar el inventario
        public void VerInventario()
        {
            Console.WriteLine("\n=== Inventario ===\n");

            Console.WriteLine("Congelado:");
            foreach (var producto in congelado)
            {
                Console.WriteLine($"- {producto.Nombre}: {producto.Cantidad} {producto.Unidad}");
            }

            Console.WriteLine("\nRefrigerado:");
            foreach (var producto in refrigerado)
            {
                Console.WriteLine($"- {producto.Nombre}: {producto.Cantidad} {producto.Unidad}");
            }

            Console.WriteLine("\nSecos:");
            foreach (var producto in secos)
            {
                Console.WriteLine($"- {producto.Nombre}: {producto.Cantidad} {producto.Unidad}");
            }
        }

        public void GuardarInventario(string rutaArchivo)
        {
            // Usando Newtonsoft.Json.JsonConvert
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);

            // Guardar el JSON en un archivo
            File.WriteAllText(rutaArchivo, json);
        }

        public void CargarInventario(string rutaArchivo)
        {
            if (File.Exists(rutaArchivo))
            {
                // Leer el archivo JSON
                string json = File.ReadAllText(rutaArchivo);

                // Deserializar el contenido JSON y asignarlo al inventario
                Inventario inventarioCargado = JsonConvert.DeserializeObject<Inventario>(json);

                // Actualizar el inventario actual con los valores cargados
                this.congelado = inventarioCargado.congelado;
                this.refrigerado = inventarioCargado.refrigerado;
                this.secos = inventarioCargado.secos;
            }
            else
            {
                Console.WriteLine("El archivo de inventario no existe.");
            }
        }

        // Cargar desde base de datos
        public void CargarDesdeBaseDatos()
        {
            congelado = Database.InventarioRepository.ObtenerPorCategoria("Congelado");
            refrigerado = Database.InventarioRepository.ObtenerPorCategoria("Refrigerado");
            secos = Database.InventarioRepository.ObtenerPorCategoria("Seco");
        }

        // Actualizar un producto en BD
        public bool ActualizarProductoEnBD(string nombre, int cantidad, string motivo = "Actualización manual")
        {
            return Database.InventarioRepository.ActualizarCantidad(nombre, cantidad, motivo);
        }
    }

    public class Producto
    {
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public string Unidad { get; set; }

        // Constructor
        public Producto(string nombre, int cantidad, string unidad)
        {
            Nombre = nombre;
            Cantidad = cantidad;
            Unidad = unidad;
        }
    }


}
