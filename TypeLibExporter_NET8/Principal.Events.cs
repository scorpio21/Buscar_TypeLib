using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace TypeLibExporter_NET8
{
    public partial class Principal
    {
        #region Menu Event Handlers
        private void CargarJsonMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using var openFileDialog = new OpenFileDialog
                {
                    Title = Clases.ClaseInicial.Textos.SeleccionarJsonTitulo,
                    Filter = Clases.ClaseInicial.Textos.FiltroArchivoJson,
                    FilterIndex = 1,
                    InitialDirectory = txtLocation.Text
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string jsonContent = File.ReadAllText(openFileDialog.FileName);
                    try
                    {
                        JsonDocument.Parse(jsonContent);
                        var jsonViewer = new ListarJson(jsonContent, Path.GetFileName(openFileDialog.FileName));
                        jsonViewer.Show();
                    }
                    catch (JsonException)
                    {
                        MessageBox.Show(
                            Clases.ClaseInicial.Textos.JsonInvalidoMensaje,
                            Clases.ClaseInicial.Textos.JsonInvalidoTitulo,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"⚠️ Error al cargar el archivo JSON:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void CerrarMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Button Event Handlers
        private void BtnSelectLocation_Click(object sender, EventArgs e)
        {
            using var folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Selecciona la carpeta donde guardar los archivos JSON";
            folderDialog.SelectedPath = txtLocation.Text;

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                txtLocation.Text = folderDialog.SelectedPath;
                SaveLocation();
            }
        }

        private async void BtnExportTypeLibs_Click(object sender, EventArgs e)
        {
            await ExportLibraries(false);
        }

        private async void BtnExportVB6_Click(object sender, EventArgs e)
        {
            await ExportLibraries(true);
        }

        private async void BtnExportClsIds_Click(object sender, EventArgs e)
        {
            await ExportClsIds(false);
        }

        private async void BtnExportCombined_Click(object sender, EventArgs e)
        {
            await ExportCombined(false);
        }
        #endregion
    }
}
