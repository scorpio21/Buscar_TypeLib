using Microsoft.Win32;

namespace TypeLibExporter_NET8.Servicios
{
    /// <summary>
    /// Servicio para escaneo del registro de Windows y obtención de TypeLibs y CLSIDs.
    /// </summary>
    public static class RegistroScanner
    {
        /// <summary>
        /// Busca TypeLibs en el registro filtrando únicamente archivos .dll y .ocx.
        /// </summary>
        public static List<TypeLibExporter_NET8.LibraryInfo> BuscarEnRegistro()
        {
            var results = new Dictionary<string, TypeLibExporter_NET8.LibraryInfo>();

            var registryBases = new (RegistryHive hive, RegistryView view)[]
            {
                (RegistryHive.ClassesRoot, RegistryView.Default),
                (RegistryHive.LocalMachine, RegistryView.Registry64),
                (RegistryHive.LocalMachine, RegistryView.Registry32)
            };

            foreach (var (hive, view) in registryBases)
            {
                try
                {
                    using var baseKey = RegistryKey.OpenBaseKey(hive, view);
                    using var typeLibRoot = baseKey.OpenSubKey("TypeLib");
                    if (typeLibRoot == null) continue;

                    foreach (var guid in typeLibRoot.GetSubKeyNames())
                    {
                        try
                        {
                            using var guidKey = typeLibRoot.OpenSubKey(guid);
                            if (guidKey == null) continue;

                            foreach (var version in guidKey.GetSubKeyNames())
                            {
                                try
                                {
                                    using var versionKey = guidKey.OpenSubKey(version);
                                    if (versionKey == null) continue;

                                    foreach (var lcid in versionKey.GetSubKeyNames())
                                    {
                                        try
                                        {
                                            using var lcidKey = versionKey.OpenSubKey(lcid);
                                            if (lcidKey == null) continue;

                                            using var win32Key = lcidKey.OpenSubKey("win32");
                                            if (win32Key == null) continue;

                                            var filePath = win32Key.GetValue(null) as string;
                                            if (string.IsNullOrEmpty(filePath)) continue;

                                            string fileName = Path.GetFileName(filePath).ToUpperInvariant();

                                            if (!ArchivoUtil.EsComponenteValido(fileName)) continue;
                                            if (results.ContainsKey(fileName)) continue;

                                            var lib = new TypeLibExporter_NET8.LibraryInfo
                                            {
                                                filename = fileName,
                                                type_lib = $"{{{guid}}}\\{version}\\{lcid}\\win32\\",
                                                version = ArchivoUtil.ObtenerVersionArchivo(filePath),
                                                checksum = ArchivoUtil.CalcularChecksum(filePath),
                                                filesize = ArchivoUtil.ObtenerTamanio(filePath)
                                            };

                                            results[fileName] = lib;
                                        }
                                        catch { }
                                    }
                                }
                                catch { }
                            }
                        }
                        catch { }
                    }
                }
                catch { }
            }

            return results.Values.OrderBy(r => r.filename).ToList();
        }

        /// <summary>
        /// Busca CLSIDs en el registro y devuelve solo los que apuntan a archivos .dll/.ocx existentes.
        /// </summary>
        public static List<TypeLibExporter_NET8.SimpleClsIdInfo> BuscarClsIdsEnRegistro()
        {
            var results = new Dictionary<string, TypeLibExporter_NET8.SimpleClsIdInfo>();

            var registryBases = new (RegistryHive hive, RegistryView view)[]
            {
                (RegistryHive.ClassesRoot, RegistryView.Default),
                (RegistryHive.LocalMachine, RegistryView.Registry64),
                (RegistryHive.LocalMachine, RegistryView.Registry32)
            };

            foreach (var (hive, view) in registryBases)
            {
                try
                {
                    using var baseKey = RegistryKey.OpenBaseKey(hive, view);
                    using var clsidRoot = baseKey.OpenSubKey("CLSID");
                    if (clsidRoot == null) continue;

                    foreach (var clsidGuid in clsidRoot.GetSubKeyNames())
                    {
                        try
                        {
                            using var clsidKey = clsidRoot.OpenSubKey(clsidGuid);
                            if (clsidKey == null) continue;

                            if (results.ContainsKey(clsidGuid)) continue;

                            var name = clsidKey.GetValue("") as string ?? string.Empty;
                            if (string.IsNullOrWhiteSpace(name)) continue;

                            string serverPath = string.Empty;
                            string serverType = string.Empty;

                            using var inprocKey = clsidKey.OpenSubKey("InprocServer32");
                            if (inprocKey != null)
                            {
                                serverPath = inprocKey.GetValue("") as string ?? string.Empty;
                                serverType = "InprocServer32";
                            }

                            if (string.IsNullOrEmpty(serverPath))
                            {
                                using var localKey = clsidKey.OpenSubKey("LocalServer32");
                                if (localKey != null)
                                {
                                    serverPath = localKey.GetValue("") as string ?? string.Empty;
                                    serverType = "LocalServer32";
                                }
                            }

                            if (string.IsNullOrEmpty(serverPath)) continue;

                            string filename;
                            try
                            {
                                string cleanPath = serverPath.Split(' ')[0].Trim('"');
                                filename = Path.GetFileName(cleanPath).ToUpperInvariant();
                            }
                            catch { continue; }

                            if (!ArchivoUtil.EsComponenteValido(filename)) continue;

                            string cleanServerPath = serverPath.Split(' ')[0].Trim('"');
                            if (!File.Exists(cleanServerPath)) continue;

                            var clsidInfo = new TypeLibExporter_NET8.SimpleClsIdInfo
                            {
                                filename = filename,
                                clsid = $"{clsidGuid}\\{serverType}",
                                version = ArchivoUtil.ObtenerVersionArchivo(cleanServerPath),
                                checksum = ArchivoUtil.CalcularChecksum(cleanServerPath),
                                filesize = ArchivoUtil.ObtenerTamanio(cleanServerPath)
                            };

                            results[clsidGuid] = clsidInfo;
                        }
                        catch { }
                    }
                }
                catch { }
            }

            return results.Values.OrderBy(r => r.filename).ToList();
        }
    }
}
