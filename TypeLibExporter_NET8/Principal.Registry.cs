using System.Collections.Generic;
using System.Linq;
using TypeLibExporter_NET8.Servicios;

namespace TypeLibExporter_NET8
{
    public partial class Principal
    {
        // Métodos de escaneo de TypeLib con filtrado
        private List<LibraryInfo> BuscarEnRegistro()
        {
            // Delegar en el servicio centralizado
            return RegistroScanner.BuscarEnRegistro();
        }

        private List<LibraryInfo> BuscarEnRegistroFiltrado(List<string> preferidos)
        {
            var todas = BuscarEnRegistro(); // Ya viene filtrado
            var filtradas = new List<LibraryInfo>();

            foreach (var pref in preferidos)
            {
                // Validar que el nombre preferido sea .dll o .ocx
                if (!IsValidComponentFile(pref)) continue;

                var encontrado = todas.FirstOrDefault(r => r.filename.Equals(pref, System.StringComparison.OrdinalIgnoreCase));
                filtradas.Add(encontrado ?? new LibraryInfo
                {
                    filename = pref,
                    type_lib = "Not Found in Registry",
                    version = "Not Found",
                    checksum = "File not accessible or missing",
                    filesize = 0
                });
            }
            return filtradas;
        }

        // Escaneo de CLSIDs
        private List<SimpleClsIdInfo> BuscarClsIdsEnRegistro()
        {
            return RegistroScanner.BuscarClsIdsEnRegistro();
        }
    }
}
