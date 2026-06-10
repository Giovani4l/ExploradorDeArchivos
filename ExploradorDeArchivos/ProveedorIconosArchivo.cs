using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    internal sealed class ProveedorIconosArchivo
    {
        private const string FolderIconKey = "__folder__";

        private readonly ImageList _smallImages = new()
        {
            ColorDepth = ColorDepth.Depth32Bit,
            ImageSize = new Size(18, 18)
        };

        private readonly ImageList _largeImages = new()
        {
            ColorDepth = ColorDepth.Depth32Bit,
            ImageSize = new Size(32, 32)
        };

        private readonly Dictionary<string, int> _iconIndexMap =
            new(StringComparer.OrdinalIgnoreCase);

        public ImageList IconosPequenos => _smallImages;
        public ImageList IconosGrandes => _largeImages;

        public int ObtenerIndiceImagen(string path, bool isDirectory)
        {
            var key = isDirectory ? FolderIconKey : (Path.GetExtension(path) is { Length: > 0 } ext ? ext : "__file__");

            if (_iconIndexMap.TryGetValue(key, out var index)) return index;

            var smallIcon = GetShellIcon(path, isDirectory, largeIcon: false)
                ?? Icon.FromHandle((isDirectory ? DrawFolderIcon(18) : DrawFileIcon(key, 18)).GetHicon());
            var largeIcon = GetShellIcon(path, isDirectory, largeIcon: true)
                ?? Icon.FromHandle((isDirectory ? DrawFolderIcon(32) : DrawFileIcon(key, 32)).GetHicon());

            _smallImages.Images.Add(smallIcon);
            _largeImages.Images.Add(largeIcon);
            index = _smallImages.Images.Count - 1;
            _iconIndexMap[key] = index;
            return index;
        }

        private static Bitmap DrawFolderIcon(int size)
        {
            var bmp = new Bitmap(size, size);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);

            float s = size / 16f;
            using (var b = new SolidBrush(Color.FromArgb(255, 174, 30)))
                g.FillRectangle(b, 1 * s, 1 * s, 5 * s, 2 * s);
            using (var b = new SolidBrush(Color.FromArgb(255, 196, 74)))
                g.FillRectangle(b, 1 * s, 2.5f * s, 14 * s, 10 * s);
            using (var b = new SolidBrush(Color.FromArgb(200, 150, 40)))
                g.FillRectangle(b, 1 * s, 11 * s, 14 * s, 1.5f * s);

            return bmp;
        }

        private static Bitmap DrawFileIcon(string ext, int size)
        {
            var bmp = new Bitmap(size, size);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);

            var fileColor = GetExtensionColor(ext);
            float s = size / 16f;

            var body = new PointF[] { new(2 * s, 0), new(11 * s, 0), new(14 * s, 3 * s), new(14 * s, 16 * s), new(2 * s, 16 * s) };
            var corner = new PointF[] { new(11 * s, 0), new(14 * s, 3 * s), new(11 * s, 3 * s) };

            using (var b = new SolidBrush(fileColor)) g.FillPolygon(b, body);
            using (var b = new SolidBrush(ControlPaint.Dark(fileColor, 0.3f))) g.FillPolygon(b, corner);

            var label = ext.TrimStart('.').ToUpperInvariant();
            if (label.Length is > 0 and <= 5)
            {
                if (label.Length > 4) label = label[..4];
                float fontSize = Math.Max(4f, size * 0.22f);
                using var font = new Font("Segoe UI", fontSize, FontStyle.Bold);
                using var textBrush = new SolidBrush(Color.White);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(label, font, textBrush, new RectangleF(2 * s, 7 * s, 12 * s, 7 * s), sf);
            }
            return bmp;
        }

        private static Color GetExtensionColor(string ext) =>
            ext.ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" or ".png" or ".bmp" or ".gif" or ".webp" or ".tiff" => Color.FromArgb(16, 185, 129),
                ".pdf" => Color.FromArgb(220, 53, 69),
                ".doc" or ".docx" => Color.FromArgb(42, 99, 183),
                ".xls" or ".xlsx" => Color.FromArgb(33, 115, 70),
                ".ppt" or ".pptx" => Color.FromArgb(210, 83, 46),
                ".zip" or ".rar" or ".7z" or ".tar" or ".gz" => Color.FromArgb(245, 124, 0),
                ".mp3" or ".wav" or ".flac" or ".aac" or ".ogg" => Color.FromArgb(139, 92, 246),
                ".mp4" or ".avi" or ".mkv" or ".mov" or ".wmv" => Color.FromArgb(6, 182, 212),
                ".exe" or ".msi" => Color.FromArgb(107, 114, 128),
                ".cs" or ".py" or ".js" or ".ts" or ".java" => Color.FromArgb(251, 191, 36),
                ".html" or ".htm" or ".css" => Color.FromArgb(249, 115, 22),
                ".txt" or ".md" or ".log" => Color.FromArgb(156, 163, 175),
                ".sln" or ".csproj" => Color.FromArgb(148, 103, 189),
                _ => Color.FromArgb(100, 149, 237)
            };

        private static Icon? GetShellIcon(string path, bool isDirectory, bool largeIcon)
        {
            var attributes = isDirectory ? FileAttributes.Directory : FileAttributes.Normal;
            var flags = SHGFI_ICON | SHGFI_USEFILEATTRIBUTES | (largeIcon ? SHGFI_LARGEICON : SHGFI_SMALLICON);
            if (SHGetFileInfo(path, attributes, out var shfi, (uint)Marshal.SizeOf<ShFileInfo>(), flags) == IntPtr.Zero
                || shfi.hIcon == IntPtr.Zero)
                return null;
            try { return (Icon?)Icon.FromHandle(shfi.hIcon).Clone(); }
            finally { DestroyIcon(shfi.hIcon); }
        }

        private const uint SHGFI_ICON = 0x000000100;
        private const uint SHGFI_SMALLICON = 0x000000001;
        private const uint SHGFI_LARGEICON = 0x000000000;
        private const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct ShFileInfo
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)] public string szTypeName;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr SHGetFileInfo(string pszPath, FileAttributes dwFileAttributes,
            out ShFileInfo psfi, uint cbFileInfo, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);
    }
}


