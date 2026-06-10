using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    public partial class Form1 : Form
    {
        // ── Historial de navegación ───────────────────────────────────────────

        private readonly Stack<string> _backHistory    = new();
        private readonly Stack<string> _forwardHistory = new();
        private string _currentPath = string.Empty;
        private bool   _suppressAddressSelection;

        // ── Íconos ────────────────────────────────────────────────────────────

        private readonly ProveedorIconosArchivo _proveedorIconos = new();

        // ── Índice de directorios ─────────────────────────────────────────────

        private readonly Dictionary<string, (int folders, int files)> _directoryContentCache =
            new(StringComparer.OrdinalIgnoreCase);

        // ── Referencias a formularios secundarios ─────────────────────────────

        private readonly List<Button> _sidebarButtons = new();
        private FormPlayerMP3?     _playerMP3;
        private FormPlayerMP4?     _playerMP4;
        private FormCamara?        _formCamara;
        private FormGrabadorAudio? _formGrabador;
        private FormMigrarDatos?   _formMigrar;
        private FormLimpiezaDatos? _formLimpieza;

        // ── Portapapeles interno ──────────────────────────────────────────────

        private string _rutaPortapapeles = string.Empty;
        private bool   _esCorte          = false;

        // ─────────────────────────────────────────────────────────────────────

        public Form1()
        {
            InitializeComponent();
            listViewFiles.SmallImageList = _proveedorIconos.IconosPequenos;
            listViewFiles.LargeImageList = _proveedorIconos.IconosGrandes;
            SetListViewView(listViewFiles.View);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BuildSidebar();
            UpdateStatus("Listo");
        }

        // ══════════════════════════════════════════════════════════════════════
        //  AYUDANTES GENERALES
        // ══════════════════════════════════════════════════════════════════════

        private void AddAddressHistory(string path)
        {
            _suppressAddressSelection = true;
            try
            {
                for (var i = 0; i < toolStripComboBoxAddress.Items.Count; i++)
                {
                    if (!string.Equals(toolStripComboBoxAddress.Items[i] as string, path,
                        StringComparison.OrdinalIgnoreCase)) continue;
                    toolStripComboBoxAddress.Items.RemoveAt(i);
                    break;
                }
                toolStripComboBoxAddress.Items.Insert(0, path);
                toolStripComboBoxAddress.Text = path;
            }
            finally { _suppressAddressSelection = false; }
        }

        private void UpdateNavigationButtons()
        {
            toolStripButtonBack.Enabled    = _backHistory.Count > 0;
            toolStripButtonForward.Enabled = _forwardHistory.Count > 0;
            toolStripButtonUp.Enabled      = !string.IsNullOrEmpty(_currentPath)
                                             && Directory.GetParent(_currentPath) != null;
        }

        private void UpdateStatus(string message) => toolStripStatusLabel.Text = message;

        private void SetListViewView(View view)
        {
            listViewFiles.View = view;
            UpdateViewMenuChecks(view);
        }

        private void UpdateViewMenuChecks(View view)
        {
            toolStripMenuItemViewLargeIcons.Checked = view == View.LargeIcon;
            toolStripMenuItemViewSmallIcons.Checked = view == View.SmallIcon;
            toolStripMenuItemViewList.Checked       = view == View.List;
            toolStripMenuItemViewDetails.Checked    = view == View.Details;
            toolStripMenuItemViewTile.Checked       = view == View.Tile;
        }

        private static string NormalizePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;
            return Path.GetFullPath(path.Trim());
        }

        private static void CopiarDirectorio(string origen, string destino)
        {
            Directory.CreateDirectory(destino);
            foreach (var archivo in Directory.GetFiles(origen))
                File.Copy(archivo, Path.Combine(destino, Path.GetFileName(archivo)), overwrite: true);
            foreach (var sub in Directory.GetDirectories(origen))
                CopiarDirectorio(sub, Path.Combine(destino, Path.GetFileName(sub)));
        }

        // ── Stubs de eventos no usados ────────────────────────────────────────

        private void listViewFiles_SelectedIndexChanged(object sender, EventArgs e) { }
        private void richTextBoxIndexResults_TextChanged(object sender, EventArgs e) { }
        private void TreeViewDirectories_BeforeExpand(object sender, TreeViewCancelEventArgs e) { }
        private void TreeViewDirectories_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) { }
    }
}
