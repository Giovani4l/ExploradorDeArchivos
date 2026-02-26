using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    public partial class Form1 : Form
    {
        private readonly Stack<string> _backHistory = new();
        private readonly Stack<string> _forwardHistory = new();
        private string _currentPath = string.Empty;
        private bool _suppressAddressSelection;
        private readonly ImageList _fileImageList = new()
        {
            ColorDepth = ColorDepth.Depth32Bit,
            ImageSize = new Size(16, 16)
        };
        private readonly ImageList _largeFileImageList = new()
        {
            ColorDepth = ColorDepth.Depth32Bit,
            ImageSize = new Size(32, 32)
        };
        private readonly Dictionary<string, int> _iconIndexMap = new(StringComparer.OrdinalIgnoreCase);
        private const string FolderIconKey = "__folder__";
        private readonly Dictionary<string, (int folders, int files)> _directoryContentCache = new(StringComparer.OrdinalIgnoreCase);
        private const int MaxIndexedDirectories = 10;
        private const int MaxFilesPerDirectoryInIndex = 6;

        public Form1()
        {
            InitializeComponent();
            listViewFiles.SmallImageList = _fileImageList;
            listViewFiles.LargeImageList = _largeFileImageList;
            SetListViewView(listViewFiles.View);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDriveNodes();
            UpdateStatus("Listo");
        }

        private void LoadDriveNodes()
        {
            treeViewDirectories.Nodes.Clear();
            foreach (var drive in DriveInfo.GetDrives().OrderBy(d => d.Name))
            {
                var rootPath = drive.RootDirectory.FullName;
                var node = new TreeNode($"{drive.Name.TrimEnd(Path.DirectorySeparatorChar)} [{drive.DriveType}]")
                {
                    Tag = rootPath
                };

                if (HasSubdirectories(rootPath))
                {
                    node.Nodes.Add(new TreeNode());
                }

                treeViewDirectories.Nodes.Add(node);
            }
        }

        private void TreeViewDirectories_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node == null || string.IsNullOrEmpty(e.Node.Tag as string))
            {
                return;
            }

            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Tag == null)
            {
                PopulateChildNodes(e.Node);
            }
        }

        private void TreeViewDirectories_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node?.Tag is string path)
            {
                NavigateTo(path);
            }
        }

        private void PopulateChildNodes(TreeNode parent)
        {
            parent.Nodes.Clear();
            var parentPath = parent.Tag as string;
            if (string.IsNullOrEmpty(parentPath))
            {
                return;
            }

            try
            {
                foreach (var directory in Directory.GetDirectories(parentPath).OrderBy(d => d))
                {
                    var info = new DirectoryInfo(directory);
                    var child = new TreeNode(info.Name) { Tag = info.FullName };
                    if (HasSubdirectories(info.FullName))
                    {
                        child.Nodes.Add(new TreeNode());
                    }

                    parent.Nodes.Add(child);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (IOException)
            {
            }
        }

        private void ListViewFiles_DoubleClick(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0)
            {
                return;
            }

            if (listViewFiles.SelectedItems[0].Tag is not string selectedPath)
            {
                return;
            }

            if (Directory.Exists(selectedPath))
            {
                NavigateTo(selectedPath);
            }
            else if (File.Exists(selectedPath))
            {
                OpenFile(selectedPath);
            }
        }

        private void ToolStripButtonBack_Click(object sender, EventArgs e)
        {
            while (_backHistory.Count > 0)
            {
                var target = _backHistory.Pop();
                if (!Directory.Exists(target))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(_currentPath))
                {
                    _forwardHistory.Push(_currentPath);
                }

                NavigateTo(target, false);
                return;
            }

            UpdateNavigationButtons();
        }

        private void ToolStripButtonForward_Click(object sender, EventArgs e)
        {
            while (_forwardHistory.Count > 0)
            {
                var target = _forwardHistory.Pop();
                if (!Directory.Exists(target))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(_currentPath))
                {
                    _backHistory.Push(_currentPath);
                }

                NavigateTo(target, false);
                return;
            }

            UpdateNavigationButtons();
        }

        private void ToolStripButtonUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentPath))
            {
                return;
            }

            var parent = Directory.GetParent(_currentPath);
            if (parent != null)
            {
                NavigateTo(parent.FullName);
            }
        }

        private void ToolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentPath))
            {
                NavigateTo(_currentPath, false);
            }
            else
            {
                LoadDriveNodes();
            }
        }

        private void ToolStripButtonNewFolder_Click(object sender, EventArgs e)
        {
            var defaultPath = string.IsNullOrEmpty(_currentPath)
                ? Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                : _currentPath;

            using var dialog = new FolderBrowserDialog
            {
                Description = "Seleccione la carpeta donde crear la nueva carpeta",
                SelectedPath = defaultPath,
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var basePath = dialog.SelectedPath;
            var folderName = GetNextAvailableFolderName(basePath);
            var targetPath = Path.Combine(basePath, folderName);
            try
            {
                Directory.CreateDirectory(targetPath);
                if (string.Equals(basePath, _currentPath, StringComparison.OrdinalIgnoreCase))
                {
                    PopulateListView(_currentPath);
                }
                else
                {
                    NavigateTo(basePath);
                }

                UpdateStatus($"Carpeta \"{folderName}\" creada en \"{basePath}\".");
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException or PathTooLongException)
            {
                UpdateStatus($"No se pudo crear la carpeta: {ex.Message}");
            }
        }

        private static string GetNextAvailableFolderName(string parentPath)
        {
            const string baseName = "Nueva carpeta";
            var candidate = baseName;
            var counter = 2;

            while (Directory.Exists(Path.Combine(parentPath, candidate)))
            {
                candidate = $"{baseName} ({counter})";
                counter++;
            }

            return candidate;
        }

        private void ToolStripComboBoxAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                NavigateTo(toolStripComboBoxAddress.Text);
            }
        }

        private void ToolStripComboBoxAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressAddressSelection)
            {
                return;
            }

            if (toolStripComboBoxAddress.SelectedItem is string path)
            {
                NavigateTo(path);
            }
        }

        private void ToolStripMenuItemView_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is string viewName &&
                Enum.TryParse<View>(viewName, out var view))
            {
                SetListViewView(view);
            }
        }

        private void ToolStripButtonIndexSearch_Click(object? sender, EventArgs e)
        {
            BeginIndexSearch(toolStripTextBoxIndex.Text);
        }

        private void ToolStripTextBoxIndex_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.Handled = true;
            e.SuppressKeyPress = true;
            BeginIndexSearch(toolStripTextBoxIndex.Text);
        }

        private void BeginIndexSearch(string query)
        {
            var targetPath = ResolveIndexTarget(query);
            if (string.IsNullOrEmpty(targetPath))
            {
                richTextBoxIndexResults.Text = "No se encontró una carpeta válida para indexar.";
                return;
            }

            richTextBoxIndexResults.Text = BuildIndexSummary(targetPath);
            UpdateStatus($"Índice generado para \"{Path.GetFileName(targetPath) ?? targetPath}\"");
        }

        private string ResolveIndexTarget(string query)
        {
            var basePath = string.IsNullOrEmpty(_currentPath)
                ? Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                : _currentPath;

            if (!string.IsNullOrWhiteSpace(query))
            {
                var normalized = query.Trim();
                if (Directory.Exists(normalized))
                {
                    return normalized;
                }

                var relative = Path.Combine(basePath, normalized);
                if (Directory.Exists(relative))
                {
                    return relative;
                }

                try
                {
                    var match = Directory.EnumerateDirectories(basePath, $"*{normalized}*", SearchOption.TopDirectoryOnly)
                        .OrderBy(d => d)
                        .FirstOrDefault();
                    if (!string.IsNullOrEmpty(match))
                    {
                        return match;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (IOException)
                {
                }
            }

            return basePath;
        }

        private string BuildIndexSummary(string path)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Carpeta");
            builder.AppendLine($"-> {Path.GetFileName(path) ?? path}");
            builder.AppendLine("Archivos");

            var rootFiles = EnumerateFilesSafe(path, MaxFilesPerDirectoryInIndex + 1);
            if (rootFiles.Length == 0)
            {
                builder.AppendLine("   (sin archivos directos visibles)");
            }
            else
            {
                var displayCount = Math.Min(rootFiles.Length, MaxFilesPerDirectoryInIndex);
                for (var i = 0; i < displayCount; i++)
                {
                    builder.AppendLine($"   * {Path.GetFileName(rootFiles[i])}");
                }

                if (rootFiles.Length > MaxFilesPerDirectoryInIndex)
                {
                    builder.AppendLine("   * ...");
                }
            }

            var subdirectories = EnumerateDirectoriesSafe(path);
            if (subdirectories.Length == 0)
            {
                builder.AppendLine("-> Subcarpetas: ninguna");
                return builder.ToString().TrimEnd();
            }

            builder.AppendLine("-> Subcarpetas");
            foreach (var directory in subdirectories)
            {
                AppendDirectoryIndex(builder, directory);
            }

            return builder.ToString().TrimEnd();
        }

        private void AppendDirectoryIndex(StringBuilder builder, string directory)
        {
            var name = Path.GetFileName(directory) ?? directory;
            var counts = GetDirectoryContentCounts(directory);
            builder.Append("   -> ");
            builder.Append(name);
            if (counts.HasValue)
            {
                builder.AppendLine($" ({counts.Value.folders} carpetas, {counts.Value.files} archivos)");
            }
            else
            {
                builder.AppendLine(" (contenido inaccesible)");
            }

            var files = EnumerateFilesSafe(directory, MaxFilesPerDirectoryInIndex + 1);
            if (files.Length > 0)
            {
                var displayCount = Math.Min(files.Length, MaxFilesPerDirectoryInIndex);
                for (var i = 0; i < displayCount; i++)
                {
                    builder.AppendLine($"      * {Path.GetFileName(files[i])}");
                }

                if (files.Length > MaxFilesPerDirectoryInIndex)
                {
                    builder.AppendLine("      * ...");
                }
            }
        }

        private string[] EnumerateDirectoriesSafe(string path)
        {
            try
            {
                return Directory.GetDirectories(path).OrderBy(d => d).Take(MaxIndexedDirectories).ToArray();
            }
            catch (UnauthorizedAccessException)
            {
                return Array.Empty<string>();
            }
            catch (IOException)
            {
                return Array.Empty<string>();
            }
        }

        private string[] EnumerateFilesSafe(string path, int limit)
        {
            try
            {
                return Directory.GetFiles(path).OrderBy(f => f).Take(limit).ToArray();
            }
            catch (UnauthorizedAccessException)
            {
                return Array.Empty<string>();
            }
            catch (IOException)
            {
                return Array.Empty<string>();
            }
        }

        private void NavigateTo(string path, bool addToHistory = true)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            path = NormalizePath(path);
            if (!Directory.Exists(path))
            {
                UpdateStatus("Ruta no válida o inaccesible.");
                return;
            }

            if (addToHistory && !string.Equals(_currentPath, path, StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(_currentPath))
                {
                    _backHistory.Push(_currentPath);
                }

                _forwardHistory.Clear();
            }

            _currentPath = path;
            AddAddressHistory(path);
            PopulateListView(path);
            ExpandTreeToPath(path);
            UpdateNavigationButtons();
        }

        private void PopulateListView(string path)
        {
            _directoryContentCache.Clear();
            richTextBoxIndexResults.Clear();
            listViewFiles.Items.Clear();

            try
            {
                var directoryInfo = new DirectoryInfo(path);
                var directories = directoryInfo.GetDirectories().OrderBy(d => d.Name).ToArray();
                foreach (var directory in directories)
                {
                    var item = new ListViewItem(directory.Name)
                    {
                        Tag = directory.FullName,
                        ImageIndex = GetImageIndexForPath(directory.FullName, true)
                    };
                    item.SubItems.Add("Carpeta");
                    item.SubItems.Add(string.Empty);
                    item.SubItems.Add(directory.LastWriteTime.ToString("g"));
                    listViewFiles.Items.Add(item);
                }

                var files = directoryInfo.GetFiles().OrderBy(f => f.Name).ToArray();
                foreach (var file in files)
                {
                    var item = new ListViewItem(file.Name)
                    {
                        Tag = file.FullName,
                        ImageIndex = GetImageIndexForPath(file.FullName, false)
                    };
                    item.SubItems.Add(file.Extension);
                    item.SubItems.Add(GetFriendlySize(file.Length));
                    item.SubItems.Add(file.LastWriteTime.ToString("g"));
                    listViewFiles.Items.Add(item);
                }

                UpdateStatus($"Carpetas: {directories.Length} · Archivos: {files.Length}");
            }
            catch (UnauthorizedAccessException)
            {
                UpdateStatus("Acceso denegado al listar el contenido.");
            }
            catch (IOException ex)
            {
                UpdateStatus($"Error al listar: {ex.Message}");
            }
        }

        private int GetImageIndexForPath(string path, bool isDirectory)
        {
            var key = isDirectory ? FolderIconKey : Path.GetExtension(path);
            if (string.IsNullOrWhiteSpace(key))
            {
                key = isDirectory ? FolderIconKey : "__file__";
            }

            if (_iconIndexMap.TryGetValue(key, out var index))
            {
                return index;
            }

            var smallIcon = GetShellIcon(path, isDirectory, largeIcon: false) ?? SystemIcons.Application;
            var largeIcon = GetShellIcon(path, isDirectory, largeIcon: true) ?? smallIcon;

            _fileImageList.Images.Add(smallIcon);
            _largeFileImageList.Images.Add(largeIcon);
            index = _fileImageList.Images.Count - 1;
            _iconIndexMap[key] = index;
            return index;
        }

        private static Icon? GetShellIcon(string path, bool isDirectory, bool largeIcon)
        {
            var attributes = isDirectory ? FileAttributes.Directory : FileAttributes.Normal;
            var flags = SHGFI_ICON | SHGFI_USEFILEATTRIBUTES | (largeIcon ? SHGFI_LARGEICON : SHGFI_SMALLICON);

            if (SHGetFileInfo(path, attributes, out var shfi, (uint)Marshal.SizeOf<ShFileInfo>(), flags) == IntPtr.Zero ||
                shfi.hIcon == IntPtr.Zero)
            {
                return null;
            }

            try
            {
                return (Icon?)Icon.FromHandle(shfi.hIcon).Clone();
            }
            finally
            {
                DestroyIcon(shfi.hIcon);
            }
        }

        private const uint SHGFI_ICON = 0x000000100;
        private const uint SHGFI_SMALLICON = 0x000000001;
        private const uint SHGFI_LARGEICON = 0x000000000;
        private const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct ShFileInfo
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr SHGetFileInfo(string pszPath, FileAttributes dwFileAttributes, out ShFileInfo psfi, uint cbFileInfo, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        private void AddAddressHistory(string path)
        {
            _suppressAddressSelection = true;
            try
            {
                for (var i = 0; i < toolStripComboBoxAddress.Items.Count; i++)
                {
                    if (string.Equals(toolStripComboBoxAddress.Items[i] as string, path, StringComparison.OrdinalIgnoreCase))
                    {
                        toolStripComboBoxAddress.Items.RemoveAt(i);
                        break;
                    }
                }

                toolStripComboBoxAddress.Items.Insert(0, path);
                toolStripComboBoxAddress.Text = path;
            }
            finally
            {
                _suppressAddressSelection = false;
            }
        }

        private void UpdateNavigationButtons()
        {
            toolStripButtonBack.Enabled = _backHistory.Count > 0;
            toolStripButtonForward.Enabled = _forwardHistory.Count > 0;
            toolStripButtonUp.Enabled = !string.IsNullOrEmpty(_currentPath) && Directory.GetParent(_currentPath) != null;
        }

        private void UpdateStatus(string message)
        {
            toolStripStatusLabel.Text = message;
        }

        private static string GetFriendlySize(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            double size = bytes;
            var index = 0;
            while (size >= 1024 && index < suffixes.Length - 1)
            {
                size /= 1024;
                index++;
            }

            return $"{size:0.##} {suffixes[index]}";
        }

        private static bool HasSubdirectories(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            try
            {
                return Directory.EnumerateDirectories(path).Any();
            }
            catch
            {
                return false;
            }
        }

        private void SetListViewView(View view)
        {
            listViewFiles.View = view;
            UpdateViewMenuChecks(view);
        }

        private void UpdateViewMenuChecks(View view)
        {
            toolStripMenuItemViewLargeIcons.Checked = view == View.LargeIcon;
            toolStripMenuItemViewSmallIcons.Checked = view == View.SmallIcon;
            toolStripMenuItemViewList.Checked = view == View.List;
            toolStripMenuItemViewDetails.Checked = view == View.Details;
            toolStripMenuItemViewTile.Checked = view == View.Tile;
        }

        private void OpenFile(string path)
        {
            try
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo(path)
                    {
                        UseShellExecute = true
                    }
                };
                process.Start();
            }
            catch (Exception ex)
            {
                UpdateStatus($"No se pudo abrir el archivo: {ex.Message}");
            }
        }

        private static string NormalizePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            // Quitar espacios y normalizar separadores
            path = path.Trim();
            path = Path.GetFullPath(path);
            return path;
        }

        private void ExpandTreeToPath(string path)
        {
            if (string.IsNullOrEmpty(path) || treeViewDirectories.Nodes.Count == 0)
            {
                return;
            }

            TreeNode? targetNode = null;
            foreach (TreeNode node in treeViewDirectories.Nodes)
            {
                if (node.Tag is string rootPath && path.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase))
                {
                    targetNode = node;
                    break;
                }
            }

            if (targetNode == null)
            {
                return;
            }

            var segments = path.TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
            var currentNode = targetNode;
            var currentPath = currentNode.Tag as string ?? string.Empty;

            for (int i = segments.Length - 1; i > 0; i--)
            {
                var nextPath = Path.Combine(currentPath, segments[i]);
                var found = false;

                foreach (TreeNode child in currentNode.Nodes)
                {
                    if (child.Tag is string childPath && string.Equals(childPath, nextPath, StringComparison.OrdinalIgnoreCase))
                    {
                        currentNode = child;
                        currentPath = childPath;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    break;
                }
            }

            treeViewDirectories.SelectedNode = currentNode;
            currentNode.Expand();
        }

        private void treeViewDirectories_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void ListViewFiles_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            if (e.Item.Tag is not string path)
            {
                return;
            }

            e.Item.ToolTipText = Directory.Exists(path)
                ? GetDirectoryTooltipText(path)
                : GetFileTooltipText(path);
        }

        private string GetDirectoryTooltipText(string path)
        {
            var counts = GetDirectoryContentCounts(path);
            return counts.HasValue
                ? $"Contiene {counts.Value.folders} carpetas · {counts.Value.files} archivos"
                : "Contenido inaccesible";
        }

        private (int folders, int files)? GetDirectoryContentCounts(string path)
        {
            if (_directoryContentCache.TryGetValue(path, out var cached))
            {
                return cached;
            }

            try
            {
                var folders = Directory.EnumerateDirectories(path).Count();
                var files = Directory.EnumerateFiles(path).Count();
                var result = (folders, files);
                _directoryContentCache[path] = result;
                return result;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
            catch (IOException)
            {
                return null;
            }
        }

        private string GetFileTooltipText(string path)
        {
            try
            {
                var info = new FileInfo(path);
                return $"Tamaño: {GetFriendlySize(info.Length)}";
            }
            catch (IOException)
            {
                return "Archivo inaccesible";
            }
            catch (UnauthorizedAccessException)
            {
                return "Archivo inaccesible";
            }
        }
    }
}
