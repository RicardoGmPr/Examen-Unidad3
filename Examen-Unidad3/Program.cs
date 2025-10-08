using Examen_Unidad3.Administrador;
using Examen_Unidad3.Database;

namespace Examen_Unidad3
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            DatabaseManager.InicializarBaseDatos();

            ApplicationConfiguration.Initialize();
            Application.Run(new Inicio());
        }
    }
}