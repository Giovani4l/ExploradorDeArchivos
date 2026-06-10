using System.Drawing;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    public partial class FormMigrarDatos
    {
private void ConfigurarInputEstilo(TextBox tb, string placeholder, int x, int y, int w, Color bg, Color fg)
        {
            tb.BackColor = bg;
            tb.ForeColor = fg;
            tb.BorderStyle = BorderStyle.FixedSingle;
            tb.Font = new Font("Segoe UI", 10F);
            tb.Location = new Point(x, y);
            tb.Size = new Size(w, 30);
            tb.Text = placeholder;
        }
    }
}

