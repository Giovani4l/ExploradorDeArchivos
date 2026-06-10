using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    /// <summary>
    /// Herramienta de limpieza de datos con IA.
    /// Pasos: 1) Cargar archivo  2) Analizar columnas con IA  3) Limpiar  4) Revisar y exportar
    /// </summary>
    public partial class FormLimpiezaDatos : Form
    {
        // ── Estado ────────────────────────────────────────────────────────────

        private string _etapa          = "cargar";   // cargar → analizar → limpiar → revisar
        private string _nombreArchivo  = "";
        private string _extArchivo     = "";

        private List<Dictionary<string, string>> _filasOriginales = new();
        private List<Dictionary<string, string>> _filasLimpias    = new();
        private List<Dictionary<string, string>> _filasEditadas   = new();
        private List<string>                     _columnas        = new();
        private Dictionary<string, ColMeta>      _metaCols        = new();

        private int _paginaActual = 0;
        private const int FilasPorPagina = 25;

        private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(60) };

        // ── Tipos de columna ──────────────────────────────────────────────────

        private record ColMeta(string Tipo, string Descripcion, string Problemas);

        private static readonly Dictionary<string, Color> ColorPorTipo = new()
        {
            ["phone"]   = AppColors.ColPhone,
            ["name"]    = AppColors.ColName,
            ["date"]    = AppColors.ColDate,
            ["email"]   = AppColors.ColEmail,
            ["id"]      = AppColors.ColId,
            ["address"] = AppColors.ColAddress,
            ["number"]  = AppColors.ColNumber,
            ["text"]    = AppColors.ColText,
        };

        // ─────────────────────────────────────────────────────────────────────

        public FormLimpiezaDatos()
        {
            InitializeComponent();
            MostrarEtapa("cargar");
        }

        // ══════════════════════════════════════════════════════════════════════
        //  NAVEGACIÓN DE ETAPAS
        // ══════════════════════════════════════════════════════════════════════

        private void MostrarEtapa(string etapa)
        {
            _etapa = etapa;
            ActualizarEstiloPasos();
            OcultarError();
            _panelContenido.Controls.Clear();

            switch (etapa)
            {
                case "cargar":
                    _panelContenido.Controls.Add(_lblCargando);
                    _panelContenido.Controls.Add(_zonaArrastrar);
                    CentrarZonaArrastrar();
                    _panelContenido.Resize += OnContenidoResize;
                    break;

                case "analizar":
                    _panelContenido.Resize -= OnContenidoResize;
                    _panelContenido.Controls.Add(_panelConfigScroll);
                    RellenarConfigColumnas();
                    break;

                case "limpiar":
                    _panelContenido.Resize -= OnContenidoResize;
                    _panelContenido.Controls.Add(_panelLimpiando);
                    CentrarControlsLimpiando();
                    break;

                case "revisar":
                    _panelContenido.Resize -= OnContenidoResize;
                    _panelContenido.Controls.Add(_panelRevisar);
                    RellenarGrid();
                    RellenarStats();
                    RellenarPaginacion();
                    break;
            }
        }

        private void OnContenidoResize(object? sender, EventArgs e) => CentrarZonaArrastrar();

        private void ActualizarEstiloPasos()
        {
            string[] etapas = { "cargar", "analizar", "limpiar", "revisar" };
            int idxActual = Array.IndexOf(etapas, _etapa);

            foreach (Control c in _panelPasos.Controls)
            {
                if (c is not Label lbl || lbl.Tag is not string tag) continue;

                int idxEtapa = Array.IndexOf(etapas, tag);
                lbl.ForeColor = idxEtapa == idxActual ? AppColors.TextoPrim
                              : idxEtapa <  idxActual ? AppColors.VerdeClaro
                              :                          Color.FromArgb(51, 65, 85);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PASO 1: CARGAR ARCHIVO
        // ══════════════════════════════════════════════════════════════════════

        private void BtnSeleccionar_Click(object? sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title  = "Seleccionar archivo de datos",
                Filter = "Archivos de datos|*.csv;*.json;*.tsv;*.xlsx;*.xls|Todos los archivos|*.*"
            };
            if (dlg.ShowDialog(this) == DialogResult.OK)
                _ = CargarArchivo(dlg.FileName);
        }

        private async Task CargarArchivo(string ruta)
        {
            OcultarError();
            SetCargando("Leyendo archivo…");
            try
            {
                _nombreArchivo   = Path.GetFileName(ruta);
                _extArchivo      = Path.GetExtension(ruta).TrimStart('.').ToLowerInvariant();
                _filasOriginales = ServicioArchivosLimpiezaDatos.Parse(ruta);

                if (_filasOriginales.Count == 0)
                    throw new Exception("El archivo está vacío o no tiene filas.");

                _columnas = _filasOriginales[0].Keys.ToList();

                SetCargando("🤖  Analizando columnas con IA…");
                _metaCols = await AnalizarColumnasIA();

                MostrarEtapa("analizar");
            }
            catch (Exception ex)
            {
                MostrarError("⚠  " + ex.Message);
            }
            finally
            {
                SetCargando("");
            }
        }

        private void ZonaArrastrar_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data!.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void ZonaArrastrar_DragDrop(object? sender, DragEventArgs e)
        {
            var archivos = (string[])e.Data!.GetData(DataFormats.FileDrop)!;
            if (archivos.Length > 0) _ = CargarArchivo(archivos[0]);
        }

        private void ZonaArrastrar_Click(object? sender, EventArgs e) =>
            BtnSeleccionar_Click(sender, e);


        private void BtnLimpiar_Click(object? sender, EventArgs e) =>
            _ = EjecutarLimpieza();


        // ══════════════════════════════════════════════════════════════════════
        //  AYUDANTES DE ESTADO UI
        // ══════════════════════════════════════════════════════════════════════

        private void SetCargando(string msg)
        {
            if (InvokeRequired) { Invoke(() => SetCargando(msg)); return; }
            _lblCargando.Text    = msg;
            _lblCargando.Visible = msg != "";
        }

        private void SetProgreso(string msg, int pct)
        {
            if (InvokeRequired) { Invoke(() => SetProgreso(msg, pct)); return; }
            _lblProgTexto.Text = msg;
            _barProg.Value     = Math.Clamp(pct, 0, 100);
            _lblProgPct.Text   = $"{pct}%";
            CentrarControlsLimpiando();
        }

        private void MostrarError(string msg)
        {
            if (InvokeRequired) { Invoke(() => MostrarError(msg)); return; }
            _lblError.Text    = msg;
            _lblError.Height  = 36;
            _lblError.Visible = true;
        }

        private void OcultarError()
        {
            _lblError.Height  = 0;
            _lblError.Visible = false;
        }

        private void CentrarZonaArrastrar()
        {
            _zonaArrastrar.Location = new Point(
                (_panelContenido.Width  - _zonaArrastrar.Width)  / 2,
                (_panelContenido.Height - _zonaArrastrar.Height) / 2 - 20);
            _lblCargando.Location = new Point(
                (_panelContenido.Width - _lblCargando.Width) / 2,
                _zonaArrastrar.Bottom + 16);
        }

        private void CentrarControlsLimpiando()
        {
            int cx = _panelLimpiando.Width  / 2;
            int cy = _panelLimpiando.Height / 2;
            _lblProgTexto.Location = new Point(cx - _lblProgTexto.Width / 2, cy - 60);
            _barProg.Location      = new Point(cx - 250, cy - 20);
            _lblProgPct.Location   = new Point(cx - 15,  cy + 5);
        }
    }
}









