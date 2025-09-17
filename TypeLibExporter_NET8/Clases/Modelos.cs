namespace TypeLibExporter_NET8
{
    /// <summary>
    /// Configuración simple de la aplicación.
    /// </summary>
    public class AppSettings
    {
        public string LastSaveLocation { get; set; } = string.Empty;
    }

    /// <summary>
    /// Modelo de una librería TypeLib exportada.
    /// </summary>
    public class LibraryInfo
    {
        public string filename { get; set; } = string.Empty;
        public string type_lib { get; set; } = string.Empty;
        public string version { get; set; } = string.Empty;
        public string checksum { get; set; } = string.Empty;
        public long filesize { get; set; }
    }

    /// <summary>
    /// Modelo simplificado para entradas de CLSID con metadatos de archivo.
    /// </summary>
    public class SimpleClsIdInfo
    {
        public string filename { get; set; } = string.Empty;
        public string clsid { get; set; } = string.Empty;
        public string version { get; set; } = string.Empty;
        public string checksum { get; set; } = string.Empty;
        public long filesize { get; set; }
    }

    /// <summary>
    /// Modelo combinado que agrupa TypeLibs y CLSIDs exportados.
    /// </summary>
    public class ComponentInfo
    {
        public List<LibraryInfo> typelibs { get; set; } = new();
        public List<SimpleClsIdInfo> clsids { get; set; } = new();
        public DateTime exported_at { get; set; } = DateTime.Now;
        public string exported_by { get; set; } = "TypeLib Exporter - .NET 8";
    }
}
