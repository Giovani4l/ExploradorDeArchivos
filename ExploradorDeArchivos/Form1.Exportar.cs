using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    partial class Form1
    {
        // ══════════════════════════════════════════════════════════════════════
        //  HERRAMIENTAS — EXPORTAR
        // ══════════════════════════════════════════════════════════════════════

        private static readonly Dictionary<string, (string Label, string Ext)[]> _mapaExportacion =
            new(StringComparer.OrdinalIgnoreCase)
            {
                [".docx"] = new[] { ("PDF (.pdf)",".pdf"), ("Texto plano (.txt)",".txt"), ("Texto enriquecido (.rtf)",".rtf"), ("Página web (.html)",".html"), ("OpenDocument (.odt)",".odt"), ("XML (.xml)",".xml"), ("Word antiguo (.doc)",".doc"), ("Plantilla de Word (.dotx)",".dotx") },
                [".doc"]  = new[] { ("PDF (.pdf)",".pdf"), ("Texto plano (.txt)",".txt"), ("RTF (.rtf)",".rtf"), ("Página web (.html)",".html"), ("OpenDocument (.odt)",".odt"), ("Word moderno (.docx)",".docx") },
                [".xlsx"] = new[] { ("CSV (.csv)",".csv"), ("PDF (.pdf)",".pdf"), ("Texto (.txt)",".txt"), ("XML (.xml)",".xml"), ("OpenDocument Spreadsheet (.ods)",".ods"), ("Excel antiguo (.xls)",".xls"), ("Página web (.html)",".html"), ("JSON (.json)",".json") },
                [".xls"]  = new[] { ("CSV (.csv)",".csv"), ("PDF (.pdf)",".pdf"), ("Excel moderno (.xlsx)",".xlsx"), ("XML (.xml)",".xml"), ("Texto (.txt)",".txt") },
                [".csv"]  = new[] { ("Excel (.xlsx)",".xlsx"), ("Texto (.txt)",".txt"), ("JSON (.json)",".json"), ("XML (.xml)",".xml"), ("SQL (.sql)",".sql"), ("PDF (.pdf)",".pdf") },
                [".txt"]  = new[] { ("Word (.docx)",".docx"), ("PDF (.pdf)",".pdf"), ("RTF (.rtf)",".rtf"), ("HTML (.html)",".html"), ("JSON (.json)",".json"), ("CSV (.csv)",".csv") },
                [".json"] = new[] { ("CSV (.csv)",".csv"), ("Excel (.xlsx)",".xlsx"), ("XML (.xml)",".xml"), ("Texto (.txt)",".txt"), ("SQL (.sql)",".sql"), ("PDF (.pdf)",".pdf") },
                [".xml"]  = new[] { ("Excel (.xlsx)",".xlsx"), ("CSV (.csv)",".csv"), ("JSON (.json)",".json"), ("Texto (.txt)",".txt"), ("HTML (.html)",".html") },
                [".pptx"] = new[] { ("PDF (.pdf)",".pdf"), ("Imágenes PNG (.png)",".png"), ("Imágenes JPG (.jpg)",".jpg"), ("Video (.mp4)",".mp4"), ("Presentación PPSX (.ppsx)",".ppsx"), ("Plantilla (.potx)",".potx") },
                [".rtf"]  = new[] { ("PDF (.pdf)",".pdf"), ("Word (.docx)",".docx"), ("Texto (.txt)",".txt"), ("HTML (.html)",".html") },
                [".html"] = new[] { ("PDF (.pdf)",".pdf"), ("Texto (.txt)",".txt"), ("Word (.docx)",".docx"), ("XML (.xml)",".xml") },
                [".htm"]  = new[] { ("PDF (.pdf)",".pdf"), ("Texto (.txt)",".txt"), ("Word (.docx)",".docx") },
                [".odt"]  = new[] { ("Word (.docx)",".docx"), ("PDF (.pdf)",".pdf"), ("RTF (.rtf)",".rtf"), ("Texto (.txt)",".txt") },
                [".ods"]  = new[] { ("Excel (.xlsx)",".xlsx"), ("CSV (.csv)",".csv"), ("PDF (.pdf)",".pdf") },
                [".pdf"]  = new[] { ("Word (.docx)",".docx"), ("Texto (.txt)",".txt"), ("HTML (.html)",".html"), ("Imágenes PNG (.png)",".png") },
            };

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            string ruta = listViewFiles.SelectedItems.Count > 0
                ? listViewFiles.SelectedItems[0].Tag?.ToString() ?? string.Empty
                : string.Empty;

            if (string.IsNullOrEmpty(ruta) || !File.Exists(ruta))
            {
                MessageBox.Show("Selecciona un archivo en el explorador primero.",
                    "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string ext = Path.GetExtension(ruta).ToLowerInvariant();
            if (!_mapaExportacion.TryGetValue(ext, out var opciones))
            {
                MessageBox.Show($"No hay formatos de exportación disponibles para archivos {ext}.",
                    "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var menu   = new ContextMenuStrip();
            var header = new ToolStripMenuItem($"Exportar \"{Path.GetFileName(ruta)}\" como:")
            {
                Enabled = false,
                Font    = new Font("Segoe UI", 8.5f, FontStyle.Italic)
            };
            menu.Items.Add(header);
            menu.Items.Add(new ToolStripSeparator());

            foreach (var (label, extDestino) in opciones)
            {
                string rutaC = ruta;
                string extC  = extDestino;
                var item     = new ToolStripMenuItem(label);
                item.Click  += (_, __) => EjecutarExportacion(rutaC, extC);
                menu.Items.Add(item);
            }

            var btn = sender as Button;
            var control = btn ?? (Control)panelSidebar;
            menu.Show(control, new Point(190, 0));
        }

        private void EjecutarExportacion(string rutaOrigen, string extDestino)
        {
            string nombreBase       = Path.GetFileNameWithoutExtension(rutaOrigen);
            string directorioOrigen = Path.GetDirectoryName(rutaOrigen) ?? _currentPath;

            using var sfd = new SaveFileDialog
            {
                Title            = "Guardar archivo exportado",
                FileName         = nombreBase + extDestino,
                InitialDirectory = directorioOrigen,
                Filter           = $"Archivo exportado (*{extDestino})|*{extDestino}|Todos los archivos (*.*)|*.*"
            };
            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            string rutaDestino = sfd.FileName;
            string extOrigen   = Path.GetExtension(rutaOrigen).ToLowerInvariant();

            try
            {
                bool completado = IntentarConversionNativa(rutaOrigen, rutaDestino, extOrigen, extDestino);

                if (completado)
                {
                    UpdateStatus($"✔ Exportado: {Path.GetFileName(rutaDestino)}");
                    MessageBox.Show($"Archivo exportado correctamente:\n{rutaDestino}",
                        "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (string.Equals(Path.GetDirectoryName(rutaDestino), _currentPath,
                        StringComparison.OrdinalIgnoreCase))
                        PopulateListView(_currentPath);
                }
                else
                {
                    MessageBox.Show(
                        $"La conversión {extOrigen} → {extDestino} requiere una aplicación externa "
                        + "(Microsoft Office, LibreOffice, etc.).\n\n"
                        + $"Ruta sugerida para el archivo destino:\n{rutaDestino}",
                        "Exportar — aplicación externa requerida",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool IntentarConversionNativa(string origen, string destino,
                                                      string extOrigen, string extDestino)
        {
            if (extOrigen == extDestino)
            { File.Copy(origen, destino, overwrite: true); return true; }

            if ((extOrigen == ".csv" && extDestino == ".txt") ||
                (extOrigen == ".txt" && extDestino == ".csv"))
            { File.Copy(origen, destino, overwrite: true); return true; }

            if (extOrigen == ".csv" && extDestino == ".json")
            { File.WriteAllText(destino, CsvAJson(File.ReadAllText(origen))); return true; }

            if (extOrigen == ".csv" && extDestino == ".xml")
            { File.WriteAllText(destino, CsvAXml(File.ReadAllText(origen))); return true; }

            if (extOrigen == ".csv" && extDestino == ".sql")
            { File.WriteAllText(destino, CsvASql(File.ReadAllText(origen), Path.GetFileNameWithoutExtension(origen))); return true; }

            if (extOrigen == ".txt" && extDestino == ".html")
            {
                string contenido = System.Security.SecurityElement.Escape(File.ReadAllText(origen)) ?? string.Empty;
                File.WriteAllText(destino,
                    $"<!DOCTYPE html><html><head><meta charset=\"UTF-8\"><title>{Path.GetFileNameWithoutExtension(origen)}</title></head>"
                    + $"<body><pre>{contenido}</pre></body></html>");
                return true;
            }

            if (extOrigen == ".xml" && extDestino == ".txt")
            {
                var doc = new System.Xml.XmlDocument();
                doc.Load(origen);
                File.WriteAllText(destino, doc.InnerText);
                return true;
            }

            return false;
        }

        // ── Helpers de conversión CSV ──────────────────────────────────────────

        private static string CsvAJson(string csv)
        {
            var lineas = csv.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lineas.Length == 0) return "[]";
            var enc = ParsearCsv(lineas[0]);
            var sb  = new StringBuilder("[\n");
            for (int i = 1; i < lineas.Length; i++)
            {
                var vals = ParsearCsv(lineas[i]);
                sb.Append("  {");
                for (int j = 0; j < enc.Count; j++)
                {
                    string v = j < vals.Count ? vals[j].Replace("\"", "\\\"") : "";
                    sb.Append($"\"{enc[j].Replace("\"", "\\\\\"")}\": \"{v}\"");
                    if (j < enc.Count - 1) sb.Append(", ");
                }
                sb.Append('}');
                if (i < lineas.Length - 1) sb.Append(',');
                sb.Append('\n');
            }
            sb.Append(']');
            return sb.ToString();
        }

        private static string CsvAXml(string csv)
        {
            var lineas = csv.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lineas.Length == 0) return "<datos/>";
            var enc = ParsearCsv(lineas[0]);
            var sb  = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<datos>\n");
            for (int i = 1; i < lineas.Length; i++)
            {
                var vals = ParsearCsv(lineas[i]);
                sb.Append("  <registro>\n");
                for (int j = 0; j < enc.Count; j++)
                {
                    string tag = TagXml(enc[j]);
                    string val = System.Security.SecurityElement.Escape(j < vals.Count ? vals[j] : "") ?? "";
                    sb.Append($"    <{tag}>{val}</{tag}>\n");
                }
                sb.Append("  </registro>\n");
            }
            sb.Append("</datos>");
            return sb.ToString();
        }

        private static string CsvASql(string csv, string tabla)
        {
            var lineas = csv.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lineas.Length == 0) return string.Empty;
            var enc = ParsearCsv(lineas[0]);
            var sb  = new StringBuilder();
            sb.Append($"CREATE TABLE IF NOT EXISTS `{tabla}` (\n  `id` INT AUTO_INCREMENT PRIMARY KEY");
            foreach (var c in enc) sb.Append($",\n  `{c.Replace("`", "")}` TEXT NULL");
            sb.Append("\n);\n\n");
            string cols = string.Join(", ", System.Linq.Enumerable.Select(enc, c => $"`{c.Replace("`", "")}`"));
            for (int i = 1; i < lineas.Length; i++)
            {
                var vals = ParsearCsv(lineas[i]);
                string v = string.Join(", ", System.Linq.Enumerable.Select(vals, x => $"'{x.Replace("'", "''")}'"));
                sb.Append($"INSERT INTO `{tabla}` ({cols}) VALUES ({v});\n");
            }
            return sb.ToString();
        }

        private static List<string> ParsearCsv(string linea)
        {
            var campos      = new List<string>();
            bool enComillas = false;
            var actual      = new StringBuilder();
            for (int i = 0; i < linea.Length; i++)
            {
                char c = linea[i];
                if (c == '"'  )
                {
                    if (enComillas && i + 1 < linea.Length && linea[i + 1] == '"'  ) { actual.Append('"'  ); i++; }
                    else enComillas = !enComillas;
                }
                else if ((c == ',' || c == ';') && !enComillas) { campos.Add(actual.ToString().Trim()); actual.Clear(); }
                else actual.Append(c);
            }
            campos.Add(actual.ToString().Trim());
            return campos;
        }

        private static string TagXml(string s)
        {
            var r = System.Text.RegularExpressions.Regex.Replace(s.Trim(), @"[^a-zA-Z0-9_\-]", "_");
            if (r.Length > 0 && char.IsDigit(r[0])) r = "_" + r;
            return string.IsNullOrEmpty(r) ? "campo" : r;
        }
    }
}
