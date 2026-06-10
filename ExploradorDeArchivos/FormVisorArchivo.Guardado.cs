using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;

using WdBody      = DocumentFormat.OpenXml.Wordprocessing.Body;
using WdParagraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using WdRun       = DocumentFormat.OpenXml.Wordprocessing.Run;
using WdText      = DocumentFormat.OpenXml.Wordprocessing.Text;
using PptShape    = DocumentFormat.OpenXml.Presentation.Shape;
using PptValues   = DocumentFormat.OpenXml.Presentation.PlaceholderValues;
using DParagraph  = DocumentFormat.OpenXml.Drawing.Paragraph;
using DRun        = DocumentFormat.OpenXml.Drawing.Run;
using DText       = DocumentFormat.OpenXml.Drawing.Text;

namespace ExploradorDeArchivos
{
    public partial class FormVisorArchivo
    {
// ══════════════════════════════════════════════════════════════════
        //  GUARDAR
        // ══════════════════════════════════════════════════════════════════
        private void Guardar()
        {
            try
            {
                if (AyudanteFormatoArchivo.ExtensionesTabla.Contains(_ext))    GuardarTabla();
                else if (AyudanteFormatoArchivo.ExtensionesTexto.Contains(_ext)) GuardarTexto();
                else if (AyudanteFormatoArchivo.ExtensionesWord.Contains(_ext))  GuardarWord();
                else if (AyudanteFormatoArchivo.ExtensionesPresentacion.Contains(_ext))   GuardarPpt();

                _modificado = false;
                this.Text = $"📄  {Path.GetFileName(_filePath)}";
                if (_lblInfo != null) _lblInfo.BackColor = Color.FromArgb(240, 243, 250);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GuardarTabla()
        {
            if (_grid?.DataSource is not DataTable dt) return;

            if (_ext.Equals(".csv", StringComparison.OrdinalIgnoreCase) ||
                _ext.Equals(".tsv", StringComparison.OrdinalIgnoreCase))
            {
                var sep = _ext.Equals(".tsv", StringComparison.OrdinalIgnoreCase) ? "\t" : ",";
                var sb  = new StringBuilder();
                sb.AppendLine(string.Join(sep, dt.Columns.Cast<DataColumn>().Select(c => AyudanteFormatoArchivo.EscaparCsv(c.ColumnName, sep))));
                foreach (DataRow row in dt.Rows)
                    sb.AppendLine(string.Join(sep, row.ItemArray.Select(v => AyudanteFormatoArchivo.EscaparCsv(v?.ToString() ?? "", sep))));
                File.WriteAllText(_filePath, sb.ToString(), Encoding.UTF8);
            }
            else
            {
                // Para Excel guardamos como CSV si no podemos escribir xlsx sin un paquete extra
                var csvPath = Path.ChangeExtension(_filePath, ".csv");
                var sb = new StringBuilder();
                sb.AppendLine(string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => AyudanteFormatoArchivo.EscaparCsv(c.ColumnName, ","))));
                foreach (DataRow row in dt.Rows)
                    sb.AppendLine(string.Join(",", row.ItemArray.Select(v => AyudanteFormatoArchivo.EscaparCsv(v?.ToString() ?? "", ","))));
                File.WriteAllText(csvPath, sb.ToString(), Encoding.UTF8);
                MessageBox.Show($"Los cambios se guardaron como CSV en:\n{csvPath}\n\n" +
                    "(El formato .xlsx requiere el paquete ClosedXML; se usó CSV por compatibilidad.)",
                    "Guardado como CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void GuardarTexto()
        {
            if (_rtb == null) return;
            File.WriteAllText(_filePath, _rtb.Text, AyudanteFormatoArchivo.DetectarCodificacion(_filePath));
        }

        private void GuardarWord()
        {
            if (_wordParrafos == null) return;
            try
            {
                using var doc  = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open(_filePath, true);
                WdBody? body   = doc.MainDocumentPart?.Document?.Body;
                if (body == null) return;

                var paragraphs = body.Elements<WdParagraph>().ToList();
                for (int i = 0; i < Math.Min(paragraphs.Count, _wordParrafos.Count); i++)
                {
                    // Limpiar los runs existentes y poner el nuevo texto
                    foreach (var run in paragraphs[i].Elements<WdRun>().ToList())
                        run.Remove();
                    var newRun = new WdRun(new WdText(_wordParrafos[i].Text));
                    paragraphs[i].AppendChild(newRun);
                }
                doc.MainDocumentPart!.Document.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar el .docx:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GuardarPpt()
        {
            if (_pptPanel?.Controls.Count == 0) return;
            var tabs = _pptPanel!.Controls[0] as TabControl;
            if (tabs == null) return;

            try
            {
                using var pres  = DocumentFormat.OpenXml.Packaging.PresentationDocument.Open(_filePath, true);
                var slidesList  = pres.PresentationPart?.SlideParts?.ToList();
                if (slidesList == null) return;

                for (int i = 0; i < Math.Min(tabs.TabPages.Count, slidesList.Count); i++)
                {
                    var tab   = tabs.TabPages[i];
                    var slide = slidesList[i].Slide;
                    var shapes = slide.Descendants<PptShape>().ToList();

                    // Buscar título y cuerpo en la UI
                    RichTextBox? rtbTit  = tab.Controls.OfType<RichTextBox>()
                                              .FirstOrDefault(r => r.Dock == DockStyle.Top);
                    RichTextBox? rtbBody = tab.Controls.OfType<RichTextBox>()
                                              .FirstOrDefault(r => r.Dock == DockStyle.Fill);

                    foreach (var shape in shapes)
                    {
                        var ph = shape.NonVisualShapeProperties?.ApplicationNonVisualDrawingProperties
                                      ?.PlaceholderShape;
                        bool esTitulo = ph?.Type?.Value == PptValues.Title ||
                                        ph?.Type?.Value == PptValues.CenteredTitle;

                        var txBody = shape.TextBody;
                        if (txBody == null) continue;

                        string nuevoTexto = esTitulo ? (rtbTit?.Text ?? "") : (rtbBody?.Text ?? "");

                        // Limpiar párrafos y poner el nuevo texto
                        foreach (var para in txBody.Elements<DParagraph>().ToList())
                            para.Remove();

                        var newPara = new DParagraph();
                        var newRun  = new DRun(
                                          new DText(nuevoTexto));
                        newPara.Append(newRun);
                        txBody.Append(newPara);
                    }
                    slide.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar el .pptx:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

