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
                lblStatus.Text = ClaseInicial.Textos.EstadoEscaneandoTypeLibs;
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
                    lblStatus.Text = ClaseInicial.Textos.EstadoProcesandoArchivos;
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

                lblStatus.Text = string.Format(ClaseInicial.Textos.ExportadoTypeLibsEstado, libraries.Count);
                MessageBox.Show(
                    string.Format(ClaseInicial.Textos.ExportTypeLibsOkCuerpo, fullPath, libraries.Count),
                    ClaseInicial.Textos.ExportTypeLibsOkTitulo,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                lblStatus.Text = ClaseInicial.Textos.ErrorPrefijo + "durante la exportación";
                MessageBox.Show(ClaseInicial.Textos.ErrorPrefijo + ex.Message, ClaseInicial.Textos.ErrorExportacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                lblStatus.Text = ClaseInicial.Textos.EstadoEscaneandoClsids;
                lstResults.Items.Clear();

                var clsids = await Task.Run(() => BuscarClsIdsEnRegistro());

                this.Invoke((MethodInvoker)delegate
                {
                    lblStatus.Text = ClaseInicial.Textos.EstadoProcesandoClsids;
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

                lblStatus.Text = string.Format(ClaseInicial.Textos.ExportadoClsidsEstado, clsids.Count);
                MessageBox.Show(
                    string.Format(ClaseInicial.Textos.ExportClsidsOkCuerpo, fullPath, clsids.Count),
                    ClaseInicial.Textos.ExportClsidsOkTitulo,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                lblStatus.Text = ClaseInicial.Textos.ErrorPrefijo + "durante la exportación de CLSIDs";
                MessageBox.Show(ClaseInicial.Textos.ErrorPrefijo + ex.Message, ClaseInicial.Textos.ErrorExportacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                lblStatus.Text = ClaseInicial.Textos.EstadoEscaneandoCombinado;
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
                    lblStatus.Text = ClaseInicial.Textos.EstadoGenerandoCombinado;
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

                lblStatus.Text = string.Format(ClaseInicial.Textos.ExportadoCombinadoEstado, libraries.Count, clsids.Count);
                MessageBox.Show(
                    string.Format(ClaseInicial.Textos.ExportCombinadoOkCuerpo, fullPath, libraries.Count, clsids.Count, libraries.Count + clsids.Count),
                    ClaseInicial.Textos.ExportCombinadoOkTitulo,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                lblStatus.Text = ClaseInicial.Textos.ErrorPrefijo + "durante la exportación combinada";
                MessageBox.Show(ClaseInicial.Textos.ErrorPrefijo + ex.Message, ClaseInicial.Textos.ErrorExportacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
