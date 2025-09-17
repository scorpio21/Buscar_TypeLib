using System;
using System.Windows.Forms;

namespace TypeLibExporter_NET8
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configuración clásica que funciona en todas las versiones
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Ejecutar la aplicación principal
            Application.Run(new Principal());
        }
    }
}


