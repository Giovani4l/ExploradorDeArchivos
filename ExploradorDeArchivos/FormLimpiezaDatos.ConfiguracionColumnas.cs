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
        //  PARSEO DE ARCHIVOS
        // ══════════════════════════════════════════════════════════════════════

        private async Task<Dictionary<string, ColMeta>> AnalizarColumnasIA()
        {
            var muestra = _filasOriginales.Take(5)
                .Select(f => _columnas.ToDictionary(c => c, c => f.TryGetValue(c, out var v) ? v : ""))
                .ToList();

            var payload = new
            {
                model      = "claude-sonnet-4-20250514",
                max_tokens = 1000,
                system     =
                    "Eres un experto en calidad de datos. Analiza las columnas de un dataset.\n" +
                    "Responde ÚNICAMENTE con JSON válido, sin texto ni backticks. Estructura:\n" +
                    "{ \"nombre_columna\": { \"type\": \"phone|name|date|email|id|address|number|text\", " +
                    "\"description\": \"descripción breve en español\", " +
                    "\"issues\": \"problemas detectados en la muestra\" } }",
                messages = new[]
                {
                    new { role = "user", content =
                        $"Columnas y muestra:\n{JsonSerializer.Serialize(new { columns = _columnas, sample = muestra })}" }
                }
            };

            var resultado = new Dictionary<string, ColMeta>();
            try
            {
                var json = JsonSerializer.Serialize(payload);
                var req  = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages");
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
                req.Headers.Add("anthropic-version", "2023-06-01");

                var apiKey = Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY") ?? "";
                if (!string.IsNullOrEmpty(apiKey))
                    req.Headers.Add("x-api-key", apiKey);

                var resp = await _http.SendAsync(req);
                var body = await resp.Content.ReadAsStringAsync();
                using var doc  = JsonDocument.Parse(body);
                var texto = doc.RootElement
                    .GetProperty("content")[0]
                    .GetProperty("text").GetString() ?? "";
                texto = texto.Replace("```json", "").Replace("```", "").Trim();

                using var doc2 = JsonDocument.Parse(texto);
                foreach (var prop in doc2.RootElement.EnumerateObject())
                {
                    var tipo = prop.Value.TryGetProperty("type",        out var t) ? t.GetString() ?? "text" : "text";
                    var desc = prop.Value.TryGetProperty("description", out var d) ? d.GetString() ?? ""     : "";
                    var iss  = prop.Value.TryGetProperty("issues",      out var i) ? i.GetString() ?? ""     : "";
                    resultado[prop.Name] = new ColMeta(tipo, desc, iss);
                }
            }
            catch
            {
                // Fallback: tipo "text" para todas las columnas
                foreach (var col in _columnas)
                    resultado[col] = new ColMeta("text", col, "");
            }

            // Garantizar que todas las columnas tienen metadata
            foreach (var col in _columnas)
                if (!resultado.ContainsKey(col))
                    resultado[col] = new ColMeta("text", col, "");

            return resultado;
        }

        //  RELLENAR CONFIGURACIÓN DE COLUMNAS (UI dinámica por datos)

        private void RellenarConfigColumnas()
        {
            _lblInfoArchivo.Text =
                $"📄  {_nombreArchivo}    |    {_filasOriginales.Count:N0} filas  ·  {_columnas.Count} columnas";

            _flowCols.Controls.Clear();

            string[] tipos = { "phone", "name", "date", "email", "id", "address", "number", "text" };

            foreach (var col in _columnas)
                _flowCols.Controls.Add(CrearTarjetaColumna(col, tipos));

            MostrarAvisoApiKey();
        }

        private Panel CrearTarjetaColumna(string col, string[] tipos)
        {
            var meta  = _metaCols.TryGetValue(col, out var m) ? m : new ColMeta("text", col, "");
            var color = ColorPorTipo.TryGetValue(meta.Tipo, out var c) ? c : AppColors.TextoSec;

            var card = new Panel
            {
                Size      = new Size(260, 110),
                Margin    = new Padding(0, 0, 10, 10),
                BackColor = AppColors.BgPanel,
                Tag       = col
            };
            card.Paint += (s, e) => UIStyler.PintarBordeTarjeta(card, color, e);

            var lblCol = new Label
            {
                Text     = col.Length > 20 ? col[..20] + "…" : col,
                Location = new Point(8, 8),
                AutoSize = false,
                Size     = new Size(140, 18),
            };
            UIStyler.AplicarLabelColumna(lblCol);

            var cbo = new ComboBox
            {
                Location      = new Point(8, 28),
                Size          = new Size(110, 24),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Tag           = col
            };
            UIStyler.AplicarComboBoxOscuro(cbo);
            cbo.Items.AddRange(tipos);
            cbo.SelectedItem = tipos.Contains(meta.Tipo) ? meta.Tipo : "text";
            cbo.SelectedIndexChanged += (s, e) =>
            {
                if (cbo.Tag is string colName && cbo.SelectedItem is string nuevoTipo)
                {
                    var old = _metaCols.TryGetValue(colName, out var om) ? om : new ColMeta("text", "", "");
                    _metaCols[colName] = new ColMeta(nuevoTipo, old.Descripcion, old.Problemas);
                }
            };

            var lblDesc = new Label
            {
                Text     = meta.Descripcion,
                Location = new Point(8, 56),
                AutoSize = false,
                Size     = new Size(244, 16),
            };
            UIStyler.AplicarLabelDescTipo(lblDesc, color);

            card.Controls.AddRange(new Control[] { lblCol, cbo, lblDesc });

            if (!string.IsNullOrEmpty(meta.Problemas))
            {
                var lblIss = new Label
                {
                    Text     = "⚠ " + (meta.Problemas.Length > 38 ? meta.Problemas[..38] + "…" : meta.Problemas),
                    Location = new Point(8, 75),
                    AutoSize = false,
                    Size     = new Size(244, 28),
                };
                UIStyler.AplicarLabelAdvertencia(lblIss);
                card.Controls.Add(lblIss);
            }

            return card;
        }

        private void MostrarAvisoApiKey()
        {
            var apiKey = Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY") ?? "";
            if (!string.IsNullOrEmpty(apiKey)) return;

            var lblApi = new Label
            {
                Text      = "ℹ  Para usar la limpieza con IA, define la variable de entorno ANTHROPIC_API_KEY antes de abrir el explorador.",
                Dock      = DockStyle.Top,
                Height    = 36,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            UIStyler.AplicarLabelApiKey(lblApi);
            _panelConfigScroll.Controls.Add(lblApi);
        }
        
    }
}

