namespace ExploradorDeArchivos
{
    partial class FormPropiedadesCamara
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

            // ── Controles ─────────────────────────────────────────────────
            _panelTop       = new System.Windows.Forms.Panel();
            _lblTitulo      = new System.Windows.Forms.Label();
            _lblSub         = new System.Windows.Forms.Label();

            _grpInfo        = new System.Windows.Forms.GroupBox();
            _lblDispositivo = new System.Windows.Forms.Label();
            _lblDispositivoVal = new System.Windows.Forms.Label();
            _lblResolucion  = new System.Windows.Forms.Label();
            _lblResolucionVal = new System.Windows.Forms.Label();
            _lblFps         = new System.Windows.Forms.Label();
            _lblFpsVal      = new System.Windows.Forms.Label();

            _grpAjustes     = new System.Windows.Forms.GroupBox();
            _lblBrillo      = new System.Windows.Forms.Label();
            _trackBrillo    = new System.Windows.Forms.TrackBar();
            _lblBrilloNum   = new System.Windows.Forms.Label();
            _lblContraste   = new System.Windows.Forms.Label();
            _trackContraste = new System.Windows.Forms.TrackBar();
            _lblContrasteNum = new System.Windows.Forms.Label();

            _panelBotones   = new System.Windows.Forms.Panel();
            _btnAceptar     = new System.Windows.Forms.Button();
            _btnCancelar    = new System.Windows.Forms.Button();

            this.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_trackBrillo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_trackContraste).BeginInit();
            _panelTop.SuspendLayout();
            _grpInfo.SuspendLayout();
            _grpAjustes.SuspendLayout();
            _panelBotones.SuspendLayout();

            // ── Panel superior ────────────────────────────────────────────
            _panelTop.Dock      = System.Windows.Forms.DockStyle.Top;
            _panelTop.Height    = 62;
            _panelTop.BackColor = System.Drawing.Color.FromArgb(28, 28, 38);
            _panelTop.Padding   = new System.Windows.Forms.Padding(16, 8, 16, 8);

            _lblTitulo.Dock      = System.Windows.Forms.DockStyle.Top;
            _lblTitulo.Height    = 28;
            _lblTitulo.Text      = "📷  Propiedades de cámara";
            _lblTitulo.Font      = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            _lblTitulo.ForeColor = System.Drawing.Color.White;

            _lblSub.Dock      = System.Windows.Forms.DockStyle.Fill;
            _lblSub.Text      = "Información del dispositivo y ajustes de imagen";
            _lblSub.Font      = new System.Drawing.Font("Segoe UI", 8.5F);
            _lblSub.ForeColor = System.Drawing.Color.FromArgb(150, 150, 170);

            _panelTop.Controls.Add(_lblSub);
            _panelTop.Controls.Add(_lblTitulo);

            // ── GroupBox Información ──────────────────────────────────────
            _grpInfo.Text      = "Información del dispositivo";
            _grpInfo.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            _grpInfo.ForeColor = System.Drawing.Color.FromArgb(80, 140, 220);
            _grpInfo.Location  = new System.Drawing.Point(16, 74);
            _grpInfo.Size      = new System.Drawing.Size(392, 110);

            AgregarFila(_grpInfo, _lblDispositivo,  _lblDispositivoVal,  "Dispositivo:",  20);
            AgregarFila(_grpInfo, _lblResolucion,   _lblResolucionVal,   "Resolución:",   50);
            AgregarFila(_grpInfo, _lblFps,          _lblFpsVal,          "Velocidad:",    80);

            // ── GroupBox Ajustes ──────────────────────────────────────────
            _grpAjustes.Text      = "Ajustes de imagen";
            _grpAjustes.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            _grpAjustes.ForeColor = System.Drawing.Color.FromArgb(80, 140, 220);
            _grpAjustes.Location  = new System.Drawing.Point(16, 196);
            _grpAjustes.Size      = new System.Drawing.Size(392, 110);

            // Brillo
            _lblBrillo.Text      = "Brillo:";
            _lblBrillo.Location  = new System.Drawing.Point(10, 28);
            _lblBrillo.Size      = new System.Drawing.Size(70, 22);
            _lblBrillo.Font      = new System.Drawing.Font("Segoe UI", 9F);
            _lblBrillo.ForeColor = System.Drawing.Color.FromArgb(200, 200, 210);

            _trackBrillo.Location = new System.Drawing.Point(86, 22);
            _trackBrillo.Size     = new System.Drawing.Size(260, 32);
            _trackBrillo.Minimum  = 0;
            _trackBrillo.Maximum  = 100;
            _trackBrillo.TickFrequency = 10;
            _trackBrillo.Scroll  += TrackBrillo_Scroll;

            _lblBrilloNum.Location  = new System.Drawing.Point(352, 28);
            _lblBrilloNum.Size      = new System.Drawing.Size(36, 22);
            _lblBrilloNum.Font      = new System.Drawing.Font("Consolas", 9F);
            _lblBrilloNum.ForeColor = System.Drawing.Color.FromArgb(200, 200, 210);
            _lblBrilloNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // Contraste
            _lblContraste.Text      = "Contraste:";
            _lblContraste.Location  = new System.Drawing.Point(10, 70);
            _lblContraste.Size      = new System.Drawing.Size(70, 22);
            _lblContraste.Font      = new System.Drawing.Font("Segoe UI", 9F);
            _lblContraste.ForeColor = System.Drawing.Color.FromArgb(200, 200, 210);

            _trackContraste.Location = new System.Drawing.Point(86, 64);
            _trackContraste.Size     = new System.Drawing.Size(260, 32);
            _trackContraste.Minimum  = 0;
            _trackContraste.Maximum  = 100;
            _trackContraste.TickFrequency = 10;
            _trackContraste.Scroll  += TrackContraste_Scroll;

            _lblContrasteNum.Location  = new System.Drawing.Point(352, 70);
            _lblContrasteNum.Size      = new System.Drawing.Size(36, 22);
            _lblContrasteNum.Font      = new System.Drawing.Font("Consolas", 9F);
            _lblContrasteNum.ForeColor = System.Drawing.Color.FromArgb(200, 200, 210);
            _lblContrasteNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            _grpAjustes.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                _lblBrillo,  _trackBrillo,  _lblBrilloNum,
                _lblContraste, _trackContraste, _lblContrasteNum
            });

            // ── Panel botones ─────────────────────────────────────────────
            _panelBotones.Dock      = System.Windows.Forms.DockStyle.Bottom;
            _panelBotones.Height    = 56;
            _panelBotones.BackColor = System.Drawing.Color.FromArgb(22, 22, 30);

            _btnAceptar.Text      = "✓  Aceptar";
            _btnAceptar.Location  = new System.Drawing.Point(196, 10);
            _btnAceptar.Size      = new System.Drawing.Size(110, 36);
            _btnAceptar.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            _btnAceptar.ForeColor = System.Drawing.Color.White;
            _btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnAceptar.FlatAppearance.BorderSize = 0;
            _btnAceptar.Font      = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            _btnAceptar.Cursor    = System.Windows.Forms.Cursors.Hand;
            _btnAceptar.Click    += BtnAceptar_Click;

            _btnCancelar.Text      = "Cancelar";
            _btnCancelar.Location  = new System.Drawing.Point(316, 10);
            _btnCancelar.Size      = new System.Drawing.Size(90, 36);
            _btnCancelar.BackColor = System.Drawing.Color.FromArgb(60, 60, 72);
            _btnCancelar.ForeColor = System.Drawing.Color.White;
            _btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            _btnCancelar.FlatAppearance.BorderSize = 0;
            _btnCancelar.Font      = new System.Drawing.Font("Segoe UI", 9.5F);
            _btnCancelar.Cursor    = System.Windows.Forms.Cursors.Hand;
            _btnCancelar.Click    += BtnCancelar_Click;

            _panelBotones.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                _btnAceptar, _btnCancelar
            });

            // ── Form ──────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor     = System.Drawing.Color.FromArgb(18, 18, 26);
            this.ClientSize    = new System.Drawing.Size(424, 370);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox   = false;
            this.MinimizeBox   = false;
            this.Name          = "FormPropiedadesCamara";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text          = "Propiedades de cámara";

            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                _panelTop, _grpInfo, _grpAjustes, _panelBotones
            });

            ((System.ComponentModel.ISupportInitialize)_trackBrillo).EndInit();
            ((System.ComponentModel.ISupportInitialize)_trackContraste).EndInit();
            _panelTop.ResumeLayout(false);
            _grpInfo.ResumeLayout(false);
            _grpAjustes.ResumeLayout(false);
            _grpAjustes.PerformLayout();
            _panelBotones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        // Helper para filas info
        private static void AgregarFila(System.Windows.Forms.GroupBox gb,
            System.Windows.Forms.Label lbl, System.Windows.Forms.Label val,
            string texto, int top)
        {
            lbl.Text      = texto;
            lbl.Location  = new System.Drawing.Point(10, top);
            lbl.Size      = new System.Drawing.Size(90, 22);
            lbl.Font      = new System.Drawing.Font("Segoe UI", 9F);
            lbl.ForeColor = System.Drawing.Color.FromArgb(140, 140, 160);

            val.Text      = "—";
            val.Location  = new System.Drawing.Point(106, top);
            val.Size      = new System.Drawing.Size(270, 22);
            val.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            val.ForeColor = System.Drawing.Color.FromArgb(210, 220, 240);

            gb.Controls.Add(lbl);
            gb.Controls.Add(val);
        }

        #endregion

        // ── Campos ────────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel    _panelTop;
        private System.Windows.Forms.Label    _lblTitulo;
        private System.Windows.Forms.Label    _lblSub;

        private System.Windows.Forms.GroupBox _grpInfo;
        private System.Windows.Forms.Label    _lblDispositivo;
        private System.Windows.Forms.Label    _lblDispositivoVal;
        private System.Windows.Forms.Label    _lblResolucion;
        private System.Windows.Forms.Label    _lblResolucionVal;
        private System.Windows.Forms.Label    _lblFps;
        private System.Windows.Forms.Label    _lblFpsVal;

        private System.Windows.Forms.GroupBox  _grpAjustes;
        private System.Windows.Forms.Label     _lblBrillo;
        private System.Windows.Forms.TrackBar  _trackBrillo;
        private System.Windows.Forms.Label     _lblBrilloNum;
        private System.Windows.Forms.Label     _lblContraste;
        private System.Windows.Forms.TrackBar  _trackContraste;
        private System.Windows.Forms.Label     _lblContrasteNum;

        private System.Windows.Forms.Panel  _panelBotones;
        private System.Windows.Forms.Button _btnAceptar;
        private System.Windows.Forms.Button _btnCancelar;
    }
}


