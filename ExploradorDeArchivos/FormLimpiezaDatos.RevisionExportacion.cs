using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    public partial class FormLimpiezaDatos
    {
// ══════════════════════════════════════════════════════════════════════
        //  PASO 4: REVISAR
        // ══════════════════════════════════════════════════════════════════════

        private void RellenarStats()
        {
            _flowStats.Controls.Clear();
            int celdas = _filasOriginales.Count > 0 ? ContarCambios() : 0;

            var datos = new[]
            {
                ("Total filas",       _filasEditadas.Count.ToString("N0"),  AppColors.AzulClaro),
                ("Celdas corregidas", celdas.ToString("N0"),                AppColors.VerdeClaro),
                ("Columnas",          _columnas.Count.ToString(),            AppColors.Naranja),
                ("Formato entrada",   _extArchivo.ToUpperInvariant(),        Color.FromArgb(167, 139, 250)),
            };

            foreach (var (lbl, val, color) in datos)
                _flowStats.Controls.Add(CrearTarjetaStat(lbl, val, color));
        }

        private static Panel CrearTarjetaStat(string etiqueta, string valor, Color color)
        {
            var card = new Panel
            {
                Size      = new Size(160, 72),
                Margin    = new Padding(0, 0, 10, 0),
                BackColor = AppColors.BgPanel
            };

            var lblValor = new Label
            {
                Text     = valor,
                Location = new Point(12, 10),
                AutoSize = true,
            };
            UIStyler.AplicarLabelTituloCard(lblValor, color);

            var lblEtiqueta = new Label
            {
                Text     = etiqueta,
                Location = new Point(12, 46),
                AutoSize = true,
            };
            UIStyler.AplicarLabelSubtituloCard(lblEtiqueta);

            card.Controls.Add(lblValor);
            card.Controls.Add(lblEtiqueta);
            return card;
        }

        private int ContarCambios()
        {
            int cambios = 0;
            for (int i = 0; i < Math.Min(_filasOriginales.Count, _filasLimpias.Count); i++)
                foreach (var col in _columnas)
                {
                    var orig   = _filasOriginales[i].TryGetValue(col, out var o) ? o : "";
                    var limpio = _filasLimpias[i].TryGetValue(col, out var l)    ? l : "";
                    if (orig != limpio) cambios++;
                }
            return cambios;
        }

        private void RellenarGrid()
        {
            _grid.Columns.Clear();
            var colNum = new DataGridViewTextBoxColumn
            {
                HeaderText = "#",
                Width      = 44,
                ReadOnly   = true,
            };
            UIStyler.AplicarEstiloGridOscuro(colNum);
            _grid.Columns.Add(colNum);

            foreach (var col in _columnas)
            {
                var color = _metaCols.TryGetValue(col, out var m) && ColorPorTipo.TryGetValue(m.Tipo, out var cc)
                    ? cc : AppColors.TextoSec;
                _grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = col,
                    Width      = 140,
                    HeaderCell = { Style = { ForeColor = color } }
                });
            }

            CargarPaginaGrid();
        }

        private void CargarPaginaGrid()
        {
            _grid.Rows.Clear();
            int inicio = _paginaActual * FilasPorPagina;
            int fin    = Math.Min(inicio + FilasPorPagina, _filasEditadas.Count);

            for (int i = inicio; i < fin; i++)
            {
                var fila = _filasEditadas[i];
                var orig = i < _filasOriginales.Count ? _filasOriginales[i] : null;

                var vals = new List<object> { i + 1 };
                vals.AddRange(_columnas.Select(c => (object)(fila.TryGetValue(c, out var v) ? v : "")));

                int rowIdx = _grid.Rows.Add(vals.ToArray());
                ResaltarCeldasModificadas(_grid.Rows[rowIdx], orig, fila);
            }
        }

        private void ResaltarCeldasModificadas(
            DataGridViewRow row,
            Dictionary<string, string>? orig,
            Dictionary<string, string> fila)
        {
            if (orig == null) return;

            for (int j = 0; j < _columnas.Count; j++)
            {
                var col     = _columnas[j];
                var valOrig = orig.TryGetValue(col,  out var o) ? o : "";
                var valNew  = fila.TryGetValue(col,  out var n) ? n : "";

                if (valOrig == valNew) continue;

                row.Cells[j + 1].Style.BackColor = Color.FromArgb(6, 40, 30);
                row.Cells[j + 1].Style.ForeColor = AppColors.VerdeClaro;
                row.Cells[j + 1].ToolTipText     = $"Antes: {valOrig}";
            }
        }

        private void RellenarPaginacion()
        {
            int total = (int)Math.Ceiling((double)_filasEditadas.Count / FilasPorPagina);
            _panelPaginacion.Controls.Clear();
            if (total <= 1) return;

            _lblPagina.Text        = $"Página {_paginaActual + 1} / {total}";
            _btnAnterior.Enabled   = _paginaActual > 0;
            _btnSiguiente.Enabled  = _paginaActual < total - 1;

            int cx = _panelPaginacion.Width / 2;
            _btnAnterior.Location  = new Point(cx - 80, 6);
            _lblPagina.Location    = new Point(cx - 40, 12);
            _btnSiguiente.Location = new Point(cx + 50, 6);

            _panelPaginacion.Controls.AddRange(new Control[] { _btnAnterior, _lblPagina, _btnSiguiente });
        }

        private void CambiarPagina(int delta)
        {
            int total = (int)Math.Ceiling((double)_filasEditadas.Count / FilasPorPagina);
            _paginaActual = Math.Clamp(_paginaActual + delta, 0, total - 1);
            CargarPaginaGrid();
            RellenarPaginacion();
        }

        private void Grid_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 1) return;
            int filaReal = _paginaActual * FilasPorPagina + e.RowIndex;
            if (filaReal >= _filasEditadas.Count) return;
            var col = _columnas[e.ColumnIndex - 1];
            _filasEditadas[filaReal][col] = _grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? "";
        }

        // ══════════════════════════════════════════════════════════════════════
        //  EXPORTAR
        // ══════════════════════════════════════════════════════════════════════

        private void Exportar(string formato)
        {
            var filtro = formato switch
            {
                "csv"  => "CSV|*.csv",
                "json" => "JSON|*.json",
                "xlsx" => "Excel|*.xlsx",
                _      => "*.*|*.*"
            };
            using var dlg = new SaveFileDialog
            {
                Title    = "Guardar archivo limpio",
                Filter   = filtro,
                FileName = $"limpio_{Path.GetFileNameWithoutExtension(_nombreArchivo)}.{formato}"
            };
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                switch (formato)
                {
                    case "csv":  ServicioArchivosLimpiezaDatos.ExportarCsv(dlg.FileName, _columnas, _filasEditadas);  break;
                    case "json": ServicioArchivosLimpiezaDatos.ExportarJson(dlg.FileName, _columnas, _filasEditadas); break;
                    case "xlsx": ExportarXLSX(dlg.FileName); break;
                }
                MessageBox.Show($"✅  Archivo guardado:\n{dlg.FileName}", "Listo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MostrarError("Error al exportar: " + ex.Message);
            }
        }

        private void ExportarXLSX(string ruta)
        {
            ServicioArchivosLimpiezaDatos.ExportarCsv(Path.ChangeExtension(ruta, ".csv"), _columnas, _filasEditadas);
            MessageBox.Show(
                "Se exportó como CSV (compatible con Excel).\n" +
                "Para exportar .xlsx nativo agrega el paquete 'ClosedXML' al proyecto.",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
    }
}

