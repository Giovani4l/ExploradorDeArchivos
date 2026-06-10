using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    partial class Form1
    {
        // ══════════════════════════════════════════════════════════════════════
        //  MENÚ CONTEXTUAL
        // ══════════════════════════════════════════════════════════════════════

        private void ListViewFiles_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var hit     = listViewFiles.HitTest(e.Location);
            bool isFile = hit.Item?.Tag is string p && File.Exists(p);
            menuItemEnviarCorreo.Enabled = isFile;
            menuItemEnviarCorreo.Tag     = isFile ? hit.Item!.Tag : null;
        }

        private void ContextMenuArchivo_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            bool haySeleccion = listViewFiles.SelectedItems.Count > 0;
            menuItemCortar.Enabled        = haySeleccion;
            menuItemCopiar.Enabled        = haySeleccion;
            menuItemPegar.Enabled         = !string.IsNullOrEmpty(_rutaPortapapeles) ||
                                             Clipboard.ContainsFileDropList();
            menuItemCrearAcceso.Enabled   = haySeleccion;
            menuItemEliminar.Enabled      = haySeleccion;
            menuItemCambiarNombre.Enabled = haySeleccion;
            menuItemPropiedades.Enabled   = haySeleccion;

            bool isFile = haySeleccion &&
                          listViewFiles.SelectedItems[0].Tag is string p && File.Exists(p);
            menuItemEnviarCorreo.Enabled = isFile;
        }

        private void MenuItemEnviarCorreo_Click(object sender, EventArgs e)
        {
            string? filePath = null;

            if (menuItemEnviarCorreo.Tag is string tagPath && File.Exists(tagPath))
                filePath = tagPath;
            else if (listViewFiles.SelectedItems.Count > 0 &&
                     listViewFiles.SelectedItems[0].Tag is string selPath && File.Exists(selPath))
                filePath = selPath;

            if (string.IsNullOrEmpty(filePath)) { UpdateStatus("Selecciona un archivo primero."); return; }

            using var dlg = new FormEnviarCorreo(filePath);
            dlg.ShowDialog(this);
        }

        private void MenuItemCortar_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0) return;
            _rutaPortapapeles = listViewFiles.SelectedItems[0].Tag?.ToString() ?? string.Empty;
            _esCorte          = true;
            if (!string.IsNullOrEmpty(_rutaPortapapeles))
            {
                Clipboard.SetText(_rutaPortapapeles);
                UpdateStatus($"Cortado: {Path.GetFileName(_rutaPortapapeles)}");
            }
        }

        private void MenuItemCopiar_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0) return;
            _rutaPortapapeles = listViewFiles.SelectedItems[0].Tag?.ToString() ?? string.Empty;
            _esCorte          = false;
            if (!string.IsNullOrEmpty(_rutaPortapapeles))
            {
                Clipboard.SetText(_rutaPortapapeles);
                UpdateStatus($"Copiado: {Path.GetFileName(_rutaPortapapeles)}");
            }
        }

        private void MenuItemPegar_Click(object sender, EventArgs e)
        {
            // Leer del portapapeles del sistema si el interno está vacío
            if (string.IsNullOrEmpty(_rutaPortapapeles) && Clipboard.ContainsFileDropList())
            {
                var files = Clipboard.GetFileDropList();
                if (files.Count > 0) _rutaPortapapeles = files[0]!;
            }

            if (string.IsNullOrEmpty(_rutaPortapapeles) ||
                (!File.Exists(_rutaPortapapeles) && !Directory.Exists(_rutaPortapapeles)))
            {
                UpdateStatus("No hay nada en el portapapeles para pegar.");
                return;
            }

            string destino = _currentPath;

            if (string.IsNullOrEmpty(destino))
            {
                using var fbd = new FolderBrowserDialog
                {
                    Description         = "Selecciona la carpeta donde pegar",
                    ShowNewFolderButton = true
                };
                if (fbd.ShowDialog(this) != DialogResult.OK) return;
                destino = fbd.SelectedPath;
            }
            else
            {
                var res = MessageBox.Show(
                    $"¿Pegar en la carpeta actual?\n\n{destino}\n\n(No = elegir otra carpeta)",
                    "Pegar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (res == DialogResult.Cancel) return;
                if (res == DialogResult.No)
                {
                    using var fbd = new FolderBrowserDialog
                    {
                        Description         = "Selecciona la carpeta donde pegar",
                        SelectedPath        = destino,
                        ShowNewFolderButton = true
                    };
                    if (fbd.ShowDialog(this) != DialogResult.OK) return;
                    destino = fbd.SelectedPath;
                }
            }

            PegarEnCarpeta(_rutaPortapapeles, destino, _esCorte);
        }

        private void PegarEnCarpeta(string origen, string destino, bool esCorte)
        {
            try
            {
                string nombreArchivo = Path.GetFileName(origen.TrimEnd('\\', '/'));
                string rutaDestino   = Path.Combine(destino, nombreArchivo);

                if (File.Exists(rutaDestino) || Directory.Exists(rutaDestino))
                {
                    var res = MessageBox.Show(
                        $"\"{nombreArchivo}\" ya existe en la carpeta destino. ¿Deseas reemplazarlo?",
                        "Pegar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res != DialogResult.Yes) return;
                }

                if (File.Exists(origen))
                {
                    if (esCorte) File.Move(origen, rutaDestino, overwrite: true);
                    else         File.Copy(origen, rutaDestino, overwrite: true);
                }
                else if (Directory.Exists(origen))
                {
                    CopiarDirectorio(origen, rutaDestino);
                    if (esCorte) Directory.Delete(origen, recursive: true);
                }

                if (esCorte) { _rutaPortapapeles = string.Empty; _esCorte = false; }

                if (string.Equals(destino, _currentPath, StringComparison.OrdinalIgnoreCase))
                    PopulateListView(_currentPath);

                UpdateStatus($"✔ {(esCorte ? "Movido" : "Copiado")}: {nombreArchivo} → {destino}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al pegar:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MenuItemCrearAcceso_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0) return;
            string ruta = listViewFiles.SelectedItems[0].Tag?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(ruta)) return;

            try
            {
                string directorio = Directory.Exists(ruta)
                    ? Path.GetDirectoryName(ruta.TrimEnd('\\', '/')) ?? ruta
                    : Path.GetDirectoryName(ruta) ?? ruta;
                string nombre     = Path.GetFileNameWithoutExtension(ruta);
                string rutaAcceso = Path.Combine(directorio, $"{nombre} - Acceso directo.lnk");

                Type?   shellType = Type.GetTypeFromProgID("WScript.Shell");
                if (shellType == null) throw new Exception("WScript.Shell no disponible.");
                dynamic shell    = Activator.CreateInstance(shellType)!;
                dynamic shortcut = shell.CreateShortcut(rutaAcceso);
                shortcut.TargetPath = ruta;
                shortcut.Save();

                UpdateStatus($"Acceso directo creado: {rutaAcceso}");
                if (string.Equals(directorio, _currentPath, StringComparison.OrdinalIgnoreCase))
                    PopulateListView(_currentPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo crear el acceso directo:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MenuItemEliminar_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0) return;
            string ruta = listViewFiles.SelectedItems[0].Tag?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(ruta)) return;

            var respuesta = MessageBox.Show(
                $"¿Deseas eliminar permanentemente?\n\n{ruta}",
                "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (respuesta != DialogResult.Yes) return;

            try
            {
                if (File.Exists(ruta))           File.Delete(ruta);
                else if (Directory.Exists(ruta)) Directory.Delete(ruta, recursive: true);

                listViewFiles.SelectedItems[0].Remove();
                UpdateStatus($"Eliminado: {Path.GetFileName(ruta)}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MenuItemCambiarNombre_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0) return;
            listViewFiles.SelectedItems[0].BeginEdit();
        }

        private void ListViewFiles_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null) return;

            var item           = listViewFiles.Items[e.Item];
            string rutaActual  = item.Tag?.ToString() ?? string.Empty;
            string nuevoNombre = e.Label.Trim();

            if (string.IsNullOrEmpty(nuevoNombre) || nuevoNombre == item.Text)
            {
                e.CancelEdit = true;
                return;
            }

            try
            {
                string? directorio = Path.GetDirectoryName(rutaActual);
                if (directorio == null) { e.CancelEdit = true; return; }

                string nuevaRuta = Path.Combine(directorio, nuevoNombre);

                if (File.Exists(rutaActual))           File.Move(rutaActual, nuevaRuta);
                else if (Directory.Exists(rutaActual)) Directory.Move(rutaActual, nuevaRuta);

                item.Tag = nuevaRuta;
                UpdateStatus($"Renombrado a: {nuevoNombre}");
            }
            catch (Exception ex)
            {
                e.CancelEdit = true;
                MessageBox.Show($"Error al renombrar:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MenuItemPropiedades_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0) return;
            string ruta = listViewFiles.SelectedItems[0].Tag?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(ruta)) return;

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName        = "rundll32.exe",
                    Arguments       = $"shell32.dll,ShellExec_RunDLL properties \"{ruta}\"",
                    UseShellExecute = false
                };
                Process.Start(psi);
            }
            catch
            {
                string carpeta = File.Exists(ruta) ? Path.GetDirectoryName(ruta) ?? ruta : ruta;
                Process.Start(new ProcessStartInfo("explorer.exe", $"\"{carpeta}\"") { UseShellExecute = true });
            }
        }
    }
}
