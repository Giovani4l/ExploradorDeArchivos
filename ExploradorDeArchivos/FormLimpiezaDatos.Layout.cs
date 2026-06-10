using System.Drawing;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    public partial class FormLimpiezaDatos
    {
        private void InicializarLayout(Panel panelHeader)
        {
            // ── Barra de pasos ────────────────────────────────────────────────
                        _panelPasos = new Panel
                        {
                            Dock      = DockStyle.Top,
                            Height    = 44,
                            BackColor = AppColors.BgPanel,
                            Padding   = new Padding(24, 0, 24, 0)
                        };
                        BuildStepsBar();
            
                        // ── Banner de error ───────────────────────────────────────────────
                        _lblError = new Label
                        {
                            Dock      = DockStyle.Top,
                            Height    = 0,
                            BackColor = Color.FromArgb(28, 10, 10),
                            ForeColor = Color.FromArgb(252, 165, 165),
                            Font      = new Font("Segoe UI", 9F),
                            TextAlign = ContentAlignment.MiddleLeft,
                            Padding   = new Padding(16, 0, 0, 0),
                            Visible   = false
                        };
            
                        // ── Panel contenido (intercambiable por etapa) ────────────────────
                        _panelContenido = new Panel
                        {
                            Dock      = DockStyle.Fill,
                            BackColor = AppColors.BgOscuro,
                            Padding   = new Padding(24)
                        };
            
                        BuildZonaCargar();
                        BuildZonaAnalizar();
                        BuildZonaLimpiando();
                        BuildZonaRevisar();
            
                        // Orden de apilado: Fill primero para que Dock funcione correctamente
                        Controls.Add(_panelContenido);
                        Controls.Add(_lblError);
                        Controls.Add(_panelPasos);
                        Controls.Add(panelHeader);
                    }

        // ── Barra de pasos ────────────────────────────────────────────────────

        private void BuildStepsBar()
        {
            string[] etapas  = { "cargar",   "analizar",    "limpiar",    "revisar" };
            string[] nombres = { "01  Subir", "02  Analizar", "03  Limpiar", "04  Revisar" };
            int x = 24;

            for (int i = 0; i < etapas.Length; i++)
            {
                var lbl = new Label
                {
                    Text     = nombres[i],
                    AutoSize = true,
                    Location = new Point(x, 12),
                    Font     = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    Tag      = etapas[i]
                };
                _panelPasos.Controls.Add(lbl);
                x += lbl.PreferredWidth + 40;

                if (i < etapas.Length - 1)
                    _panelPasos.Controls.Add(new Label
                    {
                        Text      = "›",
                        AutoSize  = true,
                        Location  = new Point(x - 30, 12),
                        ForeColor = AppColors.Borde,
                        Font      = new Font("Segoe UI", 10F)
                    });
            }
        }

        // ── Zona CARGAR ───────────────────────────────────────────────────────

        private void BuildZonaCargar()
        {
            _zonaArrastrar = new Panel
            {
                Size      = new Size(600, 260),
                BackColor = AppColors.BgPanel,
                Cursor    = Cursors.Hand,
                AllowDrop = true
            };
            _zonaArrastrar.Paint += (s, e) =>
            {
                using var pen = new Pen(AppColors.Borde, 1)
                    { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
                e.Graphics.DrawRectangle(pen, 0, 0, _zonaArrastrar.Width - 1, _zonaArrastrar.Height - 1);
            };
            _zonaArrastrar.DragEnter += ZonaArrastrar_DragEnter;
            _zonaArrastrar.DragDrop  += ZonaArrastrar_DragDrop;
            _zonaArrastrar.Click     += ZonaArrastrar_Click;

            _lblDropIcon = new Label
            {
                Text      = "⬆",
                Font      = new Font("Segoe UI", 28F),
                ForeColor = AppColors.TextoSec,
                AutoSize  = true,
                Location  = new Point(260, 50)
            };
            _lblDropText = new Label
            {
                Text      = "Arrastra tu archivo aquí o haz clic",
                Font      = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = AppColors.AzulClaro,
                AutoSize  = true,
                Location  = new Point(140, 110)
            };
            _lblDropSub = new Label
            {
                Text      = "CSV · JSON · TSV · XLSX · XLS",
                Font      = new Font("Segoe UI", 9F),
                ForeColor = AppColors.TextoSec,
                AutoSize  = true,
                Location  = new Point(210, 140)
            };
            _btnSeleccionar = new Button
            {
                Text      = "Seleccionar archivo",
                Size      = new Size(180, 36),
                Location  = new Point(210, 180),
                BackColor = AppColors.Azul,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            _btnSeleccionar.FlatAppearance.BorderSize = 0;
            _btnSeleccionar.Click += BtnSeleccionar_Click;

            _lblCargando = new Label
            {
                Text      = "",
                AutoSize  = true,
                ForeColor = AppColors.AzulClaro,
                Font      = new Font("Segoe UI", 9.5F),
                Location  = new Point(0, 270),
                Visible   = false
            };

            _zonaArrastrar.Controls.AddRange(
                new Control[] { _lblDropIcon, _lblDropText, _lblDropSub, _btnSeleccionar });
        }

        // ── Zona ANALIZAR / CONFIGURAR ────────────────────────────────────────

        private void BuildZonaAnalizar()
        {
            _panelConfigScroll = new Panel
            {
                Dock       = DockStyle.Fill,
                AutoScroll = true,
                BackColor  = AppColors.BgOscuro
            };

            _lblInfoArchivo = new Label
            {
                AutoSize  = false,
                Height    = 36,
                Dock      = DockStyle.Top,
                ForeColor = AppColors.AzulClaro,
                Font      = new Font("Segoe UI", 10F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(4, 0, 0, 0)
            };

            _flowCols = new FlowLayoutPanel
            {
                Dock          = DockStyle.Top,
                AutoSize      = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents  = true,
                BackColor     = AppColors.BgOscuro,
                Padding       = new Padding(0, 8, 0, 8)
            };

            _btnLimpiar = new Button
            {
                Text      = "🤖  Limpiar con IA →",
                Height    = 44,
                Width     = 220,
                BackColor = AppColors.Azul,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor    = Cursors.Hand,
                Margin    = new Padding(0, 16, 0, 0)
            };
            _btnLimpiar.FlatAppearance.BorderSize = 0;
            _btnLimpiar.Click += BtnLimpiar_Click;

            var pnlBtn = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 70,
                BackColor = AppColors.BgOscuro
            };
            pnlBtn.Controls.Add(_btnLimpiar);
            _btnLimpiar.Location = new Point(0, 16);

            _panelConfigScroll.Controls.Add(pnlBtn);
            _panelConfigScroll.Controls.Add(_flowCols);
            _panelConfigScroll.Controls.Add(_lblInfoArchivo);
        }

        // ── Zona LIMPIANDO ────────────────────────────────────────────────────

        private void BuildZonaLimpiando()
        {
            _panelLimpiando = new Panel { Dock = DockStyle.Fill, BackColor = AppColors.BgOscuro };

            _lblProgTexto = new Label
            {
                Text      = "Limpiando…",
                ForeColor = AppColors.AzulClaro,
                Font      = new Font("Segoe UI", 12F),
                AutoSize  = true
            };
            _barProg = new ProgressBar
            {
                Size      = new Size(500, 8),
                Style     = ProgressBarStyle.Continuous,
                Minimum   = 0,
                Maximum   = 100,
                BackColor = AppColors.Borde,
                ForeColor = AppColors.AzulClaro
            };
            _lblProgPct = new Label
            {
                Text      = "0%",
                ForeColor = AppColors.TextoSec,
                Font      = new Font("Segoe UI", 9F),
                AutoSize  = true
            };

            _panelLimpiando.Controls.AddRange(new Control[] { _lblProgTexto, _barProg, _lblProgPct });
            _panelLimpiando.Resize += (s, e) => CentrarControlsLimpiando();
        }

        // ── Zona REVISAR ──────────────────────────────────────────────────────

        private void BuildZonaRevisar()
        {
            _panelRevisar = new Panel { Dock = DockStyle.Fill, BackColor = AppColors.BgOscuro };

            // Tarjetas de estadísticas
            _flowStats = new FlowLayoutPanel
            {
                Dock          = DockStyle.Top,
                Height        = 90,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents  = false,
                BackColor     = AppColors.BgOscuro,
                Padding       = new Padding(0, 0, 0, 8)
            };

            // Grilla de datos
            _grid = new DataGridView
            {
                Dock                    = DockStyle.Fill,
                BackgroundColor         = AppColors.BgCard,
                BorderStyle             = BorderStyle.None,
                GridColor               = AppColors.Borde,
                RowHeadersVisible       = false,
                AllowUserToAddRows      = false,
                AllowUserToDeleteRows   = false,
                SelectionMode           = DataGridViewSelectionMode.FullRowSelect,
                Font                    = new Font("Segoe UI", 8.5F),
                ForeColor               = AppColors.TextoPrim,
                AutoSizeColumnsMode     = DataGridViewAutoSizeColumnsMode.None,
                ScrollBars              = ScrollBars.Both,
                EnableHeadersVisualStyles = false
            };
            _grid.DefaultCellStyle.BackColor               = AppColors.BgCard;
            _grid.DefaultCellStyle.ForeColor               = AppColors.TextoPrim;
            _grid.DefaultCellStyle.SelectionBackColor      = Color.FromArgb(30, 58, 138);
            _grid.DefaultCellStyle.SelectionForeColor      = AppColors.TextoPrim;
            _grid.ColumnHeadersDefaultCellStyle.BackColor  = AppColors.BgPanel;
            _grid.ColumnHeadersDefaultCellStyle.ForeColor  = AppColors.AzulClaro;
            _grid.ColumnHeadersDefaultCellStyle.Font       = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _grid.ColumnHeadersHeight                      = 32;
            _grid.RowTemplate.Height                       = 26;
            _grid.CellEndEdit += Grid_CellEndEdit;

            // Paginación
            _panelPaginacion = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = AppColors.BgOscuro };
            _btnAnterior  = CreateNavButton("‹");
            _btnSiguiente = CreateNavButton("›");
            _lblPagina    = new Label { ForeColor = AppColors.TextoSec, Font = new Font("Segoe UI", 9F), AutoSize = true };
            _btnAnterior.Click  += (s, e) => CambiarPagina(-1);
            _btnSiguiente.Click += (s, e) => CambiarPagina(1);

            // Botones de exportación
            _panelBotones = new Panel { Dock = DockStyle.Bottom, Height = 52, BackColor = AppColors.BgOscuro, Padding = new Padding(0, 8, 0, 0) };
            _btnExportCSV    = CreateExportButton("⬇  CSV");
            _btnExportJSON   = CreateExportButton("⬇  JSON");
            _btnExportXLSX   = CreateExportButton("⬇  Excel");
            _btnReconfigurar = new Button
            {
                Text      = "← Reconfigurar",
                Height    = 34,
                Width     = 130,
                BackColor = Color.Transparent,
                ForeColor = AppColors.AzulClaro,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9F),
                Cursor    = Cursors.Hand
            };
            _btnReconfigurar.FlatAppearance.BorderColor = AppColors.Borde;
            _btnReconfigurar.FlatAppearance.BorderSize  = 1;

            _btnExportCSV.Click    += (s, e) => Exportar("csv");
            _btnExportJSON.Click   += (s, e) => Exportar("json");
            _btnExportXLSX.Click   += (s, e) => Exportar("xlsx");
            _btnReconfigurar.Click += (s, e) => MostrarEtapa("analizar");

            LayoutExportButtons();
            _panelBotones.Controls.AddRange(new Control[]
                { _btnExportCSV, _btnExportJSON, _btnExportXLSX, _btnReconfigurar });

            _panelRevisar.Controls.Add(_grid);
            _panelRevisar.Controls.Add(_panelPaginacion);
            _panelRevisar.Controls.Add(_panelBotones);
            _panelRevisar.Controls.Add(_flowStats);
        }

        private void LayoutExportButtons()
        {
            int x = 0;
            foreach (var b in new[] { _btnExportCSV, _btnExportJSON, _btnExportXLSX })
            {
                b.Location = new Point(x, 8);
                x += b.Width + 10;
            }
            _btnReconfigurar.Location = new Point(x + 10, 9);
        }

        // ── Factory helpers ───────────────────────────────────────────────────

        private static Button CreateNavButton(string texto) => new Button
        {
            Text      = texto,
            Size      = new Size(36, 28),
            BackColor = Color.Transparent,
            ForeColor = AppColors.AzulClaro,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 12F),
            Cursor    = Cursors.Hand,
            FlatAppearance = { BorderColor = AppColors.Borde }
        };

        private static Button CreateExportButton(string texto) => new Button
        {
            Text      = texto,
            Size      = new Size(110, 34),
            BackColor = AppColors.Verde,
            ForeColor = AppColors.VerdeClaro,
            FlatStyle = FlatStyle.Flat,
            Font      = new Font("Segoe UI", 9F, FontStyle.Bold),
            Cursor    = Cursors.Hand,
            FlatAppearance = { BorderColor = Color.FromArgb(5, 150, 105), BorderSize = 1 }
        };
    }
}


