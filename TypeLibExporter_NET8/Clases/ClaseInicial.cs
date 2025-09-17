using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Drawing;
using TypeLibExporter_NET8.Servicios;

namespace TypeLibExporter_NET8.Clases
{
    /// <summary>
    /// Punto central de configuración e instancias compartidas.
    /// Esqueleto básico para ir migrando responsabilidades comunes.
    /// </summary>
    public static class ClaseInicial
    {
        // Configuración global
        public static class Config
        {
            /// <summary>Versión de la aplicación (AssemblyInformationalVersion o FileVersion).</summary>
            public static readonly string Version =
                Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? "1.0.0";

            /// <summary>Cultura UI por defecto. Cambia a "es-ES" si quieres forzarla.</summary>
            public static readonly CultureInfo CulturaUi = CultureInfo.CurrentUICulture;

            /// <summary>Opciones JSON comunes (solo lectura).</summary>
            public static readonly JsonSerializerOptions JsonOpciones = new()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        // Rutas usadas por la aplicación
        public static class Rutas
        {
            /// <summary>Carpeta base del ejecutable.</summary>
            public static readonly string Base = AppContext.BaseDirectory;

            /// <summary>Carpeta de imágenes dentro del proyecto/publicación.</summary>
            public static readonly string Img = Path.Combine(Base, "img");

            /// <summary>Carpeta de datos por usuario en AppData.</summary>
            public static readonly string AppData = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TypeLibExporter");

            /// <summary>Carpeta por defecto para publicar/guardar.</summary>
            public static readonly string PublishDefault = Path.Combine(Base, "publish");
        }

        // Acceso simplificado a servicios
        public static class Servicios
        {
            /// <summary>Utilidades de archivos.</summary>
            public static ArchivoUtil Archivos => new ArchivoUtil();

            /// <summary>Inspector/detección de JSON.</summary>
            public static ResultadoJson Inspect(string json, string file) =>
                new ResultadoJson(JsonInspector.Inspeccionar(json, file));

            /// <summary>Búsqueda/filtrado de listas.</summary>
            public static List<object> Filtrar(List<object> origen, string termino, bool esClsId) =>
                BusquedaJson.Filtrar(origen, termino, esClsId);
        }

        // Wrapper pequeño para tipar mejor el resultado del inspector
        public readonly struct ResultadoJson
        {
            public readonly List<object> Items;
            public readonly bool EsClsId;
            public readonly bool EsCombinadoAmbos;
            public readonly List<LibraryInfo> TypeLibs;
            public readonly List<SimpleClsIdInfo> Clsids;
            public readonly string SufijoTitulo;

            public ResultadoJson(JsonInspector.Resultado r)
            {
                Items = r.Items;
                EsClsId = r.EsClsId;
                EsCombinadoAmbos = r.EsCombinadoAmbos;
                TypeLibs = r.TypeLibs;
                Clsids = r.Clsids;
                SufijoTitulo = r.SufijoTitulo;
            }
        }

        /// <summary>
        /// Inicialización mínima: crea carpetas necesarias y prepara cultura (si aplica).
        /// Llamar una vez al arrancar la app (por ejemplo, en Program.cs o en Principal()).
        /// </summary>
        public static void Inicializar()
        {
            try
            {
                Directory.CreateDirectory(Rutas.AppData);
                // Si quieres forzar cultura UI:
                // Thread.CurrentThread.CurrentUICulture = Config.CulturaUi;
            }
            catch
            {
                // Intencionalmente silencioso; en producción podrías loguear si añades logging.
            }
        }

        /// <summary>
        /// Carga un icono desde la carpeta img/ por nombre de archivo. Devuelve null si no existe.
        /// </summary>
        public static Icon? CargarIcono(string fileName)
        {
            try
            {
                var path = Path.Combine(Rutas.Img, fileName);
                if (File.Exists(path)) return new Icon(path);
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Resuelve una ruta dentro de la carpeta base del ejecutable de forma segura.
        /// </summary>
        public static string EnBase(params string[] partes)
        {
            var p = Rutas.Base;
            foreach (var parte in partes) p = Path.Combine(p, parte);
            return p;
        }
    }
}
