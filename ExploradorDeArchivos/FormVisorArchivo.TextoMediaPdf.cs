using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    public partial class FormVisorArchivo
    {
// ══════════════════════════════════════════════════════════════════
        //  TEXTO / CÓDIGO  — editable, con Guardar
        // ══════════════════════════════════════════════════════════════════
        private void CargarTexto()
        {
            var enc    = AyudanteFormatoArchivo.DetectarCodificacion(_filePath);
            var texto  = File.ReadAllText(_filePath, enc);
            var lineas = texto.Split('\n').Length;

            _lblInfo = CrearLabelInfo($"📝  {lineas} líneas  ·  {AyudanteFormatoArchivo.ObtenerTamanoLegible(new FileInfo(_filePath).Length)}  —  {enc.EncodingName}");
            panelContenido.Controls.Add(_lblInfo);

            _rtb = new RichTextBox
            {
                Dock        = DockStyle.Fill,
                ReadOnly    = false,
                BorderStyle = BorderStyle.None,
                ScrollBars  = RichTextBoxScrollBars.Both,
                WordWrap    = false,
                Text        = texto,
            };
            UIStyler.AplicarEstiloTextEditor(_rtb, esCodigo: true);
            _rtb.TextChanged      += (s, e) => MarcarModificado();
            _rtb.SelectionChanged += (s, e) =>
            {
                int line = _rtb.GetLineFromCharIndex(_rtb.SelectionStart) + 1;
                int col  = _rtb.SelectionStart - _rtb.GetFirstCharIndexOfCurrentLine() + 1;
                if (_lblInfo != null)
                    _lblInfo.Text = $"  Línea {line}, Col {col}  ·  {lineas} líneas  ·  {Path.GetFileName(_filePath)}";
            };

            panelContenido.Controls.Add(_rtb);
            _rtb.BringToFront();
            MostrarBotones(guardar: true, wrap: true);
        }

        // ══════════════════════════════════════════════════════════════════
        //  IMAGEN
        // ══════════════════════════════════════════════════════════════════
        private void CargarImagen()
        {
            var fi = new FileInfo(_filePath);
            _lblInfo = CrearLabelInfo($"🖼️  {AyudanteFormatoArchivo.ObtenerTamanoLegible(fi.Length)}  —  {Path.GetFileName(_filePath)}");
            panelContenido.Controls.Add(_lblInfo);

            using var tmp = Image.FromFile(_filePath);
            var img = new Bitmap(tmp);
            if (_lblInfo != null)
                _lblInfo.Text = $"  🖼️  {img.Width} × {img.Height} px  ·  {AyudanteFormatoArchivo.ObtenerTamanoLegible(fi.Length)}  —  {Path.GetFileName(_filePath)}";

            _pic = new PictureBox
            {
                Dock        = DockStyle.Fill,
                Image       = img,
                SizeMode    = PictureBoxSizeMode.Zoom,
                BackColor   = Color.FromArgb(40, 40, 40),
                BorderStyle = BorderStyle.None,
            };
            panelContenido.Controls.Add(_pic);
            _pic.BringToFront();
        }

        // ══════════════════════════════════════════════════════════════════
        //  PDF  — WebView2 (visor del sistema, con zoom y búsqueda)
        // ══════════════════════════════════════════════════════════════════
        private async void CargarPdf()
        {
            _lblInfo = CrearLabelInfo($"📕  PDF  ·  {AyudanteFormatoArchivo.ObtenerTamanoLegible(new FileInfo(_filePath).Length)}  —  {Path.GetFileName(_filePath)}");
            panelContenido.Controls.Add(_lblInfo);

            _web = new Microsoft.Web.WebView2.WinForms.WebView2
            {
                Dock = DockStyle.Fill,
            };
            panelContenido.Controls.Add(_web);
            _web.BringToFront();

            // WebView2 necesita un directorio de datos de usuario
            var userDataDir = Path.Combine(Path.GetTempPath(), "ExploradorWV2");
            Directory.CreateDirectory(userDataDir);

            var env = await Microsoft.Web.WebView2.Core.CoreWebView2Environment
                          .CreateAsync(null, userDataDir);
            await _web.EnsureCoreWebView2Async(env);

            // Copiar a temp con nombre seguro (sin espacios) para que WebView2 lo abra
            _pdfTempPath = Path.Combine(Path.GetTempPath(),
                               "vis_" + Path.GetFileNameWithoutExtension(_filePath)
                               .Replace(" ", "_") + ".pdf");
            File.Copy(_filePath, _pdfTempPath, true);
            _web.CoreWebView2.Navigate("file:///" + _pdfTempPath.Replace("\\", "/"));
        }
    }
}

