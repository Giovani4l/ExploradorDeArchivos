using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ExploradorDeArchivos
{
    internal static class ServicioIndiceDirectorios
    {
        private const int MaxIndexedDirectories = 10;
        private const int MaxFilesPerDirectoryInIndex = 6;

        public static string ResolverDestino(string query, string currentPath)
        {
            var basePath = string.IsNullOrEmpty(currentPath)
                ? Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                : currentPath;

            if (string.IsNullOrWhiteSpace(query)) return basePath;

            var normalized = query.Trim();
            if (Directory.Exists(normalized)) return normalized;

            var relative = Path.Combine(basePath, normalized);
            if (Directory.Exists(relative)) return relative;

            try
            {
                var match = Directory
                    .EnumerateDirectories(basePath, $"*{normalized}*", SearchOption.TopDirectoryOnly)
                    .OrderBy(d => d)
                    .FirstOrDefault();
                if (!string.IsNullOrEmpty(match)) return match;
            }
            catch { }

            return basePath;
        }

        public static string CrearResumen(
            string path,
            Func<string, (int folders, int files)?> getDirectoryContentCounts)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Carpeta: {Path.GetFileName(path) ?? path}");

            var rootFiles = EnumerateFilesSafe(path, MaxFilesPerDirectoryInIndex + 1);
            builder.AppendLine(rootFiles.Length == 0
                ? "  (sin archivos directos)"
                : string.Join(", ", rootFiles.Take(MaxFilesPerDirectoryInIndex).Select(Path.GetFileName))
                  + (rootFiles.Length > MaxFilesPerDirectoryInIndex ? ", ..." : ""));

            foreach (var directory in EnumerateDirectoriesSafe(path))
                AppendDirectoryIndex(builder, directory, getDirectoryContentCounts);

            return builder.ToString().TrimEnd();
        }

        private static void AppendDirectoryIndex(
            StringBuilder builder,
            string directory,
            Func<string, (int folders, int files)?> getDirectoryContentCounts)
        {
            var name = Path.GetFileName(directory) ?? directory;
            var counts = getDirectoryContentCounts(directory);
            builder.Append($"  -> {name}");
            builder.AppendLine(counts.HasValue
                ? $" ({counts.Value.folders} carpetas, {counts.Value.files} archivos)"
                : " (inaccesible)");
        }

        private static string[] EnumerateDirectoriesSafe(string path)
        {
            try { return Directory.GetDirectories(path).OrderBy(d => d).Take(MaxIndexedDirectories).ToArray(); }
            catch { return Array.Empty<string>(); }
        }

        private static string[] EnumerateFilesSafe(string path, int limit)
        {
            try { return Directory.GetFiles(path).OrderBy(f => f).Take(limit).ToArray(); }
            catch { return Array.Empty<string>(); }
        }
    }
}


