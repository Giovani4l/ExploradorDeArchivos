using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    /// <summary>
    /// Diálogo para enviar un archivo por correo electrónico.
    /// Pide destinatario, asunto y mensaje, luego abre el cliente
    /// de correo predeterminado con todo precargado (mailto:).
    /// </summary>
    public partial class FormEnviarCorreo : Form
    {
        private readonly string _archivoPath;

        // Tu constructor que recibe la ruta del archivo
        public FormEnviarCorreo(string archivoPath)
        {
            InitializeComponent(); // Esto carga todo lo del diseñador
            _archivoPath = archivoPath;

            // Lógica inicial rápida tras cargar los componentes
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            if (string.IsNullOrEmpty(_archivoPath)) return;

            string nombreArchivo = Path.GetFileName(_archivoPath);

            // Asignamos los textos dinámicos que dependen del archivo recibido
            _lblArchivo.Text = $"📎  Adjunto: {nombreArchivo}";
            _txtAsunto.Text = $"Te envío el archivo: {nombreArchivo}";
            _txtMensaje.Text = $"Hola,\n\nTe comparto el archivo \"{nombreArchivo}\".\n\nSaludos.";
        }

        private void BtnEnviar_Click(object sender, EventArgs e)
        {
            string para = _txtPara.Text.Trim();
            string asunto = _txtAsunto.Text.Trim();
            string cuerpo = _txtMensaje.Text.Trim();

            if (string.IsNullOrWhiteSpace(para))
            {
                MostrarError("Por favor ingresa la dirección de correo del destinatario.");
                _txtPara.Focus();
                return;
            }

            if (!EsCorreoValido(para))
            {
                MostrarError($"La dirección \"{para}\" no parece válida.");
                _txtPara.Focus();
                return;
            }

            try
            {
                // 1. Configuración del servidor de correo (Remitente)
                string correoRemitente = "axelgonzalez0364@gmail.com"; // ⚠️ Pon tu correo aquí
                string contraseniaOApp = "mllx hlyg xdyi liyo"; // ⚠️ Pon tu contraseña aquí

                // 2. Crear el mensaje
                MailMessage correo = new MailMessage();
                correo.From = new MailAddress(correoRemitente);
                correo.To.Add(para);
                correo.Subject = asunto;
                correo.Body = cuerpo;
                correo.IsBodyHtml = false; // Cambia a true si usas etiquetas HTML en el mensaje

                // 📎 ADJUNTAR EL ARCHIVO AUTOMÁTICAMENTE
                if (!string.IsNullOrEmpty(_archivoPath) && File.Exists(_archivoPath))
                {
                    Attachment adjunto = new Attachment(_archivoPath);
                    correo.Attachments.Add(adjunto);
                }

                // 3. Configurar el cliente SMTP (En este ejemplo, el de Gmail)
                SmtpClient clienteSmtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587, // Puerto estándar para TLS
                    Credentials = new NetworkCredential(correoRemitente, contraseniaOApp),
                    EnableSsl = true // Seguridad activada
                };

                // 4. Enviar
                Cursor = Cursors.WaitCursor; // Cambia el cursor a "cargando"
                clienteSmtp.Send(correo);
                Cursor = Cursors.Default;

                MessageBox.Show("¡Correo enviado con éxito directamente desde la app!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MostrarError($"Error al enviar el correo: {ex.Message}");
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private static bool EsCorreoValido(string correo)
        {
            try { var _ = new MailAddress(correo); return true; }
            catch { return false; }
        }

        private void MostrarError(string msg) =>
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private void FormEnviarCorreo_Load(object sender, EventArgs e)
        {
        }
    }
}


