using System.Text.Json;
using System.Drawing;
using System.Linq;
using TypeLibExporter_NET8.Servicios;
using TypeLibExporter_NET8.Clases;

namespace TypeLibExporter_NET8
{
    public partial class ListarJson : Form
    {
        private readonly string jsonContent;
        private readonly string fileName;
        private List<object> itemsList = new(); // ✅ INICIALIZADO
        private List<object> originalItemsList = new(); // ✅ INICIALIZADO
        private bool isClsIdData = false;
        // Pendiente para combinado
        private bool openCombinedPending = false;
        private List<LibraryInfo> pendingTypeLibs = new();
        private List<SimpleClsIdInfo> pendingClsids = new();
        // Debounce de búsqueda
        private readonly System.Windows.Forms.Timer searchTimer = new System.Windows.Forms.Timer();

        public ListarJson(string jsonContent, string fileName)
        {
            this.jsonContent = jsonContent ?? string.Empty; // ✅ PROTECCIÓN NULL
            this.fileName = fileName ?? "archivo.json"; // ✅ PROTECCIÓN NULL
            InitializeComponent();
            // Diferir operaciones que abren/cerran formularios a Shown
            this.Shown += ListarJson_Shown;
            // Configurar debounce de búsqueda
            searchTimer.Interval = 250; // ms
            searchTimer.Tick += OnSearchTimerTick;
            // Etiqueta de búsqueda sin ícono (como al principio)
            lblSearch.Image = null;
            lblSearch.Text = "🔍 Buscar:";
            LoadJsonContent();
        }

        // ✅ MÉTODO CORREGIDO PARA DETECTAR TIPO CORRECTAMENTE (delegado a servicio)
        private void LoadJsonContent()
        {
            try
            {
                var resultado = ClaseInicial.Servicios.Inspect(jsonContent, fileName);
                if (resultado.EsCombinadoAmbos)
                {
                    // Guardar datos y diferir apertura al evento Shown (handle ya creado)
                    pendingTypeLibs = resultado.TypeLibs;
                    pendingClsids = resultado.Clsids;
                    openCombinedPending = true;
                    return;
                }

                if (resultado.Items.Count == 0)
                {
                    throw new Exception("No se pudo determinar el formato del JSON");
                }

                itemsList = resultado.Items;
                originalItemsList = new List<object>(resultado.Items);
                isClsIdData = resultado.EsClsId;
                Text = $"Editor JSON - {fileName} {resultado.SufijoTitulo}".Trim();

                RefreshItemsList();
            }
            catch (Exception ex)
            {
                txtJsonDisplay.Text = $"Error al procesar el JSON:\n\n{ex.Message}\n\nContenido original:\n\n{jsonContent}";
                lblInfo.Text = $"⚠️ Error al procesar: {fileName}";
                itemsList = new List<object>();
                originalItemsList = new List<object>();
            }
        }

        
    }
}
