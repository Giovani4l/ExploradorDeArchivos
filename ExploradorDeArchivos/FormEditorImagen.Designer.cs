namespace ExploradorDeArchivos
{
    partial class FormEditorImagen
    {
        private System.ComponentModel.IContainer components = null;

        // Imagen
        private System.Windows.Forms.PictureBox picBox;

        // Tabs
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabEdicion;
        private System.Windows.Forms.TabPage tabMetadatos;
        private System.Windows.Forms.TabPage tabGPS;

        // Panel lateral edicion
        private System.Windows.Forms.Panel panelHerramientas;

        // Filtros
        private System.Windows.Forms.GroupBox grpFiltros;
        private System.Windows.Forms.Button btnFiltroOriginal;
        private System.Windows.Forms.Button btnFiltroGris;
        private System.Windows.Forms.Button btnFiltroSepia;
        private System.Windows.Forms.Button btnFiltroInvertir;
        private System.Windows.Forms.Button btnFiltroRojo;
        private System.Windows.Forms.Button btnFiltroVerde;
        private System.Windows.Forms.Button btnFiltroAzul;
        private System.Windows.Forms.Button btnFiltroContraste;
        private System.Windows.Forms.Button btnFiltroCalido;
        private System.Windows.Forms.Button btnFiltroFrio;
        private System.Windows.Forms.Label lblFiltroActivo;

        // Transformaciones
        private System.Windows.Forms.GroupBox grpTransform;
        private System.Windows.Forms.Button btnRotarIzq;
        private System.Windows.Forms.Button btnRotarDer;
        private System.Windows.Forms.Button btnVoltearH;
        private System.Windows.Forms.Button btnVoltearV;
        private System.Windows.Forms.Button btnRestaurar;

        // Pintura
        private System.Windows.Forms.GroupBox grpPintura;
        private System.Windows.Forms.CheckBox chkPintar;
        private System.Windows.Forms.Button btnColorPincel;
        private System.Windows.Forms.TrackBar trackGrosor;
        private System.Windows.Forms.Label lblGrosor;

        // Guardar
        private System.Windows.Forms.GroupBox grpGuardar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnGuardarSobre;

        // Metadatos
        private System.Windows.Forms.ListView lvMetadatos;
        private System.Windows.Forms.ColumnHeader colNombre;
        private System.Windows.Forms.ColumnHeader colValor;

        // GPS
        private System.Windows.Forms.Label lblGPS;
        private System.Windows.Forms.Label lblLatLabel;
        private System.Windows.Forms.TextBox txtLat;
        private System.Windows.Forms.Label lblLonLabel;
        private System.Windows.Forms.TextBox txtLon;
        private System.Windows.Forms.Button btnMostrarMapa;
        private System.Windows.Forms.Button btnAbrirGMaps;
        private Microsoft.Web.WebView2.WinForms.WebView2 webMapa;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // Instanciar controles
            picBox           = new System.Windows.Forms.PictureBox();
            tabControl       = new System.Windows.Forms.TabControl();
            tabEdicion       = new System.Windows.Forms.TabPage();
            tabMetadatos     = new System.Windows.Forms.TabPage();
            tabGPS           = new System.Windows.Forms.TabPage();
            panelHerramientas = new System.Windows.Forms.Panel();
            grpFiltros       = new System.Windows.Forms.GroupBox();
            grpTransform     = new System.Windows.Forms.GroupBox();
            grpPintura       = new System.Windows.Forms.GroupBox();
            grpGuardar       = new System.Windows.Forms.GroupBox();
            btnFiltroOriginal = Boton("Original", "Original");
            btnFiltroGris    = Boton("Grises", "Escala de grises");
            btnFiltroSepia   = Boton("Sepia", "Sepia");
            btnFiltroInvertir = Boton("Invertir", "Invertir");
            btnFiltroRojo    = Boton("Rojo", "Rojo");
            btnFiltroVerde   = Boton("Verde", "Verde");
            btnFiltroAzul    = Boton("Azul", "Azul");
            btnFiltroContraste = Boton("Contraste", "Alto contraste");
            btnFiltroCalido  = Boton("Cálido 🌅", "Cálido");
            btnFiltroFrio    = Boton("Frío ❄️", "Frío");
            lblFiltroActivo  = new System.Windows.Forms.Label();
            btnRotarIzq      = new System.Windows.Forms.Button();
            btnRotarDer      = new System.Windows.Forms.Button();
            btnVoltearH      = new System.Windows.Forms.Button();
            btnVoltearV      = new System.Windows.Forms.Button();
            btnRestaurar     = new System.Windows.Forms.Button();
            chkPintar        = new System.Windows.Forms.CheckBox();
            btnColorPincel   = new System.Windows.Forms.Button();
            trackGrosor      = new System.Windows.Forms.TrackBar();
            lblGrosor        = new System.Windows.Forms.Label();
            btnGuardar       = new System.Windows.Forms.Button();
            btnGuardarSobre  = new System.Windows.Forms.Button();
            lvMetadatos      = new System.Windows.Forms.ListView();
            colNombre        = new System.Windows.Forms.ColumnHeader();
            colValor         = new System.Windows.Forms.ColumnHeader();
            lblGPS           = new System.Windows.Forms.Label();
            lblLatLabel      = new System.Windows.Forms.Label();
            txtLat           = new System.Windows.Forms.TextBox();
            lblLonLabel      = new System.Windows.Forms.Label();
            txtLon           = new System.Windows.Forms.TextBox();
            btnMostrarMapa   = new System.Windows.Forms.Button();
            btnAbrirGMaps    = new System.Windows.Forms.Button();
            webMapa          = new Microsoft.Web.WebView2.WinForms.WebView2();

            SuspendLayout();

            // ── Form ─────────────────────────────────────────────────────────
            this.Text            = "Editor de Imagen";
            this.Size            = new System.Drawing.Size(1280, 820);
            this.MinimumSize     = new System.Drawing.Size(1024, 700);
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Font            = new System.Drawing.Font("Segoe UI", 9F);

            // ── picBox ───────────────────────────────────────────────────────
            picBox.Dock         = System.Windows.Forms.DockStyle.Fill;
            picBox.SizeMode     = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            picBox.BackColor    = System.Drawing.Color.FromArgb(30, 30, 30);
            picBox.Cursor       = System.Windows.Forms.Cursors.Cross;
            picBox.MouseDown   += picBox_MouseDown;
            picBox.MouseMove   += picBox_MouseMove;
            picBox.MouseUp     += picBox_MouseUp;
            ((System.ComponentModel.ISupportInitialize)picBox).BeginInit();

            // ── tabControl ───────────────────────────────────────────────────
            tabControl.Dock  = System.Windows.Forms.DockStyle.Fill;
            tabControl.Font  = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            tabEdicion.Text    = "🖼 Editor";
            tabMetadatos.Text  = "📋 Metadatos";
            tabGPS.Text        = "🗺 Geolocalización";
            tabControl.TabPages.Add(tabEdicion);
            tabControl.TabPages.Add(tabMetadatos);
            tabControl.TabPages.Add(tabGPS);

            // ════════════ TAB EDICIÓN ════════════════════════════════════════
            // Panel herramientas (derecha, fijo)
            panelHerramientas.Dock        = System.Windows.Forms.DockStyle.Right;
            panelHerramientas.Width       = 220;
            panelHerramientas.AutoScroll  = true;
            panelHerramientas.BackColor   = System.Drawing.Color.FromArgb(240, 240, 245);
            panelHerramientas.Padding     = new System.Windows.Forms.Padding(6);

            // Grupo FILTROS
            grpFiltros.Text    = "Filtros de color";
            grpFiltros.Dock    = System.Windows.Forms.DockStyle.Top;
            grpFiltros.Height  = 285;
            grpFiltros.Padding = new System.Windows.Forms.Padding(5);
            int fy = 20, fw = 93, fh = 28;
            PosBtn(btnFiltroOriginal,  5, fy);
            PosBtn(btnFiltroGris,    fw + 8, fy);  fy += fh + 3;
            PosBtn(btnFiltroSepia,     5, fy);
            PosBtn(btnFiltroInvertir,fw + 8, fy);  fy += fh + 3;
            PosBtn(btnFiltroRojo,      5, fy);
            PosBtn(btnFiltroVerde,   fw + 8, fy);  fy += fh + 3;
            PosBtn(btnFiltroAzul,      5, fy);
            PosBtn(btnFiltroContraste,fw+8, fy); fy += fh + 3;
            PosBtn(btnFiltroCalido,    5, fy);
            PosBtn(btnFiltroFrio,    fw + 8, fy);  fy += fh + 3;
            btnFiltroOriginal.Click += btnFiltro_Click;
            btnFiltroGris.Click += btnFiltro_Click;
            btnFiltroSepia.Click += btnFiltro_Click;
            btnFiltroInvertir.Click += btnFiltro_Click;
            btnFiltroRojo.Click += btnFiltro_Click;
            btnFiltroVerde.Click += btnFiltro_Click;
            btnFiltroAzul.Click += btnFiltro_Click;
            btnFiltroContraste.Click += btnFiltro_Click;
            btnFiltroCalido.Click += btnFiltro_Click;
            btnFiltroFrio.Click += btnFiltro_Click;

            lblFiltroActivo.Text      = "Filtro activo: Original";
            lblFiltroActivo.Location  = new System.Drawing.Point(5, fy + 2);
            lblFiltroActivo.Size      = new System.Drawing.Size(195, 20);
            lblFiltroActivo.ForeColor = System.Drawing.Color.DarkSlateBlue;
            lblFiltroActivo.Font      = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            grpFiltros.Controls.Add(btnFiltroOriginal);
            grpFiltros.Controls.Add(btnFiltroGris);
            grpFiltros.Controls.Add(btnFiltroSepia);
            grpFiltros.Controls.Add(btnFiltroInvertir);
            grpFiltros.Controls.Add(btnFiltroRojo);
            grpFiltros.Controls.Add(btnFiltroVerde);
            grpFiltros.Controls.Add(btnFiltroAzul);
            grpFiltros.Controls.Add(btnFiltroContraste);
            grpFiltros.Controls.Add(btnFiltroCalido);
            grpFiltros.Controls.Add(btnFiltroFrio);
            grpFiltros.Controls.Add(lblFiltroActivo);

            // Grupo TRANSFORMACIONES
            grpTransform.Text    = "Transformar";
            grpTransform.Dock    = System.Windows.Forms.DockStyle.Top;
            grpTransform.Height  = 110;
            grpTransform.Padding = new System.Windows.Forms.Padding(5);
            btnRotarIzq.Text  = "↺ Rot. Izq";  btnRotarIzq.Size  = new System.Drawing.Size(93, 30);
            btnRotarIzq.Location = new System.Drawing.Point(5, 22);
            btnRotarDer.Text  = "↻ Rot. Der";  btnRotarDer.Size  = new System.Drawing.Size(93, 30);
            btnRotarDer.Location = new System.Drawing.Point(104, 22);
            btnVoltearH.Text  = "↔ Voltear H"; btnVoltearH.Size  = new System.Drawing.Size(93, 30);
            btnVoltearH.Location = new System.Drawing.Point(5, 57);
            btnVoltearV.Text  = "↕ Voltear V"; btnVoltearV.Size  = new System.Drawing.Size(93, 30);
            btnVoltearV.Location = new System.Drawing.Point(104, 57);
            btnRestaurar.Text = "🔄 Restaurar original";
            btnRestaurar.Size = new System.Drawing.Size(195, 30);
            btnRestaurar.Location = new System.Drawing.Point(5, 75);
            btnRestaurar.BackColor = System.Drawing.Color.FromArgb(220, 100, 60);
            btnRestaurar.ForeColor = System.Drawing.Color.White;
            btnRotarIzq.Click  += btnRotarIzq_Click;
            btnRotarDer.Click  += btnRotarDer_Click;
            btnVoltearH.Click  += btnVoltearH_Click;
            btnVoltearV.Click  += btnVoltearV_Click;
            btnRestaurar.Click += btnRestaurar_Click;
            grpTransform.Controls.Add(btnRotarIzq);
            grpTransform.Controls.Add(btnRotarDer);
            grpTransform.Controls.Add(btnVoltearH);
            grpTransform.Controls.Add(btnVoltearV);
            grpTransform.Controls.Add(btnRestaurar);

            // Grupo PINTURA
            grpPintura.Text    = "Pintar sobre imagen";
            grpPintura.Dock    = System.Windows.Forms.DockStyle.Top;
            grpPintura.Height  = 115;
            grpPintura.Padding = new System.Windows.Forms.Padding(5);
            chkPintar.Text     = "✏ Activar pincel";
            chkPintar.Location = new System.Drawing.Point(5, 22);
            chkPintar.Size     = new System.Drawing.Size(140, 22);
            chkPintar.Font     = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnColorPincel.Text      = "Color";
            btnColorPincel.Location  = new System.Drawing.Point(5, 48);
            btnColorPincel.Size      = new System.Drawing.Size(90, 28);
            btnColorPincel.BackColor = System.Drawing.Color.Red;
            btnColorPincel.ForeColor = System.Drawing.Color.White;
            btnColorPincel.Click    += btnColorPincel_Click;
            trackGrosor.Minimum  = 1;
            trackGrosor.Maximum  = 40;
            trackGrosor.Value    = 5;
            trackGrosor.TickFrequency = 5;
            trackGrosor.Location = new System.Drawing.Point(5, 78);
            trackGrosor.Size     = new System.Drawing.Size(130, 30);
            trackGrosor.Scroll  += trackGrosor_Scroll;
            lblGrosor.Text     = "Grosor: 5";
            lblGrosor.Location = new System.Drawing.Point(142, 83);
            lblGrosor.Size     = new System.Drawing.Size(65, 20);
            ((System.ComponentModel.ISupportInitialize)trackGrosor).BeginInit();
            grpPintura.Controls.Add(chkPintar);
            grpPintura.Controls.Add(btnColorPincel);
            grpPintura.Controls.Add(trackGrosor);
            grpPintura.Controls.Add(lblGrosor);

            // Grupo GUARDAR
            grpGuardar.Text    = "Guardar";
            grpGuardar.Dock    = System.Windows.Forms.DockStyle.Top;
            grpGuardar.Height  = 90;
            grpGuardar.Padding = new System.Windows.Forms.Padding(5);
            btnGuardar.Text      = "💾 Guardar como...";
            btnGuardar.Size      = new System.Drawing.Size(195, 30);
            btnGuardar.Location  = new System.Drawing.Point(5, 22);
            btnGuardar.BackColor = System.Drawing.Color.FromArgb(50, 130, 200);
            btnGuardar.ForeColor = System.Drawing.Color.White;
            btnGuardar.Click    += btnGuardar_Click;
            btnGuardarSobre.Text      = "💾 Sobreescribir original";
            btnGuardarSobre.Size      = new System.Drawing.Size(195, 30);
            btnGuardarSobre.Location  = new System.Drawing.Point(5, 55);
            btnGuardarSobre.BackColor = System.Drawing.Color.FromArgb(80, 160, 80);
            btnGuardarSobre.ForeColor = System.Drawing.Color.White;
            btnGuardarSobre.Click    += btnGuardarSobre_Click;
            grpGuardar.Controls.Add(btnGuardar);
            grpGuardar.Controls.Add(btnGuardarSobre);

            // Añadir grupos al panel (LIFO = de abajo a arriba)
            panelHerramientas.Controls.Add(grpGuardar);
            panelHerramientas.Controls.Add(grpPintura);
            panelHerramientas.Controls.Add(grpTransform);
            panelHerramientas.Controls.Add(grpFiltros);

            tabEdicion.Controls.Add(panelHerramientas);
            tabEdicion.Controls.Add(picBox);

            // ════════════ TAB METADATOS ══════════════════════════════════════
            lvMetadatos.Dock         = System.Windows.Forms.DockStyle.Fill;
            lvMetadatos.View         = System.Windows.Forms.View.Details;
            lvMetadatos.FullRowSelect = true;
            lvMetadatos.GridLines    = true;
            colNombre.Text  = "Propiedad";  colNombre.Width = 200;
            colValor.Text   = "Valor";      colValor.Width  = 500;
            lvMetadatos.Columns.Add(colNombre);
            lvMetadatos.Columns.Add(colValor);
            tabMetadatos.Controls.Add(lvMetadatos);

            // ════════════ TAB GPS ════════════════════════════════════════════
            var pnlGPSTop = new System.Windows.Forms.Panel();
            pnlGPSTop.Dock   = System.Windows.Forms.DockStyle.Top;
            pnlGPSTop.Height = 90;
            pnlGPSTop.Padding = new System.Windows.Forms.Padding(8);

            lblGPS.Location  = new System.Drawing.Point(8, 6);
            lblGPS.Size      = new System.Drawing.Size(700, 22);
            lblGPS.Font      = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            lblLatLabel.Text     = "Latitud:";
            lblLatLabel.Location = new System.Drawing.Point(8, 33);
            lblLatLabel.Size     = new System.Drawing.Size(60, 22);
            txtLat.Location      = new System.Drawing.Point(75, 30);
            txtLat.Size          = new System.Drawing.Size(130, 22);

            lblLonLabel.Text     = "Longitud:";
            lblLonLabel.Location = new System.Drawing.Point(215, 33);
            lblLonLabel.Size     = new System.Drawing.Size(65, 22);
            txtLon.Location      = new System.Drawing.Point(285, 30);
            txtLon.Size          = new System.Drawing.Size(130, 22);

            btnMostrarMapa.Text      = "🗺 Mostrar mapa";
            btnMostrarMapa.Location  = new System.Drawing.Point(430, 28);
            btnMostrarMapa.Size      = new System.Drawing.Size(130, 28);
            btnMostrarMapa.BackColor = System.Drawing.Color.FromArgb(50, 130, 200);
            btnMostrarMapa.ForeColor = System.Drawing.Color.White;
            btnMostrarMapa.Click    += btnMostrarMapa_Click;

            btnAbrirGMaps.Text      = "🌐 Abrir en Google Maps";
            btnAbrirGMaps.Location  = new System.Drawing.Point(570, 28);
            btnAbrirGMaps.Size      = new System.Drawing.Size(175, 28);
            btnAbrirGMaps.BackColor = System.Drawing.Color.FromArgb(34, 139, 34);
            btnAbrirGMaps.ForeColor = System.Drawing.Color.White;
            btnAbrirGMaps.Click    += btnAbrirGMaps_Click;

            pnlGPSTop.Controls.Add(lblGPS);
            pnlGPSTop.Controls.Add(lblLatLabel);
            pnlGPSTop.Controls.Add(txtLat);
            pnlGPSTop.Controls.Add(lblLonLabel);
            pnlGPSTop.Controls.Add(txtLon);
            pnlGPSTop.Controls.Add(btnMostrarMapa);
            pnlGPSTop.Controls.Add(btnAbrirGMaps);

            ((System.ComponentModel.ISupportInitialize)webMapa).BeginInit();
            webMapa.Dock = System.Windows.Forms.DockStyle.Fill;

            // IMPORTANTE: en WinForms el Dock=Fill debe agregarse ANTES que
            // los controles Dock=Top, de lo contrario el Fill se calcula sin
            // descontar el espacio del panel superior y el mapa queda cortado.
            // El orden de Add es inverso al orden visual.
            tabGPS.Controls.Add(webMapa);
            tabGPS.Controls.Add(pnlGPSTop);

            // ── Ensamble final ───────────────────────────────────────────────
            this.Controls.Add(tabControl);

            ((System.ComponentModel.ISupportInitialize)picBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackGrosor).EndInit();
            ((System.ComponentModel.ISupportInitialize)webMapa).EndInit();

            ResumeLayout(false);
            PerformLayout();
        }
    }
}



