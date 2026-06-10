using System;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    partial class Form1
    {
        // ══════════════════════════════════════════════════════════════════════
        //  DRAG & DROP
        // ══════════════════════════════════════════════════════════════════════

        // Resaltado visual del botón del sidebar que está bajo el cursor
        private Button? _sidebarDragTarget;

        // Variables para detectar drag vs click
        private Point _dragStartPoint;
        private ListViewItem? _dragStartItem;
        private bool _isDragging;

        // ── Iniciar drag desde el ListView hacia afuera ───────────────────────

        private void ListViewFiles_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var hit = listViewFiles.HitTest(e.Location);
            if (hit.Item == null) return;

            string? ruta = hit.Item.Tag?.ToString();
            if (string.IsNullOrEmpty(ruta)) return;
            if (!File.Exists(ruta) && !Directory.Exists(ruta)) return;

            // Guardar información del inicio del drag
            _dragStartPoint = e.Location;
            _dragStartItem = hit.Item;
            _isDragging = false;
        }

        // ── Detectar movimiento de mouse para iniciar drag ─────────────────────

        private void ListViewFiles_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (_dragStartItem == null) return;
            if (_isDragging) return;

            // Calcular la distancia movida
            int dx = Math.Abs(e.X - _dragStartPoint.X);
            int dy = Math.Abs(e.Y - _dragStartPoint.Y);

            // Si se movió más de 5 píxeles, iniciar drag
            if (dx > 5 || dy > 5)
            {
                _isDragging = true;
                string? ruta = _dragStartItem.Tag?.ToString();
                if (string.IsNullOrEmpty(ruta)) return;

                var col = new StringCollection();
                col.Add(ruta);
                var data = new DataObject();
                data.SetFileDropList(col);

                // Shift = mover, sin modificador = copiar
                listViewFiles.DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        // ── Limpiar estado de drag al soltar ──────────────────────────────────

        private void ListViewFiles_MouseUp(object sender, MouseEventArgs e)
        {
            _dragStartItem = null;
            _isDragging = false;
        }

        // ── Soltar sobre el ListView (carpeta actual o subcarpeta) ────────────

        private void ListViewFiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = TieneArchivos(e.Data) ? EfectoDrag(e) : DragDropEffects.None;
        }

        private void ListViewFiles_DragOver(object sender, DragEventArgs e)
        {
            if (!TieneArchivos(e.Data)) { e.Effect = DragDropEffects.None; return; }

            // Resaltar subcarpeta bajo el cursor si existe
            var pt   = listViewFiles.PointToClient(new Point(e.X, e.Y));
            var hit  = listViewFiles.HitTest(pt);
            bool esSubcarpeta = hit.Item?.Tag is string p && Directory.Exists(p);

            e.Effect = esSubcarpeta ? EfectoDrag(e) : (string.IsNullOrEmpty(_currentPath)
                ? DragDropEffects.None : EfectoDrag(e));
        }

        private void ListViewFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (!TieneArchivos(e.Data)) return;

            // Determinar carpeta destino: subcarpeta bajo el cursor o carpeta actual
            var pt      = listViewFiles.PointToClient(new Point(e.X, e.Y));
            var hit     = listViewFiles.HitTest(pt);
            string destino = (hit.Item?.Tag is string p && Directory.Exists(p))
                ? p
                : _currentPath;

            if (string.IsNullOrEmpty(destino))
            {
                MessageBox.Show("Navega a una carpeta primero o suelta sobre una subcarpeta.",
                    "Drag & Drop", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            EjecutarDropEnCarpeta(e, destino);
        }

        // ── Soltar sobre botones del Sidebar ──────────────────────────────────

        private void SidebarButton_DragEnter(object sender, DragEventArgs e)
        {
            if (!TieneArchivos(e.Data)) { e.Effect = DragDropEffects.None; return; }

            if (sender is Button btn && btn.Tag is string ruta && Directory.Exists(ruta))
            {
                ResaltarSidebarTarget(btn);
                e.Effect = EfectoDrag(e);
            }
            else { e.Effect = DragDropEffects.None; }
        }

        private void SidebarButton_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = (TieneArchivos(e.Data) && sender is Button btn &&
                        btn.Tag is string ruta && Directory.Exists(ruta))
                ? EfectoDrag(e)
                : DragDropEffects.None;
        }

        private void SidebarButton_DragLeave(object sender, EventArgs e) =>
            LimpiarResaltadoSidebar();

        private void SidebarButton_DragDrop(object sender, DragEventArgs e)
        {
            LimpiarResaltadoSidebar();
            if (!TieneArchivos(e.Data)) return;
            if (sender is not Button btn || btn.Tag is not string destino) return;
            if (!Directory.Exists(destino)) return;

            EjecutarDropEnCarpeta(e, destino);
        }

        // ── Lógica compartida de drop ─────────────────────────────────────────

        private void EjecutarDropEnCarpeta(DragEventArgs e, string destino)
        {
            string[]? archivos = e.Data?.GetData(DataFormats.FileDrop) as string[];
            if (archivos == null || archivos.Length == 0) return;

            bool mover = e.Effect == DragDropEffects.Move;
            int  ok    = 0;

            foreach (string origen in archivos)
            {
                try
                {
                    string nombre   = Path.GetFileName(origen.TrimEnd('\\', '/'));
                    string rutaDest = Path.Combine(destino, nombre);

                    // Mismo directorio: saltar
                    if (string.Equals(Path.GetDirectoryName(origen), destino,
                        StringComparison.OrdinalIgnoreCase)) continue;

                    // Preguntar si ya existe
                    if ((File.Exists(rutaDest) || Directory.Exists(rutaDest)) &&
                        MessageBox.Show(
                            $"\"{nombre}\" ya existe en el destino. ¿Reemplazar?",
                            "Drag & Drop", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                        != DialogResult.Yes) continue;

                    if (File.Exists(origen))
                    {
                        if (mover) File.Move(origen, rutaDest, overwrite: true);
                        else       File.Copy(origen, rutaDest, overwrite: true);
                    }
                    else if (Directory.Exists(origen))
                    {
                        CopiarDirectorio(origen, rutaDest);
                        if (mover) Directory.Delete(origen, recursive: true);
                    }
                    ok++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error con \"{Path.GetFileName(origen)}\":\n{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Refrescar si el destino es la carpeta visible
            if (string.Equals(destino, _currentPath, StringComparison.OrdinalIgnoreCase))
                PopulateListView(_currentPath);

            if (ok > 0)
                UpdateStatus($"✔ {ok} elemento(s) {(mover ? "movido(s)" : "copiado(s)")} → {destino}");
        }

        // ── Registro de D&D en botones del sidebar ────────────────────────────

        /// <summary>
        /// Llama a este método desde AddSidebarItem para registrar los eventos
        /// de Drag &amp; Drop en cada botón de carpeta del sidebar.
        /// </summary>
        private static void RegistrarDragDropEnBoton(Button btn)
        {
            btn.AllowDrop   = true;
            btn.DragEnter  += (s, e) => ((Form1)((Button)s!).FindForm()!).SidebarButton_DragEnter(s!, e);
            btn.DragOver   += (s, e) => ((Form1)((Button)s!).FindForm()!).SidebarButton_DragOver(s!, e);
            btn.DragLeave  += (s, e) => ((Form1)((Button)s!).FindForm()!).SidebarButton_DragLeave(s!, e);
            btn.DragDrop   += (s, e) => ((Form1)((Button)s!).FindForm()!).SidebarButton_DragDrop(s!, e);
        }

        // ── Ayudantes ─────────────────────────────────────────────────────────

        private static bool TieneArchivos(IDataObject? data) =>
            data != null && data.GetDataPresent(DataFormats.FileDrop);

        private static DragDropEffects EfectoDrag(DragEventArgs e) =>
            (e.KeyState & 4) != 0 ? DragDropEffects.Move : DragDropEffects.Copy;

        private void ResaltarSidebarTarget(Button btn)
        {
            LimpiarResaltadoSidebar();
            _sidebarDragTarget           = btn;
            _sidebarDragTarget.BackColor = AppColors.SidebarDragOver;
        }

        private void LimpiarResaltadoSidebar()
        {
            if (_sidebarDragTarget == null) return;
            _sidebarDragTarget.BackColor = Color.Transparent;
            _sidebarDragTarget           = null;
        }
    }
}
