using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using NAudio.Wave;

namespace ExploradorDeArchivos
{
    public partial class FormPlayerMP3
    {
// ══════════════════════════════════════════════════════════════════════
        //  WAVE ANIMATION
        // ══════════════════════════════════════════════════════════════════════

        private void StartWave()
        {
            _wave = new float[26];
            _waveTimer.Tick += (s, e) =>
            {
                bool playing = _out?.PlaybackState == PlaybackState.Playing;
                for (int i = 0; i < _wave.Length; i++)
                {
                    float target = playing ? (float)(_rng.NextDouble() * 0.88 + 0.06) : 0.04f;
                    _wave[i] = _wave[i] * 0.5f + target * 0.5f;
                }
                pnlWave.Invalidate();
            };
            _waveTimer.Start();
        }
// ══════════════════════════════════════════════════════════════════════
        //  COLOR EXTRACTION
        // ══════════════════════════════════════════════════════════════════════

        private void ExtractAccent(Image img)
        {
            try
            {
                using var bmp = new Bitmap(img, 16, 16);
                long r = 0, g = 0, b = 0, n = 0;
                for (int x = 0; x < 16; x++)
                for (int y = 0; y < 16; y++)
                {
                    var c = bmp.GetPixel(x, y);
                    if (c.GetSaturation() > 0.3f && c.GetBrightness() > 0.18f && c.GetBrightness() < 0.82f)
                    { r += c.R; g += c.G; b += c.B; n++; }
                }
                _accent = n > 10
                    ? Boost(Color.FromArgb((int)(r/n), (int)(g/n), (int)(b/n)))
                    : AppColors.PlayerAccent;
            }
            catch { _accent = AppColors.PlayerAccent; }
        }

        private static Color Boost(Color c)
        {
            float h = c.GetHue();
            float s = Math.Min(1f, c.GetSaturation() * 1.5f);
            float l = Math.Max(0.45f, Math.Min(0.72f, c.GetBrightness() * 1.2f));
            return HslToRgb(h, s, l);
        }

        private static Color HslToRgb(float h, float s, float l)
        {
            float cc = (1 - MathF.Abs(2*l - 1)) * s;
            float x  = cc * (1 - MathF.Abs((h / 60) % 2 - 1));
            float m  = l - cc / 2;
            float r, g, b;
            if      (h <  60) { r = cc; g = x;  b = 0;  }
            else if (h < 120) { r = x;  g = cc; b = 0;  }
            else if (h < 180) { r = 0;  g = cc; b = x;  }
            else if (h < 240) { r = 0;  g = x;  b = cc; }
            else if (h < 300) { r = x;  g = 0;  b = cc; }
            else               { r = cc; g = 0;  b = x;  }
            return Color.FromArgb(
                Math.Clamp((int)((r+m)*255), 0, 255),
                Math.Clamp((int)((g+m)*255), 0, 255),
                Math.Clamp((int)((b+m)*255), 0, 255));
        }

        // ══════════════════════════════════════════════════════════════════════
        //  THEME
        // ══════════════════════════════════════════════════════════════════════

        private void ApplyTheme(Color accent)
        {
            UIStyler.AplicarTemaReproductor(accent,
                btnPlayPause, lblAccentBar, lblSpotifyBadge,
                new Button[] { btnPrev, btnNext, btnStop, btnShuffle, btnLoop,
                               btnLyrics, btnOpenFile, btnOpenFolder,
                               btnSavePlaylist, btnLoadPlaylist, btnClearList });
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PAINTING
        // ══════════════════════════════════════════════════════════════════════

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            if (_bgBlur != null)
            {
                g.DrawImage(_bgBlur, new Rectangle(0, 0, Width, Height));
                using var ov = new SolidBrush(Color.FromArgb(205, AppColors.PlayerBgDark));
                g.FillRectangle(ov, ClientRectangle);
            }
            else
            {
                using var gr = new LinearGradientBrush(ClientRectangle,
                    Color.FromArgb(13, 13, 19), Color.FromArgb(8, 8, 12), 130f);
                g.FillRectangle(gr, ClientRectangle);
            }
        }

        private void CoverPaint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var r = new Rectangle(2, 2, pnlCover.Width - 4, pnlCover.Height - 4);

            if (_coverImg != null)
            {
                var path = RndRect(r, 18);
                g.SetClip(path);
                g.DrawImage(_coverImg, r);
                g.ResetClip();
                using var glow = new Pen(Color.FromArgb(70, _accent), 2);
                g.DrawPath(glow, path);
            }
            else
            {
                var path = RndRect(r, 18);
                using var gr = new LinearGradientBrush(r, Color.FromArgb(45, 45, 65), Color.FromArgb(22, 22, 38), 135f);
                g.FillPath(gr, path);
                using var f  = new Font("Segoe UI", 52f);
                using var sb = new SolidBrush(Color.FromArgb(55, 255, 255, 255));
                var sz = g.MeasureString("♪", f);
                g.DrawString("♪", f, sb, r.X + (r.Width - sz.Width) / 2, r.Y + (r.Height - sz.Height) / 2);
            }
        }

        private void WavePaint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int bw = 4, gap = 3, n = _wave.Length;
            int tw = n * bw + (n - 1) * gap;
            int sx = (pnlWave.Width - tw) / 2;
            int mh = pnlWave.Height - 6;

            for (int i = 0; i < n; i++)
            {
                int h     = Math.Max(3, (int)(_wave[i] * mh));
                int x     = sx + i * (bw + gap);
                int y     = (pnlWave.Height - h) / 2;
                int alpha = (int)((_wave[i] * 0.7f + 0.3f) * 220);
                using var br   = new SolidBrush(Color.FromArgb(alpha, _accent));
                using var path = UIStyler.CrearRectRedondeadoF(new RectangleF(x, y, bw, h), 2);
                g.FillPath(br, path);
            }
        }

        private static GraphicsPath RndRect(Rectangle r, int rad)
            => UIStyler.CrearRectRedondeado(r, rad);

        // ══════════════════════════════════════════════════════════════════════
        //  AYUDANTES
        // ══════════════════════════════════════════════════════════════════════

        private static Image BlurBg(Image src) =>
            new Bitmap(new Bitmap(src, 10, 10), 1920, 1080);

        private static Image ResizeImg(Image img, int w, int h)
        {
            var b = new Bitmap(w, h);
            using var g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, 0, 0, w, h);
            return b;
        }

        private static Image MakeThumb(string title)
        {
            var b = new Bitmap(48, 48);
            using var g = Graphics.FromImage(b);
            g.Clear(Color.FromArgb(40, 40, 62));
            string letter = title.Length > 0 ? title[0].ToString().ToUpper() : "♪";
            using var f  = new Font("Segoe UI", 20f, FontStyle.Bold);
            using var sb = new SolidBrush(Color.FromArgb(180, 180, 220));
            var sz = g.MeasureString(letter, f);
            g.DrawString(letter, f, sb, (48 - sz.Width) / 2, (48 - sz.Height) / 2);
            return b;
        }

        private static string Fmt(int secs) => $"{secs / 60}:{(secs % 60):D2}";

        private void UpdateCount() =>
            lblTrackCount.Text = $"{_tracks.Count} canción{(_tracks.Count != 1 ? "es" : "")}";
    }
}

