namespace ExploradorDeArchivos
{
    partial class FormGrabadorAudio
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._btnGrabar      = new System.Windows.Forms.Button();
            this._btnDetener     = new System.Windows.Forms.Button();
            this._btnReproducir  = new System.Windows.Forms.Button();
            this._btnGuardar     = new System.Windows.Forms.Button();
            this._btnCarpeta     = new System.Windows.Forms.Button();
            this._lblTitulo      = new System.Windows.Forms.Label();
            this._lblReloj       = new System.Windows.Forms.Label();
            this._lblArchivo     = new System.Windows.Forms.Label();
            this._lblEstado      = new System.Windows.Forms.Label();
            this._panelViz       = new System.Windows.Forms.Panel();
            this._timerUI        = new System.Windows.Forms.Timer(this.components);
            this._timerAnim      = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();

            // ── _lblTitulo ───────────────────────────────────────────────────
            this._lblTitulo.AutoSize  = true;
            this._lblTitulo.Font      = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this._lblTitulo.ForeColor = System.Drawing.Color.White;
            this._lblTitulo.Location  = new System.Drawing.Point(20, 18);
            this._lblTitulo.Name      = "_lblTitulo";
            this._lblTitulo.Text      = "🎙️  Grabador de audio";

            // ── _panelViz ────────────────────────────────────────────────────
            this._panelViz.BackColor = System.Drawing.Color.FromArgb(18, 18, 22);
            this._panelViz.Location  = new System.Drawing.Point(20, 58);
            this._panelViz.Name      = "_panelViz";
            this._panelViz.Size      = new System.Drawing.Size(400, 70);
            this._panelViz.TabIndex  = 0;

            // ── _lblReloj ────────────────────────────────────────────────────
            this._lblReloj.AutoSize  = true;
            this._lblReloj.Font      = new System.Drawing.Font("Consolas", 22F, System.Drawing.FontStyle.Bold);
            this._lblReloj.ForeColor = System.Drawing.Color.FromArgb(0, 200, 100);
            this._lblReloj.Location  = new System.Drawing.Point(170, 144);
            this._lblReloj.Name      = "_lblReloj";
            this._lblReloj.Text      = "00:00";

            // ── _lblEstado ───────────────────────────────────────────────────
            this._lblEstado.AutoSize  = false;
            this._lblEstado.Font      = new System.Drawing.Font("Segoe UI", 8.5F);
            this._lblEstado.ForeColor = System.Drawing.Color.FromArgb(160, 160, 160);
            this._lblEstado.Location  = new System.Drawing.Point(20, 186);
            this._lblEstado.Name      = "_lblEstado";
            this._lblEstado.Size      = new System.Drawing.Size(400, 22);
            this._lblEstado.Text      = "Listo para grabar";

            // ── _btnGrabar ───────────────────────────────────────────────────
            this._btnGrabar.BackColor                         = System.Drawing.Color.FromArgb(200, 40, 40);
            this._btnGrabar.Cursor                            = System.Windows.Forms.Cursors.Hand;
            this._btnGrabar.FlatStyle                         = System.Windows.Forms.FlatStyle.Flat;
            this._btnGrabar.FlatAppearance.BorderSize         = 0;
            this._btnGrabar.Font                              = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnGrabar.ForeColor                         = System.Drawing.Color.White;
            this._btnGrabar.Location                          = new System.Drawing.Point(20, 220);
            this._btnGrabar.Name                              = "_btnGrabar";
            this._btnGrabar.Size                              = new System.Drawing.Size(105, 38);
            this._btnGrabar.TabIndex                          = 1;
            this._btnGrabar.Text                              = "🔴  Grabar";

            // ── _btnDetener ──────────────────────────────────────────────────
            this._btnDetener.BackColor                         = System.Drawing.Color.FromArgb(70, 70, 78);
            this._btnDetener.Cursor                            = System.Windows.Forms.Cursors.Hand;
            this._btnDetener.Enabled                           = false;
            this._btnDetener.FlatStyle                         = System.Windows.Forms.FlatStyle.Flat;
            this._btnDetener.FlatAppearance.BorderSize         = 0;
            this._btnDetener.Font                              = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnDetener.ForeColor                         = System.Drawing.Color.White;
            this._btnDetener.Location                          = new System.Drawing.Point(130, 220);
            this._btnDetener.Name                              = "_btnDetener";
            this._btnDetener.Size                              = new System.Drawing.Size(105, 38);
            this._btnDetener.TabIndex                          = 2;
            this._btnDetener.Text                              = "⏹  Detener";

            // ── _btnReproducir ───────────────────────────────────────────────
            this._btnReproducir.BackColor                         = System.Drawing.Color.FromArgb(0, 122, 204);
            this._btnReproducir.Cursor                            = System.Windows.Forms.Cursors.Hand;
            this._btnReproducir.Enabled                           = false;
            this._btnReproducir.FlatStyle                         = System.Windows.Forms.FlatStyle.Flat;
            this._btnReproducir.FlatAppearance.BorderSize         = 0;
            this._btnReproducir.Font                              = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnReproducir.ForeColor                         = System.Drawing.Color.White;
            this._btnReproducir.Location                          = new System.Drawing.Point(240, 220);
            this._btnReproducir.Name                              = "_btnReproducir";
            this._btnReproducir.Size                              = new System.Drawing.Size(105, 38);
            this._btnReproducir.TabIndex                          = 3;
            this._btnReproducir.Text                              = "▶  Reproducir";

            // ── _btnGuardar ──────────────────────────────────────────────────
            this._btnGuardar.BackColor                         = System.Drawing.Color.FromArgb(34, 139, 60);
            this._btnGuardar.Cursor                            = System.Windows.Forms.Cursors.Hand;
            this._btnGuardar.Enabled                           = false;
            this._btnGuardar.FlatStyle                         = System.Windows.Forms.FlatStyle.Flat;
            this._btnGuardar.FlatAppearance.BorderSize         = 0;
            this._btnGuardar.Font                              = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnGuardar.ForeColor                         = System.Drawing.Color.White;
            this._btnGuardar.Location                          = new System.Drawing.Point(20, 280);
            this._btnGuardar.Name                              = "_btnGuardar";
            this._btnGuardar.Size                              = new System.Drawing.Size(105, 38);
            this._btnGuardar.TabIndex                          = 4;
            this._btnGuardar.Text                              = "💾  Guardar";

            // ── _btnCarpeta ──────────────────────────────────────────────────
            this._btnCarpeta.BackColor                         = System.Drawing.Color.FromArgb(70, 70, 78);
            this._btnCarpeta.Cursor                            = System.Windows.Forms.Cursors.Hand;
            this._btnCarpeta.FlatStyle                         = System.Windows.Forms.FlatStyle.Flat;
            this._btnCarpeta.FlatAppearance.BorderSize         = 0;
            this._btnCarpeta.Font                              = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._btnCarpeta.ForeColor                         = System.Drawing.Color.White;
            this._btnCarpeta.Location                          = new System.Drawing.Point(130, 280);
            this._btnCarpeta.Name                              = "_btnCarpeta";
            this._btnCarpeta.Size                              = new System.Drawing.Size(105, 38);
            this._btnCarpeta.TabIndex                          = 5;
            this._btnCarpeta.Text                              = "📁  Ver grabaciones";

            // ── _lblArchivo ──────────────────────────────────────────────────
            this._lblArchivo.AutoSize  = false;
            this._lblArchivo.Font      = new System.Drawing.Font("Segoe UI", 8F);
            this._lblArchivo.ForeColor = System.Drawing.Color.FromArgb(120, 200, 120);
            this._lblArchivo.Location  = new System.Drawing.Point(20, 348);
            this._lblArchivo.Name      = "_lblArchivo";
            this._lblArchivo.Size      = new System.Drawing.Size(400, 20);
            this._lblArchivo.Text      = "";

            // ── Timers ───────────────────────────────────────────────────────
            this._timerUI.Interval    = 1000;
            this._timerAnim.Interval  = 80;

            // ── FormGrabadorAudio ────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor           = System.Drawing.Color.FromArgb(28, 28, 32);
            this.ClientSize          = new System.Drawing.Size(460, 420);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this._lblTitulo, this._panelViz, this._lblReloj, this._lblEstado,
                this._btnGrabar, this._btnDetener, this._btnReproducir,
                this._btnGuardar, this._btnCarpeta, this._lblArchivo
            });
            this.Font            = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MaximumSize     = new System.Drawing.Size(600, 480);
            this.MinimumSize     = new System.Drawing.Size(400, 380);
            this.Name            = "FormGrabadorAudio";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text            = "🎙️ Grabador de audio";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Button  _btnGrabar;
        private System.Windows.Forms.Button  _btnDetener;
        private System.Windows.Forms.Button  _btnReproducir;
        private System.Windows.Forms.Button  _btnGuardar;
        private System.Windows.Forms.Button  _btnCarpeta;
        private System.Windows.Forms.Label   _lblTitulo;
        private System.Windows.Forms.Label   _lblReloj;
        private System.Windows.Forms.Label   _lblArchivo;
        private System.Windows.Forms.Label   _lblEstado;
        private System.Windows.Forms.Panel   _panelViz;
        private System.Windows.Forms.Timer   _timerUI;
        private System.Windows.Forms.Timer   _timerAnim;
    }
}


