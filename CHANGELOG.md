# Changelog

Todas las novedades relevantes de este proyecto se documentarán en este archivo.

## [v0.1.0Beta] - 2025-09-18

### Cambios destacados
- UI más moderna:
  - Botones con bordes redondeados, efectos hover/pressed y sombras sutiles en paneles.
  - Divisor inferior en el encabezado para mejor separación visual.
- Refactor de `Principal` en archivos parciales:
  - `Principal.Helpers.cs`, `Principal.Events.cs`, `Principal.Export.cs`.
- Cursores direccionales personalizados en botones:
  - Soporte para `puntero-left.cur` y `puntero-right.cur` (cambia según el movimiento).
- Estabilidad al cerrar:
  - Correcciones para evitar `ObjectDisposedException` en efectos de hover.
- Layout más robusto:
  - `lstResults` con altura mínima dinámica y anclajes.
- Textos de UI centralizados en `ClaseInicial.Textos`.

### Notas de desarrollo
- Se creó `Clases/EstilosUI.cs` para aplicar estilos de forma no intrusiva.
- Se migraron helpers de `Principal` a `Principal.Helpers.cs`.
- Se añadieron cursores personalizados (si los archivos `.cur` no existen, se hace fallback a `Cursors.Hand`).

### Próximos pasos sugeridos
- Reemplazar emojis por iconos PNG/SVG de 24 px en botones para un acabado más profesional.
- Crear `Principal.Registry.cs` si se desea aislar aún más las búsquedas.
- Añadir modo oscuro opcional.

