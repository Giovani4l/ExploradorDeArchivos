using LibVLCSharp.Shared;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    public partial class FormPlayerMP4 : Form
    {
        // ── VLC ───────────────────────────────────────────────────────────────
        private LibVLC?      _vlc;
        private MediaPlayer? _player;

        // ── State ─────────────────────────────────────────────────────────────
        private bool _seeking      = false;
        private bool _isFullScreen = false;
        private bool _ctrlsHidden  = false;
        private FormWindowState _prevState;
        private FormBorderStyle _prevBorder;

        // ── Mouse-idle tracking ───────────────────────────────────────────────
        private int _idleMs = 0;
        private const int HideAfterMs = 3000;
        private System.Drawing.Point _lastMouseScreen;

        // ── Palette ───────────────────────────────────────────────────────────
        static readonly Color Accent   = Color.FromArgb(99,  102, 241);
        static readonly Color BgBar    = Color.FromArgb(18,  18,  26);
        static readonly Color TPrim    = Color.FromArgb(235, 235, 245);
        static readonly Color TSec     = Color.FromArgb(130, 130, 160);
        static readonly Color BtnHover = Color.FromArgb(46,  46,  68);

        public FormPlayerMP4()
        {
            InitializeComponent();
        }

        // ══════════════════════════════════════════════════════════════════════
        //  WndProc — intercepta WM_MOUSEMOVE al nivel del SO
        // ══════════════════════════════════════════════════════════════════════
        private const int WM_MOUSEMOVE   = 0x0200;
        private const int WM_NCMOUSEMOVE = 0x00A0;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_MOUSEMOVE || m.Msg == WM_NCMOUSEMOVE)
            {
                var cur = Cursor.Position;
                if (cur != _lastMouseScreen)
                {
                    _lastMouseScreen = cur;
                    OnMouseActivity();
                }
            }
        }

        void OnMouseActivity()
        {
            _idleMs = 0;
            if (_ctrlsHidden) ShowControls();
            if (_isFullScreen) Cursor.Show();
        }

        // ── Filtro a nivel de aplicación ─────────────────────────────────────
        private class MouseFilter : IMessageFilter
        {
            private readonly FormPlayerMP4 _owner;
            public MouseFilter(FormPlayerMP4 owner) => _owner = owner;
            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == WM_MOUSEMOVE)
                {
                    var cur = Cursor.Position;
                    if (cur != _owner._lastMouseScreen)
                    {
                        _owner._lastMouseScreen = cur;
                        _owner.OnMouseActivity();
                    }
                }
                return false;
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  LOAD / CLOSE
        // ══════════════════════════════════════════════════════════════════════
        private void FormPlayerMP4_Load(object sender, EventArgs e)
        {
            Application.AddMessageFilter(new MouseFilter(this));

            Core.Initialize();
            _vlc    = new LibVLC();
            _player = new MediaPlayer(_vlc);
            videoView.MediaPlayer = _player;

            _player.Volume = volBar.Value;

            _player.EndReached += (s, _) => BeginInvoke(() =>
            {
                seekBar.Value     = 0;
                lblNow.Text       = "0:00";
                btnPlayPause.Text = "▶";
            });

            _player.ESAdded += (s, _) => BeginInvoke(EnforceAspectRatio);

            timer1.Start();
        }

        private void FormPlayerMP4_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isFullScreen) Cursor.Show();
            mouseIdleTimer.Stop();
            _player?.Stop();
            _player?.Dispose();
            _vlc?.Dispose();
        }

        // ══════════════════════════════════════════════════════════════════════
        //  ASPECT RATIO
        // ══════════════════════════════════════════════════════════════════════
        void EnforceAspectRatio()
        {
            if (_player == null) return;
            try { _player.AspectRatio = ""; } catch { }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  ABRIR ARCHIVO — integración con el Explorador
        // ══════════════════════════════════════════════════════════════════════
        public void AbrirArchivo(string path)
        {
            if (_vlc == null || _player == null)
                this.Load += (s, e) => OpenFile(path);
            else
                OpenFile(path);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  FILE OPEN
        // ══════════════════════════════════════════════════════════════════════
        private void BtnOpen_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "Video|*.mp4;*.avi;*.mkv;*.wmv;*.mov;*.flv;*.webm;*.ts;*.m4v|Todos|*.*",
                Title  = "Abrir archivo de video"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            OpenFile(ofd.FileName);
        }

        private void OpenFile(string path)
        {
            if (_vlc == null || _player == null) return;
            var media = new Media(_vlc, path, FromType.FromPath);
            _player.Play(media);
            lblTitle.Text     = Path.GetFileNameWithoutExtension(path);
            btnPlayPause.Text = "⏸";
            seekBar.Value     = 0;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  TRANSPORT
        // ══════════════════════════════════════════════════════════════════════
        private void BtnPlayPause_Click(object sender, EventArgs e)
        {
            if (_player == null) return;
            if (_player.IsPlaying)
            {
                _player.SetPause(true);
                btnPlayPause.Text = "▶";
            }
            else
            {
                _player.SetPause(false);
                btnPlayPause.Text = "⏸";
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            _player?.Stop();
            seekBar.Value     = 0;
            lblNow.Text       = "0:00";
            lblDuration.Text  = "0:00";
            btnPlayPause.Text = "▶";
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            if (_player == null || _player.Length <= 0) return;
            _player.Time = Math.Max(0, _player.Time - 10_000);
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (_player == null || _player.Length <= 0) return;
            _player.Time = Math.Min(_player.Length, _player.Time + 10_000);
        }

        private void BtnMute_Click(object sender, EventArgs e)
        {
            if (_player == null) return;
            _player.Mute = !_player.Mute;
            btnMute.Text = _player.Mute ? "🔇" : (volBar.Value < 50 ? "🔉" : "🔊");
        }

        private void VolumeBar_Scroll(object sender, EventArgs e)
        {
            if (_player != null) _player.Volume = volBar.Value;
            lblVol.Text  = $"{volBar.Value}%";
            btnMute.Text = volBar.Value == 0 ? "🔇" : (volBar.Value < 50 ? "🔉" : "🔊");
        }

        private void BtnFullScreen_Click(object sender, EventArgs e) => ToggleFullScreen();

        // ══════════════════════════════════════════════════════════════════════
        //  FULLSCREEN
        // ══════════════════════════════════════════════════════════════════════
        void ToggleFullScreen()
        {
            if (!_isFullScreen)
            {
                _prevState  = WindowState;
                _prevBorder = FormBorderStyle;
                FormBorderStyle   = FormBorderStyle.None;
                WindowState       = FormWindowState.Maximized;
                pnlTopBar.Visible = false;
                mouseIdleTimer.Start();
                _isFullScreen = true;
            }
            else
            {
                FormBorderStyle   = _prevBorder;
                WindowState       = _prevState;
                pnlTopBar.Visible = true;
                ShowControls();
                Cursor.Show();
                mouseIdleTimer.Stop();
                _isFullScreen = false;
            }
        }

        void ShowControls()
        {
            pnlControls.Visible = true;
            _ctrlsHidden        = false;
            _idleMs             = 0;
        }

        void HideControls()
        {
            pnlControls.Visible = false;
            _ctrlsHidden        = true;
        }

        private void MouseIdle_Tick(object? sender, EventArgs e)
        {
            _idleMs += mouseIdleTimer.Interval;
            if (_idleMs >= HideAfterMs && _isFullScreen && _player?.IsPlaying == true)
            {
                if (!_ctrlsHidden) HideControls();
                Cursor.Hide();
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  TIMER
        // ══════════════════════════════════════════════════════════════════════
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_player == null || _seeking || _player.Length <= 0) return;
            long len  = _player.Length;
            long time = _player.Time;
            try { seekBar.Value = Math.Clamp((int)((time * 100) / len), 0, 100); } catch { }
            lblNow.Text      = Fmt(time);
            lblDuration.Text = Fmt(len);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  SEEKBAR EVENTS
        // ══════════════════════════════════════════════════════════════════════
        private void SeekBar_MouseDown(object sender, MouseEventArgs e)
        {
            _seeking = true;
        }

        private void SeekBar_MouseUp(object? sender, MouseEventArgs e)
        {
            if (_player != null && _player.Length > 0)
                _player.Position = seekBar.Value / 100f;
            _seeking = false;
        }

        private void SeekBar_Scroll(object sender, EventArgs e)
        {
            if (_seeking) UpdateTimeLabel(seekBar.Value);
        }

        void UpdateTimeLabel(int pct)
        {
            if (_player == null || _player.Length <= 0) return;
            lblNow.Text = Fmt((long)(_player.Length * (pct / 100f)));
        }

        // ══════════════════════════════════════════════════════════════════════
        //  RESIZE HANDLER — reposiciona controles anclados a la derecha
        // ══════════════════════════════════════════════════════════════════════
        private void PnlControls_Resize(object sender, EventArgs e)
        {
            int w = pnlControls.Width;

            seekBar.Width    = Math.Max(10, w - 64 - 58);
            lblDuration.Left = w - 58;

            int r = w - 10;
            btnFullScreen.Left = r - btnFullScreen.Width;
            lblVol.Left        = btnFullScreen.Left - lblVol.Width - 2;
            volBar.Left        = lblVol.Left - volBar.Width - 2;
            btnMute.Left       = volBar.Left - btnMute.Width - 4;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  KEYBOARD SHORTCUTS
        // ══════════════════════════════════════════════════════════════════════
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Space:
                    BtnPlayPause_Click(this, EventArgs.Empty); return true;
                case Keys.Escape when _isFullScreen:
                    ToggleFullScreen(); return true;
                case Keys.Right:
                    BtnNext_Click(this, EventArgs.Empty); return true;
                case Keys.Left:
                    BtnPrev_Click(this, EventArgs.Empty); return true;
                case Keys.Up:
                    volBar.Value = Math.Min(100, volBar.Value + 5);
                    VolumeBar_Scroll(this, EventArgs.Empty); return true;
                case Keys.Down:
                    volBar.Value = Math.Max(0, volBar.Value - 5);
                    VolumeBar_Scroll(this, EventArgs.Empty); return true;
                case Keys.F:
                case Keys.F11:
                    ToggleFullScreen(); return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  AYUDANTES
        // ══════════════════════════════════════════════════════════════════════
        static string Fmt(long ms)
        {
            var ts = TimeSpan.FromMilliseconds(ms);
            return ts.TotalHours >= 1
                ? $"{(int)ts.TotalHours}:{ts.Minutes:D2}:{ts.Seconds:D2}"
                : $"{ts.Minutes}:{ts.Seconds:D2}";
        }
    }
}


