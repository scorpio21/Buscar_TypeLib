using System.Linq;
using TypeLibExporter_NET8.Clases;
using TypeLibExporter_NET8.Servicios;

namespace TypeLibExporter_NET8
{
    public partial class ListarJson
    {
        // Eventos de búsqueda
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void OnSearchTimerTick(object? sender, EventArgs e)
        {
            searchTimer.Stop();
            string searchTerm = txtSearch.Text ?? string.Empty;
            itemsList = BusquedaJson.Filtrar(originalItemsList, searchTerm, isClsIdData);
            RefreshItemsList();
            UpdateSearchResultsInfo();
        }

        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            txtSearch.Focus();
        }

        // Selección y edición
        private void LstLibraries_DoubleClick(object sender, EventArgs e)
        {
            if (lstLibraries.SelectedIndex == -1) return;
            EditSelectedItem();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (lstLibraries.SelectedIndex == -1)
            {
                MessageBox.Show(ClaseInicial.Textos.SeleccionRequeridaEditar, "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            EditSelectedItem();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (lstLibraries.SelectedIndex == -1)
            {
                MessageBox.Show(ClaseInicial.Textos.SeleccionRequeridaEliminar, "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("✅ Elemento eliminado exitosamente!", "Eliminación Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        if (newIndex >= 0) lstLibraries.SelectedIndex = newIndex;
                        MessageBox.Show("✅ CLSID agregado exitosamente!", "CLSID Agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        if (newIndex >= 0) lstLibraries.SelectedIndex = newIndex;
                        MessageBox.Show("✅ Librería agregada exitosamente!", "Entrada Agregada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Error al agregar la librería:\n\n{ex.Message}", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(txtJsonDisplay.Text);
                MessageBox.Show(ClaseInicial.Textos.CopiadoOk, "Copiado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ClaseInicial.Textos.CopiadoError}\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using var saveFileDialog = new SaveFileDialog
                {
                    Title = ClaseInicial.Textos.GuardarJsonTitulo,
                    Filter = ClaseInicial.Textos.FiltroArchivoJson,
                    FilterIndex = 1,
                    FileName = fileName
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    object dataToSave = isClsIdData
                        ? originalItemsList.Cast<SimpleClsIdInfo>().ToList()
                        : originalItemsList.Cast<LibraryInfo>().ToList();
                    var jsonToSave = JsonSerializerHelper.SerializeIndented(dataToSave);
                    File.WriteAllText(saveFileDialog.FileName, jsonToSave);
                    string itemType = isClsIdData ? "CLSIDs" : "TypeLibs";
                    MessageBox.Show($"💾 {itemType} guardados exitosamente en:\n{saveFileDialog.FileName}", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ClaseInicial.Textos.ErrorGuardarArchivo}\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.TopLevel)
                {
                    var outer = this.TopLevelControl as ListarJsonCombinado
                               ?? this.Parent?.TopLevelControl as ListarJsonCombinado;
                    if (outer != null)
                    {
                        outer.Close();
                        return;
                    }
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
            Close();
        }

        private void ListarJson_Shown(object? sender, EventArgs e)
        {
            if (openCombinedPending)
            {
                try
                {
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
