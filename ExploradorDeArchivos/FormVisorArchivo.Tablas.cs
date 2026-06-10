using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using System.Globalization;

namespace ExploradorDeArchivos
{
    public partial class FormVisorArchivo
    {
// ══════════════════════════════════════════════════════════════════
        //  TABLA  CSV / TSV / Excel
        // ══════════════════════════════════════════════════════════════════
        private void CargarTabla()
        {
            var dt = _ext.Equals(".csv", StringComparison.OrdinalIgnoreCase) ||
                     _ext.Equals(".tsv", StringComparison.OrdinalIgnoreCase)
                     ? LeerCsv() : LeerExcel();

            _lblInfo = CrearLabelInfo($"📊  {dt.Rows.Count} filas  ·  {dt.Columns.Count} columnas  —  {Path.GetFileName(_filePath)}");
            panelContenido.Controls.Add(_lblInfo);

            _grid = new DataGridView
            {
                Dock                        = DockStyle.Fill,
                DataSource                  = dt,
                ReadOnly                    = false,
                AllowUserToAddRows          = true,
                AllowUserToDeleteRows       = true,
                AllowUserToResizeRows       = false,
                RowHeadersVisible           = true,
                AutoSizeColumnsMode         = DataGridViewAutoSizeColumnsMode.AllCells,
                SelectionMode               = DataGridViewSelectionMode.CellSelect,
                ClipboardCopyMode           = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText,
            };
            UIStyler.AplicarEstiloGridTabla(_grid);
            _grid.CellValueChanged += (s, ev) => MarcarModificado();
            _grid.DataBindingComplete += (s, ev) =>
            {
                foreach (DataGridViewColumn col in _grid.Columns)
                    if (col.Width > 300) col.Width = 300;
            };

            panelContenido.Controls.Add(_grid);
            _grid.BringToFront();

            MostrarBotones(guardar: true, ajustarCols: true, fuente: true);
        }

        private DataTable LeerCsv()
        {
            var dt        = new DataTable();
            var delimiter = _ext.Equals(".tsv", StringComparison.OrdinalIgnoreCase) ? '\t' : ',';
            using var reader = new StreamReader(_filePath, AyudanteFormatoArchivo.DetectarCodificacion(_filePath));
            using var csv    = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                { Delimiter = delimiter.ToString(), BadDataFound = null, MissingFieldFound = null });
            csv.Read(); csv.ReadHeader();
            foreach (var h in csv.HeaderRecord ?? Array.Empty<string>())
                dt.Columns.Add(string.IsNullOrWhiteSpace(h) ? $"Col{dt.Columns.Count + 1}" : h);
            while (csv.Read())
            {
                var row = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                    row[i] = csv.TryGetField(i, out string? val) ? val ?? "" : "";
                dt.Rows.Add(row);
            }
            return dt;
        }

        private DataTable LeerExcel()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var dt = new DataTable();
            using var stream = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using IExcelDataReader reader = _ext.Equals(".xls", StringComparison.OrdinalIgnoreCase)
                ? ExcelReaderFactory.CreateBinaryReader(stream)
                : ExcelReaderFactory.CreateOpenXmlReader(stream);
            if (!reader.Read()) return dt;
            for (int c = 0; c < reader.FieldCount; c++)
            {
                var h = reader.GetValue(c)?.ToString();
                dt.Columns.Add(string.IsNullOrWhiteSpace(h) ? $"Col{c + 1}" : h);
            }
            while (reader.Read())
            {
                var row = dt.NewRow();
                for (int c = 0; c < dt.Columns.Count && c < reader.FieldCount; c++)
                    row[c] = reader.GetValue(c)?.ToString() ?? "";
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}

