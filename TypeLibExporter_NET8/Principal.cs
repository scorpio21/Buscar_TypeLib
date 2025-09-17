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
            
            // Ubicación por defecto
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

        #endregion

        #region Menu Event Handlers

        private void CargarJsonMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using var openFileDialog = new OpenFileDialog
                {
                    Title = ClaseInicial.Textos.SeleccionarJsonTitulo,
                    Filter = ClaseInicial.Textos.FiltroArchivoJson,
                    FilterIndex = 1,
                    InitialDirectory = txtLocation.Text
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string jsonContent = File.ReadAllText(openFileDialog.FileName);
                    
                    // Validar que sea JSON válido
                    try
                    {
                        JsonDocument.Parse(jsonContent);
                        
                        var jsonViewer = new ListarJson(jsonContent, Path.GetFileName(openFileDialog.FileName));
                        jsonViewer.Show();
                    }
                    catch (JsonException)
                    {
                        MessageBox.Show(
                            ClaseInicial.Textos.JsonInvalidoMensaje,
                            ClaseInicial.Textos.JsonInvalidoTitulo,
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

        #region Export Functions

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

        // ✅ MÉTODO CORREGIDO PARA CLSIDS
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
                // ✅ VALIDAR que el nombre preferido sea .dll o .ocx
                if (!IsValidComponentFile(pref)) continue;
                
                var encontrado = todas.FirstOrDefault(r => r.filename.Equals(pref, StringComparison.OrdinalIgnoreCase));
                if (encontrado != null)
                {
                    filtradas.Add(encontrado);
                }
                else
                {
                    filtradas.Add(new LibraryInfo 
                    { 
                        filename = pref,
                        type_lib = "Not Found in Registry",
                        version = "Not Found",
                        checksum = "File not accessible or missing",
                        filesize = 0
                    });
                }
            }

            return filtradas;
        }

        #endregion

        #region CLSID Scanning Methods - COMPLETAMENTE REESCRITO

        // ✅ MÉTODO COMPLETAMENTE CORREGIDO PARA CLSIDS
        private List<SimpleClsIdInfo> BuscarClsIdsEnRegistro()
        {
            // Delegar en el servicio centralizado
            return RegistroScanner.BuscarClsIdsEnRegistro();
        }

        #endregion

        #region Helper Functions

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

        #endregion

        private void Principal_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveLocation();
        }
    }

}
