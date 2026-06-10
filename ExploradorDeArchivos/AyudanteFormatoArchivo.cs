using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExploradorDeArchivos
{
    internal static class AyudanteFormatoArchivo
    {
        public static readonly HashSet<string> ExtensionesImagen =
            new(StringComparer.OrdinalIgnoreCase)
            { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".tif", ".webp", ".ico" };

        public static readonly HashSet<string> ExtensionesAudio =
            new(StringComparer.OrdinalIgnoreCase)
            { ".mp3", ".m4a", ".wav", ".flac", ".ogg", ".aac", ".wma" };

        public static readonly HashSet<string> ExtensionesVideo =
            new(StringComparer.OrdinalIgnoreCase)
            { ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm", ".ts", ".m4v" };

        public static readonly HashSet<string> ExtensionesTabla =
            new(StringComparer.OrdinalIgnoreCase) { ".csv", ".tsv", ".xlsx", ".xls", ".xlsm" };

        public static readonly HashSet<string> ExtensionesTexto =
            new(StringComparer.OrdinalIgnoreCase)
            { ".txt", ".md", ".log", ".xml", ".json", ".yaml", ".yml",
              ".ini", ".cfg", ".config", ".cs", ".py", ".js", ".ts",
              ".java", ".cpp", ".c", ".h", ".html", ".htm", ".css",
              ".sql", ".sh", ".bat", ".ps1", ".toml", ".env", ".rtf" };

        public static readonly HashSet<string> ExtensionesPdf =
            new(StringComparer.OrdinalIgnoreCase) { ".pdf" };

        public static readonly HashSet<string> ExtensionesWord =
            new(StringComparer.OrdinalIgnoreCase) { ".docx", ".doc" };

        public static readonly HashSet<string> ExtensionesPresentacion =
            new(StringComparer.OrdinalIgnoreCase) { ".pptx", ".ppt" };

        public static bool TieneVisorIntegrado(string ext) =>
            ExtensionesTabla.Contains(ext) || ExtensionesTexto.Contains(ext) ||
            ExtensionesImagen.Contains(ext) || ExtensionesPdf.Contains(ext) ||
            ExtensionesWord.Contains(ext) || ExtensionesPresentacion.Contains(ext);

        public static string ObtenerTamanoLegible(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };
            double size = bytes;
            var index = 0;
            while (size >= 1024 && index < suffixes.Length - 1)
            {
                size /= 1024;
                index++;
            }
            return $"{size:0.##} {suffixes[index]}";
        }

        public static Encoding DetectarCodificacion(string path)
        {
            var raw = new byte[4];
            using var fs = File.OpenRead(path);
            int read = fs.Read(raw, 0, 4);
            if (read >= 3 && raw[0] == 0xEF && raw[1] == 0xBB && raw[2] == 0xBF) return new UTF8Encoding(true);
            if (read >= 2 && raw[0] == 0xFF && raw[1] == 0xFE) return Encoding.Unicode;
            if (read >= 2 && raw[0] == 0xFE && raw[1] == 0xFF) return Encoding.BigEndianUnicode;
            return new UTF8Encoding(false);
        }

        public static string EscaparCsv(string value, string separator = ",")
        {
            if (value.Contains(separator) || value.Contains('"') || value.Contains('\n'))
                return $"\"{value.Replace("\"", "\"\"")}\"";
            return value;
        }
    }
}


