using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    public partial class FormSpotifyConfig : Form
    {
        public static string ConfigPath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PulsePlayer", "spotify.json");

        public string ClientId     { get; private set; } = "";
        public string ClientSecret { get; private set; } = "";

        public FormSpotifyConfig(string existingId = "", string existingSecret = "")
        {
            InitializeComponent();

            txtClientId.Text     = existingId;
            txtClientSecret.Text = existingSecret;

            // Eventos
            chkShow.CheckedChanged += (s, e) =>
                txtClientSecret.UseSystemPasswordChar = !chkShow.Checked;

            lnkHelp.LinkClicked += (s, e) =>
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    { FileName = "https://developer.spotify.com/dashboard", UseShellExecute = true });

            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            btnSave.Click   += BtnSave_Click;

            Shown += (s, e) => txtClientId.Focus();
        }

        public static (string id, string secret) LoadSaved()
        {
            try
            {
                if (!File.Exists(ConfigPath)) return ("", "");
                var json = File.ReadAllText(ConfigPath);
                using var doc = JsonDocument.Parse(json);
                var id  = doc.RootElement.GetProperty("client_id").GetString()     ?? "";
                var sec = doc.RootElement.GetProperty("client_secret").GetString() ?? "";
                return (id, sec);
            }
            catch { return ("", ""); }
        }

        private void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigPath)!);
            var obj = new { client_id = txtClientId.Text.Trim(), client_secret = txtClientSecret.Text.Trim() };
            File.WriteAllText(ConfigPath, JsonSerializer.Serialize(obj));
            ClientId     = obj.client_id;
            ClientSecret = obj.client_secret;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtClientId.Text) ||
                string.IsNullOrWhiteSpace(txtClientSecret.Text))
            {
                MessageBox.Show("Completa ambos campos antes de continuar.",
                    "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                Save();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar la configuración:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


