namespace ExploradorDeArchivos
{
    public partial class FormPlayerMP3
    {
// ── Factory helpers de botones ────────────────────────────────────────

        private static System.Windows.Forms.Button MkBtn(string text) => new System.Windows.Forms.Button
        {
            Text      = text,
            Height    = 32,
            Width     = 102,
            AutoSize  = false,
            Font      = new System.Drawing.Font("Segoe UI", 9f),
            ForeColor = System.Drawing.Color.FromArgb(175, 175, 210),
            BackColor = System.Drawing.Color.FromArgb(26, 26, 40),
            FlatStyle = System.Windows.Forms.FlatStyle.Flat,
            Cursor    = System.Windows.Forms.Cursors.Hand,
            Margin    = new System.Windows.Forms.Padding(3, 0, 3, 0),
            FlatAppearance =
            {
                BorderColor         = System.Drawing.Color.FromArgb(42, 42, 64),
                BorderSize          = 1,
                MouseOverBackColor  = System.Drawing.Color.FromArgb(34, 34, 52),
                MouseDownBackColor  = System.Drawing.Color.FromArgb(44, 44, 66)
            }
        };

        private static System.Windows.Forms.Button MkIcon(string icon, int left, int top) =>
            new System.Windows.Forms.Button
            {
                Text      = icon,
                Left      = left, Top = top,
                Width     = 42, Height = 42,
                Font      = new System.Drawing.Font("Segoe UI", 13f),
                ForeColor = System.Drawing.Color.FromArgb(195, 195, 225),
                BackColor = System.Drawing.Color.FromArgb(28, 28, 42),
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Cursor    = System.Windows.Forms.Cursors.Hand,
                FlatAppearance =
                {
                    BorderColor        = System.Drawing.Color.FromArgb(44, 44, 66),
                    BorderSize         = 1,
                    MouseOverBackColor = System.Drawing.Color.FromArgb(36, 36, 54),
                    MouseDownBackColor = System.Drawing.Color.FromArgb(48, 48, 70)
                }
            };
    }
}

