namespace ExploradorDeArchivos
{
    partial class FormPlayerMP3
    {
        private System.ComponentModel.IContainer components = null;

        // ── Paneles ───────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel          pnlLeft;
        private System.Windows.Forms.Panel          pnlMain;
        private System.Windows.Forms.Panel          pnlCover;
        private System.Windows.Forms.Panel          pnlWave;
        private System.Windows.Forms.Panel          pnlTransport;
        private System.Windows.Forms.Panel          pnlLyrics;
        private System.Windows.Forms.Panel          pnlTopBar;
        private System.Windows.Forms.Panel          lblAccentBar;
        private System.Windows.Forms.FlowLayoutPanel flowToolbar;

        // ── Labels ────────────────────────────────────────────────────────────
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblArtist;
        private System.Windows.Forms.Label lblAlbum;
        private System.Windows.Forms.Label lblSpotifyBadge;
        private System.Windows.Forms.Label lblNow;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label lblVol;
        private System.Windows.Forms.Label lblTrackCount;
        private System.Windows.Forms.Label lblVolIcon;
        private System.Windows.Forms.Label lblLyricsTitle;
        private System.Windows.Forms.Label lblAppName;

        // ── TrackBars ─────────────────────────────────────────────────────────
        private System.Windows.Forms.TrackBar seekBar;
        private System.Windows.Forms.TrackBar volBar;

        // ── Buttons ───────────────────────────────────────────────────────────
        private System.Windows.Forms.Button btnPlayPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnShuffle;
        private System.Windows.Forms.Button btnLoop;
        private System.Windows.Forms.Button btnLyrics;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Button btnSavePlaylist;
        private System.Windows.Forms.Button btnLoadPlaylist;
        private System.Windows.Forms.Button btnClearList;

        // ── Otros controles ───────────────────────────────────────────────────
        private System.Windows.Forms.ListView     lvPlaylist;
        private System.Windows.Forms.ImageList    imageList;
        private System.Windows.Forms.RichTextBox  txtLyrics;
        private System.Windows.Forms.ProgressBar  progressBar;
        private System.Windows.Forms.Timer        playbackTimer;

        // ── Columnas del ListView ─────────────────────────────────────────────
        private System.Windows.Forms.ColumnHeader chTitle;
        private System.Windows.Forms.ColumnHeader chArtist;
        private System.Windows.Forms.ColumnHeader chAlbum;
        private System.Windows.Forms.ColumnHeader chDur;
        private System.Windows.Forms.ColumnHeader chPop;
        private System.Windows.Forms.ColumnHeader chYear;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ── Timer e ImageList ─────────────────────────────────────────────
            playbackTimer = new System.Windows.Forms.Timer(components) { Interval = 500 };
            imageList = new System.Windows.Forms.ImageList(components)
            {
                ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit,
                ImageSize  = new System.Drawing.Size(48, 48)
            };

            // ── Form ──────────────────────────────────────────────────────────
            Text           = "Pulse — Music Player";
            BackColor      = AppColors.PlayerBgDark;
            ForeColor      = AppColors.PlayerTextPrim;
            Font           = new System.Drawing.Font("Segoe UI", 9f);
            MinimumSize    = new System.Drawing.Size(1140, 700);
            Size           = new System.Drawing.Size(1340, 780);
            StartPosition  = System.Windows.Forms.FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            SetStyle(
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);

            // ── TOP BAR ───────────────────────────────────────────────────────
            pnlTopBar = new System.Windows.Forms.Panel
            {
                Dock      = System.Windows.Forms.DockStyle.Top,
                Height    = 54,
                BackColor = System.Drawing.Color.FromArgb(14, 14, 21)
            };

            lblAppName = new System.Windows.Forms.Label
            {
                Text      = "PULSE",
                AutoSize  = true,
                Left      = 24,
                Top       = 13,
                Font      = new System.Drawing.Font("Segoe UI", 16f, System.Drawing.FontStyle.Bold),
                ForeColor = AppColors.PlayerTextPrim
            };

            flowToolbar = new System.Windows.Forms.FlowLayoutPanel
            {
                Dock          = System.Windows.Forms.DockStyle.Right,
                AutoSize      = true,
                FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight,
                WrapContents  = false,
                BackColor     = System.Drawing.Color.Transparent,
                Padding       = new System.Windows.Forms.Padding(4, 10, 12, 8)
            };

            btnOpenFile     = MkBtn("📂 Archivo");  btnOpenFile.Click     += BtnOpenFile_Click;
            btnOpenFolder   = MkBtn("📁 Carpeta");  btnOpenFolder.Click   += BtnOpenFolder_Click;
            btnSavePlaylist = MkBtn("💾 Guardar");  btnSavePlaylist.Click += BtnSavePlaylist_Click;
            btnLoadPlaylist = MkBtn("📋 Cargar");   btnLoadPlaylist.Click += BtnLoadPlaylist_Click;
            btnClearList    = MkBtn("🗑 Limpiar");  btnClearList.Click    += BtnClearList_Click;

            // Botón Spotify (requiere click handler de lógica)
            var btnSpotify = MkBtn("🎵 Spotify");
            btnSpotify.Width     = 110;
            btnSpotify.ForeColor = AppColors.PlayerAccent;
            btnSpotify.FlatAppearance.BorderColor = AppColors.PlayerAccent;
            btnSpotify.Click += (s, e) => ShowSpotifyConfig();

            flowToolbar.Controls.AddRange(new System.Windows.Forms.Control[]
                { btnOpenFile, btnOpenFolder, btnSavePlaylist, btnLoadPlaylist, btnClearList, btnSpotify });
            pnlTopBar.Controls.Add(flowToolbar);
            pnlTopBar.Controls.Add(lblAppName);

            // Accent bar
            lblAccentBar = new System.Windows.Forms.Panel
            {
                Dock      = System.Windows.Forms.DockStyle.Top,
                Height    = 3,
                BackColor = AppColors.PlayerAccent
            };

            // ── LEFT PANEL ────────────────────────────────────────────────────
            pnlLeft = new System.Windows.Forms.Panel
            {
                Dock      = System.Windows.Forms.DockStyle.Left,
                Width     = 344,
                BackColor = AppColors.PlayerBgSide
            };

            pnlCover = new System.Windows.Forms.Panel
            {
                Left      = 22,
                Top       = 22,
                Width     = 300,
                Height    = 300,
                BackColor = System.Drawing.Color.Transparent
            };

            lblTitle = new System.Windows.Forms.Label
            {
                Left         = 22, Top = 336, Width = 300,
                Text         = "Sin canción",
                Font         = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold),
                ForeColor    = AppColors.PlayerTextPrim,
                AutoEllipsis = true,
                AutoSize     = false,
                Height       = 26
            };

            lblArtist = new System.Windows.Forms.Label
            {
                Left         = 22, Top = 364, Width = 300,
                Text         = "—",
                Font         = new System.Drawing.Font("Segoe UI", 10.5f),
                ForeColor    = AppColors.PlayerTextSec,
                AutoEllipsis = true,
                AutoSize     = false,
                Height       = 22
            };

            lblAlbum = new System.Windows.Forms.Label
            {
                Left         = 22, Top = 388, Width = 300,
                Text         = "",
                Font         = new System.Drawing.Font("Segoe UI", 9f),
                ForeColor    = AppColors.PlayerTextDim,
                AutoEllipsis = true,
                AutoSize     = false,
                Height       = 18
            };

            lblSpotifyBadge = new System.Windows.Forms.Label
            {
                Left         = 22, Top = 408, Width = 300, Height = 18,
                Text         = "",
                Font         = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold),
                ForeColor    = AppColors.PlayerAccent,
                AutoEllipsis = true,
                AutoSize     = false,
                Visible      = false
            };

            seekBar = new System.Windows.Forms.TrackBar
            {
                Left      = 14, Top = 430, Width = 314, Height = 30,
                Minimum   = 0, Maximum = 100,
                Enabled   = false,
                TickStyle = System.Windows.Forms.TickStyle.None,
                BackColor = AppColors.PlayerBgSide
            };

            lblNow = new System.Windows.Forms.Label
            {
                Left      = 14, Top = 458, Width = 48,
                Text      = "0:00",
                Font      = new System.Drawing.Font("Segoe UI", 8f),
                ForeColor = AppColors.PlayerTextDim
            };
            lblDuration = new System.Windows.Forms.Label
            {
                Left      = 280, Top = 458, Width = 50,
                Text      = "0:00",
                Font      = new System.Drawing.Font("Segoe UI", 8f),
                ForeColor = AppColors.PlayerTextDim,
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };

            pnlWave = new System.Windows.Forms.Panel
            {
                Left      = 22, Top = 478, Width = 300, Height = 36,
                BackColor = System.Drawing.Color.Transparent
            };

            pnlTransport = new System.Windows.Forms.Panel
            {
                Left      = 0, Top = 522, Width = 344, Height = 56,
                BackColor = System.Drawing.Color.Transparent
            };

            btnPrev      = MkIcon("⏮", 28,  7);  btnPrev.Click      += BtnPrev_Click;
            btnPlayPause = MkIcon("▶", 76,  4);  btnPlayPause.Click += BtnPlayPause_Click;
            btnPlayPause.Width     = 52;
            btnPlayPause.Height    = 48;
            btnPlayPause.Font      = new System.Drawing.Font("Segoe UI", 17f, System.Drawing.FontStyle.Bold);
            btnPlayPause.BackColor = AppColors.PlayerAccent;
            btnPlayPause.ForeColor = System.Drawing.Color.Black;
            btnNext      = MkIcon("⏭", 136, 7);  btnNext.Click      += BtnNext_Click;
            btnStop      = MkIcon("⏹", 184, 7);  btnStop.Click      += BtnStop_Click;

            foreach (var b in new[] { btnPrev, btnNext, btnStop })
            {
                b.BackColor = AppColors.PlayerBtnBg;
                b.ForeColor = AppColors.PlayerTextSec;
            }

            pnlTransport.Controls.AddRange(new System.Windows.Forms.Control[]
                { btnPrev, btnPlayPause, btnNext, btnStop });

            lblVolIcon = new System.Windows.Forms.Label
            {
                Left      = 14, Top = 588, Width = 26,
                Text      = "🔊",
                Font      = new System.Drawing.Font("Segoe UI", 11f),
                ForeColor = AppColors.PlayerTextSec,
                AutoSize  = false,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            volBar = new System.Windows.Forms.TrackBar
            {
                Left      = 42, Top = 586, Width = 184, Height = 28,
                Minimum   = 0, Maximum = 100, Value = 80,
                TickStyle = System.Windows.Forms.TickStyle.None,
                BackColor = AppColors.PlayerBgSide
            };
            lblVol = new System.Windows.Forms.Label
            {
                Left      = 228, Top = 588, Width = 42,
                Text      = "80%",
                Font      = new System.Drawing.Font("Segoe UI", 8.5f),
                ForeColor = AppColors.PlayerTextDim
            };

            btnShuffle = MkBtn("⇄ Aleatorio"); btnShuffle.Left = 14;  btnShuffle.Top = 626; btnShuffle.Width = 100; btnShuffle.Click += BtnShuffle_Click;
            btnLoop    = MkBtn("↻ Repetir");   btnLoop.Left    = 120; btnLoop.Top    = 626; btnLoop.Width    = 96;  btnLoop.Click    += BtnLoop_Click;
            btnLyrics  = MkBtn("🎵 Letra");    btnLyrics.Left  = 222; btnLyrics.Top  = 626; btnLyrics.Width  = 92;  btnLyrics.Click  += BtnLyrics_Click;

            pnlLeft.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                pnlCover, lblTitle, lblArtist, lblAlbum, lblSpotifyBadge,
                seekBar, lblNow, lblDuration, pnlWave,
                pnlTransport, lblVolIcon, volBar, lblVol,
                btnShuffle, btnLoop, btnLyrics
            });

            // ── MAIN PANEL ────────────────────────────────────────────────────
            pnlMain = new System.Windows.Forms.Panel
            {
                Dock      = System.Windows.Forms.DockStyle.Fill,
                BackColor = System.Drawing.Color.Transparent
            };

            lblTrackCount = new System.Windows.Forms.Label
            {
                Left      = 18, Top = 14,
                AutoSize  = true,
                Text      = "0 canciones",
                Font      = new System.Drawing.Font("Segoe UI", 8.5f),
                ForeColor = AppColors.PlayerTextDim
            };

            chTitle  = new System.Windows.Forms.ColumnHeader { Text = "♪  Título",  Width = 250 };
            chArtist = new System.Windows.Forms.ColumnHeader { Text = "Artista",    Width = 160 };
            chAlbum  = new System.Windows.Forms.ColumnHeader { Text = "Álbum",      Width = 160 };
            chDur    = new System.Windows.Forms.ColumnHeader { Text = "⏱",          Width = 58  };
            chPop    = new System.Windows.Forms.ColumnHeader { Text = "♥ Pop.",     Width = 68  };
            chYear   = new System.Windows.Forms.ColumnHeader { Text = "Año",        Width = 52  };

            lvPlaylist = new System.Windows.Forms.ListView
            {
                Left          = 18, Top = 36,
                Anchor        = System.Windows.Forms.AnchorStyles.Top    | System.Windows.Forms.AnchorStyles.Bottom |
                                System.Windows.Forms.AnchorStyles.Left   | System.Windows.Forms.AnchorStyles.Right,
                View          = System.Windows.Forms.View.Details,
                SmallImageList = imageList,
                FullRowSelect = true,
                MultiSelect   = false,
                GridLines     = false,
                HideSelection = false,
                BorderStyle   = System.Windows.Forms.BorderStyle.None,
                BackColor     = System.Drawing.Color.FromArgb(16, 16, 24),
                ForeColor     = System.Drawing.Color.FromArgb(210, 210, 230),
                Font          = new System.Drawing.Font("Segoe UI", 9.5f),
                OwnerDraw     = true
            };
            lvPlaylist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
                { chTitle, chArtist, chAlbum, chDur, chPop, chYear });
            lvPlaylist.DrawColumnHeader += LvPlaylist_DrawColumnHeader;
            lvPlaylist.DrawSubItem      += LvPlaylist_DrawSubItem;
            lvPlaylist.DrawItem         += LvPlaylist_DrawItem;
            lvPlaylist.DoubleClick      += (s, e) => PlaySelected();

            progressBar = new System.Windows.Forms.ProgressBar
            {
                Left      = 18, Height = 3,
                Style     = System.Windows.Forms.ProgressBarStyle.Continuous,
                Visible   = false,
                BackColor = AppColors.PlayerBgCard,
                ForeColor = AppColors.PlayerAccent,
                Anchor    = System.Windows.Forms.AnchorStyles.Bottom |
                            System.Windows.Forms.AnchorStyles.Left   |
                            System.Windows.Forms.AnchorStyles.Right
            };

            pnlLyrics = new System.Windows.Forms.Panel
            {
                Dock      = System.Windows.Forms.DockStyle.Bottom,
                Height    = 210,
                BackColor = System.Drawing.Color.FromArgb(13, 13, 20),
                Visible   = false
            };
            lblLyricsTitle = new System.Windows.Forms.Label
            {
                Text      = "♪   LETRA",
                Dock      = System.Windows.Forms.DockStyle.Top,
                Height    = 30,
                Font      = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Bold),
                ForeColor = AppColors.PlayerTextSec,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Padding   = new System.Windows.Forms.Padding(14, 0, 0, 0)
            };
            txtLyrics = new System.Windows.Forms.RichTextBox
            {
                Dock        = System.Windows.Forms.DockStyle.Fill,
                BackColor   = System.Drawing.Color.FromArgb(13, 13, 20),
                ForeColor   = System.Drawing.Color.FromArgb(195, 195, 220),
                BorderStyle = System.Windows.Forms.BorderStyle.None,
                Font        = new System.Drawing.Font("Segoe UI", 10.5f),
                ReadOnly    = true,
                ScrollBars  = System.Windows.Forms.RichTextBoxScrollBars.Vertical
            };
            pnlLyrics.Controls.Add(txtLyrics);
            pnlLyrics.Controls.Add(lblLyricsTitle);

            pnlMain.Controls.AddRange(new System.Windows.Forms.Control[]
                { pnlLyrics, progressBar, lvPlaylist, lblTrackCount });

            pnlMain.Resize += (s, e) =>
            {
                int w = pnlMain.Width - 36;
                lvPlaylist.Width  = w;
                lvPlaylist.Height = pnlMain.Height - 52 - (pnlLyrics.Visible ? pnlLyrics.Height : 0) - 10;
                progressBar.Top   = lvPlaylist.Top + lvPlaylist.Height + 4;
                progressBar.Width = w;
            };

            // Seek bar events
            seekBar.MouseDown += (s, e) => _seeking = true;
            seekBar.MouseUp   += SeekUp;
            seekBar.Scroll    += (s, e) => { if (_seeking) lblNow.Text = Fmt(seekBar.Value); };

            // Vol bar event
            volBar.Scroll += (s, e) =>
            {
                lblVol.Text = $"{volBar.Value}%";
                if (_out != null) _out.Volume = volBar.Value / 100f;
            };

            // Resize event
            Resize += (s, e) => { _bgBlur = null; Invalidate(); };

            // ── ENSAMBLADO ────────────────────────────────────────────────────
            SuspendLayout();
            Controls.Add(pnlMain);
            Controls.Add(pnlLeft);
            Controls.Add(lblAccentBar);
            Controls.Add(pnlTopBar);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}



