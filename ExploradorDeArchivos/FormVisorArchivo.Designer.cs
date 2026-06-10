namespace ExploradorDeArchivos
{
    partial class FormVisorArchivo
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel   panelToolbar;
        private System.Windows.Forms.Panel   panelContenido;
        private System.Windows.Forms.Button  btnGuardar;
        private System.Windows.Forms.Button  btnAbrirCon;
        private System.Windows.Forms.Button  btnCopiarRuta;
        private System.Windows.Forms.Button  btnAjustarColumnas;
        private System.Windows.Forms.Button  btnAlternarWrap;
        private System.Windows.Forms.Button  btnFuenteGrid;
        private System.Windows.Forms.ToolTip toolTipVis;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components         = new System.ComponentModel.Container();
            toolTipVis         = new System.Windows.Forms.ToolTip(components);
            panelToolbar       = new System.Windows.Forms.Panel();
            panelContenido     = new System.Windows.Forms.Panel();
            btnGuardar         = new System.Windows.Forms.Button();
            btnAbrirCon        = new System.Windows.Forms.Button();
            btnCopiarRuta      = new System.Windows.Forms.Button();
            btnAjustarColumnas = new System.Windows.Forms.Button();
            btnAlternarWrap    = new System.Windows.Forms.Button();
            btnFuenteGrid      = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // ── Formulario ────────────────────────────────────────────────
            this.ClientSize    = new System.Drawing.Size(1100, 700);
            this.MinimumSize   = new System.Drawing.Size(700, 500);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor     = System.Drawing.Color.White;
            this.Font          = new System.Drawing.Font("Segoe UI", 9F);

            // ── panelToolbar ──────────────────────────────────────────────
            panelToolbar.Dock      = System.Windows.Forms.DockStyle.Top;
            panelToolbar.Height    = 44;
            panelToolbar.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            panelToolbar.Padding   = new System.Windows.Forms.Padding(6, 6, 6, 6);

            var botones = new (System.Windows.Forms.Button btn, string texto, bool visible)[]
            {
                (btnGuardar,         "💾  Guardar",         false),
                (btnAbrirCon,        "📂  Abrir con…",      false),
                (btnCopiarRuta,      "📋  Copiar ruta",     true),
                (btnAjustarColumnas, "⇔  Ajustar cols.",   false),
                (btnAlternarWrap,    "↵  Ajustar líneas",  false),
                (btnFuenteGrid,      "🔤  Fuente",          false),
            };

            int x = 8;
            foreach (var (btn, texto, visible) in botones)
            {
                btn.Text      = texto;
                btn.Height    = 30;
                btn.Width     = texto.Contains("Guardar") ? 110 : 140;
                btn.Left      = x;
                btn.Top       = 7;
                btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btn.FlatAppearance.BorderColor        = System.Drawing.Color.FromArgb(255, 255, 255, 60);
                btn.FlatAppearance.BorderSize         = 1;
                btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(96, 165, 250);
                btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(29, 78, 216);
                btn.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
                btn.ForeColor = System.Drawing.Color.White;
                btn.Font      = new System.Drawing.Font("Segoe UI", 8.5F);
                btn.Cursor    = System.Windows.Forms.Cursors.Hand;
                btn.Visible   = visible;
                panelToolbar.Controls.Add(btn);
                x += btn.Width + 6;
            }

            // Guardar: verde para destacar
            btnGuardar.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            btnGuardar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(34, 197, 94);

            btnGuardar.Click         += BtnGuardar_Click;
            btnAbrirCon.Click        += BtnAbrirCon_Click;
            btnCopiarRuta.Click      += BtnCopiarRuta_Click;
            btnAjustarColumnas.Click += BtnAjustarColumnas_Click;
            btnAlternarWrap.Click    += BtnAlternarWrap_Click;
            btnFuenteGrid.Click      += BtnFuenteGrid_Click;

            toolTipVis.SetToolTip(btnGuardar,         "Guardar cambios (Ctrl+S)");
            toolTipVis.SetToolTip(btnCopiarRuta,      "Copiar ruta completa al portapapeles");
            toolTipVis.SetToolTip(btnAjustarColumnas, "Ajustar ancho de columnas al contenido");
            toolTipVis.SetToolTip(btnAlternarWrap,    "Activar / desactivar ajuste de líneas");
            toolTipVis.SetToolTip(btnFuenteGrid,      "Cambiar fuente");

            // Ctrl+S para guardar
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == System.Windows.Forms.Keys.S)
                { e.Handled = true; Guardar(); }
            };

            // ── panelContenido ────────────────────────────────────────────
            panelContenido.Dock      = System.Windows.Forms.DockStyle.Fill;
            panelContenido.BackColor = System.Drawing.Color.White;

            this.Controls.Add(panelContenido);
            this.Controls.Add(panelToolbar);

            this.ResumeLayout(false);
        }
    }
}


