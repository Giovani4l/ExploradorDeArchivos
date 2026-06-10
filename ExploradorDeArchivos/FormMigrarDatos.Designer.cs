namespace ExploradorDeArchivos
{
    partial class FormMigrarDatos
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            Color bgOscuro = Color.FromArgb(8, 11, 20);
            Color bgPanel = Color.FromArgb(13, 21, 37);
            Color azulClaro = Color.FromArgb(59, 130, 246);
            Color textoPrim = Color.FromArgb(225, 232, 240);
            Color textoSec = Color.FromArgb(100, 116, 139);
            Color verdeClaro = Color.FromArgb(52, 211, 153);
            Color azul = Color.FromArgb(29, 78, 216);

            _lblTitulo = new Label();
            _lblSubtitulo = new Label();
            _lblError = new Label();
            _panelPasos = new Panel();
            _panelContenido = new Panel();

            _zonaCargar = new Panel();
            _zonaArrastrar = new Panel();
            _lblDropIcon = new Label();
            _lblDropText = new Label();
            _lblDropSub = new Label();
            _btnSeleccionar = new Button();
            _lblInfoArchivo = new Label();

            _zonaConfig = new Panel();
            _lblMotorInfo = new Label();
            _btnSqlServer = new Button();
            _btnHeidi = new Button();
            _txtServidor = new TextBox();
            _txtPuerto = new TextBox();
            _txtUsuario = new TextBox();
            _txtPassword = new TextBox();
            _txtBaseDatos = new TextBox();
            _txtTabla = new TextBox();
            _chkCrearTabla = new CheckBox();
            _btnIniciarMigracion = new Button();

            _zonaMigrando = new Panel();
            _lblProgTexto = new Label();
            _barProg = new ProgressBar();
            _lblProgPct = new Label();

            _zonaResultado = new Panel();
            _lblResultIcon = new Label();
            _lblResultMsg = new Label();
            _lblResultDetalle = new Label();
            _btnNuevaMigracion = new Button();

            _panelHeader = new Panel();

            _zonaArrastrar.SuspendLayout();
            _zonaCargar.SuspendLayout();
            _zonaConfig.SuspendLayout();
            _zonaMigrando.SuspendLayout();
            _zonaResultado.SuspendLayout();
            _panelContenido.SuspendLayout();
            _panelHeader.SuspendLayout();
            SuspendLayout();

            // _panelHeader
            _panelHeader.BackColor = bgOscuro;
            _panelHeader.Controls.Add(_lblTitulo);
            _panelHeader.Controls.Add(_lblSubtitulo);
            _panelHeader.Dock = DockStyle.Top;
            _panelHeader.Location = new Point(0, 0);
            _panelHeader.Size = new Size(942, 64);

            // _lblTitulo
            _lblTitulo.AutoSize = true;
            _lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            _lblTitulo.ForeColor = textoPrim;
            _lblTitulo.Location = new Point(24, 15);
            _lblTitulo.Size = new Size(231, 37);
            _lblTitulo.Text = "MIGRAR  ✦  DATOS";

            // _lblSubtitulo
            _lblSubtitulo.AutoSize = true;
            _lblSubtitulo.Font = new Font("Segoe UI", 8.5F);
            _lblSubtitulo.ForeColor = textoSec;
            _lblSubtitulo.Location = new Point(265, 28);
            _lblSubtitulo.Size = new Size(330, 20);
            _lblSubtitulo.Text = "Importa cualquier archivo a SQL Server o HeidiSQL";

            // _panelPasos
            _panelPasos.BackColor = bgPanel;
            _panelPasos.Dock = DockStyle.Top;
            _panelPasos.Location = new Point(0, 64);
            _panelPasos.Size = new Size(942, 40);

            // _lblError
            _lblError.BackColor = Color.FromArgb(28, 10, 10);
            _lblError.Dock = DockStyle.Top;
            _lblError.Font = new Font("Segoe UI", 9F);
            _lblError.ForeColor = Color.FromArgb(252, 165, 165);
            _lblError.Location = new Point(0, 104);
            _lblError.Size = new Size(942, 0);
            _lblError.Visible = false;

            // _panelContenido
            _panelContenido.BackColor = bgOscuro;
            _panelContenido.Controls.Add(_zonaCargar);
            _panelContenido.Controls.Add(_zonaConfig);
            _panelContenido.Controls.Add(_zonaMigrando);
            _panelContenido.Controls.Add(_zonaResultado);
            _panelContenido.Dock = DockStyle.Fill;
            _panelContenido.Location = new Point(0, 104);
            _panelContenido.Size = new Size(942, 539);

            // ZONA 1: CARGAR (Diseño original restaurado)
            _zonaCargar.BackColor = bgOscuro;
            _zonaCargar.Controls.Add(_zonaArrastrar);
            _zonaCargar.Controls.Add(_lblInfoArchivo);
            _zonaCargar.Location = new Point(81, 45);
            _zonaCargar.Size = new Size(780, 420);
            _zonaCargar.Anchor = AnchorStyles.None; // Flota al centro perfecto

            _zonaArrastrar.AllowDrop = true;
            _zonaArrastrar.BackColor = bgPanel;
            _zonaArrastrar.Controls.Add(_lblDropIcon);
            _zonaArrastrar.Controls.Add(_lblDropText);
            _zonaArrastrar.Controls.Add(_lblDropSub);
            _zonaArrastrar.Controls.Add(_btnSeleccionar);
            _zonaArrastrar.Location = new Point(0, 0);
            _zonaArrastrar.Size = new Size(780, 320);

            _lblDropIcon.Font = new Font("Segoe UI", 42F);
            _lblDropIcon.ForeColor = azulClaro;
            _lblDropIcon.Location = new Point(0, 35);
            _lblDropIcon.Size = new Size(780, 75);
            _lblDropIcon.Text = "📂";
            _lblDropIcon.TextAlign = ContentAlignment.MiddleCenter;

            _lblDropText.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            _lblDropText.ForeColor = textoPrim;
            _lblDropText.Location = new Point(0, 130);
            _lblDropText.Size = new Size(780, 35);
            _lblDropText.Text = "Arrastra tu archivo aquí";
            _lblDropText.TextAlign = ContentAlignment.MiddleCenter;

            _lblDropSub.Font = new Font("Segoe UI", 9.5F);
            _lblDropSub.ForeColor = textoSec;
            _lblDropSub.Location = new Point(0, 175);
            _lblDropSub.Size = new Size(780, 30);
            _lblDropSub.Text = "CSV, Excel (.xlsx/.xls), JSON, TXT — o haz clic para buscar";
            _lblDropSub.TextAlign = ContentAlignment.MiddleCenter;

            _btnSeleccionar.BackColor = azul;
            _btnSeleccionar.Cursor = Cursors.Hand;
            _btnSeleccionar.FlatAppearance.BorderSize = 0;
            _btnSeleccionar.FlatStyle = FlatStyle.Flat;
            _btnSeleccionar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _btnSeleccionar.ForeColor = Color.White;
            _btnSeleccionar.Location = new Point(280, 230);
            _btnSeleccionar.Size = new Size(220, 42);
            _btnSeleccionar.Text = "Seleccionar archivo";

            _lblInfoArchivo.Font = new Font("Segoe UI", 9.5F);
            _lblInfoArchivo.ForeColor = verdeClaro;
            _lblInfoArchivo.Location = new Point(0, 350);
            _lblInfoArchivo.Size = new Size(780, 35);
            _lblInfoArchivo.TextAlign = ContentAlignment.MiddleCenter;
            _lblInfoArchivo.Visible = false;

            // ZONA 2: CONFIGURACIÓN
            _zonaConfig.BackColor = bgOscuro;
            _zonaConfig.Controls.Add(_lblMotorInfo);
            _zonaConfig.Controls.Add(_btnSqlServer);
            _zonaConfig.Controls.Add(_btnHeidi);
            _zonaConfig.Controls.Add(_txtServidor);
            _zonaConfig.Controls.Add(_txtPuerto);
            _zonaConfig.Controls.Add(_txtUsuario);
            _zonaConfig.Controls.Add(_txtPassword);
            _zonaConfig.Controls.Add(_txtBaseDatos);
            _zonaConfig.Controls.Add(_txtTabla);
            _zonaConfig.Controls.Add(_chkCrearTabla);
            _zonaConfig.Controls.Add(_btnIniciarMigracion);
            _zonaConfig.Location = new Point(81, 45);
            _zonaConfig.Size = new Size(780, 420);
            _zonaConfig.Anchor = AnchorStyles.None;

            _lblMotorInfo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _lblMotorInfo.ForeColor = azulClaro;
            _lblMotorInfo.Location = new Point(40, 15);
            _lblMotorInfo.Size = new Size(700, 30);
            _lblMotorInfo.Text = "Configurando conexión para: SQL Server";

            _btnSqlServer.BackColor = azul;
            _btnSqlServer.FlatStyle = FlatStyle.Flat;
            _btnSqlServer.ForeColor = Color.White;
            _btnSqlServer.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnSqlServer.Location = new Point(40, 55);
            _btnSqlServer.Size = new Size(140, 35);
            _btnSqlServer.Text = "SQL Server";

            _btnHeidi.BackColor = bgPanel;
            _btnHeidi.FlatStyle = FlatStyle.Flat;
            _btnHeidi.ForeColor = textoPrim;
            _btnHeidi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnHeidi.Location = new Point(190, 55);
            _btnHeidi.Size = new Size(140, 35);
            _btnHeidi.Text = "HeidiSQL (MySQL)";

            ConfigurarInputEstilo(_txtServidor, "Servidor (Host)", 40, 115, 480, bgPanel, textoPrim);
            ConfigurarInputEstilo(_txtPuerto, "Puerto", 540, 115, 200, bgPanel, textoPrim);
            ConfigurarInputEstilo(_txtUsuario, "Usuario", 40, 175, 330, bgPanel, textoPrim);
            ConfigurarInputEstilo(_txtPassword, "Contraseña", 410, 175, 330, bgPanel, textoPrim);
            _txtPassword.UseSystemPasswordChar = true;

            ConfigurarInputEstilo(_txtBaseDatos, "Base de Datos", 40, 235, 330, bgPanel, textoPrim);
            ConfigurarInputEstilo(_txtTabla, "Nombre de la Tabla Destino", 410, 235, 330, bgPanel, textoPrim);

            _chkCrearTabla.AutoSize = true;
            _chkCrearTabla.ForeColor = textoPrim;
            _chkCrearTabla.Location = new Point(40, 300);
            _chkCrearTabla.Size = new Size(280, 25);
            _chkCrearTabla.Text = "Crear tabla automáticamente si no existe";

            _btnIniciarMigracion.BackColor = verdeClaro;
            _btnIniciarMigracion.FlatStyle = FlatStyle.Flat;
            _btnIniciarMigracion.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _btnIniciarMigracion.ForeColor = Color.FromArgb(8, 11, 20);
            _btnIniciarMigracion.Location = new Point(40, 345);
            _btnIniciarMigracion.Size = new Size(700, 45);
            _btnIniciarMigracion.Text = "🚀 INICIAR MIGRACIÓN DE DATOS";

            // ZONA 3: MIGRANDO
            _zonaMigrando.BackColor = bgOscuro;
            _zonaMigrando.Controls.Add(_lblProgTexto);
            _zonaMigrando.Controls.Add(_barProg);
            _zonaMigrando.Controls.Add(_lblProgPct);
            _zonaMigrando.Location = new Point(81, 45);
            _zonaMigrando.Size = new Size(780, 420);
            _zonaMigrando.Anchor = AnchorStyles.None;

            _lblProgTexto.Font = new Font("Segoe UI", 11F);
            _lblProgTexto.ForeColor = textoPrim;
            _lblProgTexto.Location = new Point(40, 140);
            _lblProgTexto.Size = new Size(700, 30);
            _lblProgTexto.Text = "Procesando registros...";
            _lblProgTexto.TextAlign = ContentAlignment.MiddleCenter;

            _barProg.Location = new Point(40, 185);
            _barProg.Size = new Size(700, 25);

            _lblProgPct.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            _lblProgPct.ForeColor = azulClaro;
            _lblProgPct.Location = new Point(40, 225);
            _lblProgPct.Size = new Size(700, 35);
            _lblProgPct.Text = "0%";
            _lblProgPct.TextAlign = ContentAlignment.MiddleCenter;

            // ZONA 4: RESULTADO
            _zonaResultado.BackColor = bgOscuro;
            _zonaResultado.Controls.Add(_lblResultIcon);
            _zonaResultado.Controls.Add(_lblResultMsg);
            _zonaResultado.Controls.Add(_lblResultDetalle);
            _zonaResultado.Controls.Add(_btnNuevaMigracion);
            _zonaResultado.Location = new Point(81, 45);
            _zonaResultado.Size = new Size(780, 420);
            _zonaResultado.Anchor = AnchorStyles.None;

            _lblResultIcon.Font = new Font("Segoe UI", 48F);
            _lblResultIcon.Location = new Point(0, 50);
            _lblResultIcon.Size = new Size(780, 90);
            _lblResultIcon.TextAlign = ContentAlignment.MiddleCenter;

            _lblResultMsg.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            _lblResultMsg.ForeColor = textoPrim;
            _lblResultMsg.Location = new Point(0, 150);
            _lblResultMsg.Size = new Size(780, 40);
            _lblResultMsg.TextAlign = ContentAlignment.MiddleCenter;

            _lblResultDetalle.Font = new Font("Segoe UI", 10F);
            _lblResultDetalle.ForeColor = textoSec;
            _lblResultDetalle.Location = new Point(60, 200);
            _lblResultDetalle.Size = new Size(660, 60);
            _lblResultDetalle.TextAlign = ContentAlignment.MiddleCenter;

            _btnNuevaMigracion.BackColor = bgPanel;
            _btnNuevaMigracion.FlatStyle = FlatStyle.Flat;
            _btnNuevaMigracion.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _btnNuevaMigracion.ForeColor = textoPrim;
            _btnNuevaMigracion.Location = new Point(290, 290);
            _btnNuevaMigracion.Size = new Size(200, 40);
            _btnNuevaMigracion.Text = "Hacer otra migración";

            // FormMigrarDatos
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = bgOscuro;
            ClientSize = new Size(942, 643);
            Controls.Add(_panelContenido);
            Controls.Add(_lblError);
            Controls.Add(_panelPasos);
            Controls.Add(_panelHeader);
            Font = new Font("Segoe UI", 9.5F);
            ForeColor = textoPrim;
            MinimumSize = new Size(960, 680);
            Name = "FormMigrarDatos";
            StartPosition = FormStartPosition.CenterParent;
            Text = "🚀 Migrar Datos a Base de Datos";

            _zonaArrastrar.ResumeLayout(false);
            _zonaCargar.ResumeLayout(false);
            _zonaConfig.ResumeLayout(false);
            _zonaConfig.PerformLayout();
            _zonaMigrando.ResumeLayout(false);
            _zonaResultado.ResumeLayout(false);
            _panelContenido.ResumeLayout(false);
            _panelHeader.ResumeLayout(false);
            _panelHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label _lblTitulo;
        private Label _lblSubtitulo;
        private Label _lblError;
        private Panel _panelPasos;
        private Panel _panelContenido;
        private Panel _panelHeader;

        private Panel _zonaCargar;
        private Panel _zonaArrastrar;
        private Label _lblDropIcon;
        private Label _lblDropText;
        private Label _lblDropSub;
        private Button _btnSeleccionar;
        private Label _lblInfoArchivo;

        private Panel _zonaConfig;
        private Label _lblMotorInfo;
        private Button _btnSqlServer;
        private Button _btnHeidi;
        private TextBox _txtServidor;
        private TextBox _txtPuerto;
        private TextBox _txtUsuario;
        private TextBox _txtPassword;
        private TextBox _txtBaseDatos;
        private TextBox _txtTabla;
        private CheckBox _chkCrearTabla;
        private Button _btnIniciarMigracion;

        private Panel _zonaMigrando;
        private Label _lblProgTexto;
        private ProgressBar _barProg;
        private Label _lblProgPct;

        private Panel _zonaResultado;
        private Label _lblResultIcon;
        private Label _lblResultMsg;
        private Label _lblResultDetalle;
        private Button _btnNuevaMigracion;
    }
}



