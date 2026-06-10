using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    partial class Form1
    {
        // ══════════════════════════════════════════════════════════════════════
        //  TOOLBAR — HANDLERS
        // ══════════════════════════════════════════════════════════════════════

        private void ToolStripButtonBack_Click(object sender, EventArgs e)
        {
            while (_backHistory.Count > 0)
            {
                var target = _backHistory.Pop();
                if (!Directory.Exists(target)) continue;
                if (!string.IsNullOrEmpty(_currentPath)) _forwardHistory.Push(_currentPath);
                NavigateTo(target, addToHistory: false);
                return;
            }
            UpdateNavigationButtons();
        }

        private void ToolStripButtonForward_Click(object sender, EventArgs e)
        {
            while (_forwardHistory.Count > 0)
            {
                var target = _forwardHistory.Pop();
                if (!Directory.Exists(target)) continue;
                if (!string.IsNullOrEmpty(_currentPath)) _backHistory.Push(_currentPath);
                NavigateTo(target, addToHistory: false);
                return;
            }
            UpdateNavigationButtons();
        }

        private void ToolStripButtonUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentPath)) return;
            var parent = Directory.GetParent(_currentPath);
            if (parent != null) NavigateTo(parent.FullName);
        }

        private void ToolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentPath)) NavigateTo(_currentPath, addToHistory: false);
            else BuildSidebar();
        }

        private void ToolStripButtonNewFolder_Click(object sender, EventArgs e)
        {
            var defaultPath = string.IsNullOrEmpty(_currentPath)
                ? Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                : _currentPath;

            using var dialog = new FolderBrowserDialog
            {
                Description         = "Seleccione la carpeta donde crear la nueva carpeta",
                SelectedPath        = defaultPath,
                ShowNewFolderButton = true
            };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            var basePath   = dialog.SelectedPath;
            var folderName = GetNextAvailableFolderName(basePath);
            var targetPath = Path.Combine(basePath, folderName);

            try
            {
                Directory.CreateDirectory(targetPath);
                if (string.Equals(basePath, _currentPath, StringComparison.OrdinalIgnoreCase))
                    PopulateListView(_currentPath);
                else
                    NavigateTo(basePath);
                UpdateStatus($"Carpeta \"{folderName}\" creada en \"{basePath}\".");
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException
                                            or ArgumentException or PathTooLongException)
            {
                UpdateStatus($"No se pudo crear la carpeta: {ex.Message}");
            }
        }

        private static string GetNextAvailableFolderName(string parentPath)
        {
            const string baseName = "Nueva carpeta";
            var candidate = baseName;
            var counter   = 2;
            while (Directory.Exists(Path.Combine(parentPath, candidate)))
            {
                candidate = $"{baseName} ({counter})";
                counter++;
            }
            return candidate;
        }

        private void ToolStripComboBoxAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.Handled          = true;
            e.SuppressKeyPress = true;
            NavigateTo(toolStripComboBoxAddress.Text);
        }

        private void ToolStripComboBoxAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressAddressSelection) return;
            if (toolStripComboBoxAddress.SelectedItem is string path) NavigateTo(path);
        }

        private void ToolStripMenuItemView_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem
                && menuItem.Tag is string viewName
                && Enum.TryParse<View>(viewName, out var view))
                SetListViewView(view);
        }

        private void ToolStripButtonIndexSearch_Click(object? sender, EventArgs e) =>
            BeginIndexSearch(toolStripTextBoxIndex.Text);

        private void ToolStripTextBoxIndex_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.Handled          = true;
            e.SuppressKeyPress = true;
            BeginIndexSearch(toolStripTextBoxIndex.Text);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  ÍNDICE
        // ══════════════════════════════════════════════════════════════════════

        private void BeginIndexSearch(string query)
        {
            var targetPath = ServicioIndiceDirectorios.ResolverDestino(query, _currentPath);
            if (string.IsNullOrEmpty(targetPath))
            {
                UpdateStatus("No se encontró una carpeta válida para indexar.");
                return;
            }
            var summary = ServicioIndiceDirectorios.CrearResumen(targetPath, GetDirectoryContentCounts);
            UpdateStatus($"Índice: {summary.Split('\n').FirstOrDefault()?.Trim() ?? summary}");
            richTextBoxIndexResults.Text = summary;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  NAVEGACIÓN Y LISTADO
        // ══════════════════════════════════════════════════════════════════════

        private void NavigateTo(string path, bool addToHistory = true)
        {
            if (string.IsNullOrWhiteSpace(path)) return;
            path = NormalizePath(path);
            if (!Directory.Exists(path)) { UpdateStatus("Ruta no válida o inaccesible."); return; }

            if (addToHistory && !string.Equals(_currentPath, path, StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(_currentPath)) _backHistory.Push(_currentPath);
                _forwardHistory.Clear();
            }
            _currentPath = path;
            AddAddressHistory(path);
            PopulateListView(path);
            UpdateNavigationButtons();
        }

        private void PopulateListView(string path)
        {
            _directoryContentCache.Clear();
            listViewFiles.Items.Clear();

            try
            {
                var directoryInfo = new DirectoryInfo(path);

                foreach (var directory in directoryInfo.GetDirectories().OrderBy(d => d.Name))
                {
                    var item = new ListViewItem(directory.Name)
                    {
                        Tag        = directory.FullName,
                        ImageIndex = ObtenerIndiceImagenForPath(directory.FullName, isDirectory: true)
                    };
                    item.SubItems.Add("Carpeta");
                    item.SubItems.Add(string.Empty);
                    item.SubItems.Add(directory.LastWriteTime.ToString("g"));
                    listViewFiles.Items.Add(item);
                }

                foreach (var file in directoryInfo.GetFiles().OrderBy(f => f.Name))
                {
                    var item = new ListViewItem(file.Name)
                    {
                        Tag        = file.FullName,
                        ImageIndex = ObtenerIndiceImagenForPath(file.FullName, isDirectory: false)
                    };
                    item.SubItems.Add(file.Extension.TrimStart('.').ToUpperInvariant() + " Archivo");
                    item.SubItems.Add(AyudanteFormatoArchivo.ObtenerTamanoLegible(file.Length));
                    item.SubItems.Add(file.LastWriteTime.ToString("g"));
                    listViewFiles.Items.Add(item);
                }

                var dirs  = directoryInfo.GetDirectories().Length;
                var files = directoryInfo.GetFiles().Length;
                UpdateStatus($"📁 {dirs} carpetas  ·  📄 {files} archivos");
            }
            catch (UnauthorizedAccessException) { UpdateStatus("Acceso denegado."); }
            catch (IOException ex)              { UpdateStatus($"Error: {ex.Message}"); }
        }

        private void ListViewFiles_DoubleClick(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0) return;
            if (listViewFiles.SelectedItems[0].Tag is not string selectedPath) return;
            if (Directory.Exists(selectedPath)) NavigateTo(selectedPath);
            else if (File.Exists(selectedPath)) OpenFile(selectedPath);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  ÍCONOS Y TOOLTIPS
        // ══════════════════════════════════════════════════════════════════════

        private int ObtenerIndiceImagenForPath(string path, bool isDirectory) =>
            _proveedorIconos.ObtenerIndiceImagen(path, isDirectory);

        private void ListViewFiles_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            if (e.Item.Tag is not string path) return;
            e.Item.ToolTipText = Directory.Exists(path)
                ? GetDirectoryTooltipText(path)
                : GetFileTooltipText(path);
        }

        private string GetDirectoryTooltipText(string path)
        {
            var counts = GetDirectoryContentCounts(path);
            return counts.HasValue
                ? $"📁 {counts.Value.folders} carpetas  ·  📄 {counts.Value.files} archivos"
                : "Contenido inaccesible";
        }

        private (int folders, int files)? GetDirectoryContentCounts(string path)
        {
            if (_directoryContentCache.TryGetValue(path, out var cached)) return cached;
            try
            {
                var result = (Directory.EnumerateDirectories(path).Count(),
                              Directory.EnumerateFiles(path).Count());
                _directoryContentCache[path] = result;
                return result;
            }
            catch { return null; }
        }

        private static string GetFileTooltipText(string path)
        {
            try   { return $"Tamaño: {AyudanteFormatoArchivo.ObtenerTamanoLegible(new FileInfo(path).Length)}"; }
            catch { return "Archivo inaccesible"; }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  APERTURA DE ARCHIVOS
        // ══════════════════════════════════════════════════════════════════════

        private void OpenFile(string path)
        {
            var ext = Path.GetExtension(path);

            if (AyudanteFormatoArchivo.ExtensionesImagen.Contains(ext))
            {
                try { new FormEditorImagen(path).Show(this); return; }
                catch { /* continúa al visor integrado */ }
            }

            if (AyudanteFormatoArchivo.ExtensionesAudio.Contains(ext))
            {
                OpenAudioPlayer(path);
                return;
            }

            if (AyudanteFormatoArchivo.ExtensionesVideo.Contains(ext))
            {
                OpenVideoPlayer(path);
                return;
            }

            if (FormVisorArchivo.TieneVisorIntegrado(ext))
            {
                try
                {
                    new FormVisorArchivo(path).Show(this);
                    UpdateStatus($"📄 {Path.GetFileName(path)}");
                }
                catch (Exception ex) { UpdateStatus($"No se pudo abrir el visor: {ex.Message}"); }
                return;
            }

            // Archivo sin visor propio: usar app predeterminada del sistema
            try
            {
                using var process = new System.Diagnostics.Process
                    { StartInfo = new System.Diagnostics.ProcessStartInfo(path) { UseShellExecute = true } };
                process.Start();
            }
            catch (Exception ex) { UpdateStatus($"No se pudo abrir el archivo: {ex.Message}"); }
        }

        private void OpenAudioPlayer(string path)
        {
            try
            {
                if (_playerMP3 == null || _playerMP3.IsDisposed)
                {
                    _playerMP3 = new FormPlayerMP3();
                    _playerMP3.FormClosed += (s, e) => _playerMP3 = null;
                    _playerMP3.Show(this);
                }
                else { _playerMP3.BringToFront(); }
                _playerMP3.AbrirArchivo(path);
                UpdateStatus($"▶ Reproduciendo: {Path.GetFileName(path)}");
            }
            catch (Exception ex) { UpdateStatus($"No se pudo abrir el reproductor de audio: {ex.Message}"); }
        }

        private void OpenVideoPlayer(string path)
        {
            try
            {
                if (_playerMP4 == null || _playerMP4.IsDisposed)
                {
                    _playerMP4 = new FormPlayerMP4();
                    _playerMP4.FormClosed += (s, e) => _playerMP4 = null;
                    _playerMP4.Show(this);
                }
                else { _playerMP4.BringToFront(); }
                _playerMP4.AbrirArchivo(path);
                UpdateStatus($"▶ Reproduciendo: {Path.GetFileName(path)}");
            }
            catch (Exception ex) { UpdateStatus($"No se pudo abrir el reproductor de video: {ex.Message}"); }
        }
    }
}
