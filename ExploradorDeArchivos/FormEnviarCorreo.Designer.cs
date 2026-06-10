namespace ExploradorDeArchivos
{
    partial class FormEnviarCorreo
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this._lblArchivo = new System.Windows.Forms.Label();
            this._lblPara = new System.Windows.Forms.Label();
            this._txtPara = new System.Windows.Forms.TextBox();
            this._lblAsunto = new System.Windows.Forms.Label();
            this._txtAsunto = new System.Windows.Forms.TextBox();
            this._lblMensaje = new System.Windows.Forms.Label();
            this._txtMensaje = new System.Windows.Forms.TextBox();
            this._lblAviso = new System.Windows.Forms.Label();
            this._btnEnviar = new System.Windows.Forms.Button();
            this._btnCancelar = new System.Windows.Forms.Button();
            this._panelTop = new System.Windows.Forms.Panel();
            this._panelBottom = new System.Windows.Forms.Panel();
            this._panelTop.SuspendLayout();
            this._panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panelTop
            // 
            this._panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
            this._panelTop.Controls.Add(this._lblArchivo);
            this._panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelTop.Location = new System.Drawing.Point(0, 0);
            this._panelTop.Name = "_panelTop";
            this._panelTop.Padding = new System.Windows.Forms.Padding(16, 0, 16, 0);
            this._panelTop.Size = new System.Drawing.Size(484, 52);
            this._panelTop.TabIndex = 0;
            // 
            // _lblArchivo
            // 
            this._lblArchivo.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblArchivo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblArchivo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
            this._lblArchivo.Location = new System.Drawing.Point(16, 0);
            this._lblArchivo.Name = "_lblArchivo";
            this._lblArchivo.Size = new System.Drawing.Size(452, 52);
            this._lblArchivo.TabIndex = 0;
            this._lblArchivo.Text = "📎  Adjunto: archivo.txt";
            this._lblArchivo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lblPara
            // 
            this._lblPara.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._lblPara.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this._lblPara.Location = new System.Drawing.Point(16, 69);
            this._lblPara.Name = "_lblPara";
            this._lblPara.Size = new System.Drawing.Size(76, 22);
            this._lblPara.TabIndex = 1;
            this._lblPara.Text = "Para:";
            // 
            // _txtPara
            // 
            this._txtPara.BackColor = System.Drawing.Color.White;
            this._txtPara.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._txtPara.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtPara.Location = new System.Drawing.Point(96, 64);
            this._txtPara.Name = "_txtPara";
            this._txtPara.PlaceholderText = "destinatario@ejemplo.com";
            this._txtPara.Size = new System.Drawing.Size(360, 24);
            this._txtPara.TabIndex = 2;
            // 
            // _lblAsunto
            // 
            this._lblAsunto.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._lblAsunto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this._lblAsunto.Location = new System.Drawing.Point(16, 109);
            this._lblAsunto.Name = "_lblAsunto";
            this._lblAsunto.Size = new System.Drawing.Size(76, 22);
            this._lblAsunto.TabIndex = 3;
            this._lblAsunto.Text = "Asunto:";
            // 
            // _txtAsunto
            // 
            this._txtAsunto.BackColor = System.Drawing.Color.White;
            this._txtAsunto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._txtAsunto.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtAsunto.Location = new System.Drawing.Point(96, 104);
            this._txtAsunto.Name = "_txtAsunto";
            this._txtAsunto.Size = new System.Drawing.Size(360, 24);
            this._txtAsunto.TabIndex = 4;
            // 
            // _lblMensaje
            // 
            this._lblMensaje.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._lblMensaje.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this._lblMensaje.Location = new System.Drawing.Point(16, 149);
            this._lblMensaje.Name = "_lblMensaje";
            this._lblMensaje.Size = new System.Drawing.Size(76, 22);
            this._lblMensaje.TabIndex = 5;
            this._lblMensaje.Text = "Mensaje:";
            // 
            // _txtMensaje
            // 
            this._txtMensaje.AcceptsReturn = true;
            this._txtMensaje.BackColor = System.Drawing.Color.White;
            this._txtMensaje.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._txtMensaje.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this._txtMensaje.Location = new System.Drawing.Point(96, 144);
            this._txtMensaje.Multiline = true;
            this._txtMensaje.Name = "_txtMensaje";
            this._txtMensaje.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtMensaje.Size = new System.Drawing.Size(360, 130);
            this._txtMensaje.TabIndex = 6;
            // 
            // _lblAviso
            // 
            this._lblAviso.Font = new System.Drawing.Font("Segoe UI", 8F);
            this._lblAviso.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this._lblAviso.Location = new System.Drawing.Point(16, 290);
            this._lblAviso.Name = "_lblAviso";
            this._lblAviso.Size = new System.Drawing.Size(456, 36);
            this._lblAviso.TabIndex = 7;
            this._lblAviso.Text = "ⓘ  Al hacer clic en Enviar se abrirá tu cliente de correo predeterminado\r\n   (Out" +
    "look, Thunderbird, etc.) con los datos precargados.";
            // 
            // _panelBottom
            // 
            this._panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            this._panelBottom.Controls.Add(this._btnEnviar);
            this._panelBottom.Controls.Add(this._btnCancelar);
            this._panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._panelBottom.Location = new System.Drawing.Point(0, 383);
            this._panelBottom.Name = "_panelBottom";
            this._panelBottom.Size = new System.Drawing.Size(484, 58);
            this._panelBottom.TabIndex = 8;
            // 
            // _btnEnviar
            // 
            this._btnEnviar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this._btnEnviar.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnEnviar.FlatAppearance.BorderSize = 0;
            this._btnEnviar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnEnviar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnEnviar.ForeColor = System.Drawing.Color.White;
            this._btnEnviar.Location = new System.Drawing.Point(254, 12);
            this._btnEnviar.Name = "_btnEnviar";
            this._btnEnviar.Size = new System.Drawing.Size(110, 36);
            this._btnEnviar.TabIndex = 0;
            this._btnEnviar.Text = "📧  Enviar";
            this._btnEnviar.UseVisualStyleBackColor = false;
            this._btnEnviar.Click += new System.EventHandler(this.BtnEnviar_Click);
            // 
            // _btnCancelar
            // 
            this._btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(110)))));
            this._btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnCancelar.FlatAppearance.BorderSize = 0;
            this._btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnCancelar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this._btnCancelar.ForeColor = System.Drawing.Color.White;
            this._btnCancelar.Location = new System.Drawing.Point(374, 12);
            this._btnCancelar.Name = "_btnCancelar";
            this._btnCancelar.Size = new System.Drawing.Size(90, 36);
            this._btnCancelar.TabIndex = 1;
            this._btnCancelar.Text = "Cancelar";
            this._btnCancelar.UseVisualStyleBackColor = false;
            this._btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            // 
            // FormEnviarCorreo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(484, 441);
            this.Controls.Add(this._panelBottom);
            this.Controls.Add(this._lblAviso);
            this.Controls.Add(this._txtMensaje);
            this.Controls.Add(this._lblMensaje);
            this.Controls.Add(this._txtAsunto);
            this.Controls.Add(this._lblAsunto);
            this.Controls.Add(this._txtPara);
            this.Controls.Add(this._lblPara);
            this.Controls.Add(this._panelTop);
            // Establecer el control activo después de añadir todos los controles al formulario
            this.ActiveControl = this._txtPara;
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 560);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(440, 440);
            this.Name = "FormEnviarCorreo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "📧 Enviar por correo electrónico";
            this.Load += new System.EventHandler(this.FormEnviarCorreo_Load);
            this._panelTop.ResumeLayout(false);
            this._panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblArchivo;
        private System.Windows.Forms.Label _lblPara;
        private System.Windows.Forms.TextBox _txtPara;
        private System.Windows.Forms.Label _lblAsunto;
        private System.Windows.Forms.TextBox _txtAsunto;
        private System.Windows.Forms.Label _lblMensaje;
        private System.Windows.Forms.TextBox _txtMensaje;
        private System.Windows.Forms.Label _lblAviso;
        private System.Windows.Forms.Button _btnEnviar;
        private System.Windows.Forms.Button _btnCancelar;
        private System.Windows.Forms.Panel _panelTop;
        private System.Windows.Forms.Panel _panelBottom;
    }
}


