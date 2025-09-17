using System.Text.Json;
using System.Linq;
using TypeLibExporter_NET8.Servicios;

namespace TypeLibExporter_NET8
{
    public partial class ListarJson
    {
        // Renderiza la lista y el JSON según el tipo actual
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

                // Serializar según el tipo
                object dataToSerialize = isClsIdData 
                    ? itemsList.Cast<SimpleClsIdInfo>().ToList() 
                    : itemsList.Cast<LibraryInfo>().ToList();

                var formattedJson = JsonSerializerHelper.SerializeIndented(dataToSerialize);
                
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

        // Actualiza la etiqueta de resultados de búsqueda
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
    }
}
