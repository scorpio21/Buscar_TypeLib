using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using TypeLibExporter_NET8.Clases;
using TypeLibExporter_NET8.Servicios;

namespace TypeLibExporter_NET8
{
    public partial class Principal
    {
        private void AjustarAlturaLista()
        {
            var paddingInferior = panelMain.Padding.Bottom;
            var espacioDisponible = panelMain.ClientSize.Height - lstResults.Top - paddingInferior;
            if (espacioDisponible < 380) espacioDisponible = 380;
            lstResults.Height = espacioDisponible;
        }

        private void LoadSavedLocation()
        {
            try
            {
                if (File.Exists(settingsFile))
                {
                    string json = File.ReadAllText(settingsFile);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json);

                    if (!string.IsNullOrEmpty(settings?.LastSaveLocation) && Directory.Exists(settings.LastSaveLocation))
                    {
                        txtLocation.Text = settings.LastSaveLocation;
                        return;
                    }
                }
            }
            catch
            {
                // Si hay error leyendo configuración, usar default
            }

            txtLocation.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        private void SaveLocation()
        {
            try
            {
                var settings = new AppSettings { LastSaveLocation = txtLocation.Text };
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(settingsFile, json);
            }
            catch
            {
                // Ignorar errores de guardado
            }
        }

        private string GetFileVersion(string filePath)
        {
            return ArchivoUtil.ObtenerVersionArchivo(filePath);
        }

        private string GetFileChecksum(string filePath)
        {
            return ArchivoUtil.CalcularChecksum(filePath);
        }

        private long GetFileSize(string filePath)
        {
            return ArchivoUtil.ObtenerTamanio(filePath);
        }

        private string FormatFileSize(long bytes)
        {
            return ArchivoUtil.FormatearTamanio(bytes);
        }

        private void Principal_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveLocation();
        }
    }
}
