using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;

using WdBody         = DocumentFormat.OpenXml.Wordprocessing.Body;
using WdParagraph    = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using WdBold         = DocumentFormat.OpenXml.Wordprocessing.Bold;
using WdText         = DocumentFormat.OpenXml.Wordprocessing.Text;
using PptShape       = DocumentFormat.OpenXml.Presentation.Shape;
using PptValues      = DocumentFormat.OpenXml.Presentation.PlaceholderValues;
using A              = DocumentFormat.OpenXml.Drawing;

namespace ExploradorDeArchivos
{
    public partial class FormVisorArchivo
    {
// ══════════════════════════════════════════════════════════════════
        //  WORD (.docx)  — párrafos en RichTextBox, guardado via OpenXml
        // ══════════════════════════════════════════════════════════════════
        private void CargarWord()
        {
            var parrafos = ExtraerParrafosWord(_filePath);

            _lblInfo = CrearLabelInfo($"📘  Word  ·  {parrafos.Count} párrafos  —  {Path.GetFileName(_filePath)}");
            panelContenido.Controls.Add(_lblInfo);

            // Panel scrolleable con un RichTextBox por párrafo
            var scroll = new Panel
            {
                Dock        = DockStyle.Fill,
                AutoScroll  = true,
                BackColor   = Color.FromArgb(245, 246, 250),
            };

            _wordParrafos = new List<RichTextBox>();
            int y = 12;
            foreach (var (texto, esNegrita, esTitulo) in parrafos)
            {
                var rtb = new RichTextBox
                {
                    Left        = 40,
                    Top         = y,
                    Width       = Math.Max(scroll.ClientSize.Width - 80, 600),
                    Height      = esTitulo ? 36 : 24,
                    BorderStyle = BorderStyle.None,
                    ScrollBars  = RichTextBoxScrollBars.None,
                    WordWrap    = true,
                    Text        = texto,
                    Multiline   = true,
                };
                UIStyler.AplicarEstiloWordParagrafo(rtb, esTitulo, esNegrita);

                // Auto-altura
                rtb.ContentsResized += (s, e) =>
                {
                    rtb.Height = e.NewRectangle.Height + 4;
                };
                rtb.TextChanged += (s, e) =>
                {
                    MarcarModificado();
                    // Recalcular posición de los siguientes
                    RecolocarPaerrafos(scroll);
                };

                scroll.Controls.Add(rtb);
                _wordParrafos.Add(rtb);
                y += rtb.Height + 6;
            }

            // Ajustar altura inicial
            foreach (var rtb in _wordParrafos)
                rtb.Width = Math.Max(scroll.ClientSize.Width - 80, 600);

            panelContenido.Controls.Add(scroll);
            scroll.BringToFront();
            MostrarBotones(guardar: true, wrap: false, ajustarCols: false, fuente: true);
        }

        private void RecolocarPaerrafos(Panel scroll)
        {
            if (_wordParrafos == null) return;
            int y = 12;
            foreach (var rtb in _wordParrafos)
            {
                rtb.Top = y;
                y += rtb.Height + 6;
            }
        }

        private static List<(string texto, bool negrita, bool titulo)> ExtraerParrafosWord(string path)
        {
            var result = new List<(string, bool, bool)>();
            try
            {
                using var doc = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open(path, false);
                WdBody? body = doc.MainDocumentPart?.Document?.Body;
                if (body == null) return result;

                foreach (var para in body.Elements<WdParagraph>())
                {
                    var texto = string.Concat(para.Descendants<WdText>().Select(t => t.Text));
                    if (string.IsNullOrEmpty(texto)) { result.Add(("", false, false)); continue; }

                    var props  = para.ParagraphProperties;
                    var style  = props?.ParagraphStyleId?.Val?.Value ?? "";
                    bool titulo  = style.StartsWith("Heading", StringComparison.OrdinalIgnoreCase)
                                || style.StartsWith("T", StringComparison.OrdinalIgnoreCase) && style.Length <= 2;
                    bool negrita = para.Descendants<WdBold>().Any();
                    result.Add((texto, negrita, titulo));
                }
            }
            catch { result.Add(("No se pudo leer el documento Word.", false, false)); }
            return result;
        }

        // ══════════════════════════════════════════════════════════════════
        //  POWERPOINT (.pptx)  — diapositivas en paneles, texto editable
        // ══════════════════════════════════════════════════════════════════
        private void CargarPpt()
        {
            var diapositivas = ExtraerDiapositivas(_filePath);

            _lblInfo = CrearLabelInfo($"📙  PowerPoint  ·  {diapositivas.Count} diapositivas  —  {Path.GetFileName(_filePath)}");
            panelContenido.Controls.Add(_lblInfo);

            // TabControl: una pestaña por diapositiva
            var tabs = new TabControl
            {
                Dock     = DockStyle.Fill,
                Font     = new Font("Segoe UI", 9F),
            };

            _pptPanel = new Panel { Dock = DockStyle.Fill };
            _pptPanel.Controls.Add(tabs);

            int num = 1;
            foreach (var (titulo, cuerpo) in diapositivas)
            {
                var tab = new TabPage($"  {num++}  ");
                UIStyler.AplicarEstiloTabPpt(tab);

                // Título
                var rtbTitulo = new RichTextBox
                {
                    Dock       = DockStyle.Top,
                    Height     = 44,
                    Text       = titulo,
                    ScrollBars = RichTextBoxScrollBars.None,
                };
                UIStyler.AplicarEstiloRtbTituloPpt(rtbTitulo);
                rtbTitulo.TextChanged += (s, e) => MarcarModificado();

                // Separador
                var sep = new Label { Dock = DockStyle.Top, Height = 2 };
                UIStyler.AplicarEstiloSeparadorPpt(sep);

                // Cuerpo
                var rtbCuerpo = new RichTextBox
                {
                    Dock       = DockStyle.Fill,
                    Text       = cuerpo,
                    WordWrap   = true,
                    ScrollBars = RichTextBoxScrollBars.Vertical,
                };
                UIStyler.AplicarEstiloRtbCuerpoPpt(rtbCuerpo);

                // Orden inverso por Dock
                tab.Controls.Add(rtbCuerpo);
                tab.Controls.Add(sep);
                tab.Controls.Add(rtbTitulo);
                tabs.TabPages.Add(tab);
            }

            panelContenido.Controls.Add(_pptPanel);
            _pptPanel.BringToFront();
            MostrarBotones(guardar: true);
        }

        private static List<(string titulo, string cuerpo)> ExtraerDiapositivas(string path)
        {
            var result = new List<(string, string)>();
            try
            {
                using var pres = DocumentFormat.OpenXml.Packaging.PresentationDocument.Open(path, false);
                var slides = pres.PresentationPart?.SlideParts;
                if (slides == null) return result;

                foreach (var slidePart in slides)
                {
                    var titulo = "";
                    var partes = new List<string>();

                    foreach (var shape in slidePart.Slide.Descendants<PptShape>())
                    {
                        var ph    = shape.NonVisualShapeProperties?.ApplicationNonVisualDrawingProperties
                                         ?.PlaceholderShape;
                        var texto = string.Concat(shape.Descendants<A.Text>().Select(t => t.Text));
                        if (string.IsNullOrWhiteSpace(texto)) continue;

                        if (ph?.Type?.Value == PptValues.Title ||
                            ph?.Type?.Value == PptValues.CenteredTitle)
                            titulo = texto;
                        else
                            partes.Add(texto);
                    }
                    result.Add((titulo, string.Join("\n\n", partes)));
                }
            }
            catch { result.Add(("Error al leer presentación", "")); }
            return result;
        }
    }
}

