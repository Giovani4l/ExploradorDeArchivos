using System.Drawing;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    partial class FormLimpiezaDatos
    {
        private System.ComponentModel.IContainer components = null;

        // ── Contenedores principales ──────────────────────────────────────────
        private Panel         _panelPasos;
        private Panel         _panelContenido;

        // ── Banner de error ───────────────────────────────────────────────────
        private Label         _lblError;

        // ── Zona CARGAR ───────────────────────────────────────────────────────
        private Panel         _zonaArrastrar;
        private Label         _lblDropIcon;
        private Label         _lblDropText;
        private Label         _lblDropSub;
        private Button        _btnSeleccionar;
        private Label         _lblCargando;

        // ── Zona ANALIZAR / CONFIGURAR ────────────────────────────────────────
        private Panel         _panelConfigScroll;
        private Label         _lblInfoArchivo;
        private FlowLayoutPanel _flowCols;
        private Button        _btnLimpiar;

        // ── Zona LIMPIANDO ────────────────────────────────────────────────────
        private Panel         _panelLimpiando;
        private Label         _lblProgTexto;
        private ProgressBar   _barProg;
        private Label         _lblProgPct;

        // ── Zona REVISAR ──────────────────────────────────────────────────────
        private Panel         _panelRevisar;
        private FlowLayoutPanel _flowStats;
        private DataGridView  _grid;
        private Panel         _panelPaginacion;
        private Button        _btnAnterior;
        private Button        _btnSiguiente;
        private Label         _lblPagina;
        private Panel         _panelBotones;
        private Button        _btnExportCSV;
        private Button        _btnExportJSON;
        private Button        _btnExportXLSX;
        private Button        _btnReconfigurar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // ── Form ──────────────────────────────────────────────────────────
            Text           = "🧹 Limpieza de Datos con IA";
            Size           = new Size(1200, 800);
            MinimumSize    = new Size(900, 650);
            StartPosition  = FormStartPosition.CenterParent;
            BackColor      = AppColors.BgOscuro;
            Font           = new Font("Segoe UI", 9.5F);
            ForeColor      = AppColors.TextoPrim;
            DoubleBuffered = true;

            // ── Encabezado ────────────────────────────────────────────────────
            var panelHeader = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 64,
                BackColor = AppColors.BgOscuro,
                Padding   = new Padding(24, 0, 24, 0)
            };
            var lblTitulo = new Label
            {
                Text      = "DATA CLEAN  ✦ IA",
                Font      = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = AppColors.TextoPrim,
                AutoSize  = true,
                Location  = new Point(24, 16)
            };
            panelHeader.Controls.Add(lblTitulo);
            InicializarLayout(panelHeader);
        }

        #endregion
    }
}




