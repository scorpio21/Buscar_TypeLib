using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using TypeLibExporter_NET8.Clases;
using TypeLibExporter_NET8.Servicios;

namespace TypeLibExporter_NET8
{
    public partial class Principal
    {
        private async Task ExportLibraries(bool onlyPreferred)
        {
            try
            {
                btnExportTypeLibs.Enabled = false;
                btnExportVB6.Enabled = false;
                btnExportClsIds.Enabled = false;
                btnExportCombined.Enabled = false;
                progressBar.Style = ProgressBarStyle.Marquee;
                lblStatus.Text = "🔍 Escaneando TypeLibs en el registro...";
                lstResults.Items.Clear();

                var libraries = await Task.Run(() =>
                {
                    if (onlyPreferred)
                    {
                        var preferidas = new List<string>
                        {
                            "MSVBVM60.DLL", "MSCOMCTL.OCX", "MSINET.OCX", "MSWINSCK.OCX",
                            "COMDLG32.OCX", "COMCTL32.OCX", "DX8VB.DLL", "RICHTX32.OCX",
                            "VBALPROGBAR6.OCX", "MBPrgBar.OCX", "MSADO15.DLL", "MSADODC.OCX",
                            "MSDATGRD.OCX", "MSHFLXGD.OCX"
                        };
                        return BuscarEnRegistroFiltrado(preferidas);
                    }
                    else
                    {
                        return BuscarEnRegistro();
                    }
                });

                this.Invoke((MethodInvoker)delegate
                {
                    lblStatus.Text = "📊 Procesando información de archivos...";
                    foreach (var lib in libraries)
                    {
                        string displayText = lib.filesize > 0 
                            ? $"✅ {lib.filename} - v{lib.version} ({ArchivoUtil.FormatearTamanio(lib.filesize)})"
                            : $"⚠️ {lib.filename} - {lib.version}";
                        lstResults.Items.Add(displayText);
                    }
                });

                string fileName = onlyPreferred ? "librerias-vb6-clasicas.json" : "librerias-typelibs.json";
                string fullPath = Path.Combine(txtLocation.Text, fileName);
                
                var jsonOptions = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                
                var json = JsonSerializer.Serialize(libraries, jsonOptions);
                await File.WriteAllTextAsync(fullPath, json);

                SaveLocation();

                lblStatus.Text = $"✅ TypeLibs exportados: {libraries.Count} librerías encontradas (solo DLL/OCX)";
                MessageBox.Show(
                    $"🎉 TypeLibs exportados exitosamente!\n\n" +
                    $"📁 Archivo: {fullPath}\n" +
                    $"📊 Librerías procesadas: {libraries.Count}\n" +
                    $"🔧 Filtrado: Solo archivos .DLL y .OCX\n" +
                    $"💡 Tip: Puedes cargar este JSON usando Archivo → Utilidades → Cargar JSON",
                    "Exportación Completada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                lblStatus.Text = "❌ Error durante la exportación";
                MessageBox.Show($"⚠️ Error: {ex.Message}", "Error de Exportación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 0;
                btnExportTypeLibs.Enabled = true;
                btnExportVB6.Enabled = true;
                btnExportClsIds.Enabled = true;
                btnExportCombined.Enabled = true;
            }
        }

        private async Task ExportClsIds(bool onlyPreferred)
        {
            try
            {
                btnExportTypeLibs.Enabled = false;
                btnExportVB6.Enabled = false;
                btnExportClsIds.Enabled = false;
                btnExportCombined.Enabled = false;
                progressBar.Style = ProgressBarStyle.Marquee;
                lblStatus.Text = "🔍 Escaneando CLSIDs con archivos válidos...";
                lstResults.Items.Clear();

                var clsids = await Task.Run(() => BuscarClsIdsEnRegistro());

                this.Invoke((MethodInvoker)delegate
                {
                    lblStatus.Text = "📊 Procesando información de CLSIDs...";
                    foreach (var clsid in clsids)
                    {
                        string displayText = clsid.filesize > 0 
                            ? $"✅ {clsid.filename} ({clsid.clsid}) - v{clsid.version} ({ArchivoUtil.FormatearTamanio(clsid.filesize)})"
                            : $"⚠️ {clsid.filename} ({clsid.clsid}) - {clsid.version}";
                        lstResults.Items.Add(displayText);
                    }
                });

                string fileName = "clsids-limpios-dll-ocx.json";
                string fullPath = Path.Combine(txtLocation.Text, fileName);
                
                var jsonOptions = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                
                var json = JsonSerializer.Serialize(clsids, jsonOptions);
                await File.WriteAllTextAsync(fullPath, json);

                SaveLocation();

                lblStatus.Text = $"✅ CLSIDs exportados: {clsids.Count} archivos válidos encontrados";
                MessageBox.Show(
                    $"🎉 CLSIDs exportados exitosamente!\n\n" +
                    $"📁 Archivo: {fullPath}\n" +
                    $"📊 CLSIDs procesados: {clsids.Count}\n" +
                    $"🔧 Formato: Limpio y simplificado\n" +
                    $"✨ Solo archivos .DLL y .OCX que existen en el sistema",
                    "Exportación de CLSIDs Completada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                lblStatus.Text = "❌ Error durante la exportación de CLSIDs";
                MessageBox.Show($"⚠️ Error: {ex.Message}", "Error de Exportación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 0;
                btnExportTypeLibs.Enabled = true;
                btnExportVB6.Enabled = true;
                btnExportClsIds.Enabled = true;
                btnExportCombined.Enabled = true;
            }
        }

        private async Task ExportCombined(bool onlyPreferred)
        {
            try
            {
                btnExportTypeLibs.Enabled = false;
                btnExportVB6.Enabled = false;
                btnExportClsIds.Enabled = false;
                btnExportCombined.Enabled = false;
                progressBar.Style = ProgressBarStyle.Marquee;
                lblStatus.Text = "🔍 Escaneando TypeLibs y CLSIDs...";
                lstResults.Items.Clear();

                var (libraries, clsids) = await Task.Run(() =>
                {
                    var libs = BuscarEnRegistro();
                    var cls = BuscarClsIdsEnRegistro();
                    return (libs, cls);
                });

                var combinedData = new ComponentInfo
                {
                    typelibs = libraries,
                    clsids = clsids,
                    exported_at = DateTime.Now,
                    exported_by = "TypeLib Exporter - .NET 8 (Filtrado DLL/OCX)"
                };

                this.Invoke((MethodInvoker)delegate
                {
                    lblStatus.Text = "📊 Generando archivo combinado...";
                    lstResults.Items.Add($"📚 TypeLibs encontradas: {libraries.Count}");
                    lstResults.Items.Add($"🔧 CLSIDs encontrados: {clsids.Count}");
                    lstResults.Items.Add($"⏰ Exportado: {combinedData.exported_at:yyyy-MM-dd HH:mm:ss}");
                    lstResults.Items.Add($"🔧 Filtro aplicado: Solo archivos .DLL/.OCX válidos");
                });

                string fileName = "componentes-dll-ocx-completos.json";
                string fullPath = Path.Combine(txtLocation.Text, fileName);
                
                var jsonOptions = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                
                var json = JsonSerializer.Serialize(combinedData, jsonOptions);
                await File.WriteAllTextAsync(fullPath, json);

                SaveLocation();

                lblStatus.Text = $"✅ Exportación combinada: {libraries.Count} TypeLibs + {clsids.Count} CLSIDs (filtrados)";
                MessageBox.Show(
                    $"🎉 Componentes exportados exitosamente!\n\n" +
                    $"📁 Archivo: {fullPath}\n" +
                    $"📚 TypeLibs: {libraries.Count}\n" +
                    $"🔧 CLSIDs: {clsids.Count}\n" +
                    $"💾 Total: {libraries.Count + clsids.Count}\n" +
                    $"🔧 Filtrado: Solo archivos .DLL/.OCX válidos",
                    "Exportación Combinada Completada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                lblStatus.Text = "❌ Error durante la exportación combinada";
                MessageBox.Show($"⚠️ Error: {ex.Message}", "Error de Exportación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 0;
                btnExportTypeLibs.Enabled = true;
                btnExportVB6.Enabled = true;
                btnExportClsIds.Enabled = true;
                btnExportCombined.Enabled = true;
            }
        }
    }
}
