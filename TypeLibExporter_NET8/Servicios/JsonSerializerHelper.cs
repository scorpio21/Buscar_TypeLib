using System.Text.Json;

namespace TypeLibExporter_NET8.Servicios
{
    /// <summary>
    /// Helper para serialización JSON con opciones consistentes.
    /// </summary>
    public static class JsonSerializerHelper
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static string SerializeIndented(object data)
        {
            return JsonSerializer.Serialize(data, Options);
        }
    }
}
