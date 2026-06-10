using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    /// <summary>
    /// Centraliza toda la lógica de estilo visual compartida entre los formularios.
    /// Los forms sólo llaman métodos de esta clase; no contienen código de diseño propio.
    /// </summary>
    internal static class UIStyler
    {
        // ══════════════════════════════════════════════════════════════════════
        //  SIDEBAR (Form1)
        // ══════════════════════════════════════════════════════════════════════

        public static void AplicarSeccionSidebar(Label lbl, string texto)
        {
            lbl.Text      = texto;
            lbl.Font      = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lbl.ForeColor = AppColors.SidebarSectionText;
            lbl.BackColor = Color.Transparent;
        }

        public static void AplicarEtiquetaSeccionSidebar(Label lbl, string texto)
        {
            lbl.Text      = texto;
            lbl.Font      = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lbl.ForeColor = AppColors.SidebarSectionText;
            lbl.BackColor = Color.Transparent;
            lbl.TextAlign = ContentAlignment.BottomLeft;
            lbl.Padding   = new Padding(10, 0, 0, 2);
        }

        public static void AplicarItemSidebar(Button btn, string texto, bool esNavegable)
        {
            btn.Text      = texto;
            btn.Size      = new Size(190, 30);
            btn.FlatStyle = FlatStyle.Flat;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding   = new Padding(8, 0, 0, 0);
            btn.Font      = new Font("Segoe UI", 9.5F);
            btn.ForeColor = AppColors.SidebarItemText;
            btn.BackColor = Color.Transparent;
            btn.Cursor    = esNavegable ? Cursors.Hand : Cursors.Default;
            btn.FlatAppearance.BorderSize         = 0;
            btn.FlatAppearance.MouseOverBackColor = AppColors.SidebarHover;
            btn.FlatAppearance.MouseDownBackColor = AppColors.SidebarPressed;
        }

        public static void AplicarToolButtonSidebar(Button btn, string texto,
            Color hoverColor, Color pressedColor)
        {
            btn.Text      = texto;
            btn.FlatStyle = FlatStyle.Flat;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding   = new Padding(8, 0, 0, 0);
            btn.Font      = new Font("Segoe UI", 9.5F);
            btn.ForeColor = AppColors.SidebarItemText;
            btn.BackColor = Color.Transparent;
            btn.Cursor    = Cursors.Hand;
            btn.FlatAppearance.BorderSize         = 0;
            btn.FlatAppearance.MouseOverBackColor = hoverColor;
            btn.FlatAppearance.MouseDownBackColor = pressedColor;
        }

        public static void AplicarSeparadorSidebar(Label lbl)
        {
            lbl.BackColor = AppColors.SidebarSeparator;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  TARJETAS / CARDS (FormLimpiezaDatos)
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Dibuja un borde coloreado alrededor de un Panel-card.</summary>
        public static void PintarBordeTarjeta(Panel card, Color color, PaintEventArgs e)
        {
            using var pen = new Pen(Color.FromArgb(40, color.R, color.G, color.B), 1);
            e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
        }

        public static void AplicarLabelTituloCard(Label lbl, Color color)
        {
            lbl.Font      = new Font("Segoe UI", 16F, FontStyle.Bold);
            lbl.ForeColor = color;
        }

        public static void AplicarLabelSubtituloCard(Label lbl)
        {
            lbl.Font      = new Font("Segoe UI", 7.5F);
            lbl.ForeColor = AppColors.TextoSec;
        }

        public static void AplicarLabelColumna(Label lbl)
        {
            lbl.ForeColor = AppColors.TextoSec;
            lbl.Font      = new Font("Segoe UI", 8F);
        }

        public static void AplicarComboBoxOscuro(ComboBox cbo)
        {
            cbo.BackColor = AppColors.BgOscuro;
            cbo.ForeColor = AppColors.TextoPrim;
            cbo.Font      = new Font("Segoe UI", 8.5F);
        }

        public static void AplicarLabelDescTipo(Label lbl, Color color)
        {
            lbl.ForeColor = color;
            lbl.Font      = new Font("Segoe UI", 7.5F);
        }

        public static void AplicarLabelAdvertencia(Label lbl)
        {
            lbl.ForeColor = AppColors.Naranja;
            lbl.Font      = new Font("Segoe UI", 7F);
            lbl.BackColor = Color.FromArgb(28, 10, 0);
        }

        public static void AplicarLabelApiKey(Label lbl)
        {
            lbl.ForeColor = Color.FromArgb(250, 204, 21);
            lbl.Font      = new Font("Segoe UI", 8.5F);
            lbl.BackColor = Color.Transparent;
            lbl.Padding   = new Padding(4, 0, 0, 0);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  GRID (FormVisorArchivo / FormLimpiezaDatos)
        // ══════════════════════════════════════════════════════════════════════

        public static void AplicarEstiloGridTabla(DataGridView grid)
        {
            grid.Font                        = new Font("Consolas", 9F);
            grid.BackgroundColor             = Color.White;
            grid.GridColor                   = Color.FromArgb(220, 220, 220);
            grid.BorderStyle                 = BorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 99, 235);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 9F, FontStyle.Bold);

            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 255);
            grid.DefaultCellStyle.Padding             = new Padding(4, 1, 4, 1);
            grid.DefaultCellStyle.SelectionBackColor  = Color.FromArgb(190, 214, 255);
            grid.DefaultCellStyle.SelectionForeColor  = Color.Black;
        }

        public static void AplicarEstiloGridOscuro(DataGridViewColumn col)
        {
            col.DefaultCellStyle = new DataGridViewCellStyle
            {
                ForeColor = AppColors.Borde,
                Font      = new Font("Segoe UI", 8F, FontStyle.Bold)
            };
        }

        // ══════════════════════════════════════════════════════════════════════
        //  VISOR (FormVisorArchivo)
        // ══════════════════════════════════════════════════════════════════════

        public static Label CrearLabelInfo(string texto) => new Label
        {
            Dock        = DockStyle.Top,
            Height      = 28,
            Text        = "  " + texto,
            Font        = new Font("Segoe UI", 8.5F),
            ForeColor   = Color.FromArgb(80, 80, 80),
            BackColor   = Color.FromArgb(240, 243, 250),
            BorderStyle = BorderStyle.FixedSingle,
            TextAlign   = ContentAlignment.MiddleLeft,
            Padding     = new Padding(4, 0, 0, 0),
        };

        public static void AplicarEstiloTextEditor(RichTextBox rtb, bool esCodigo = false)
        {
            if (esCodigo)
            {
                rtb.Font      = new Font("Consolas", 10F);
                rtb.BackColor = Color.FromArgb(30, 30, 30);
                rtb.ForeColor = Color.FromArgb(212, 212, 212);
            }
            else
            {
                rtb.BackColor = Color.FromArgb(245, 246, 250);
            }
        }

        public static void AplicarEstiloLabelError(Label lbl, string texto)
        {
            lbl.Font      = new Font("Segoe UI", 10F);
            lbl.ForeColor = Color.FromArgb(180, 30, 30);
            lbl.Text      = $"❌  Error al abrir el archivo:\n\n{texto}";
        }

        public static void AplicarEstiloLabelSinSoporte(Label lbl)
        {
            lbl.Font      = new Font("Segoe UI", 11F);
            lbl.ForeColor = Color.FromArgb(100, 100, 100);
        }

        public static void AplicarEstiloTabPpt(TabPage tab)
        {
            tab.BackColor = Color.FromArgb(248, 249, 252);
            tab.Padding   = new Padding(8);
        }

        public static void AplicarEstiloRtbTituloPpt(RichTextBox rtb)
        {
            rtb.Font        = new Font("Segoe UI", 14F, FontStyle.Bold);
            rtb.ForeColor   = Color.FromArgb(37, 99, 235);
            rtb.BackColor   = Color.FromArgb(248, 249, 252);
            rtb.BorderStyle = BorderStyle.None;
        }

        public static void AplicarEstiloRtbCuerpoPpt(RichTextBox rtb)
        {
            rtb.Font        = new Font("Segoe UI", 11F);
            rtb.ForeColor   = Color.FromArgb(40, 40, 40);
            rtb.BackColor   = Color.FromArgb(248, 249, 252);
            rtb.BorderStyle = BorderStyle.None;
        }

        public static void AplicarEstiloSeparadorPpt(Label sep)
        {
            sep.BackColor = Color.FromArgb(37, 99, 235);
        }

        public static void AplicarEstiloWordParagrafo(Control lbl, bool esTitulo, bool esSubtitulo)
        {
            lbl.BackColor = Color.FromArgb(245, 246, 250);
            lbl.Font = esTitulo    ? new Font("Segoe UI", 12F, FontStyle.Bold)
                     : esSubtitulo ? new Font("Segoe UI", 10F, FontStyle.Bold)
                                   : new Font("Segoe UI", 10F);
            lbl.ForeColor = Color.FromArgb(30, 30, 30);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PLAYER MP3  (FormPlayerMP3)
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Aplica el color de acento al tema del reproductor.</summary>
        public static void AplicarTemaReproductor(Color accent,
            Button btnPlayPause, Control lblAccentBar, Control lblSpotifyBadge,
            Button[] botonesSecundarios)
        {
            btnPlayPause.BackColor    = accent;
            btnPlayPause.ForeColor    = Color.Black;
            lblAccentBar.BackColor    = accent;
            lblSpotifyBadge.ForeColor = accent;

            foreach (var btn in botonesSecundarios)
            {
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(28, accent.R, accent.G, accent.B);
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(50, accent.R, accent.G, accent.B);
            }
        }

        public static void AplicarEstadoBotonToggle(Button btn, bool activo, Color accent)
        {
            btn.ForeColor = activo ? accent : AppColors.PlayerTextSec;
            btn.Font      = new Font(btn.Font, activo ? FontStyle.Bold : FontStyle.Regular);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  AYUDANTES GRAFICOS  (usados por varios forms)
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Crea un GraphicsPath con esquinas redondeadas.</summary>
        public static GraphicsPath CrearRectRedondeado(Rectangle r, int radio)
        {
            var p = new GraphicsPath();
            p.AddArc(r.Left,           r.Top,            radio * 2, radio * 2, 180, 90);
            p.AddArc(r.Right - radio*2, r.Top,            radio * 2, radio * 2, 270, 90);
            p.AddArc(r.Right - radio*2, r.Bottom - radio*2, radio * 2, radio * 2, 0,   90);
            p.AddArc(r.Left,           r.Bottom - radio*2, radio * 2, radio * 2, 90,  90);
            p.CloseFigure();
            return p;
        }

        /// <summary>Crea un GraphicsPath con esquinas redondeadas (versión float).</summary>
        public static GraphicsPath CrearRectRedondeadoF(RectangleF r, float radio)
        {
            var p = new GraphicsPath();
            p.AddArc(r.Left,             r.Top,               radio*2, radio*2, 180, 90);
            p.AddArc(r.Right - radio*2,  r.Top,               radio*2, radio*2, 270, 90);
            p.AddArc(r.Right - radio*2,  r.Bottom - radio*2,  radio*2, radio*2, 0,   90);
            p.AddArc(r.Left,             r.Bottom - radio*2,  radio*2, radio*2, 90,  90);
            p.CloseFigure();
            return p;
        }
    }
}



