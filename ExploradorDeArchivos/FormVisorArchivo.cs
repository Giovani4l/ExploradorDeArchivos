using System;
using System.IO;
using System.Windows.Forms;


namespace ExploradorDeArchivos
{
    /// <summary>
    /// Visor + editor integrado para:
    ///   • CSV / TSV / Excel  → DataGridView editable
    ///   • Texto / Código     → RichTextBox editable con Guardar
    ///   • Imágenes           → PictureBox con zoom
    ///   • PDF                → WebView2 (visor nativo del navegador)
    ///   • Word (.docx)       → RichTextBox con lectura/escritura via OpenXml
    ///   • PowerPoint (.pptx) → Panel de diapositivas con texto editable
    /// </summary>
    public partial class FormVisorArchivo : Form
    {
        private readonly string _filePath;
        private readonly string _ext;
        private bool _modificado = false;


        // ── controles dinámicos ────────────────────────────────────────────
        private DataGridView?                   _grid;
        private RichTextBox?                    _rtb;
        private PictureBox?                     _pic;
        private Microsoft.Web.WebView2.WinForms.WebView2? _web;
        private Panel?                          _pptPanel;
        private Label?                          _lblInfo;
        private string?                         _pdfTempPath; // copia temporal para WebView2

        // Para Word: mantenemos el texto por párrafos para poder guardarlo
        private List<RichTextBox>?              _wordParrafos;

        public FormVisorArchivo(string filePath)
        {
            _filePath = filePath;
            _ext      = Path.GetExtension(filePath);
            InitializeComponent();
            this.Text = $"📄  {Path.GetFileName(filePath)}";
        }

        /// <summary>API pública: Form1.OpenFile la usa para decidir si abrir aquí o con el SO.</summary>
        public static bool TieneVisorIntegrado(string ext) => AyudanteFormatoArchivo.TieneVisorIntegrado(ext);

        // ══════════════════════════════════════════════════════════════════
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                if      (AyudanteFormatoArchivo.ExtensionesTabla.Contains(_ext))  CargarTabla();
                else if (AyudanteFormatoArchivo.ExtensionesTexto.Contains(_ext))  CargarTexto();
                else if (AyudanteFormatoArchivo.ExtensionesImagen.Contains(_ext)) CargarImagen();
                else if (AyudanteFormatoArchivo.ExtensionesPdf.Contains(_ext))    CargarPdf();
                else if (AyudanteFormatoArchivo.ExtensionesWord.Contains(_ext))   CargarWord();
                else if (AyudanteFormatoArchivo.ExtensionesPresentacion.Contains(_ext))    CargarPpt();
                else                                MostrarSinSoporte();
            }
            catch (Exception ex) { MostrarError(ex.Message); }
        }


        // ══════════════════════════════════════════════════════════════════
        //  GUARDAR AL CERRAR si hay cambios
        // ══════════════════════════════════════════════════════════════════
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_modificado)
            {
                var r = MessageBox.Show("Hay cambios sin guardar.\n¿Deseas guardar antes de cerrar?",
                    "Cambios sin guardar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (r == DialogResult.Cancel) { e.Cancel = true; return; }
                if (r == DialogResult.Yes) Guardar();
            }
            base.OnFormClosing(e);
        }

        // ══════════════════════════════════════════════════════════════════
        //  UTILIDADES
        // ══════════════════════════════════════════════════════════════════
        private static Label CrearLabelInfo(string texto)
            => UIStyler.CrearLabelInfo(texto);


        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _pic?.Image?.Dispose();
            // Limpiar PDF temporal
            if (_pdfTempPath != null && File.Exists(_pdfTempPath))
                try { File.Delete(_pdfTempPath); } catch { }
        }
    }
}









