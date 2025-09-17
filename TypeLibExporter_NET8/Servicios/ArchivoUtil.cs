using System.Diagnostics;
using System.Security.Cryptography;

namespace TypeLibExporter_NET8.Servicios
{
    /// <summary>
    /// Utilidades de manejo de archivos y formateo.
    /// </summary>
    public static class ArchivoUtil
    {
        /// <summary>
        /// Verifica si el nombre de archivo representa un componente válido (.dll o .ocx).
        /// </summary>
        public static bool EsComponenteValido(string? filename)
        {
            if (string.IsNullOrWhiteSpace(filename)) return false;
            var extension = Path.GetExtension(filename).ToLowerInvariant();
            return extension == ".dll" || extension == ".ocx";
        }

        /// <summary>
        /// Obtiene la versión de un archivo si existe.
        /// </summary>
        public static string ObtenerVersionArchivo(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return "File not found";
                var versionInfo = FileVersionInfo.GetVersionInfo(filePath);
                return !string.IsNullOrEmpty(versionInfo.FileVersion) ? versionInfo.FileVersion : "No version info";
            }
            catch
            {
                return "Version unavailable";
            }
        }

        /// <summary>
        /// Calcula el hash SHA256 del archivo.
        /// </summary>
        public static string CalcularChecksum(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return "File not found";
                using var sha256 = SHA256.Create();
                using var stream = File.OpenRead(filePath);
                var hash = sha256.ComputeHash(stream);
                return Convert.ToHexString(hash).ToLowerInvariant();
            }
            catch
            {
                return "Checksum unavailable";
            }
        }

        /// <summary>
        /// Obtiene el tamaño del archivo en bytes.
        /// </summary>
        public static long ObtenerTamanio(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return 0;
                return new FileInfo(filePath).Length;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Formatea un tamaño en bytes a una cadena legible.
        /// </summary>
        public static string FormatearTamanio(long bytes)
        {
            if (bytes == 0) return "0 B";
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double size = bytes;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return $"{size:0.##} {sizes[order]}";
        }
    }
}
