using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    public partial class FormVisorArchivo
    {
// ══════════════════════════════════════════════════════════════════
        //  SIN SOPORTE / ERROR
        // ══════════════════════════════════════════════════════════════════
        private void MostrarSinSoporte()
        {
            var fi  = new FileInfo(_filePath);
            var lbl = new Label
            {
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Text      = $"⚠️  No hay visor integrado para archivos «{_ext}».\n\n" +
                            $"Archivo: {Path.GetFileName(_filePath)}\n" +
                            $"Tamaño: {AyudanteFormatoArchivo.ObtenerTamanoLegible(fi.Length)}\n\n" +
                            "Usa el botón  Abrir con…  para abrirlo con otra aplicación.",
            };
            UIStyler.AplicarEstiloLabelSinSoporte(lbl);
            panelContenido.Controls.Add(lbl);
            btnAbrirCon.Visible = true;
        }

        private void MostrarError(string msg)
        {
            var lbl = new Label
            {
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            UIStyler.AplicarEstiloLabelError(lbl, msg);
            panelContenido.Controls.Add(lbl);
        }

        // ══════════════════════════════════════════════════════════════════
        //  BOTONES TOOLBAR
        // ══════════════════════════════════════════════════════════════════
        private void MostrarBotones(bool guardar = false, bool wrap = false,
                                    bool ajustarCols = false, bool fuente = false)
        {
            btnGuardar.Visible      = guardar;
            btnAlternarWrap.Visible = wrap;
            btnAjustarColumnas.Visible = ajustarCols;
            btnFuenteGrid.Visible   = fuente;
        }

        private void MarcarModificado()
        {
            if (_modificado) return;
            _modificado  = true;
            this.Text    = $"📄  {Path.GetFileName(_filePath)}  •";
            if (_lblInfo != null) _lblInfo.BackColor = Color.FromArgb(255, 244, 220);
        }

        private void BtnGuardar_Click(object sender, EventArgs e) => Guardar();

        private void BtnAbrirCon_Click(object sender, EventArgs e)
        {
            try { System.Diagnostics.Process.Start(
                    new System.Diagnostics.ProcessStartInfo(_filePath)
                    { UseShellExecute = true, Verb = "openas" }); }
            catch { System.Diagnostics.Process.Start(
                    new System.Diagnostics.ProcessStartInfo(_filePath)
                    { UseShellExecute = true }); }
        }

        private void BtnCopiarRuta_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_filePath);
            toolTipVis.Show("¡Ruta copiada!", btnCopiarRuta, 0, -24, 1500);
        }

        private void BtnAjustarColumnas_Click(object sender, EventArgs e)
        {
            if (_grid == null) return;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void BtnAlternarWrap_Click(object sender, EventArgs e)
        {
            if (_rtb == null) return;
            _rtb.WordWrap   = !_rtb.WordWrap;
            _rtb.ScrollBars = _rtb.WordWrap ? RichTextBoxScrollBars.Vertical : RichTextBoxScrollBars.Both;
            btnAlternarWrap.Text = _rtb.WordWrap ? "↔ Sin ajuste" : "↵ Ajustar líneas";
        }

        private void BtnFuenteGrid_Click(object sender, EventArgs e)
        {
            var target = (Control?)_grid ?? _rtb;
            if (target == null) return;
            using var dlg = new FontDialog { Font = target.Font };
            if (dlg.ShowDialog(this) == DialogResult.OK) target.Font = dlg.Font;
        }
    }
}

