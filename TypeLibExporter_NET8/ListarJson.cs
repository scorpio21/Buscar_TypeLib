using System.Text.Json;
using System.Linq;
using TypeLibExporter_NET8.Servicios;

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

        public ListarJson(string jsonContent, string fileName)
        {
            this.jsonContent = jsonContent ?? string.Empty; // ✅ PROTECCIÓN NULL
            this.fileName = fileName ?? "archivo.json"; // ✅ PROTECCIÓN NULL
            InitializeComponent();
            // Diferir operaciones que abren/cerran formularios a Shown
            this.Shown += ListarJson_Shown;
            LoadJsonContent();
        }

        // ✅ MÉTODO CORREGIDO PARA DETECTAR TIPO CORRECTAMENTE (delegado a servicio)
        private void LoadJsonContent()
        {
            try
            {
                var resultado = JsonInspector.Inspeccionar(jsonContent, fileName);
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

        // ✅ MÉTODO DE BÚSQUEDA ADAPTADO PARA AMBOS TIPOS
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (sender is not TextBox textBox) return;
            
            string searchTerm = textBox.Text ?? string.Empty;
            itemsList = BusquedaJson.Filtrar(originalItemsList, searchTerm, isClsIdData);

            RefreshItemsList();
            UpdateSearchResultsInfo();
        }

        private void UpdateSearchResultsInfo()
        {
            var info = BusquedaJson.ObtenerInfoResultados(
                txtSearch.Text?.Trim() ?? string.Empty,
                itemsList.Count,
                originalItemsList.Count,
                isClsIdData);
            lblSearchResults.Text = info.Texto;
            lblSearchResults.ForeColor = info.Color;
        }

        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            txtSearch.Focus();
        }

        // ✅ MÉTODO ADAPTADO PARA AMBOS TIPOS
        private void RefreshItemsList()
        {
            try
            {
                lstLibraries.Items.Clear();
                
                foreach (var item in itemsList)
                {
                    string displayText = string.Empty;
                    
                    if (isClsIdData && item is SimpleClsIdInfo clsid)
                    {
                        displayText = $"{clsid.filename ?? "N/A"} - v{clsid.version ?? "N/A"} ({ArchivoUtil.FormatearTamanio(clsid.filesize)}) - {clsid.clsid ?? "N/A"}";
                    }
                    else if (!isClsIdData && item is LibraryInfo lib)
                    {
                        displayText = $"{lib.filename ?? "N/A"} - v{lib.version ?? "N/A"} ({ArchivoUtil.FormatearTamanio(lib.filesize)})";
                    }
                    
                    lstLibraries.Items.Add(displayText);
                }

                // ✅ SERIALIZAR SEGÚN EL TIPO
                object dataToSerialize = isClsIdData 
                    ? itemsList.Cast<SimpleClsIdInfo>().ToList() 
                    : itemsList.Cast<LibraryInfo>().ToList();

                var formattedJson = JsonSerializer.Serialize(dataToSerialize, new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
                
                txtJsonDisplay.Text = formattedJson;
                
                string itemType = isClsIdData ? "CLSIDs" : "TypeLibs";
                lblInfo.Text = $"📄 Archivo: {fileName} | 📊 {itemType}: {originalItemsList.Count} | 💾 Tamaño: {ArchivoUtil.FormatearTamanio(formattedJson.Length)}";
                UpdateSearchResultsInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la visualización:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LstLibraries_DoubleClick(object sender, EventArgs e)
        {
            if (lstLibraries.SelectedIndex == -1) return;
            EditSelectedItem();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (lstLibraries.SelectedIndex == -1)
            {
                string itemType = isClsIdData ? "CLSID" : "librería";
                MessageBox.Show($"⚠️ Selecciona un elemento de la lista para editar.", "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            EditSelectedItem();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (lstLibraries.SelectedIndex == -1)
            {
                string itemType = isClsIdData ? "CLSID" : "librería";
                MessageBox.Show($"⚠️ Selecciona un elemento de la lista para eliminar.", "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedIndex = lstLibraries.SelectedIndex;
            var selectedItem = itemsList[selectedIndex];
            
            string confirmText = string.Empty;
            if (isClsIdData && selectedItem is SimpleClsIdInfo clsid)
            {
                confirmText = $"¿Estás seguro de que deseas eliminar este CLSID?\n\n📄 Filename: {clsid.filename ?? "N/A"}\n🔧 CLSID: {clsid.clsid ?? "N/A"}";
            }
            else if (!isClsIdData && selectedItem is LibraryInfo lib)
            {
                confirmText = $"¿Estás seguro de que deseas eliminar esta librería?\n\n📄 Filename: {lib.filename ?? "N/A"}\n🏷️ Version: {lib.version ?? "N/A"}";
            }

            var result = MessageBox.Show(confirmText, "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                originalItemsList.Remove(selectedItem);
                itemsList.RemoveAt(selectedIndex);
                RefreshItemsList();
                
                string itemType = isClsIdData ? "CLSID" : "Librería";
                MessageBox.Show($"✅ {itemType} eliminado exitosamente!", "Eliminación Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ✅ MÉTODO ADAPTADO PARA AMBOS TIPOS
        private void EditSelectedItem()
        {
            var selectedIndex = lstLibraries.SelectedIndex;
            var selectedItem = itemsList[selectedIndex];

            if (isClsIdData && selectedItem is SimpleClsIdInfo clsid)
            {
                using var editForm = CreateClsIdForm("✏️ Editar CLSID", clsid);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var updatedClsId = GetClsIdFromForm(editForm);
                        
                        var originalIndex = originalItemsList.FindIndex(item => 
                            item is SimpleClsIdInfo c && c.filename == clsid.filename && c.clsid == clsid.clsid);
                        if (originalIndex >= 0)
                        {
                            originalItemsList[originalIndex] = updatedClsId;
                        }
                        
                        itemsList[selectedIndex] = updatedClsId;
                        RefreshItemsList();
                        lstLibraries.SelectedIndex = selectedIndex;

                        MessageBox.Show(
                            $"✅ CLSID editado exitosamente!\n\n📄 Filename: {updatedClsId.filename}\n🔧 CLSID: {updatedClsId.clsid}",
                            "Edición Completada",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Error al guardar los cambios:\n\n{ex.Message}", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (!isClsIdData && selectedItem is LibraryInfo lib)
            {
                using var editForm = CreateLibraryForm("✏️ Editar Librería", lib);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var updatedLib = GetLibraryFromForm(editForm);
                        
                        var originalIndex = originalItemsList.FindIndex(item => 
                            item is LibraryInfo l && l.filename == lib.filename && l.type_lib == lib.type_lib);
                        if (originalIndex >= 0)
                        {
                            originalItemsList[originalIndex] = updatedLib;
                        }
                        
                        itemsList[selectedIndex] = updatedLib;
                        RefreshItemsList();
                        lstLibraries.SelectedIndex = selectedIndex;

                        MessageBox.Show(
                            $"✅ Librería editada exitosamente!\n\n📄 Filename: {updatedLib.filename}\n🏷️ Version: {updatedLib.version}",
                            "Edición Completada",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Error al guardar los cambios:\n\n{ex.Message}", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnAddNew_Click(object sender, EventArgs e)
        {
            if (isClsIdData)
            {
                using var addForm = CreateClsIdForm("➕ Agregar Nuevo CLSID", null);
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var newClsId = GetClsIdFromForm(addForm);
                        
                        originalItemsList.Add(newClsId);
                        
                        string searchTerm = txtSearch.Text?.ToLowerInvariant().Trim() ?? string.Empty;
                        if (string.IsNullOrEmpty(searchTerm) ||
                            (newClsId.filename?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                            (newClsId.version?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                            (newClsId.clsid?.ToLowerInvariant().Contains(searchTerm) ?? false))
                        {
                            itemsList.Add(newClsId);
                        }
                        
                        RefreshItemsList();
                        
                        var newIndex = itemsList.FindIndex(item => 
                            item is SimpleClsIdInfo c && c.filename == newClsId.filename && c.clsid == newClsId.clsid);
                        if (newIndex >= 0)
                        {
                            lstLibraries.SelectedIndex = newIndex;
                        }

                        MessageBox.Show(
                            $"✅ CLSID agregado exitosamente!\n\n📄 Filename: {newClsId.filename}\n🔧 CLSID: {newClsId.clsid}",
                            "CLSID Agregado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Error al agregar el CLSID:\n\n{ex.Message}", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                using var addForm = CreateLibraryForm("➕ Agregar Nueva Librería", null);
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var newLib = GetLibraryFromForm(addForm);
                        
                        originalItemsList.Add(newLib);
                        
                        string searchTerm = txtSearch.Text?.ToLowerInvariant().Trim() ?? string.Empty;
                        if (string.IsNullOrEmpty(searchTerm) ||
                            (newLib.filename?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                            (newLib.version?.ToLowerInvariant().Contains(searchTerm) ?? false) ||
                            (newLib.type_lib?.ToLowerInvariant().Contains(searchTerm) ?? false))
                        {
                            itemsList.Add(newLib);
                        }
                        
                        RefreshItemsList();
                        
                        var newIndex = itemsList.FindIndex(item => 
                            item is LibraryInfo l && l.filename == newLib.filename && l.type_lib == newLib.type_lib);
                        if (newIndex >= 0)
                        {
                            lstLibraries.SelectedIndex = newIndex;
                        }

                        MessageBox.Show(
                            $"✅ Librería agregada exitosamente!\n\n📄 Filename: {newLib.filename}\n🏷️ Version: {newLib.version}",
                            "Entrada Agregada",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Error al agregar la librería:\n\n{ex.Message}", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // ✅ FORMULARIO PARA EDITAR CLSIDS
        private Form CreateClsIdForm(string title, SimpleClsIdInfo? existingClsId)
        {
            return JsonEditorUI.CrearFormularioClsId(title, existingClsId);
        }

        private Form CreateLibraryForm(string title, LibraryInfo? existingLib)
        {
            return JsonEditorUI.CrearFormularioLibreria(title, existingLib);
        }

        // ✅ VALIDACIÓN PARA CLSIDS
        private SimpleClsIdInfo GetClsIdFromForm(Form form)
        {
            return JsonEditorUI.ObtenerClsIdDesdeFormulario(form);
        }

        // ✅ VALIDACIÓN PARA TYPELIBS
        private LibraryInfo GetLibraryFromForm(Form form)
        {
            return JsonEditorUI.ObtenerLibreriaDesdeFormulario(form);
        }

        

        private void BtnClose_Click(object sender, EventArgs e)
        {
            // Si este formulario está embebido dentro de ListarJsonCombinado, cerrar el contenedor
            try
            {
                if (!this.TopLevel)
                {
                    // Intento 1: usar TopLevelControl
                    var outer = this.TopLevelControl as ListarJsonCombinado
                               ?? this.Parent?.TopLevelControl as ListarJsonCombinado;
                    if (outer != null)
                    {
                        outer.Close();
                        return;
                    }

                    // Intento 2: buscar en formularios abiertos
                    var combinado = Application.OpenForms
                        .OfType<ListarJsonCombinado>()
                        .FirstOrDefault();
                    if (combinado != null)
                    {
                        combinado.Close();
                        return;
                    }
                }
            }
            catch { }

            // Comportamiento normal
            Close();
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(txtJsonDisplay.Text);
                MessageBox.Show("📋 JSON copiado al portapapeles exitosamente!", "Copiado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al copiar al portapapeles:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using var saveFileDialog = new SaveFileDialog
                {
                    Title = "Guardar JSON como...",
                    Filter = "Archivos JSON (*.json)|*.json|Todos los archivos (*.*)|*.*",
                    FilterIndex = 1,
                    FileName = fileName
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // ✅ GUARDAR SEGÚN EL TIPO
                    object dataToSave = isClsIdData 
                        ? originalItemsList.Cast<SimpleClsIdInfo>().ToList() 
                        : originalItemsList.Cast<LibraryInfo>().ToList();

                    var jsonToSave = JsonSerializer.Serialize(dataToSave, new JsonSerializerOptions 
                    { 
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    });
                    
                    File.WriteAllText(saveFileDialog.FileName, jsonToSave);
                    
                    string itemType = isClsIdData ? "CLSIDs" : "TypeLibs";
                    MessageBox.Show($"💾 {itemType} guardados exitosamente en:\n{saveFileDialog.FileName}", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al guardar archivo:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListarJson_Shown(object? sender, EventArgs e)
        {
            if (openCombinedPending)
            {
                try
                {
                    // Cerrar cualquier visor combinado previo para evitar duplicados
                    var abiertos = Application.OpenForms
                        .OfType<ListarJsonCombinado>()
                        .ToList();
                    foreach (var frm in abiertos)
                    {
                        try { frm.Close(); } catch { }
                    }

                    var combinadoForm = new ListarJsonCombinado(pendingTypeLibs, pendingClsids, fileName);
                    combinadoForm.Show();
                    Close();
                }
                finally
                {
                    openCombinedPending = false;
                }
            }
        }
    }
}
