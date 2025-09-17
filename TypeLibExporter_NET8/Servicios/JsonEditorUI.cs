using System.Drawing;
using System.Windows.Forms;

namespace TypeLibExporter_NET8.Servicios
{
    /// <summary>
    /// Construcción de formularios de edición y validación para entradas de JSON.
    /// </summary>
    public static class JsonEditorUI
    {
        public static Form CrearFormularioClsId(string title, TypeLibExporter_NET8.SimpleClsIdInfo? existing)
        {
            var form = new Form
            {
                Text = title,
                Size = new Size(450, 350),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            int yPos = 30;

            var lblFilename = new Label { Text = "📄 filename:", Location = new Point(20, yPos), Width = 100, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(31, 41, 55) };
            var txtFilename = new TextBox { Name = "txtFilename", Location = new Point(130, yPos - 3), Width = 280, Font = new Font("Segoe UI", 9F), Text = existing?.filename ?? string.Empty, PlaceholderText = "Ejemplo: SCRRUN.DLL (OBLIGATORIO)" };
            yPos += 40;

            var lblClsId = new Label { Text = "🔧 clsid:", Location = new Point(20, yPos), Width = 100, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(31, 41, 55) };
            var txtClsId = new TextBox { Name = "txtClsId", Location = new Point(130, yPos - 3), Width = 280, Font = new Font("Segoe UI", 9F), Text = existing?.clsid ?? string.Empty, PlaceholderText = "Ejemplo: {GUID}\\InprocServer32" };
            yPos += 40;

            var lblVersion = new Label { Text = "🏷️ version:", Location = new Point(20, yPos), Width = 100, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(31, 41, 55) };
            var txtVersion = new TextBox { Name = "txtVersion", Location = new Point(130, yPos - 3), Width = 280, Font = new Font("Segoe UI", 9F), Text = existing?.version ?? string.Empty, PlaceholderText = "Ejemplo: 6.0.98.48" };
            yPos += 40;

            var lblChecksum = new Label { Text = "🔐 checksum:", Location = new Point(20, yPos), Width = 100, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(31, 41, 55) };
            var txtChecksum = new TextBox { Name = "txtChecksum", Location = new Point(130, yPos - 3), Width = 280, Font = new Font("Segoe UI", 9F), Text = existing?.checksum ?? string.Empty, PlaceholderText = "SHA256 hash en minúsculas" };
            yPos += 40;

            var lblFilesize = new Label { Text = "📏 filesize:", Location = new Point(20, yPos), Width = 100, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(31, 41, 55) };
            var txtFilesize = new TextBox { Name = "txtFilesize", Location = new Point(130, yPos - 3), Width = 280, Font = new Font("Segoe UI", 9F), Text = existing?.filesize.ToString() ?? string.Empty, PlaceholderText = "Ejemplo: 165888 (números solamente)" };
            yPos += 60;

            var btnOk = new Button { Text = existing == null ? "✅ Agregar" : "✅ Guardar", Location = new Point(130, yPos), Width = 120, Height = 40, BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F, FontStyle.Bold), DialogResult = DialogResult.OK };
            btnOk.FlatAppearance.BorderSize = 0;
            var btnCancel = new Button { Text = "❌ Cancelar", Location = new Point(260, yPos), Width = 120, Height = 40, BackColor = Color.FromArgb(107, 114, 128), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F, FontStyle.Bold), DialogResult = DialogResult.Cancel };
            btnCancel.FlatAppearance.BorderSize = 0;

            form.Controls.AddRange(new Control[] { lblFilename, txtFilename, lblClsId, txtClsId, lblVersion, txtVersion, lblChecksum, txtChecksum, lblFilesize, txtFilesize, btnOk, btnCancel });
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;
            return form;
        }

        public static Form CrearFormularioLibreria(string title, TypeLibExporter_NET8.LibraryInfo? existing)
        {
            var form = new Form
            {
                Text = title,
                Size = new Size(450, 380),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            int yPos = 30;

            var lblFilename = new Label { Text = "📄 filename:", Location = new Point(20, yPos), Width = 100, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(31, 41, 55) };
            var txtFilename = new TextBox { Name = "txtFilename", Location = new Point(130, yPos - 3), Width = 280, Font = new Font("Segoe UI", 9F), Text = existing?.filename ?? string.Empty, PlaceholderText = "Ejemplo: MSVBVM60.DLL (OBLIGATORIO)" };
            yPos += 40;

            var lblTypeLib = new Label { Text = "🔗 type_lib:", Location = new Point(20, yPos), Width = 100, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(31, 41, 55) };
            var txtTypeLib = new TextBox { Name = "txtTypeLib", Location = new Point(130, yPos - 3), Width = 280, Font = new Font("Segoe UI", 9F), Text = existing?.type_lib ?? string.Empty, PlaceholderText = "Ejemplo: {GUID}\\version\\lcid\\win32\\" };
            yPos += 40;

            var lblVersion = new Label { Text = "🏷️ version:", Location = new Point(20, yPos), Width = 100, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(31, 41, 55) };
            var txtVersion = new TextBox { Name = "txtVersion", Location = new Point(130, yPos - 3), Width = 280, Font = new Font("Segoe UI", 9F), Text = existing?.version ?? string.Empty, PlaceholderText = "Ejemplo: 6.0.98.48" };
            yPos += 40;

            var lblChecksum = new Label { Text = "🔐 checksum:", Location = new Point(20, yPos), Width = 100, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(31, 41, 55) };
            var txtChecksum = new TextBox { Name = "txtChecksum", Location = new Point(130, yPos - 3), Width = 280, Font = new Font("Segoe UI", 9F), Text = existing?.checksum ?? string.Empty, PlaceholderText = "SHA256 hash en minúsculas" };
            yPos += 40;

            var lblFilesize = new Label { Text = "📏 filesize:", Location = new Point(20, yPos), Width = 100, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(31, 41, 55) };
            var txtFilesize = new TextBox { Name = "txtFilesize", Location = new Point(130, yPos - 3), Width = 280, Font = new Font("Segoe UI", 9F), Text = existing?.filesize.ToString() ?? string.Empty, PlaceholderText = "Ejemplo: 1436032 (números solamente)" };
            yPos += 60;

            var btnOk = new Button { Text = existing == null ? "✅ Agregar" : "✅ Guardar", Location = new Point(130, yPos), Width = 120, Height = 40, BackColor = Color.FromArgb(16, 185, 129), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F, FontStyle.Bold), DialogResult = DialogResult.OK };
            btnOk.FlatAppearance.BorderSize = 0;
            var btnCancel = new Button { Text = "❌ Cancelar", Location = new Point(260, yPos), Width = 120, Height = 40, BackColor = Color.FromArgb(107, 114, 128), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10F, FontStyle.Bold), DialogResult = DialogResult.Cancel };
            btnCancel.FlatAppearance.BorderSize = 0;

            form.Controls.AddRange(new Control[] { lblFilename, txtFilename, lblTypeLib, txtTypeLib, lblVersion, txtVersion, lblChecksum, txtChecksum, lblFilesize, txtFilesize, btnOk, btnCancel });
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;
            return form;
        }

        public static TypeLibExporter_NET8.SimpleClsIdInfo ObtenerClsIdDesdeFormulario(Form form)
        {
            var txtFilename = form.Controls.Find("txtFilename", true).FirstOrDefault() as TextBox;
            var txtClsId = form.Controls.Find("txtClsId", true).FirstOrDefault() as TextBox;
            var txtVersion = form.Controls.Find("txtVersion", true).FirstOrDefault() as TextBox;
            var txtChecksum = form.Controls.Find("txtChecksum", true).FirstOrDefault() as TextBox;
            var txtFilesize = form.Controls.Find("txtFilesize", true).FirstOrDefault() as TextBox;

            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(txtFilename?.Text)) errors.Add("• El campo 'filename' es obligatorio y no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(txtClsId?.Text)) errors.Add("• El campo 'clsid' es obligatorio y no puede estar vacío.");

            long filesize = 0;
            if (!string.IsNullOrWhiteSpace(txtFilesize?.Text) && !long.TryParse(txtFilesize.Text.Trim(), out filesize))
            {
                errors.Add("• El campo 'filesize' debe ser un número válido.");
            }
            if (errors.Count > 0) throw new ArgumentException("Por favor corrige los siguientes errores:\n\n" + string.Join("\n", errors));

            return new TypeLibExporter_NET8.SimpleClsIdInfo
            {
                filename = txtFilename?.Text?.Trim() ?? string.Empty,
                clsid = txtClsId?.Text?.Trim() ?? string.Empty,
                version = txtVersion?.Text?.Trim() ?? string.Empty,
                checksum = txtChecksum?.Text?.Trim() ?? string.Empty,
                filesize = filesize
            };
        }

        public static TypeLibExporter_NET8.LibraryInfo ObtenerLibreriaDesdeFormulario(Form form)
        {
            var txtFilename = form.Controls.Find("txtFilename", true).FirstOrDefault() as TextBox;
            var txtTypeLib = form.Controls.Find("txtTypeLib", true).FirstOrDefault() as TextBox;
            var txtVersion = form.Controls.Find("txtVersion", true).FirstOrDefault() as TextBox;
            var txtChecksum = form.Controls.Find("txtChecksum", true).FirstOrDefault() as TextBox;
            var txtFilesize = form.Controls.Find("txtFilesize", true).FirstOrDefault() as TextBox;

            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(txtFilename?.Text)) errors.Add("• El campo 'filename' es obligatorio y no puede estar vacío.");

            long filesize = 0;
            if (!string.IsNullOrWhiteSpace(txtFilesize?.Text) && !long.TryParse(txtFilesize.Text.Trim(), out filesize))
            {
                errors.Add("• El campo 'filesize' debe ser un número válido.");
            }
            if (errors.Count > 0) throw new ArgumentException("Por favor corrige los siguientes errores:\n\n" + string.Join("\n", errors));

            return new TypeLibExporter_NET8.LibraryInfo
            {
                filename = txtFilename?.Text?.Trim() ?? string.Empty,
                type_lib = txtTypeLib?.Text?.Trim() ?? string.Empty,
                version = txtVersion?.Text?.Trim() ?? string.Empty,
                checksum = txtChecksum?.Text?.Trim() ?? string.Empty,
                filesize = filesize
            };
        }
    }
}
