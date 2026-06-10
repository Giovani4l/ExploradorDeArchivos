using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using TagLib;

namespace ExploradorDeArchivos
{
    public partial class FormPlayerMP3 : Form
    {
        // ── Playback ──────────────────────────────────────────────────────────

        private WaveOutEvent?    _out;
        private AudioFileReader? _audio;
        private bool             _seeking    = false;
        private int              _currentIdx = -1;
        private bool             _shuffle    = false;
        private bool             _loop       = false;
        private readonly List<int> _shuffleOrder = new();

        // ── Data ──────────────────────────────────────────────────────────────

        private readonly HttpClient              _http  = new() { Timeout = TimeSpan.FromSeconds(15) };
        private readonly Dictionary<int, Image>  _hiRes = new();
        private readonly List<TrackInfo>         _tracks = new();
        private CancellationTokenSource?         _lyricsCts;

        // ── Spotify ───────────────────────────────────────────────────────────

        private string   _spotifyClientId     = "";
        private string   _spotifyClientSecret = "";
        private string   _spotifyToken        = "";
        private DateTime _spotifyTokenExpiry  = DateTime.MinValue;

        // ── Visual ────────────────────────────────────────────────────────────

        private Image?  _bgBlur;
        private Image?  _coverImg;
        private Color   _accent     = AppColors.PlayerAccent;
        private float[] _wave       = new float[26];
        private bool    _lyricsOpen = false;

        private readonly System.Windows.Forms.Timer _waveTimer = new() { Interval = 80 };
        private readonly Random _rng = new();

        // ─────────────────────────────────────────────────────────────────────

        public FormPlayerMP3()
        {
            InitializeComponent();
            StartWave();
            LoadSpotifyCredentials();
            ApplyTheme(_accent);
            UpdateCount();
        }


        // ══════════════════════════════════════════════════════════════════════
        //  OWNER-DRAW LISTVIEW
        

        private void LvPlaylist_DrawColumnHeader(object? sender, DrawListViewColumnHeaderEventArgs e)
        {
            using var bg  = new SolidBrush(Color.FromArgb(20, 20, 30));
            e.Graphics.FillRectangle(bg, e.Bounds);
            using var pen = new Pen(Color.FromArgb(40, 40, 58));
            e.Graphics.DrawLine(pen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

            using var tf    = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near, Trimming = StringTrimming.EllipsisCharacter };
            using var brush = new SolidBrush(AppColors.PlayerTextSec);
            var rect = new Rectangle(e.Bounds.Left + 8, e.Bounds.Top, e.Bounds.Width - 8, e.Bounds.Height);
            e.Graphics.DrawString(e.Header?.Text ?? "", new Font("Segoe UI", 8.5f, FontStyle.Bold), brush, rect, tf);
        }

        private void LvPlaylist_DrawItem(object? sender, DrawListViewItemEventArgs e) =>
            e.DrawDefault = false;

        private void LvPlaylist_DrawSubItem(object? sender, DrawListViewSubItemEventArgs e)
        {
            if (e.Item == null) return;

            bool isPlaying  = e.Item.Tag is int t && t == _currentIdx;
            bool isSelected = e.Item.Selected;

            Color bg = isPlaying  ? Color.FromArgb(38, _accent.R, _accent.G, _accent.B)
                     : isSelected ? Color.FromArgb(28, 28, 44)
                     : e.Item.Index % 2 == 0 ? Color.FromArgb(16, 16, 24) : Color.FromArgb(18, 18, 27);

            using (var bgBrush = new SolidBrush(bg))
                e.Graphics.FillRectangle(bgBrush, e.Bounds);

            if (isPlaying && e.ColumnIndex == 0)
                using (var accentBrush = new SolidBrush(_accent))
                    e.Graphics.FillRectangle(accentBrush, new Rectangle(e.Bounds.Left, e.Bounds.Top, 3, e.Bounds.Height));

            if (e.ColumnIndex == 0 && e.Item.ImageIndex >= 0 && e.Item.ImageIndex < imageList.Images.Count)
                e.Graphics.DrawImage(imageList.Images[e.Item.ImageIndex],
                    new Rectangle(e.Bounds.Left + 4, e.Bounds.Top + 2, 44, 44));

            Color fg = isPlaying ? _accent : Color.FromArgb(210, 210, 230);
            using var fgBrush = new SolidBrush(fg);
            using var sf = new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };

            int textLeft = e.ColumnIndex == 0 ? e.Bounds.Left + 56 : e.Bounds.Left + 8;
            var textRect = new Rectangle(textLeft, e.Bounds.Top, e.Bounds.Width - textLeft + e.Bounds.Left - 4, e.Bounds.Height);

            using var font = new Font("Segoe UI", 9.5f, isPlaying ? FontStyle.Bold : FontStyle.Regular);
            e.Graphics.DrawString(e.SubItem?.Text ?? "", font, fgBrush, textRect, sf);

            using var sepPen = new Pen(Color.FromArgb(22, 22, 34));
            e.Graphics.DrawLine(sepPen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  TRACK LOADING
        // ══════════════════════════════════════════════════════════════════════

        private async Task LoadFilesAsync(string[] paths)
        {
            progressBar.Visible = true;
            progressBar.Maximum = paths.Length;
            progressBar.Value   = 0;
            foreach (var p in paths)
            {
                await LoadOneAsync(p);
                progressBar.Value++;
                Application.DoEvents();
            }
            progressBar.Visible = false;
            UpdateCount();
        }

        private async Task LoadOneAsync(string path)
        {
            try
            {
                var tf          = TagLib.File.Create(path);
                string rawName  = Path.GetFileNameWithoutExtension(path);
                string id3Artist = (tf.Tag.FirstPerformer ?? tf.Tag.Performers?.FirstOrDefault() ?? "").Trim();
                string id3Title  = (tf.Tag.Title ?? "").Trim();
                string localAlbum = (tf.Tag.Album ?? "").Trim();
                var localDur    = tf.Properties.Duration;

                string localArtist, localTitle;
                if (!string.IsNullOrEmpty(id3Artist) && !string.IsNullOrEmpty(id3Title))
                {
                    localArtist = id3Artist;
                    localTitle  = id3Title;
                }
                else
                {
                    string clean = Regex.Replace(rawName, @"^\d{1,3}[\s\.\-]+", "").Trim();
                    var parts = clean.Split(new[] { " - " }, 2, StringSplitOptions.RemoveEmptyEntries);
                    localArtist = parts.Length == 2 && !string.IsNullOrEmpty(id3Artist) ? id3Artist
                                : parts.Length == 2 ? parts[0].Trim() : id3Artist;
                    localTitle  = parts.Length == 2 && !string.IsNullOrEmpty(id3Title) ? id3Title
                                : parts.Length == 2 ? parts[1].Trim() : clean;
                }

                Image? localCover = null;
                if (tf.Tag.Pictures?.Length > 0)
                    try { using var ms = new MemoryStream(tf.Tag.Pictures[0].Data.Data); localCover = Image.FromStream(ms); }
                    catch { }

                var spotify = await SearchSpotifyAsync(localArtist, localTitle);

                string finalTitle  = !string.IsNullOrWhiteSpace(spotify?.Title)  ? spotify!.Title  : localTitle;
                string finalArtist = !string.IsNullOrWhiteSpace(spotify?.Artist) ? spotify!.Artist
                                   : !string.IsNullOrEmpty(localArtist)          ? localArtist : "Desconocido";
                string finalAlbum  = !string.IsNullOrWhiteSpace(spotify?.Album)  ? spotify!.Album  : localAlbum;
                TimeSpan finalDur  = spotify?.Duration.TotalSeconds > 1          ? spotify!.Duration : localDur;
                Image? finalCover  = spotify?.Cover ?? localCover
                                   ?? await FetchCoverFallbackAsync(finalArtist, finalTitle, finalAlbum);

                int idx = _tracks.Count;
                _tracks.Add(new TrackInfo(path, finalTitle, finalArtist, finalAlbum, finalDur, finalCover,
                    spotify?.AlbumType ?? "", spotify?.Popularity ?? 0,
                    spotify?.ReleaseDate ?? "", spotify?.SpotifyUrl ?? ""));

                if (finalCover != null) _hiRes[idx] = (Image)finalCover.Clone();

                var thumb = finalCover != null ? ResizeImg(finalCover, 48, 48) : MakeThumb(finalTitle);
                imageList.Images.Add(thumb);

                string durStr  = $"{(int)finalDur.TotalMinutes}:{finalDur.Seconds:D2}";
                string popStr  = spotify != null ? $"♥ {spotify.Popularity}" : "—";
                string yearStr = spotify?.ReleaseDate?.Length >= 4 ? spotify.ReleaseDate[..4] : "";

                var lvi = new ListViewItem(finalTitle) { ImageIndex = idx, Tag = idx };
                lvi.SubItems.Add(finalArtist);
                lvi.SubItems.Add(finalAlbum);
                lvi.SubItems.Add(durStr);
                lvi.SubItems.Add(popStr);
                lvi.SubItems.Add(yearStr);
                lvPlaylist.Items.Add(lvi);
            }
            catch { /* ignorar archivos que no se puedan leer */ }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PLAYBACK
        // ══════════════════════════════════════════════════════════════════════

        private void PlaySelected()
        {
            if (lvPlaylist.SelectedItems.Count > 0 && lvPlaylist.SelectedItems[0].Tag is int idx)
                PlayTrack(idx);
        }

        private void PlayTrack(int idx)
        {
            if (idx < 0 || idx >= _tracks.Count) return;
            _currentIdx = idx;
            var t = _tracks[idx];
            StopCore(resetUI: false);

            try
            {
                _audio = new AudioFileReader(t.Path);
                _out   = new WaveOutEvent();
                _out.Init(_audio);
                _out.Volume = volBar.Value / 100f;
                _out.PlaybackStopped += Stopped;
                _out.Play();

                var durToShow    = t.Duration.TotalSeconds > 1 ? t.Duration : _audio.TotalTime;
                seekBar.Maximum  = Math.Max(1, (int)_audio.TotalTime.TotalSeconds);
                seekBar.Value    = 0;
                seekBar.Enabled  = true;
                lblDuration.Text = Fmt((int)durToShow.TotalSeconds);
                lblNow.Text      = "0:00";
                playbackTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se puede reproducir:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpdateNowPlaying(t, idx);
            RefreshPlaylist(idx);
            GetLyricsAsync(t.Artist, t.Title);
            UpdatePlayPauseBtn();
        }

        private void UpdateNowPlaying(TrackInfo t, int idx)
        {
            lblTitle.Text  = t.Title;
            lblArtist.Text = t.Artist;

            string albumLine = t.Album;
            if (!string.IsNullOrEmpty(t.ReleaseDate) && t.ReleaseDate.Length >= 4)
                albumLine += $"  ·  {t.ReleaseDate[..4]}";
            lblAlbum.Text = albumLine;

            lblSpotifyBadge.Visible = t.Popularity > 0;
            if (t.Popularity > 0)
                lblSpotifyBadge.Text = $"🎵 Spotify  ·  ♥ {t.Popularity}/100";

            if (_hiRes.TryGetValue(idx, out var img))
            {
                _coverImg = img;
                _bgBlur   = BlurBg(img);
                ExtractAccent(img);
            }
            else
            {
                _coverImg = null;
                _bgBlur   = null;
                _accent   = AppColors.PlayerAccent;
            }

            ApplyTheme(_accent);
            pnlCover.Invalidate();
            Invalidate();
        }

        private void RefreshPlaylist(int playingIdx)
        {
            lvPlaylist.Invalidate();
            foreach (ListViewItem li in lvPlaylist.Items)
                if (li.Tag is int idx && idx == playingIdx) { li.EnsureVisible(); break; }
        }

        private void Stopped(object? sender, StoppedEventArgs e)
        {
            BeginInvoke(() =>
            {
                playbackTimer.Stop();
                if (_audio != null)
                {
                    int next = _loop ? _currentIdx : NextIdx();
                    if (next >= 0) PlayTrack(next);
                    else { seekBar.Value = 0; lblNow.Text = "0:00"; UpdatePlayPauseBtn(); }
                }
            });
        }

        private void StopCore(bool resetUI = true)
        {
            playbackTimer.Stop();
            try
            {
                if (_out != null)
                {
                    _out.PlaybackStopped -= Stopped;
                    _out.Stop();
                    _out.Dispose();
                    _out = null;
                }
                _audio?.Dispose();
                _audio = null;
            }
            catch { }

            if (resetUI)
            {
                seekBar.Value    = 0;
                lblNow.Text      = "0:00";
                lblDuration.Text = "0:00";
                seekBar.Enabled  = false;
                UpdatePlayPauseBtn();
            }
        }

        private int NextIdx()
        {
            if (_tracks.Count == 0) return -1;
            if (_shuffle && _shuffleOrder.Count > 0)
            {
                int p = _shuffleOrder.IndexOf(_currentIdx);
                return p < _shuffleOrder.Count - 1 ? _shuffleOrder[p + 1] : -1;
            }
            return _currentIdx < _tracks.Count - 1 ? _currentIdx + 1 : -1;
        }

        private int PrevIdx()
        {
            if (_tracks.Count == 0) return -1;
            if (_shuffle && _shuffleOrder.Count > 0)
            {
                int p = _shuffleOrder.IndexOf(_currentIdx);
                return p > 0 ? _shuffleOrder[p - 1] : -1;
            }
            return _currentIdx > 0 ? _currentIdx - 1 : -1;
        }

        private void RebuildShuffle()
        {
            _shuffleOrder.Clear();
            var ids = Enumerable.Range(0, _tracks.Count).ToList();
            while (ids.Count > 0)
            {
                int r = _rng.Next(ids.Count);
                _shuffleOrder.Add(ids[r]);
                ids.RemoveAt(r);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  PLAY / PAUSE
        // ══════════════════════════════════════════════════════════════════════

        private void UpdatePlayPauseBtn()
        {
            bool playing = _out?.PlaybackState == PlaybackState.Playing;
            btnPlayPause.Text = playing ? "⏸" : "▶";
        }

        private void BtnPlayPause_Click(object? s, EventArgs e)
        {
            if (_out == null) { PlaySelected(); return; }
            switch (_out.PlaybackState)
            {
                case PlaybackState.Playing: _out.Pause(); break;
                case PlaybackState.Paused:  _out.Play();  break;
                default:                    PlaySelected(); break;
            }
            UpdatePlayPauseBtn();
        }

        // ══════════════════════════════════════════════════════════════════════
        //  TIMER / SEEK
        // ══════════════════════════════════════════════════════════════════════

        private void TimerTick(object? sender, EventArgs e)
        {
            if (_audio == null || _seeking) return;
            var pos     = _audio.CurrentTime;
            lblNow.Text = Fmt((int)pos.TotalSeconds);
            try
            {
                int v = (int)pos.TotalSeconds;
                if (v >= seekBar.Minimum && v <= seekBar.Maximum) seekBar.Value = v;
            }
            catch { }
        }

        private void SeekUp(object? sender, MouseEventArgs e)
        {
            if (_audio != null)
                try { _audio.CurrentTime = TimeSpan.FromSeconds(seekBar.Value); } catch { }
            _seeking = false;
        }


        // ══════════════════════════════════════════════════════════════════════
        //  BUTTON EVENTS
        // ══════════════════════════════════════════════════════════════════════

        public async void AbrirArchivo(string path)
        {
            await LoadFilesAsync(new[] { path });
            if (_tracks.Count > 0) PlayTrack(_tracks.Count - 1);
        }

        private async void BtnOpenFile_Click(object? s, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "Audio|*.mp3;*.m4a;*.wav;*.flac;*.ogg|Todos|*.*", Multiselect = true };
            if (ofd.ShowDialog() == DialogResult.OK) await LoadFilesAsync(ofd.FileNames);
        }

        private async void BtnOpenFolder_Click(object? s, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK) return;
            var files = Directory
                .EnumerateFiles(fbd.SelectedPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(f => new[] { ".mp3", ".m4a", ".wav", ".flac", ".ogg" }
                    .Contains(Path.GetExtension(f).ToLower()))
                .ToArray();
            await LoadFilesAsync(files);
        }

        private void BtnStop_Click(object? s, EventArgs e) => StopCore(resetUI: true);

        private void BtnPrev_Click(object? s, EventArgs e)
        {
            if (_audio != null && _audio.CurrentTime.TotalSeconds > 3)
            { _audio.CurrentTime = TimeSpan.Zero; return; }
            int p = PrevIdx();
            if (p >= 0) PlayTrack(p);
        }

        private void BtnNext_Click(object? s, EventArgs e)
        {
            int n = NextIdx();
            if (n >= 0) PlayTrack(n);
        }

        private void BtnShuffle_Click(object? s, EventArgs e)
        {
            _shuffle = !_shuffle;
            if (_shuffle) RebuildShuffle();
            UIStyler.AplicarEstadoBotonToggle(btnShuffle, _shuffle, _accent);
        }

        private void BtnLoop_Click(object? s, EventArgs e)
        {
            _loop = !_loop;
            UIStyler.AplicarEstadoBotonToggle(btnLoop, _loop, _accent);
        }

        private void BtnLyrics_Click(object? s, EventArgs e)
        {
            _lyricsOpen = !_lyricsOpen;
            pnlLyrics.Visible = _lyricsOpen;
            UIStyler.AplicarEstadoBotonToggle(btnLyrics, _lyricsOpen, _accent);
        }

        private void BtnClearList_Click(object? s, EventArgs e)
        {
            if (MessageBox.Show("¿Limpiar toda la lista?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            StopCore(resetUI: true);
            lvPlaylist.Items.Clear();
            imageList.Images.Clear();
            _tracks.Clear();
            _hiRes.Clear();
            _currentIdx  = -1;
            _coverImg    = null;
            _bgBlur      = null;
            lblTitle.Text   = "Sin canción";
            lblArtist.Text  = "—";
            lblAlbum.Text   = "";
            lblSpotifyBadge.Visible = false;
            pnlCover.Invalidate();
            Invalidate();
            UpdateCount();
        }

        private async void BtnSavePlaylist_Click(object? s, EventArgs e)
        {
            if (_tracks.Count == 0) return;
            using var sfd = new SaveFileDialog { Filter = "Playlist|*.m3u", DefaultExt = "m3u", FileName = "MiPlaylist" };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            await System.IO.File.WriteAllLinesAsync(sfd.FileName, _tracks.Select(t => t.Path).Prepend("#EXTM3U"));
        }

        private async void BtnLoadPlaylist_Click(object? s, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "Playlist|*.m3u;*.txt|Todos|*.*" };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            var lines = await System.IO.File.ReadAllLinesAsync(ofd.FileName);
            var paths = lines.Where(l => !l.StartsWith("#") && System.IO.File.Exists(l)).ToArray();
            if (paths.Length == 0) return;
            BtnClearList_Click(null, EventArgs.Empty);
            await LoadFilesAsync(paths);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopCore(resetUI: false);
            _http.Dispose();
            _waveTimer.Dispose();
            base.OnFormClosing(e);
        }
    }

    // ── Modelos ───────────────────────────────────────────────────────────────

    public record TrackInfo(
        string   Path,
        string   Title,
        string   Artist,
        string   Album,
        TimeSpan Duration,
        Image?   Cover,
        string   AlbumType   = "",
        int      Popularity  = 0,
        string   ReleaseDate = "",
        string   SpotifyUrl  = "");

    public record SpotifyTrackData(
        string   Title,
        string   Artist,
        string   Album,
        string   AlbumType,
        TimeSpan Duration,
        int      Popularity,
        string   ReleaseDate,
        string   SpotifyUrl,
        Image?   Cover);
}









