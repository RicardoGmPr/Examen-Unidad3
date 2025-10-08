using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen_Unidad3
{
    public class AdministradorConsole
    {
        public static void Iniciar()
        {
            Console.Clear();
            Console.WriteLine("===== Acceso Administrador =====");

            Console.Write("Usuario: ");
            string usuario = Console.ReadLine();

            Console.Write("Contraseña: ");
            string password = LeerContrasena();

            if (usuario == "Paseo2000" && password == "Paseo2000")
            {
                Console.WriteLine("\nAcceso concedido.");
                // Aquí puedes poner el menú de administración
                MostrarMenuAdmin();
            }
            else
            {
                Console.WriteLine("\nAcceso denegado.");
            }

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }

        private static string LeerContrasena()
        {
            string contrasena = "";
            ConsoleKeyInfo tecla;

            do
            {
                tecla = Console.ReadKey(true);
                if (tecla.Key != ConsoleKey.Backspace && tecla.Key != ConsoleKey.Enter)
                {
                    contrasena += tecla.KeyChar;
                    Console.Write("*");
                }
                else if (tecla.Key == ConsoleKey.Backspace && contrasena.Length > 0)
                {
                    contrasena = contrasena.Substring(0, contrasena.Length - 1);
                    Console.Write("\b \b");
                }
            } while (tecla.Key != ConsoleKey.Enter);

            return contrasena;
        }

        private static void MostrarMenuAdmin()
        {
            Console.WriteLine("\n--- Panel de Administración ---");
            Console.WriteLine("1. Ver estadísticas");
            Console.WriteLine("2. Agregar productos");
            Console.WriteLine("3. Salir");

            Console.Write("\nSelecciona una opción: ");
            string opcion = Console.ReadLine();

            // Lógica según la opción...
            Console.WriteLine("Funcionalidad aún no implementada.");
        }
    }
}
