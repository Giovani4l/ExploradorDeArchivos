namespace ExploradorDeArchivos
{
    partial class FormCamara
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _picPreview = new PictureBox();
            _panelBottom = new Panel();
            _btnGrabar = new Button();
            _lblEstado = new Label();
            _picMiniatura = new PictureBox();
            _btnCarpeta = new Button();
            _btnCaptura = new Button();
            _btnConectar = new Button();
            _cboCamaras = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)_picPreview).BeginInit();
            _panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_picMiniatura).BeginInit();
            SuspendLayout();
            // 
            // _picPreview
            // 
            _picPreview.BackColor = Color.Black;
            _picPreview.Dock = DockStyle.Fill;
            _picPreview.Location = new Point(0, 0);
            _picPreview.Name = "_picPreview";
            _picPreview.Size = new Size(740, 534);
            _picPreview.SizeMode = PictureBoxSizeMode.Zoom;
            _picPreview.TabIndex = 0;
            _picPreview.TabStop = false;
            // 
            // _panelBottom
            // 
            _panelBottom.BackColor = Color.FromArgb(40, 40, 45);
            _panelBottom.Controls.Add(_btnGrabar);
            _panelBottom.Controls.Add(_lblEstado);
            _panelBottom.Controls.Add(_picMiniatura);
            _panelBottom.Controls.Add(_btnCarpeta);
            _panelBottom.Controls.Add(_btnCaptura);
            _panelBottom.Controls.Add(_btnConectar);
            _panelBottom.Controls.Add(_cboCamaras);
            _panelBottom.Dock = DockStyle.Bottom;
            _panelBottom.Location = new Point(0, 534);
            _panelBottom.Name = "_panelBottom";
            _panelBottom.Size = new Size(740, 110);
            _panelBottom.TabIndex = 1;
            // 
            // _btnGrabar
            // 
            _btnGrabar.BackColor = Color.FromArgb(0, 122, 204);
            _btnGrabar.Cursor = Cursors.Hand;
            _btnGrabar.FlatAppearance.BorderSize = 0;
            _btnGrabar.FlatStyle = FlatStyle.Flat;
            _btnGrabar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnGrabar.ForeColor = Color.White;
            _btnGrabar.Location = new Point(434, 5);
            _btnGrabar.Name = "_btnGrabar";
            _btnGrabar.Size = new Size(130, 34);
            _btnGrabar.TabIndex = 6;
            _btnGrabar.Text = "🎥  Grabar Video";
            _btnGrabar.UseVisualStyleBackColor = false;
            _btnGrabar.Click += _btnGrabar_Click;
            // 
            // _lblEstado
            // 
            _lblEstado.Font = new Font("Segoe UI", 8F);
            _lblEstado.ForeColor = Color.FromArgb(170, 170, 170);
            _lblEstado.Location = new Point(3, 44);
            _lblEstado.Name = "_lblEstado";
            _lblEstado.Size = new Size(441, 30);
            _lblEstado.TabIndex = 5;
            _lblEstado.Text = "Buscando cámaras…";
            // 
            // _picMiniatura
            // 
            _picMiniatura.BackColor = Color.FromArgb(50, 50, 55);
            _picMiniatura.BorderStyle = BorderStyle.FixedSingle;
            _picMiniatura.Location = new Point(597, 5);
            _picMiniatura.Name = "_picMiniatura";
            _picMiniatura.Size = new Size(140, 102);
            _picMiniatura.SizeMode = PictureBoxSizeMode.Zoom;
            _picMiniatura.TabIndex = 4;
            _picMiniatura.TabStop = false;
            // 
            // _btnCarpeta
            // 
            _btnCarpeta.BackColor = Color.FromArgb(60, 60, 72);
            _btnCarpeta.Cursor = Cursors.Hand;
            _btnCarpeta.FlatAppearance.BorderSize = 0;
            _btnCarpeta.FlatStyle = FlatStyle.Flat;
            _btnCarpeta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnCarpeta.ForeColor = Color.White;
            _btnCarpeta.Location = new Point(450, 44);
            _btnCarpeta.Name = "_btnCarpeta";
            _btnCarpeta.Size = new Size(140, 30);
            _btnCarpeta.TabIndex = 3;
            _btnCarpeta.Text = "📁  Ver capturas";
            _btnCarpeta.UseVisualStyleBackColor = false;
            // 
            // _btnCaptura
            // 
            _btnCaptura.BackColor = Color.FromArgb(0, 150, 136);
            _btnCaptura.Cursor = Cursors.Hand;
            _btnCaptura.FlatAppearance.BorderSize = 0;
            _btnCaptura.FlatStyle = FlatStyle.Flat;
            _btnCaptura.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnCaptura.ForeColor = Color.White;
            _btnCaptura.Location = new Point(315, 5);
            _btnCaptura.Name = "_btnCaptura";
            _btnCaptura.Size = new Size(115, 34);
            _btnCaptura.TabIndex = 2;
            _btnCaptura.Text = "📸  Capturar";
            _btnCaptura.UseVisualStyleBackColor = false;
            // 
            // _btnConectar
            // 
            _btnConectar.BackColor = Color.FromArgb(0, 150, 80);
            _btnConectar.Cursor = Cursors.Hand;
            _btnConectar.FlatAppearance.BorderSize = 0;
            _btnConectar.FlatStyle = FlatStyle.Flat;
            _btnConectar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnConectar.ForeColor = Color.White;
            _btnConectar.Location = new Point(201, 5);
            _btnConectar.Name = "_btnConectar";
            _btnConectar.Size = new Size(110, 34);
            _btnConectar.TabIndex = 1;
            _btnConectar.Text = "▶  Conectar";
            _btnConectar.UseVisualStyleBackColor = false;
            // 
            // _cboCamaras
            // 
            _cboCamaras.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboCamaras.Font = new Font("Segoe UI", 9F);
            _cboCamaras.FormattingEnabled = true;
            _cboCamaras.Location = new Point(3, 8);
            _cboCamaras.Name = "_cboCamaras";
            _cboCamaras.Size = new Size(195, 28);
            _cboCamaras.TabIndex = 0;
            // 
            // FormCamara
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(28, 28, 32);
            ClientSize = new Size(740, 644);
            Controls.Add(_picPreview);
            Controls.Add(_panelBottom);
            Font = new Font("Segoe UI", 9.5F);
            MinimumSize = new Size(520, 480);
            Name = "FormCamara";
            StartPosition = FormStartPosition.CenterParent;
            Text = "📷 Cámara";
            Load += FormCamara_Load;
            ((System.ComponentModel.ISupportInitialize)_picPreview).EndInit();
            _panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_picMiniatura).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox _picPreview;
        private Panel _panelBottom;
        private ComboBox _cboCamaras;
        private Button _btnConectar;
        private Button _btnCaptura;
        private Button _btnCarpeta;
        private Label _lblEstado;
        private PictureBox _picMiniatura;
        private Button _btnGrabar; // 🎥 Declaración del nuevo botón de grabación
    }
}


