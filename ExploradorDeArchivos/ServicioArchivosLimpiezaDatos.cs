using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ExploradorDeArchivos
{
    internal static class ServicioArchivosLimpiezaDatos
    {
        public static List<Dictionary<string, string>> Parse(string path)
        {
            var ext = Path.GetExtension(path).TrimStart('.').ToLowerInvariant();

            return ext switch
            {
                "json" => ParsearJson(path),
                "csv" => ParsearDelimitado(path, ','),
                "tsv" => ParsearDelimitado(path, '\t'),
                "xlsx" or "xls" => ParsearExcel(path),
                _ => throw new Exception("Formato no soportado: " + ext)
            };
        }

        public static void ExportarCsv(string path, IReadOnlyList<string> columns, IEnumerable<Dictionary<string, string>> rows)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Join(",", columns.Select(c => AyudanteFormatoArchivo.EscaparCsv(c))));
            foreach (var row in rows)
                sb.AppendLine(string.Join(",",
                    columns.Select(c => AyudanteFormatoArchivo.EscaparCsv(row.TryGetValue(c, out var value) ? value : ""))));
            File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
        }

        public static void ExportarJson(string path, IReadOnlyList<string> columns, IEnumerable<Dictionary<string, string>> rows)
        {
            var list = rows
                .Select(row => columns.ToDictionary(c => c, c => (object)(row.TryGetValue(c, out var value) ? value : "")))
                .ToList();

            File.WriteAllText(path,
                JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }),
                new UTF8Encoding(true));
        }

        private static List<Dictionary<string, string>> ParsearJson(string path)
        {
            var text = File.ReadAllText(path, Encoding.UTF8);
            using var doc = JsonDocument.Parse(text);
            var root = doc.RootElement;

            JsonElement array;
            if (root.ValueKind == JsonValueKind.Array)
            {
                array = root;
            }
            else if (root.ValueKind == JsonValueKind.Object)
            {
                string[] candidates = { "data", "records", "items", "results" };
                JsonElement found = default;
                foreach (var key in candidates)
                    if (root.TryGetProperty(key, out var value) && value.ValueKind == JsonValueKind.Array)
                    {
                        found = value;
                        break;
                    }
                array = found.ValueKind == JsonValueKind.Undefined ? root : found;
            }
            else
            {
                throw new Exception("JSON no reconocido (se esperaba array u objeto).");
            }

            var rows = new List<Dictionary<string, string>>();
            if (array.ValueKind == JsonValueKind.Array)
                foreach (var item in array.EnumerateArray())
                    rows.Add(AplanarJson(item));
            else
                rows.Add(AplanarJson(array));

            return rows;
        }

        private static Dictionary<string, string> AplanarJson(JsonElement element, string prefix = "")
        {
            var row = new Dictionary<string, string>();
            if (element.ValueKind == JsonValueKind.Object)
                foreach (var property in element.EnumerateObject())
                {
                    var key = prefix == "" ? property.Name : $"{prefix}.{property.Name}";
                    if (property.Value.ValueKind == JsonValueKind.Object)
                        foreach (var item in AplanarJson(property.Value, key))
                            row[item.Key] = item.Value;
                    else
                        row[key] = property.Value.ValueKind == JsonValueKind.Null ? "" : property.Value.ToString();
                }
            else
                row[prefix == "" ? "value" : prefix] = element.ToString();
            return row;
        }

        private static List<Dictionary<string, string>> ParsearDelimitado(string path, char separator)
        {
            var rows = new List<Dictionary<string, string>>();
            var lines = File.ReadAllLines(path, Encoding.UTF8);
            if (lines.Length == 0) return rows;

            var headers = SepararLinea(lines[0], separator);
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                var values = SepararLinea(lines[i], separator);
                var row = new Dictionary<string, string>();
                for (int j = 0; j < headers.Length; j++)
                    row[headers[j]] = j < values.Length ? values[j] : "";
                rows.Add(row);
            }
            return rows;
        }

        private static string[] SepararLinea(string line, char separator)
        {
            var result = new List<string>();
            bool inQuotes = false;
            var current = new StringBuilder();

            foreach (char c in line)
            {
                if (c == '"') inQuotes = !inQuotes;
                else if (c == separator && !inQuotes)
                {
                    result.Add(current.ToString().Trim('"'));
                    current.Clear();
                }
                else current.Append(c);
            }
            result.Add(current.ToString().Trim('"'));
            return result.ToArray();
        }

        private static List<Dictionary<string, string>> ParsearExcel(string path)
        {
            try
            {
                return ParsearDelimitado(path, ';');
            }
            catch
            {
                throw new Exception(
                    "Para leer XLSX/XLS instala el paquete 'ExcelDataReader' en el proyecto.\n" +
                    "Por ahora guarda el archivo como CSV y vuelve a intentarlo.");
            }
        }
    }
}


