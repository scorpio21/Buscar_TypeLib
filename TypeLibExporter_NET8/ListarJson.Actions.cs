using TypeLibExporter_NET8.Servicios;

namespace TypeLibExporter_NET8
{
    public partial class ListarJson
    {
        // Editar elemento seleccionado (CLSID o Librería)
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

        private Form CreateClsIdForm(string title, SimpleClsIdInfo? existingClsId)
        {
            return JsonEditorUI.CrearFormularioClsId(title, existingClsId);
        }

        private Form CreateLibraryForm(string title, LibraryInfo? existingLib)
        {
            return JsonEditorUI.CrearFormularioLibreria(title, existingLib);
        }

        private SimpleClsIdInfo GetClsIdFromForm(Form form)
        {
            return JsonEditorUI.ObtenerClsIdDesdeFormulario(form);
        }

        private LibraryInfo GetLibraryFromForm(Form form)
        {
            return JsonEditorUI.ObtenerLibreriaDesdeFormulario(form);
        }
    }
}
