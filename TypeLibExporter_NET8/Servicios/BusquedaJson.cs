using System.Drawing;

namespace TypeLibExporter_NET8.Servicios
{
    /// <summary>
    /// Servicio para búsqueda y filtrado de listas en el visor JSON.
    /// </summary>
    public static class BusquedaJson
    {
        public class InfoResultados
        {
            public string Texto { get; set; } = string.Empty;
            public Color Color { get; set; } = Color.FromArgb(107, 114, 128);
        }

        public static List<object> Filtrar(List<object> originalItemsList, string searchTerm, bool esClsId)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<object>(originalItemsList);

            string term = searchTerm.ToLowerInvariant().Trim();

            return originalItemsList.Where(item =>
            {
                if (esClsId && item is TypeLibExporter_NET8.SimpleClsIdInfo c)
                {
                    return (c.filename?.ToLowerInvariant().Contains(term) ?? false)
                        || (c.version?.ToLowerInvariant().Contains(term) ?? false)
                        || (c.clsid?.ToLowerInvariant().Contains(term) ?? false);
                }
                else if (!esClsId && item is TypeLibExporter_NET8.LibraryInfo l)
                {
                    return (l.filename?.ToLowerInvariant().Contains(term) ?? false)
                        || (l.version?.ToLowerInvariant().Contains(term) ?? false)
                        || (l.type_lib?.ToLowerInvariant().Contains(term) ?? false);
                }
                return false;
            }).ToList();
        }

        public static InfoResultados ObtenerInfoResultados(string searchTerm, int count, int total, bool esClsId)
        {
            var info = new InfoResultados();
            string itemType = esClsId ? "CLSIDs" : "librerías";

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                info.Texto = $"📋 Mostrando {count} {itemType}";
                info.Color = Color.FromArgb(107, 114, 128);
            }
            else
            {
                info.Texto = $"🔍 {count} de {total} {itemType} encontradas";
                info.Color = count > 0 ? Color.FromArgb(16, 185, 129) : Color.FromArgb(239, 68, 68);
            }
            return info;
        }
    }
}
