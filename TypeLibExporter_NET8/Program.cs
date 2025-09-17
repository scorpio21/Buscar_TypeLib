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
            // Configuraci�n cl�sica que funciona en todas las versiones
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Ejecutar la aplicaci�n principal
            Application.Run(new Principal());
        }
    }
}


