using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Drawing;
using System.Windows.Forms;
using TypeLibExporter_NET8.Servicios;

namespace TypeLibExporter_NET8.Clases
{
    /// <summary>
    /// Punto central de configuración e instancias compartidas.
    /// Esqueleto básico para ir migrando responsabilidades comunes.
    /// </summary>
    public static class ClaseInicial
    {
        // Textos de UI centralizados (esqueleto inicial)
        public static class Textos
        {
            public const string SeleccionRequeridaEditar = "⚠️ Selecciona un elemento de la lista para editar.";
            public const string SeleccionRequeridaEliminar = "⚠️ Selecciona un elemento de la lista para eliminar.";
            public const string GuardarJsonTitulo = "Guardar JSON como...";
            public const string FiltroArchivoJson = "Archivos JSON (*.json)|*.json|Todos los archivos (*.*)|*.*";
            public const string CopiadoOk = "📋 JSON copiado al portapapeles exitosamente!";
            public const string CopiadoError = "❌ Error al copiar al portapapeles:";
            public const string ErrorGuardarArchivo = "❌ Error al guardar archivo:";
            public const string ErrorVisualizacion = "Error al actualizar la visualización:";
            public const string JsonInvalidoTitulo = "Archivo JSON Inválido";
            public const string JsonInvalidoMensaje = "❌ El archivo seleccionado no contiene JSON válido.\n\nPor favor, selecciona un archivo JSON válido.";
            public const string SeleccionarJsonTitulo = "Seleccionar archivo JSON para visualizar";
            public const string EliminadoOk = "✅ Elemento eliminado exitosamente!";
        }
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
            /// <summary>Inspector/detección de JSON.</summary>
            public static ResultadoJson Inspect(string json, string file) =>
                new ResultadoJson(JsonInspector.Inspeccionar(json, file));

            /// <summary>Búsqueda/filtrado de listas.</summary>
            public static List<object> Filtrar(List<object> origen, string termino, bool esClsId) =>
                BusquedaJson.Filtrar(origen, termino, esClsId);
        }

        // Soporte de cursores direccionales (izquierda/derecha)
        public static class Cursores
        {
            private static Cursor? cursorLeft;
            private static Cursor? cursorRight;
            private static readonly Dictionary<Control, int> ultimoX = new();
            private static readonly Dictionary<object, int> ultimoXItems = new();

            private static void CargarSiNecesario()
            {
                if (cursorLeft == null)
                {
                    try { cursorLeft = CargarCursor("puntero-left.cur"); } catch { }
                }
                if (cursorRight == null)
                {
                    try { cursorRight = CargarCursor("puntero-right.cur"); } catch { }
                }
            }

            /// <summary>
            /// Aplica un cursor que cambia a izquierda/derecha según el movimiento horizontal del mouse.
            /// Si no existen los cursores, usa Cursors.Hand.
            /// </summary>
            public static void Aplicar(Control control)
            {
                if (control == null) return;
                CargarSiNecesario();

                var fallback = Cursors.Hand;
                control.Cursor = cursorRight ?? fallback;

                control.MouseMove -= OnMouseMove;
                control.MouseMove += OnMouseMove;
            }

            private static void OnMouseMove(object? sender, MouseEventArgs e)
            {
                if (sender is not Control ctrl) return;
                CargarSiNecesario();
                int lastX = 0;
                if (!ultimoX.TryGetValue(ctrl, out lastX)) lastX = e.X;
                // Determinar dirección
                if (e.X > lastX)
                {
                    ctrl.Cursor = cursorRight ?? Cursors.Hand;
                }
                else if (e.X < lastX)
                {
                    ctrl.Cursor = cursorLeft ?? Cursors.Hand;
                }
                ultimoX[ctrl] = e.X;
            }

            /// <summary>
            /// Aplica cursor direccional a elementos de menú (ToolStripItem). Cambia Cursor.Current.
            /// </summary>
            public static void Aplicar(ToolStripItem item)
            {
                if (item == null) return;
                CargarSiNecesario();
                item.MouseEnter -= OnItemMouseEnter;
                item.MouseEnter += OnItemMouseEnter;
                item.MouseMove -= OnItemMouseMove;
                item.MouseMove += OnItemMouseMove;
            }

            private static void OnItemMouseEnter(object? sender, EventArgs e)
            {
                CargarSiNecesario();
                Cursor.Current = cursorRight ?? Cursors.Hand;
            }

            private static void OnItemMouseMove(object? sender, MouseEventArgs e)
            {
                if (sender == null) return;
                CargarSiNecesario();
                int lastX = 0;
                if (!ultimoXItems.TryGetValue(sender, out lastX)) lastX = e.X;
                if (e.X > lastX)
                    Cursor.Current = cursorRight ?? Cursors.Hand;
                else if (e.X < lastX)
                    Cursor.Current = cursorLeft ?? Cursors.Hand;
                ultimoXItems[sender] = e.X;
            }
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
        /// Carga un cursor (.cur) desde la carpeta img/. Devuelve null si no existe o no es válido.
        /// </summary>
        public static Cursor? CargarCursor(string fileName)
        {
            try
            {
                var path = Path.Combine(Rutas.Img, fileName);
                if (File.Exists(path)) return new Cursor(path);
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
