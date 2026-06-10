using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    /// <summary>
    /// Formulario de grabación de audio mediante la API MCI de Windows.
    /// Graba en formato WAV y guarda en la carpeta de Música del usuario.
    /// </summary>
    public partial class FormGrabadorAudio : Form
    {
        // ── MCI interop ──────────────────────────────────────────────────────
        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        private static extern int mciSendString(string command, System.Text.StringBuilder? returnString,
            int returnLength, IntPtr callBack);

        // ── Estado ───────────────────────────────────────────────────────────
        private bool   _grabando;
        private bool   _hayGrabacion;
        private int    _segundos;
        private string _archivoTemp;
        private string _carpetaGuardado;
        private int    _animFrame;
        private readonly Random _rnd = new();

        public FormGrabadorAudio()
        {
            InitializeComponent();

            _carpetaGuardado = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                "Explorador_Grabaciones");
            Directory.CreateDirectory(_carpetaGuardado);
            _archivoTemp = Path.Combine(Path.GetTempPath(), $"grab_temp_{Guid.NewGuid():N}.wav");

            // Conectar eventos
            _panelViz.Paint     += PanelViz_Paint;
            _btnGrabar.Click    += BtnGrabar_Click;
            _btnDetener.Click   += BtnDetener_Click;
            _btnReproducir.Click += BtnReproducir_Click;
            _btnGuardar.Click   += BtnGuardar_Click;
            _btnCarpeta.Click   += (s, e) =>
                System.Diagnostics.Process.Start("explorer.exe", _carpetaGuardado);
            _timerUI.Tick       += TimerUI_Tick;
            _timerAnim.Tick     += (s, e) => { _animFrame++; _panelViz.Invalidate(); };
            FormClosing         += FormGrabador_Closing;

            _timerAnim.Start();
        }

        // ── Visualizador animado ─────────────────────────────────────────────
        private void PanelViz_Paint(object? sender, PaintEventArgs e)
        {
            var g   = e.Graphics;
            var mid = _panelViz.Height / 2;
            int w   = _panelViz.Width;
            int barras = 48;
            int bw  = w / barras;

            using var pincel = new SolidBrush(_grabando
                ? Color.FromArgb(220, 50, 50)
                : Color.FromArgb(0, 140, 80));

            for (int i = 0; i < barras; i++)
            {
                double fase = (_animFrame * 0.18) + i * 0.32;
                int altura  = _grabando
                    ? (int)(mid * 0.85 * (0.3 + 0.7 * Math.Abs(Math.Sin(fase + _rnd.NextDouble() * 0.4))))
                    : (int)(mid * 0.15 * Math.Abs(Math.Sin(fase)));

                g.FillRectangle(pincel, i * bw + 1, mid - altura, bw - 2, altura * 2);
            }
        }

        // ── Lógica de grabación ──────────────────────────────────────────────
        private void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (_grabando) return;

            mciSendString("close recsound", null, 0, IntPtr.Zero);
            if (File.Exists(_archivoTemp))
                try { File.Delete(_archivoTemp); } catch { }

            mciSendString("open new type waveaudio alias recsound", null, 0, IntPtr.Zero);
            mciSendString("record recsound",                         null, 0, IntPtr.Zero);

            _grabando              = true;
            _hayGrabacion          = false;
            _segundos              = 0;
            _lblReloj.Text         = "00:00";
            _lblEstado.Text        = "🔴  Grabando…";
            _lblArchivo.Text       = "";
            _btnGrabar.Enabled     = false;
            _btnDetener.Enabled    = true;
            _btnReproducir.Enabled = false;
            _btnGuardar.Enabled    = false;
            _timerUI.Start();
        }

        private void BtnDetener_Click(object sender, EventArgs e)
        {
            if (!_grabando) return;
            _timerUI.Stop();
            mciSendString($"save recsound \"{_archivoTemp}\"", null, 0, IntPtr.Zero);
            mciSendString("close recsound",                   null, 0, IntPtr.Zero);

            _grabando              = false;
            _hayGrabacion          = true;
            _lblEstado.Text        = $"✅  Grabación lista  ({_segundos}s)";
            _btnGrabar.Enabled     = true;
            _btnDetener.Enabled    = false;
            _btnReproducir.Enabled = true;
            _btnGuardar.Enabled    = true;
        }

        private void BtnReproducir_Click(object sender, EventArgs e)
        {
            if (!_hayGrabacion || !File.Exists(_archivoTemp)) return;
            mciSendString("close playback",                           null, 0, IntPtr.Zero);
            mciSendString($"open \"{_archivoTemp}\" alias playback",  null, 0, IntPtr.Zero);
            mciSendString("play playback",                            null, 0, IntPtr.Zero);
            _lblEstado.Text = "▶  Reproduciendo…";
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (!_hayGrabacion || !File.Exists(_archivoTemp)) return;

            var nombre  = $"Grabacion_{DateTime.Now:yyyyMMdd_HHmmss}.wav";
            var destino = Path.Combine(_carpetaGuardado, nombre);
            try
            {
                File.Copy(_archivoTemp, destino, true);
                _lblEstado.Text  = "💾  Guardado correctamente";
                _lblArchivo.Text = destino;
            }
            catch (Exception ex)
            {
                _lblEstado.Text = $"Error al guardar: {ex.Message}";
            }
        }

        private void TimerUI_Tick(object? sender, EventArgs e)
        {
            _segundos++;
            _lblReloj.Text = $"{_segundos / 60:00}:{_segundos % 60:00}";
        }

        private void FormGrabador_Closing(object? sender, FormClosingEventArgs e)
        {
            _timerUI.Stop();
            _timerAnim.Stop();
            mciSendString("close recsound", null, 0, IntPtr.Zero);
            mciSendString("close playback", null, 0, IntPtr.Zero);
            if (File.Exists(_archivoTemp))
                try { File.Delete(_archivoTemp); } catch { }
        }
    }
}


