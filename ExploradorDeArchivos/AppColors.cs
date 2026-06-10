using System.Drawing;

namespace ExploradorDeArchivos
{
    /// <summary>
    /// Paleta de colores centralizada para todos los formularios de la aplicación.
    /// </summary>
    internal static class AppColors
    {
        // ── Paleta oscura (compartida por FormLimpiezaDatos, FormMigrarDatos, FormPlayerMP3) ──

        public static readonly Color BgOscuro   = Color.FromArgb(8,  11, 20);
        public static readonly Color BgPanel    = Color.FromArgb(13, 21, 37);
        public static readonly Color BgCard     = Color.FromArgb(15, 23, 42);
        public static readonly Color Borde      = Color.FromArgb(30, 45, 69);
        public static readonly Color Azul       = Color.FromArgb(29, 78, 216);
        public static readonly Color AzulClaro  = Color.FromArgb(59, 130, 246);
        public static readonly Color Verde      = Color.FromArgb(6,  95, 70);
        public static readonly Color VerdeClaro = Color.FromArgb(52, 211, 153);
        public static readonly Color TextoPrim  = Color.FromArgb(225, 232, 240);
        public static readonly Color TextoSec   = Color.FromArgb(100, 116, 139);
        public static readonly Color Naranja    = Color.FromArgb(249, 115, 22);
        public static readonly Color Rojo       = Color.FromArgb(239, 68, 68);

        // ── Paleta del reproductor MP3 ────────────────────────────────────────

        public static readonly Color PlayerBgDark   = Color.FromArgb(10,  10,  14);
        public static readonly Color PlayerBgSide   = Color.FromArgb(13,  13,  19);
        public static readonly Color PlayerBgCard   = Color.FromArgb(24,  24,  36);
        public static readonly Color PlayerTextPrim = Color.FromArgb(235, 235, 245);
        public static readonly Color PlayerTextSec  = Color.FromArgb(140, 140, 168);
        public static readonly Color PlayerTextDim  = Color.FromArgb(70,  70,  95);
        public static readonly Color PlayerAccent   = Color.FromArgb(30,  215, 96);
        public static readonly Color PlayerBtnBg    = Color.FromArgb(28,  28,  42);

        // ── Paleta de la barra lateral (Form1) ───────────────────────────────

        public static readonly Color SidebarBg          = Color.FromArgb(247, 247, 247);
        public static readonly Color SidebarSeparator   = Color.FromArgb(210, 210, 215);
        public static readonly Color SidebarSectionText = Color.FromArgb(130, 130, 130);
        public static readonly Color SidebarItemText    = Color.FromArgb(40,  40,  40);
        public static readonly Color SidebarHover       = Color.FromArgb(220, 235, 252);
        public static readonly Color SidebarPressed     = Color.FromArgb(190, 215, 245);
        public static readonly Color SidebarSelected    = Color.FromArgb(205, 228, 255);
        public static readonly Color SidebarDragOver    = Color.FromArgb(160, 210, 255);
        public static readonly Color SidebarToolHover   = Color.FromArgb(255, 220, 195);
        public static readonly Color SidebarToolPressed = Color.FromArgb(255, 190, 150);

        // ── Colores de tipo de columna (FormLimpiezaDatos) ───────────────────

        public static readonly Color ColPhone   = Color.FromArgb(251, 146, 60);
        public static readonly Color ColName    = Color.FromArgb(167, 139, 250);
        public static readonly Color ColDate    = Color.FromArgb(52,  211, 153);
        public static readonly Color ColEmail   = Color.FromArgb(56,  189, 248);
        public static readonly Color ColId      = Color.FromArgb(244, 114, 182);
        public static readonly Color ColAddress = Color.FromArgb(250, 204, 21);
        public static readonly Color ColNumber  = Color.FromArgb(74,  222, 128);
        public static readonly Color ColText    = Color.FromArgb(148, 163, 184);

        // ── Colores de íconos de archivo (Form1) ─────────────────────────────

        public static readonly Color ToolbarBg = Color.FromArgb(243, 243, 243);
        public static readonly Color StatusBg  = Color.FromArgb(240, 240, 240);
    }
}


