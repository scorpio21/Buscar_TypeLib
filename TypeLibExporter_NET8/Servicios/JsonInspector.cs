using System.Text.Json;

namespace TypeLibExporter_NET8.Servicios
{
    /// <summary>
    /// Servicio para detección de tipo de JSON y deserialización a listas de objetos.
    /// </summary>
    public static class JsonInspector
    {
        public class Resultado
        {
            public List<object> Items { get; set; } = new();
            public bool EsClsId { get; set; }
            public string SufijoTitulo { get; set; } = string.Empty;
            public bool EsCombinadoAmbos { get; set; }
            public List<TypeLibExporter_NET8.LibraryInfo> TypeLibs { get; set; } = new();
            public List<TypeLibExporter_NET8.SimpleClsIdInfo> Clsids { get; set; } = new();
        }

        /// <summary>
        /// Inspecciona el contenido JSON y determina su tipo, devolviendo una lista de items y metadatos.
        /// </summary>
        public static Resultado Inspeccionar(string jsonContent, string fileName)
        {
            var resultado = new Resultado();
            string safeJson = jsonContent ?? string.Empty;
            bool containsClsid = safeJson.ToLowerInvariant().Contains("\"clsid\":");
            bool containsTypeLib = safeJson.ToLowerInvariant().Contains("\"type_lib\":");

            // 1) Lista de CLSIDs
            if (containsClsid && !containsTypeLib)
            {
                try
                {
                    var clsidList = JsonSerializer.Deserialize<List<TypeLibExporter_NET8.SimpleClsIdInfo>>(safeJson);
                    if (clsidList?.Count > 0)
                    {
                        resultado.Items = clsidList.Cast<object>().ToList();
                        resultado.EsClsId = true;
                        resultado.SufijoTitulo = "(CLSIDs)";
                        return resultado;
                    }
                }
                catch { }
            }

            // 2) Lista de TypeLibs
            if (containsTypeLib && !containsClsid)
            {
                try
                {
                    var libraryList = JsonSerializer.Deserialize<List<TypeLibExporter_NET8.LibraryInfo>>(safeJson);
                    if (libraryList?.Count > 0)
                    {
                        resultado.Items = libraryList.Cast<object>().ToList();
                        resultado.EsClsId = false;
                        resultado.SufijoTitulo = "(TypeLibs)";
                        return resultado;
                    }
                }
                catch { }
            }

            // 3) Combinado ComponentInfo
            try
            {
                var componentInfo = JsonSerializer.Deserialize<TypeLibExporter_NET8.ComponentInfo>(safeJson);
                if (componentInfo != null)
                {
                    int countCls = componentInfo.clsids?.Count ?? 0;
                    int countLib = componentInfo.typelibs?.Count ?? 0;
                    if (countCls > 0 && countLib > 0)
                    {
                        resultado.EsCombinadoAmbos = true;
                        resultado.TypeLibs = componentInfo.typelibs ?? new();
                        resultado.Clsids = componentInfo.clsids ?? new();
                        resultado.SufijoTitulo = "(Combinado)";
                        return resultado;
                    }
                    if (countCls > 0)
                    {
                        resultado.Items = componentInfo.clsids!.Cast<object>().ToList();
                        resultado.EsClsId = true;
                        resultado.SufijoTitulo = "(CLSIDs del Combinado)";
                        return resultado;
                    }
                    if (countLib > 0)
                    {
                        resultado.Items = componentInfo.typelibs!.Cast<object>().ToList();
                        resultado.EsClsId = false;
                        resultado.SufijoTitulo = "(TypeLibs del Combinado)";
                        return resultado;
                    }
                }
            }
            catch { }

            // 4) Fallback a listas simples
            try
            {
                var libraryList = JsonSerializer.Deserialize<List<TypeLibExporter_NET8.LibraryInfo>>(safeJson);
                if (libraryList?.Count > 0)
                {
                    resultado.Items = libraryList.Cast<object>().ToList();
                    resultado.EsClsId = false;
                    resultado.SufijoTitulo = "(TypeLibs - Fallback)";
                    return resultado;
                }
            }
            catch { }

            try
            {
                var clsidList = JsonSerializer.Deserialize<List<TypeLibExporter_NET8.SimpleClsIdInfo>>(safeJson);
                if (clsidList?.Count > 0)
                {
                    resultado.Items = clsidList.Cast<object>().ToList();
                    resultado.EsClsId = true;
                    resultado.SufijoTitulo = "(CLSIDs - Fallback)";
                    return resultado;
                }
            }
            catch { }

            // 5) Si todo falla, devolver vacío para que el consumidor maneje el error
            return resultado;
        }
    }
}
