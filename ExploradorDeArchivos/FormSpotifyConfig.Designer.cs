namespace ExploradorDeArchivos
{
    partial class FormSpotifyConfig
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
            this.pnlTop         = new System.Windows.Forms.Panel();
            this.lblHeader      = new System.Windows.Forms.Label();
            this.lblSub         = new System.Windows.Forms.Label();
            this.lblClientId    = new System.Windows.Forms.Label();
            this.txtClientId    = new System.Windows.Forms.TextBox();
            this.lblClientSecret = new System.Windows.Forms.Label();
            this.txtClientSecret = new System.Windows.Forms.TextBox();
            this.chkShow        = new System.Windows.Forms.CheckBox();
            this.lnkHelp        = new System.Windows.Forms.LinkLabel();
            this.btnCancel      = new System.Windows.Forms.Button();
            this.btnSave        = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // ── pnlTop (barra verde superior) ────────────────────────────────
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(30, 215, 96);
            this.pnlTop.Dock      = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height    = 4;
            this.pnlTop.Name      = "pnlTop";
            this.pnlTop.TabStop   = false;

            // ── lblHeader ────────────────────────────────────────────────────
            this.lblHeader.AutoSize  = false;
            this.lblHeader.Font      = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(235, 235, 245);
            this.lblHeader.Location  = new System.Drawing.Point(36, 24);
            this.lblHeader.Name      = "lblHeader";
            this.lblHeader.Size      = new System.Drawing.Size(440, 34);
            this.lblHeader.Text      = "🎵  Conectar con Spotify";

            // ── lblSub ───────────────────────────────────────────────────────
            this.lblSub.AutoSize  = false;
            this.lblSub.Font      = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSub.ForeColor = System.Drawing.Color.FromArgb(140, 140, 168);
            this.lblSub.Location  = new System.Drawing.Point(36, 62);
            this.lblSub.Name      = "lblSub";
            this.lblSub.Size      = new System.Drawing.Size(440, 44);
            this.lblSub.Text      = "Ingresa tus credenciales de la Spotify Developer Dashboard.\r\nSe guardan localmente en tu perfil de usuario.";

            // ── lblClientId ──────────────────────────────────────────────────
            this.lblClientId.AutoSize  = true;
            this.lblClientId.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblClientId.ForeColor = System.Drawing.Color.FromArgb(140, 140, 168);
            this.lblClientId.Location  = new System.Drawing.Point(36, 122);
            this.lblClientId.Name      = "lblClientId";
            this.lblClientId.Text      = "Client ID";

            // ── txtClientId ──────────────────────────────────────────────────
            this.txtClientId.BackColor   = System.Drawing.Color.FromArgb(22, 22, 34);
            this.txtClientId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClientId.Font        = new System.Drawing.Font("Consolas", 10.5F);
            this.txtClientId.ForeColor   = System.Drawing.Color.FromArgb(235, 235, 245);
            this.txtClientId.Location    = new System.Drawing.Point(36, 144);
            this.txtClientId.Name        = "txtClientId";
            this.txtClientId.Size        = new System.Drawing.Size(440, 36);
            this.txtClientId.TabIndex    = 0;

            // ── lblClientSecret ──────────────────────────────────────────────
            this.lblClientSecret.AutoSize  = true;
            this.lblClientSecret.Font      = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblClientSecret.ForeColor = System.Drawing.Color.FromArgb(140, 140, 168);
            this.lblClientSecret.Location  = new System.Drawing.Point(36, 200);
            this.lblClientSecret.Name      = "lblClientSecret";
            this.lblClientSecret.Text      = "Client Secret";

            // ── txtClientSecret ──────────────────────────────────────────────
            this.txtClientSecret.BackColor            = System.Drawing.Color.FromArgb(22, 22, 34);
            this.txtClientSecret.BorderStyle          = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClientSecret.Font                 = new System.Drawing.Font("Consolas", 10.5F);
            this.txtClientSecret.ForeColor            = System.Drawing.Color.FromArgb(235, 235, 245);
            this.txtClientSecret.Location             = new System.Drawing.Point(36, 222);
            this.txtClientSecret.Name                 = "txtClientSecret";
            this.txtClientSecret.Size                 = new System.Drawing.Size(440, 36);
            this.txtClientSecret.TabIndex             = 1;
            this.txtClientSecret.UseSystemPasswordChar = true;

            // ── chkShow ──────────────────────────────────────────────────────
            this.chkShow.AutoSize  = true;
            this.chkShow.Cursor    = System.Windows.Forms.Cursors.Hand;
            this.chkShow.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.chkShow.ForeColor = System.Drawing.Color.FromArgb(140, 140, 168);
            this.chkShow.Location  = new System.Drawing.Point(36, 264);
            this.chkShow.Name      = "chkShow";
            this.chkShow.TabIndex  = 2;
            this.chkShow.Text      = "Mostrar secret";

            // ── lnkHelp ──────────────────────────────────────────────────────
            this.lnkHelp.AutoSize  = false;
            this.lnkHelp.Font      = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lnkHelp.LinkColor = System.Drawing.Color.FromArgb(30, 215, 96);
            this.lnkHelp.Location  = new System.Drawing.Point(36, 292);
            this.lnkHelp.Name      = "lnkHelp";
            this.lnkHelp.Size      = new System.Drawing.Size(440, 20);
            this.lnkHelp.TabIndex  = 5;
            this.lnkHelp.Text      = "¿Cómo obtener mis credenciales? → developer.spotify.com/dashboard";

            // ── btnCancel ────────────────────────────────────────────────────
            this.btnCancel.BackColor                         = System.Drawing.Color.FromArgb(30, 30, 46);
            this.btnCancel.Cursor                            = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatStyle                         = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderColor        = System.Drawing.Color.FromArgb(55, 55, 80);
            this.btnCancel.Font                              = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnCancel.ForeColor                         = System.Drawing.Color.FromArgb(140, 140, 168);
            this.btnCancel.Location                          = new System.Drawing.Point(36, 336);
            this.btnCancel.Name                              = "btnCancel";
            this.btnCancel.Size                              = new System.Drawing.Size(120, 38);
            this.btnCancel.TabIndex                          = 4;
            this.btnCancel.Text                              = "Cancelar";

            // ── btnSave ──────────────────────────────────────────────────────
            this.btnSave.BackColor                         = System.Drawing.Color.FromArgb(30, 215, 96);
            this.btnSave.Cursor                            = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatStyle                         = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize         = 0;
            this.btnSave.Font                              = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor                         = System.Drawing.Color.Black;
            this.btnSave.Location                          = new System.Drawing.Point(172, 336);
            this.btnSave.Name                              = "btnSave";
            this.btnSave.Size                              = new System.Drawing.Size(200, 38);
            this.btnSave.TabIndex                          = 3;
            this.btnSave.Text                              = "✓  Guardar y conectar";

            // ── FormSpotifyConfig ────────────────────────────────────────────
            this.AcceptButton        = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor           = System.Drawing.Color.FromArgb(12, 12, 18);
            this.CancelButton        = this.btnCancel;
            this.ClientSize          = new System.Drawing.Size(520, 430);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.pnlTop, this.lblHeader, this.lblSub,
                this.lblClientId,     this.txtClientId,
                this.lblClientSecret, this.txtClientSecret,
                this.chkShow, this.lnkHelp,
                this.btnCancel, this.btnSave
            });
            this.Font            = new System.Drawing.Font("Segoe UI", 9.5F);
            this.ForeColor       = System.Drawing.Color.FromArgb(235, 235, 245);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MaximumSize     = new System.Drawing.Size(520, 430);
            this.MinimizeBox     = false;
            this.MinimumSize     = new System.Drawing.Size(520, 430);
            this.Name            = "FormSpotifyConfig";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text            = "Configuración de Spotify";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Panel     pnlTop;
        private System.Windows.Forms.Label     lblHeader;
        private System.Windows.Forms.Label     lblSub;
        private System.Windows.Forms.Label     lblClientId;
        private System.Windows.Forms.TextBox   txtClientId;
        private System.Windows.Forms.Label     lblClientSecret;
        private System.Windows.Forms.TextBox   txtClientSecret;
        private System.Windows.Forms.CheckBox  chkShow;
        private System.Windows.Forms.LinkLabel lnkHelp;
        private System.Windows.Forms.Button    btnCancel;
        private System.Windows.Forms.Button    btnSave;
    }
}


