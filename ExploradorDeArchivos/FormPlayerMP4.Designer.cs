namespace ExploradorDeArchivos
{
    partial class FormPlayerMP4
    {
        private System.ComponentModel.IContainer components = null;

        private LibVLCSharp.WinForms.VideoView videoView;
        private System.Windows.Forms.Panel     pnlTopBar;
        private System.Windows.Forms.Panel     pnlAccentLine;
        private System.Windows.Forms.Panel     pnlVideo;
        private System.Windows.Forms.Panel     pnlControls;

        private System.Windows.Forms.Label    lblTitle;
        private System.Windows.Forms.Label    lblNow;
        private System.Windows.Forms.Label    lblDuration;
        private System.Windows.Forms.Label    lblVol;

        private System.Windows.Forms.TrackBar seekBar;
        private System.Windows.Forms.TrackBar volBar;

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnPlayPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnMute;
        private System.Windows.Forms.Button btnFullScreen;

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer mouseIdleTimer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlTopBar = new System.Windows.Forms.Panel();
            this.pnlAccentLine = new System.Windows.Forms.Panel();
            this.pnlVideo = new System.Windows.Forms.Panel();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.videoView = new LibVLCSharp.WinForms.VideoView();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnOpen = new System.Windows.Forms.Button();
            this.lblNow = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lblVol = new System.Windows.Forms.Label();
            this.seekBar = new System.Windows.Forms.TrackBar();
            this.volBar = new System.Windows.Forms.TrackBar();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnPlayPause = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnMute = new System.Windows.Forms.Button();
            this.btnFullScreen = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.mouseIdleTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.seekBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.volBar)).BeginInit();
            this.pnlTopBar.SuspendLayout();
            this.pnlVideo.SuspendLayout();
            this.pnlControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // mouseIdleTimer
            // 
            this.mouseIdleTimer.Interval = 250;
            this.mouseIdleTimer.Tick += new System.EventHandler(this.MouseIdle_Tick);
            // 
            // pnlTopBar
            // 
            this.pnlTopBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(20)))));
            this.pnlTopBar.Controls.Add(this.lblTitle);
            this.pnlTopBar.Controls.Add(this.btnOpen);
            this.pnlTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopBar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopBar.Name = "pnlTopBar";
            this.pnlTopBar.Size = new System.Drawing.Size(1100, 46);
            this.pnlTopBar.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = false;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(245)))));
            this.lblTitle.Location = new System.Drawing.Point(18, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(600, 46);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "NOVA \u2014 Video Player";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.btnOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpen.FlatAppearance.BorderSize = 0;
            this.btnOpen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(68)))));
            this.btnOpen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(84)))));
            this.btnOpen.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnOpen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(225)))));
            this.btnOpen.Location = new System.Drawing.Point(956, 7);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(130, 32);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "\U0001f4c2  Abrir archivo";
            this.btnOpen.UseVisualStyleBackColor = false;
            this.btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // pnlAccentLine
            // 
            this.pnlAccentLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.pnlAccentLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAccentLine.Location = new System.Drawing.Point(0, 46);
            this.pnlAccentLine.Name = "pnlAccentLine";
            this.pnlAccentLine.Size = new System.Drawing.Size(1100, 3);
            this.pnlAccentLine.TabIndex = 1;
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(26)))));
            this.pnlControls.Controls.Add(this.lblNow);
            this.pnlControls.Controls.Add(this.seekBar);
            this.pnlControls.Controls.Add(this.lblDuration);
            this.pnlControls.Controls.Add(this.btnPrev);
            this.pnlControls.Controls.Add(this.btnPlayPause);
            this.pnlControls.Controls.Add(this.btnNext);
            this.pnlControls.Controls.Add(this.btnStop);
            this.pnlControls.Controls.Add(this.btnMute);
            this.pnlControls.Controls.Add(this.volBar);
            this.pnlControls.Controls.Add(this.lblVol);
            this.pnlControls.Controls.Add(this.btnFullScreen);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControls.Location = new System.Drawing.Point(0, 582);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(1100, 98);
            this.pnlControls.TabIndex = 2;
            this.pnlControls.Resize += new System.EventHandler(this.PnlControls_Resize);
            // 
            // lblNow
            // 
            this.lblNow.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblNow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(160)))));
            this.lblNow.Location = new System.Drawing.Point(12, 12);
            this.lblNow.Name = "lblNow";
            this.lblNow.Size = new System.Drawing.Size(48, 20);
            this.lblNow.TabIndex = 0;
            this.lblNow.Text = "0:00";
            this.lblNow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // seekBar
            // 
            this.seekBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(26)))));
            this.seekBar.Location = new System.Drawing.Point(64, 8);
            this.seekBar.Maximum = 100;
            this.seekBar.Minimum = 0;
            this.seekBar.Name = "seekBar";
            this.seekBar.Size = new System.Drawing.Size(920, 26);
            this.seekBar.TabIndex = 1;
            this.seekBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.seekBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SeekBar_MouseDown);
            this.seekBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SeekBar_MouseUp);
            this.seekBar.Scroll += new System.EventHandler(this.SeekBar_Scroll);
            // 
            // lblDuration
            // 
            this.lblDuration.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblDuration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(160)))));
            this.lblDuration.Location = new System.Drawing.Point(988, 12);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(54, 20);
            this.lblDuration.TabIndex = 2;
            this.lblDuration.Text = "0:00";
            this.lblDuration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrev
            // 
            this.btnPrev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.btnPrev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(68)))));
            this.btnPrev.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(84)))));
            this.btnPrev.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnPrev.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(225)))));
            this.btnPrev.Location = new System.Drawing.Point(14, 46);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(42, 42);
            this.btnPrev.TabIndex = 3;
            this.btnPrev.Text = "\u23ee";
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.BtnPrev_Click);
            // 
            // btnPlayPause
            // 
            this.btnPlayPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.btnPlayPause.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPlayPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlayPause.FlatAppearance.BorderSize = 0;
            this.btnPlayPause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(120)))), ((int)(((byte)(255)))));
            this.btnPlayPause.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(82)))), ((int)(((byte)(200)))));
            this.btnPlayPause.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.btnPlayPause.ForeColor = System.Drawing.Color.White;
            this.btnPlayPause.Location = new System.Drawing.Point(62, 43);
            this.btnPlayPause.Name = "btnPlayPause";
            this.btnPlayPause.Size = new System.Drawing.Size(52, 48);
            this.btnPlayPause.TabIndex = 4;
            this.btnPlayPause.Text = "\u25b6";
            this.btnPlayPause.UseVisualStyleBackColor = false;
            this.btnPlayPause.Click += new System.EventHandler(this.BtnPlayPause_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(68)))));
            this.btnNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(84)))));
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnNext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(225)))));
            this.btnNext.Location = new System.Drawing.Point(122, 46);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(42, 42);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = "\u23ed";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(68)))));
            this.btnStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(84)))));
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnStop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(225)))));
            this.btnStop.Location = new System.Drawing.Point(170, 46);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(42, 42);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "\u23f9";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // btnMute
            // 
            this.btnMute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.btnMute.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMute.FlatAppearance.BorderSize = 0;
            this.btnMute.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(68)))));
            this.btnMute.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(84)))));
            this.btnMute.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnMute.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(225)))));
            this.btnMute.Location = new System.Drawing.Point(874, 46);
            this.btnMute.Name = "btnMute";
            this.btnMute.Size = new System.Drawing.Size(42, 42);
            this.btnMute.TabIndex = 7;
            this.btnMute.Text = "\U0001f50a";
            this.btnMute.UseVisualStyleBackColor = false;
            this.btnMute.Click += new System.EventHandler(this.BtnMute_Click);
            // 
            // volBar
            // 
            this.volBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(26)))));
            this.volBar.Location = new System.Drawing.Point(920, 53);
            this.volBar.Maximum = 100;
            this.volBar.Minimum = 0;
            this.volBar.Name = "volBar";
            this.volBar.Size = new System.Drawing.Size(110, 26);
            this.volBar.TabIndex = 8;
            this.volBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.volBar.Value = 70;
            this.volBar.Scroll += new System.EventHandler(this.VolumeBar_Scroll);
            // 
            // lblVol
            // 
            this.lblVol.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblVol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(160)))));
            this.lblVol.Location = new System.Drawing.Point(1034, 58);
            this.lblVol.Name = "lblVol";
            this.lblVol.Size = new System.Drawing.Size(36, 18);
            this.lblVol.TabIndex = 9;
            this.lblVol.Text = "70%";
            this.lblVol.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnFullScreen
            // 
            this.btnFullScreen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(46)))));
            this.btnFullScreen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFullScreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFullScreen.FlatAppearance.BorderSize = 0;
            this.btnFullScreen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(68)))));
            this.btnFullScreen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(84)))));
            this.btnFullScreen.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnFullScreen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(225)))));
            this.btnFullScreen.Location = new System.Drawing.Point(1048, 46);
            this.btnFullScreen.Name = "btnFullScreen";
            this.btnFullScreen.Size = new System.Drawing.Size(42, 42);
            this.btnFullScreen.TabIndex = 10;
            this.btnFullScreen.Text = "\u26f6";
            this.btnFullScreen.UseVisualStyleBackColor = false;
            this.btnFullScreen.Click += new System.EventHandler(this.BtnFullScreen_Click);
            // 
            // pnlVideo
            // 
            this.pnlVideo.BackColor = System.Drawing.Color.Black;
            this.pnlVideo.Controls.Add(this.videoView);
            this.pnlVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVideo.Location = new System.Drawing.Point(0, 49);
            this.pnlVideo.Name = "pnlVideo";
            this.pnlVideo.Size = new System.Drawing.Size(1100, 533);
            this.pnlVideo.TabIndex = 3;
            // 
            // videoView
            // 
            this.videoView.BackColor = System.Drawing.Color.Black;
            this.videoView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoView.Location = new System.Drawing.Point(0, 0);
            this.videoView.MediaPlayer = null;
            this.videoView.Name = "videoView";
            this.videoView.Size = new System.Drawing.Size(1100, 533);
            this.videoView.TabIndex = 0;
            // 
            // FormPlayerMP4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(14)))));
            this.ClientSize = new System.Drawing.Size(1100, 680);
            this.Controls.Add(this.pnlVideo);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlAccentLine);
            this.Controls.Add(this.pnlTopBar);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(780, 500);
            this.Name = "FormPlayerMP4";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NOVA \u2014 Video Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPlayerMP4_FormClosing);
            this.Load += new System.EventHandler(this.FormPlayerMP4_Load);
            ((System.ComponentModel.ISupportInitialize)(this.seekBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.volBar)).EndInit();
            this.pnlTopBar.ResumeLayout(false);
            this.pnlVideo.ResumeLayout(false);
            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}


