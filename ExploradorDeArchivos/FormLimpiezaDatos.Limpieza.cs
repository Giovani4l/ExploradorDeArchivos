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
//LIMPIAR CON IA
   

        private async Task EjecutarLimpieza()
        {
            MostrarEtapa("limpiar");
            _filasLimpias.Clear();

            const int LoteTamano = 20;
            int total = _filasOriginales.Count;

            for (int i = 0; i < total; i += LoteTamano)
            {
                var lote  = _filasOriginales.Skip(i).Take(LoteTamano).ToList();
                var texto = $"Limpiando filas {i + 1}–{Math.Min(i + LoteTamano, total)} de {total}…";
                SetProgreso(texto, (int)((double)i / total * 100));
                _filasLimpias.AddRange(await LimpiarLoteIA(lote));
            }

            SetProgreso("¡Listo!", 100);
            _filasEditadas = _filasLimpias.Select(f => new Dictionary<string, string>(f)).ToList();
            _paginaActual  = 0;
            await Task.Delay(400);
            MostrarEtapa("revisar");
        }

        private async Task<List<Dictionary<string, string>>> LimpiarLoteIA(
            List<Dictionary<string, string>> lote)
        {
            var reglas = string.Join("\n", _metaCols.Select(kv =>
                $"- \"{kv.Key}\" ({kv.Value.Tipo}): " +
                $"{(string.IsNullOrEmpty(kv.Value.Problemas) ? "limpiar y estandarizar" : kv.Value.Problemas)}"));

            var systemPrompt = BuildSystemPrompt(reglas);

            var payload = new
            {
                model      = "claude-sonnet-4-20250514",
                max_tokens = 1000,
                system     = systemPrompt,
                messages   = new[]
                {
                    new { role = "user", content = $"Limpia estos registros:\n{JsonSerializer.Serialize(lote)}" }
                }
            };

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
                var texto = doc.RootElement.GetProperty("content")[0].GetProperty("text").GetString() ?? "[]";
                texto = texto.Replace("```json", "").Replace("```", "").Trim();

                using var doc2 = JsonDocument.Parse(texto);
                return doc2.RootElement.EnumerateArray()
                    .Select(el => el.EnumerateObject()
                        .ToDictionary(p => p.Name, p => p.Value.ToString()))
                    .ToList();
            }
            catch
            {
                return lote.Select(LimpiarFilaLocal).ToList();
            }
        }

        private static string BuildSystemPrompt(string reglas) =>
            "Eres un limpiador de datos. Recibirás un array JSON de registros y devolverás el mismo array corregido.\n\n" +
            "Reglas por columna:\n" + reglas + "\n\n" +
            "Reglas generales OBLIGATORIAS:\n" +
            "- phone: extraer exactamente 10 dígitos (ignorando +52, lada, guiones, paréntesis, espacios). " +
            "  Si hay 10 dígitos: formatear como +52 (XXX) XXX-XXXX. Si hay menos de 10 dígitos: dejar vacío.\n" +
            "- name: Title Case ESTRICTO. Sin espacios dobles.\n" +
            "- date: formato DD-MM-YYYY (día-mes-año)\n" +
            "- address: Title Case en ciudades, colonias y municipios. Eliminar espacios dobles.\n" +
            "- email: convertir a minúsculas, eliminar espacios, corregir formato usuario@dominio.ext.\n" +
            "- number: solo dígitos y punto decimal, sin letras ni símbolos\n" +
            "- text: trim, sin espacios dobles, primera letra mayúscula si el campo tiene contenido\n" +
            "- Vacíos/nulos quedan como cadena vacía \"\"\n" +
            "- NO cambies el significado, solo el formato\n\n" +
            "Responde ÚNICAMENTE con el array JSON limpio, sin texto extra ni backticks.";

        // ── Limpieza local (fallback cuando falla la IA) ──────────────────────

        private Dictionary<string, string> LimpiarFilaLocal(Dictionary<string, string> fila)
        {
            var result = new Dictionary<string, string>();
            foreach (var col in _columnas)
            {
                var val  = fila.TryGetValue(col, out var v) ? v?.Trim() ?? "" : "";
                var tipo = _metaCols.TryGetValue(col, out var m) ? m.Tipo : "text";
                result[col] = tipo switch
                {
                    "name"    => LimpiarNombreLocal(val),
                    "phone"   => LimpiarTelefonoLocal(val),
                    "email"   => LimpiarEmailLocal(val),
                    "date"    => LimpiarFechaLocal(val),
                    "address" => LimpiarNombreLocal(val),
                    _         => string.IsNullOrWhiteSpace(val) ? "" :
                                 System.Text.RegularExpressions.Regex.Replace(val.Trim(), @"\s+", " ")
                };
            }
            return result;
        }

        private static string LimpiarNombreLocal(string val)
        {
            if (string.IsNullOrWhiteSpace(val)) return "";
            val = System.Text.RegularExpressions.Regex.Replace(val.Trim(), @"\s+", " ");
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(val.ToLower());
        }

        private static string LimpiarTelefonoLocal(string val)
        {
            if (string.IsNullOrWhiteSpace(val)) return "";
            var solo = System.Text.RegularExpressions.Regex.Replace(val, @"\D", "");
            if (solo.StartsWith("521") && solo.Length == 13) solo = solo[2..];
            else if (solo.StartsWith("52") && solo.Length == 12) solo = solo[2..];
            if (solo.Length != 10) return "";
            return $"+52 ({solo[..3]}) {solo[3..6]}-{solo[6..]}";
        }

        private static string LimpiarEmailLocal(string val)
        {
            if (string.IsNullOrWhiteSpace(val)) return "";
            val = val.ToLower().Replace(" ", "");
            var at = val.IndexOf('@');
            if (at < 1) return "";
            var dominio = val[(at + 1)..];
            if (!dominio.Contains('.') || dominio.StartsWith('.') || dominio.EndsWith('.')) return "";
            return val;
        }

        private static string LimpiarFechaLocal(string val)
        {
            if (string.IsNullOrWhiteSpace(val)) return "";
            val = val.Trim();

            if (DateTime.TryParse(val, System.Globalization.CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.None, out var fecha))
                return fecha.ToString("dd-MM-yyyy");

            string[] formatos = { "yyyy-MM-dd", "yyyy/MM/dd", "dd-MM-yyyy", "dd/MM/yyyy",
                                   "MM-dd-yyyy", "MM/dd/yyyy", "dd-MM-yy", "dd/MM/yy" };
            foreach (var fmt in formatos)
                if (DateTime.TryParseExact(val, fmt, System.Globalization.CultureInfo.CurrentCulture,
                    System.Globalization.DateTimeStyles.None, out var f))
                    return f.ToString("dd-MM-yyyy");

            return "";
        }
        
    }
}

