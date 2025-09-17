using System.Text.Json;

namespace TypeLibExporter_NET8
{
    public class ListarJsonCombinado : Form
    {
        private readonly List<LibraryInfo> typeLibs;
        private readonly List<SimpleClsIdInfo> clsids;
        private readonly string fileName;

        public ListarJsonCombinado(List<LibraryInfo> typeLibs, List<SimpleClsIdInfo> clsids, string fileName)
        {
            this.typeLibs = typeLibs ?? new();
            this.clsids = clsids ?? new();
            this.fileName = fileName;
            InitializeUi();
        }

        private void InitializeUi()
        {
            Text = $"Editor JSON - {fileName} (Combinado)";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(900, 600);

            var tabs = new TabControl
            {
                Dock = DockStyle.Fill,
            };

            var tabTypelibs = new TabPage("TypeLibs");
            var tabClsids = new TabPage("CLSIDs");

            tabs.TabPages.Add(tabTypelibs);
            tabs.TabPages.Add(tabClsids);

            Controls.Add(tabs);

            // Crear dos formularios ListarJson embebidos (uno por pestaña)
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var typeLibsJson = JsonSerializer.Serialize(typeLibs, jsonOptions);
            var clsidsJson = JsonSerializer.Serialize(clsids, jsonOptions);

            var formTypeLibs = new ListarJson(typeLibsJson, fileName + " (TypeLibs)")
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            tabTypelibs.Controls.Add(formTypeLibs);
            formTypeLibs.Show();

            var formClsids = new ListarJson(clsidsJson, fileName + " (CLSIDs)")
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };
            tabClsids.Controls.Add(formClsids);
            formClsids.Show();
        }

        private void InitializeComponent()
        {

        }
    }
}
