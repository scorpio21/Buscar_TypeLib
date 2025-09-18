using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TypeLibExporter_NET8.Clases
{
    /// <summary>
    /// Utilidades de estilo para WinForms: botones redondeados, hover/down y divisores.
    /// Mantiene el código no intrusivo (sin modificar el diseñador).
    /// </summary>
    public static class EstilosUI
    {
        /// <summary>
        /// Aplica esquinas redondeadas y colores de hover/pressed a un botón.
        /// </summary>
        public static void AplicarBotonRedondeado(Button btn, int radio = 8, Color? hover = null, Color? pressed = null)
        {
            if (btn == null) return;

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.UseVisualStyleBackColor = false;

            var baseColor = btn.BackColor;
            var hoverColor = hover ?? Oscurecer(baseColor, 0.08f);
            var pressedColor = pressed ?? Oscurecer(baseColor, 0.16f);

            // Región redondeada
            void applyRegion()
            {
                if (btn.IsDisposed || btn.Disposing) return;
                try
                {
                    var old = btn.Region;
                    btn.Region = new Region(CrearPathRedondeado(btn.ClientRectangle, radio));
                    if (old != null && !ReferenceEquals(old, btn.Region)) old.Dispose();
                }
                catch { }
            }
            btn.Resize += (s, e) => applyRegion();
            applyRegion();

            // Hover / Down / Leave
            void onEnter(object? s, EventArgs e)
            {
                if (btn.IsDisposed || btn.Disposing) return;
                btn.BackColor = hoverColor;
            }
            void onLeave(object? s, EventArgs e)
            {
                if (btn.IsDisposed || btn.Disposing) return;
                btn.BackColor = baseColor;
            }
            void onDown(object? s, MouseEventArgs e)
            {
                if (btn.IsDisposed || btn.Disposing) return;
                if (e.Button == MouseButtons.Left) btn.BackColor = pressedColor;
            }
            void onUp(object? s, MouseEventArgs e)
            {
                if (btn.IsDisposed || btn.Disposing) return;
                try
                {
                    var pos = btn.PointToClient(Control.MousePosition);
                    btn.BackColor = btn.ClientRectangle.Contains(pos) ? hoverColor : baseColor;
                }
                catch { btn.BackColor = baseColor; }
            }

            btn.MouseEnter += onEnter;
            btn.MouseLeave += onLeave;
            btn.MouseDown += onDown;
            btn.MouseUp += onUp;

            // Limpieza segura al destruir el control
            btn.Disposed += (s, e) =>
            {
                try
                {
                    btn.MouseEnter -= onEnter;
                    btn.MouseLeave -= onLeave;
                    btn.MouseDown -= onDown;
                    btn.MouseUp -= onUp;
                    var old = btn.Region;
                    if (old != null) old.Dispose();
                    btn.Region = null;
                }
                catch { }
            };
        }

        /// <summary>
        /// Dibuja una línea divisoria inferior (1px) en un control (por ejemplo el header).
        /// </summary>
        public static void AplicarDivisorInferior(Control ctrl, Color? color = null)
        {
            if (ctrl == null) return;
            var divider = color ?? Color.FromArgb(220, 230, 245);
            ctrl.Paint += (s, e) =>
            {
                using var pen = new Pen(divider, 1);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawLine(pen, 0, ctrl.Height - 1, ctrl.Width, ctrl.Height - 1);
            };
            ctrl.Invalidate();
        }

        /// <summary>
        /// Aplica sombra interna sutil al panel (borde difuminado hacia adentro).
        /// </summary>
        public static void AplicarSombraInterna(Panel panel)
        {
            if (panel == null) return;
            panel.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = panel.ClientRectangle;
                // Reducir para no pintar sobre el borde
                rect.Inflate(-1, -1);
                using var brush = new LinearGradientBrush(new Rectangle(0, 0, rect.Width, 8),
                    Color.FromArgb(40, 0, 0, 0), Color.Transparent, LinearGradientMode.Vertical);
                // Superior
                g.FillRectangle(brush, new Rectangle(rect.Left, rect.Top, rect.Width, 8));
                // Inferior (invertido)
                using var brush2 = new LinearGradientBrush(new Rectangle(0, 0, rect.Width, 8),
                    Color.Transparent, Color.FromArgb(40, 0, 0, 0), LinearGradientMode.Vertical);
                g.FillRectangle(brush2, new Rectangle(rect.Left, rect.Bottom - 8, rect.Width, 8));
                // Izquierda/Derecha sombras leves
                using var left = new LinearGradientBrush(new Rectangle(0, 0, 8, rect.Height),
                    Color.FromArgb(30, 0, 0, 0), Color.Transparent, LinearGradientMode.Horizontal);
                g.FillRectangle(left, new Rectangle(rect.Left, rect.Top, 8, rect.Height));
                using var right = new LinearGradientBrush(new Rectangle(0, 0, 8, rect.Height),
                    Color.Transparent, Color.FromArgb(30, 0, 0, 0), LinearGradientMode.Horizontal);
                g.FillRectangle(right, new Rectangle(rect.Right - 8, rect.Top, 8, rect.Height));
            };
            panel.Invalidate();
        }

        private static GraphicsPath CrearPathRedondeado(Rectangle r, int radio)
        {
            int d = radio * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static Color Oscurecer(Color c, float amount)
        {
            amount = Math.Max(0f, Math.Min(1f, amount));
            int r = (int)(c.R * (1f - amount));
            int g = (int)(c.G * (1f - amount));
            int b = (int)(c.B * (1f - amount));
            return Color.FromArgb(c.A, r, g, b);
        }
    }
}
