using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    partial class Form1
    {
        // ══════════════════════════════════════════════════════════════════════
        //  SIDEBAR
        // ══════════════════════════════════════════════════════════════════════

        private void BuildSidebar()
        {
            panelSidebar.Controls.Clear();
            _sidebarButtons.Clear();

            int y = 8;
            y = AddSidebarSection("Acceso rápido", y);
            y = AddSidebarItem("⭐  Acceso rápido",  Color.FromArgb(255, 200, 0),  null,                                                              y);
            y = AddSidebarItem("🖥️  Escritorio",     Color.FromArgb(70, 130, 180), Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), y);
            y = AddSidebarItem("⬇️  Descargas",      Color.FromArgb(34, 139, 34),  GetKnownFolder("Downloads"),                                       y);
            y = AddSidebarItem("📄  Documentos",     Color.FromArgb(100, 80, 200), Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),   y);
            y = AddSidebarItem("🖼️  Imágenes",       Color.FromArgb(0, 160, 180),  Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),    y);
            y = AddSidebarItem("🎵  Música",         Color.FromArgb(200, 80, 160), Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),       y);
            y = AddSidebarItem("🎬  Vídeos",         Color.FromArgb(220, 100, 30), Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),      y);

            y += 8;
            y = AddSidebarSection("Este equipo", y);

            foreach (var drive in DriveInfo.GetDrives().Where(d => d.IsReady).OrderBy(d => d.Name))
            {
                string emoji = drive.DriveType switch
                {
                    DriveType.CDRom     => "💿",
                    DriveType.Removable => "💾",
                    DriveType.Network   => "🌐",
                    _                   => "🗄️"
                };
                string label = string.IsNullOrEmpty(drive.VolumeLabel)
                    ? drive.Name.TrimEnd('\\')
                    : $"{drive.VolumeLabel} ({drive.Name.TrimEnd('\\')})";
                y = AddSidebarItem($"{emoji}  {label}", Color.FromArgb(80, 120, 180), drive.RootDirectory.FullName, y);
            }

            AddSidebarToolsFooter();
        }

        private void AddSidebarToolsFooter()
        {
            panelSidebar.Controls.Add(CreateSidebarSeparator());
            panelSidebar.Controls.Add(CreateSidebarSectionLabel("HERRAMIENTAS"));
            panelSidebar.Controls.Add(CreateSidebarToolButton("🎙️  Grabar audio", MenuItemGrabarAudio_Click, AppColors.SidebarHover,    AppColors.SidebarPressed));
            panelSidebar.Controls.Add(CreateSidebarToolButton("📷  Abrir cámara", MenuItemAbrirCamara_Click,  AppColors.SidebarHover,    AppColors.SidebarPressed));
            panelSidebar.Controls.Add(CreateSidebarToolButton("📤  Exportar",     BtnExportar_Click,          AppColors.SidebarHover,    AppColors.SidebarPressed));

            panelSidebar.Controls.Add(CreateSidebarSeparator());
            panelSidebar.Controls.Add(CreateSidebarSectionLabel("DATOS"));
            panelSidebar.Controls.Add(CreateSidebarToolButton("🧹  Limpiar datos", BtnLimpiarDatos_Click, AppColors.SidebarHover,    AppColors.SidebarPressed));
            panelSidebar.Controls.Add(CreateSidebarToolButton("🚀  Migrar",        BtnMigrarDatos_Click,  AppColors.SidebarToolHover, AppColors.SidebarToolPressed));
        }

        private int AddSidebarSection(string title, int y)
        {
            var lbl = new Label { Location = new Point(10, y), Size = new System.Drawing.Size(170, 16) };
            AplicarEstiloSidebarSeccion(lbl, title.ToUpperInvariant());
            panelSidebar.Controls.Add(lbl);
            return y + 20;
        }

        private static void AplicarEstiloSidebarSeccion(Label lbl, string texto)
            => UIStyler.AplicarSeccionSidebar(lbl, texto);

        private int AddSidebarItem(string text, Color accentColor, string? path, int y)
        {
            var btn = new Button { Location = new Point(0, y), Tag = path };
            AplicarEstiloSidebarItem(btn, text, path != null);

            if (path != null)
            {
                btn.Click += (s, e) =>
                {
                    if (Directory.Exists(path)) NavigateTo(path);
                    foreach (var b in _sidebarButtons) b.BackColor = Color.Transparent;
                    btn.BackColor = AppColors.SidebarSelected;
                };

                // Habilitar soltar archivos directamente sobre el botón
                RegistrarDragDropEnBoton(btn);
            }

            _sidebarButtons.Add(btn);
            panelSidebar.Controls.Add(btn);
            return y + 30;
        }

        private static void AplicarEstiloSidebarItem(Button btn, string texto, bool esNavegable)
            => UIStyler.AplicarItemSidebar(btn, texto, esNavegable);

        // ── Ayudantes de construcción del sidebar ─────────────────────────────

        private static Label CreateSidebarSeparator()
        {
            var lbl = new Label { Dock = DockStyle.Bottom, Height = 1 };
            AplicarEstiloSidebarSeparador(lbl);
            return lbl;
        }

        private static void AplicarEstiloSidebarSeparador(Label lbl)
            => UIStyler.AplicarSeparadorSidebar(lbl);

        private static Label CreateSidebarSectionLabel(string text)
        {
            var lbl = new Label { Dock = DockStyle.Bottom, Height = 24 };
            AplicarEstiloSidebarEtiquetaSeccion(lbl, text);
            return lbl;
        }

        private static void AplicarEstiloSidebarEtiquetaSeccion(Label lbl, string texto)
            => UIStyler.AplicarEtiquetaSeccionSidebar(lbl, texto);

        private static Button CreateSidebarToolButton(
            string text,
            EventHandler handler,
            Color hoverColor,
            Color pressedColor)
        {
            var btn = new Button { Dock = DockStyle.Bottom, Height = 36 };
            AplicarEstiloSidebarToolButton(btn, text, hoverColor, pressedColor);
            btn.Click += handler;
            return btn;
        }

        private static void AplicarEstiloSidebarToolButton(Button btn, string texto,
            Color hoverColor, Color pressedColor)
            => UIStyler.AplicarToolButtonSidebar(btn, texto, hoverColor, pressedColor);

        private static string GetKnownFolder(string name)
        {
            try
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), name);
                return Directory.Exists(path) ? path
                    : Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            }
            catch { return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); }
        }
    }
}
