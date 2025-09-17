using System.Text.Json;
using System.Drawing;

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

            var tabTypelibs = new TabPage($"TypeLibs ({typeLibs.Count})");
            var tabClsids = new TabPage($"CLSIDs ({clsids.Count})");

            tabs.TabPages.Add(tabTypelibs);
            tabs.TabPages.Add(tabClsids);

            // Panel inferior con botón Cerrar
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(249, 250, 251)
            };

            var btnClose = new Button
            {
                Text = "✖ Cerrar",
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Width = 120,
                Height = 36,
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(0, 12) // se ajustará en Resize
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            bottomPanel.Controls.Add(btnClose);

            // Reposicionar en función del tamaño del panel (anclaje a la derecha)
            bottomPanel.Resize += (s, e) =>
            {
                btnClose.Location = new Point(bottomPanel.ClientSize.Width - btnClose.Width - 16, 12);
            };
            // Forzar primera posición correcta
            btnClose.Location = new Point(bottomPanel.ClientSize.Width - btnClose.Width - 16, 12);

            Controls.Add(bottomPanel);
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
