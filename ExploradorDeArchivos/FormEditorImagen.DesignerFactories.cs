namespace ExploradorDeArchivos
{
    public partial class FormEditorImagen
    {
// Helpers de construcción
        private static System.Windows.Forms.Button Boton(string texto, string tag)
        {
            return new System.Windows.Forms.Button { Text = texto, Tag = tag };
        }

        private static void PosBtn(System.Windows.Forms.Button b, int x, int y)
        {
            b.Location = new System.Drawing.Point(x, y);
            b.Size     = new System.Drawing.Size(93, 28);
        }
    }
}

