using System.Text.Json;
using Microsoft.Win32;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Drawing;
using System.IO;
using TypeLibExporter_NET8.Servicios;
using TypeLibExporter_NET8.Clases;

namespace TypeLibExporter_NET8
{
    public partial class Principal : Form
    {
        private string settingsFile;

        public Principal()
        {
            InitializeComponent();
            
            // Inicialización global
            ClaseInicial.Inicializar();
            // Archivo para guardar configuraciones
            settingsFile = Path.Combine(ClaseInicial.Rutas.AppData, "settings.json");
            
            // Iconos para menús (usa img/*.ico)
            try
            {
                // Tamaño de íconos del menú (ajusta si usas PNG de 24px)
                menuStrip.ImageScalingSize = new Size(16, 16);
                var cerrarIco = ClaseInicial.CargarIcono("cerrar.ico");
                if (cerrarIco != null)
                {
                    cerrarMenuItem.Image = cerrarIco.ToBitmap();
                    cerrarMenuItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                }

                var abrirIco = ClaseInicial.CargarIcono("Abrir.ico");
                if (abrirIco != null)
                {
                    cargarJsonMenuItem.Image = abrirIco.ToBitmap();
                    cargarJsonMenuItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                }
            }
            catch { /* Ignorar si no está disponible el ícono */ }

            LoadSavedLocation();

            // Cursores direccionales en botones principales
            try { ClaseInicial.Cursores.Aplicar(btnSelectLocation); } catch { }
            try { ClaseInicial.Cursores.Aplicar(btnExportTypeLibs); } catch { }
            try { ClaseInicial.Cursores.Aplicar(btnExportVB6); } catch { }
            try { ClaseInicial.Cursores.Aplicar(btnExportClsIds); } catch { }
            try { ClaseInicial.Cursores.Aplicar(btnExportCombined); } catch { }

            // Asegurar visibilidad y tamaño del lstResults
            try
            {
                lstResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                AjustarAlturaLista();
                lstResults.BringToFront();
                panelMain.Resize += (s, e) => AjustarAlturaLista();
            }
            catch { }

            // Estilos UI: divisor en header, sombras en paneles y botones redondeados
            try
            {
                EstilosUI.AplicarDivisorInferior(panelHeader);
                EstilosUI.AplicarSombraInterna(panelMain);
                EstilosUI.AplicarSombraInterna(panelFooter);

                EstilosUI.AplicarBotonRedondeado(btnSelectLocation, 8);
                EstilosUI.AplicarBotonRedondeado(btnExportTypeLibs, 10);
                EstilosUI.AplicarBotonRedondeado(btnExportVB6, 10);
                EstilosUI.AplicarBotonRedondeado(btnExportClsIds, 10);
                EstilosUI.AplicarBotonRedondeado(btnExportCombined, 10);
            }
            catch { }
        }

        


        #region File Extension Filtering

        private bool IsValidComponentFile(string filename)
        {
            return ArchivoUtil.EsComponenteValido(filename);
        }

        private List<LibraryInfo> FilterValidComponents(List<LibraryInfo> libraries)
        {
            return libraries.Where(lib => IsValidComponentFile(lib.filename)).ToList();
        }

        #endregion

        #region Settings Management
        #endregion

        #region Menu Event Handlers
        #endregion

        #region Export Functions
        #endregion

        #region TypeLib Scanning Methods WITH FILTERING

        private List<LibraryInfo> BuscarEnRegistro()
        {
            // Delegar en el servicio centralizado
            return RegistroScanner.BuscarEnRegistro();
        }

        private List<LibraryInfo> BuscarEnRegistroFiltrado(List<string> preferidos)
        {
            var todas = BuscarEnRegistro(); // Ya viene filtrado
            var filtradas = new List<LibraryInfo>();

            foreach (var pref in preferidos)
            {
                if (!IsValidComponentFile(pref)) continue;
                var encontrado = todas.FirstOrDefault(r => r.filename.Equals(pref, StringComparison.OrdinalIgnoreCase));
                filtradas.Add(encontrado ?? new LibraryInfo
                {
                    filename = pref,
                    type_lib = "Not Found in Registry",
                    version = "Not Found",
                    checksum = "File not accessible or missing",
                    filesize = 0
                });
            }
            return filtradas;
        }

        #endregion

        #region CLSID Scanning Methods - COMPLETAMENTE REESCRITO

        private List<SimpleClsIdInfo> BuscarClsIdsEnRegistro()
        {
            return RegistroScanner.BuscarClsIdsEnRegistro();
        }

        #endregion
    }

}
