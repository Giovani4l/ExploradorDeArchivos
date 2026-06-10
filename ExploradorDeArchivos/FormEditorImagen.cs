using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq; 
using System.Text;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    public partial class FormEditorImagen : Form
    {
        private string _rutaArchivo = string.Empty;
        private Bitmap? _imagenOriginal;
        private Bitmap? _imagenActual;

        private bool _pintando;
        private Point _ultimoPunto;
        private Color _colorPincel = Color.Red;
        private int _grosorPincel = 5;
        private Graphics? _gPintura;

        private double _lat, _lon;
        private string _filtroActivo = "Original";

        public FormEditorImagen(string rutaArchivo)
        {
            InitializeComponent();
            _rutaArchivo = rutaArchivo;
            CargarImagen(rutaArchivo);
        }

        // ── Carga ────────────────────────────────────────────────────────────
        private void CargarImagen(string ruta)
        {
            try
            {
                // Image.FromFile preserva los metadatos EXIF (incluido GPS).
                // new Bitmap(FileStream) los descarta — por eso las coordenadas
                // no aparecían aunque la foto sí tuviera GPS.
                using var imgConExif = Image.FromFile(ruta);
                _imagenOriginal = new Bitmap(imgConExif.Width, imgConExif.Height,
                    PixelFormat.Format32bppArgb);
                using (var g = Graphics.FromImage(_imagenOriginal))
                    g.DrawImage(imgConExif, 0, 0);

                // Copiar todos los PropertyItems (EXIF) al nuevo Bitmap
                foreach (var prop in imgConExif.PropertyItems)
                    _imagenOriginal.SetPropertyItem(prop);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar la imagen:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _imagenActual = new Bitmap(_imagenOriginal);
            _filtroActivo = "Original";
            this.Text = "Editor – " + Path.GetFileName(ruta);
            LeerMetadatos();
            LeerCoordenadas();
            RefrescarVista();
        }

        // ── Metadatos EXIF ───────────────────────────────────────────────────
        private static readonly Dictionary<int, string> NombresExif = new()
        {
            {0x010F,"Fabricante"},{0x0110,"Modelo"},{0x0112,"Orientación"},
            {0x011A,"Resolución X"},{0x011B,"Resolución Y"},
            {0x0128,"Unidad Resolución"},{0x0132,"Fecha/Hora"},
            {0x013B,"Artista"},{0x9003,"Fecha Original"},
            {0x9004,"Fecha Digitalización"},{0x9286,"Comentario"},
            {0x0001,"GPS Lat Ref"},{0x0002,"GPS Latitud"},
            {0x0003,"GPS Lon Ref"},{0x0004,"GPS Longitud"},
            {0x0006,"GPS Altitud"},{0x0007,"GPS Hora"},{0x001D,"GPS Fecha"},
        };

        private void LeerMetadatos()
        {
            lvMetadatos.Items.Clear();
            if (_imagenOriginal == null) return;
            var fi = new FileInfo(_rutaArchivo);
            Fila("Archivo", fi.Name);
            Fila("Tamaño", ObtenerTamanoLegible(fi.Length));
            Fila("Dimensiones", $"{_imagenOriginal.Width} × {_imagenOriginal.Height} px");
            Fila("Formato", Path.GetExtension(_rutaArchivo).ToUpperInvariant());
            foreach (var id in _imagenOriginal.PropertyIdList)
            {
                try
                {
                    var prop = _imagenOriginal.GetPropertyItem(id);
                    if (prop == null) continue;
                    var nom = NombresExif.TryGetValue(id, out var n) ? n : $"Tag 0x{id:X4}";
                    Fila(nom, ExifStr(prop));
                }
                catch { }
            }
        }

        private void Fila(string n, string v)
        {
            var item = new ListViewItem(n);
            item.SubItems.Add(v);
            lvMetadatos.Items.Add(item);
        }

        private static string ExifStr(PropertyItem p)
        {
            if (p.Value == null || p.Len == 0) return string.Empty;
            return p.Type switch
            {
                2 => Encoding.ASCII.GetString(p.Value).TrimEnd('\0'),
                5 => string.Join(", ", Enumerable.Range(0, p.Len / 8).Select(i =>
                     {
                         uint n = BitConverter.ToUInt32(p.Value, i * 8);
                         uint d = BitConverter.ToUInt32(p.Value, i * 8 + 4);
                         return d == 0 ? "0" : $"{(double)n / d:0.###}";
                     })),
                _ => Encoding.UTF8.GetString(p.Value).TrimEnd('\0')
            };
        }

        // ── GPS ──────────────────────────────────────────────────────────────
        private void LeerCoordenadas()
        {
            if (_imagenOriginal == null) { SinGPS(); return; }
            try
            {
                if (!_imagenOriginal.PropertyIdList.Contains(0x0002) ||
                    !_imagenOriginal.PropertyIdList.Contains(0x0004))
                { SinGPS(); return; }

                var lR = _imagenOriginal.PropertyIdList.Contains(0x0001)
                    ? ExifStr(_imagenOriginal.GetPropertyItem(0x0001)!) : "N";
                var nR = _imagenOriginal.PropertyIdList.Contains(0x0003)
                    ? ExifStr(_imagenOriginal.GetPropertyItem(0x0003)!) : "E";
                _lat = GradosDec(_imagenOriginal.GetPropertyItem(0x0002)!.Value);
                _lon = GradosDec(_imagenOriginal.GetPropertyItem(0x0004)!.Value);
                if (lR == "S") _lat = -_lat;
                if (nR == "W") _lon = -_lon;
                txtLat.Text = _lat.ToString("F6", CultureInfo.InvariantCulture);
                txtLon.Text = _lon.ToString("F6", CultureInfo.InvariantCulture);
                lblGPS.Text = "✔ GPS encontrado";
                lblGPS.ForeColor = Color.Green;
                _ = MostrarMapaAsync(_lat, _lon);
            }
            catch { SinGPS(); }
        }

        private void SinGPS()
        {
            lblGPS.Text = "✘ Sin GPS – edite las coordenadas y pulse \"Mostrar mapa\"";
            lblGPS.ForeColor = Color.OrangeRed;
        }

        private static double GradosDec(byte[] v)
        {
            double g = Rac(v, 0), m = Rac(v, 8), s = Rac(v, 16);
            return g + m / 60.0 + s / 3600.0;
        }

        private static double Rac(byte[] b, int i)
        {
            uint n = BitConverter.ToUInt32(b, i);
            uint d = BitConverter.ToUInt32(b, i + 4);
            return d == 0 ? 0 : (double)n / d;
        }

        private async System.Threading.Tasks.Task MostrarMapaAsync(double lat, double lon)
        {
            try
            {
                await webMapa.EnsureCoreWebView2Async();
                string ls = lat.ToString("F6", CultureInfo.InvariantCulture);
                string ns = lon.ToString("F6", CultureInfo.InvariantCulture);
                webMapa.NavigateToString($@"<!DOCTYPE html>
<html><head><meta charset='utf-8'>
<style>html,body{{margin:0;padding:0;width:100%;height:100%;overflow:hidden}}
iframe{{position:fixed;top:0;left:0;width:100%;height:100%;border:0}}</style>
</head><body>
<iframe src='https://www.google.com/maps?q={ls},{ns}&z=14&output=embed' allowfullscreen></iframe>
</body></html>");
            }
            catch { }
        }

        // ── Filtros ──────────────────────────────────────────────────────────
        private static Bitmap AplicarFiltro(Bitmap src, string filtro)
        {
            var bmp = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
            using var g = Graphics.FromImage(bmp);
            ColorMatrix cm = filtro switch
            {
                "Escala de grises" => new ColorMatrix(new float[][]
                {
                    new[]{.299f,.299f,.299f,0,0},
                    new[]{.587f,.587f,.587f,0,0},
                    new[]{.114f,.114f,.114f,0,0},
                    new[]{0f,0f,0f,1f,0f},
                    new[]{0f,0f,0f,0f,1f}
                }),
                "Sepia" => new ColorMatrix(new float[][]
                {
                    new[]{.393f,.349f,.272f,0,0},
                    new[]{.769f,.686f,.534f,0,0},
                    new[]{.189f,.168f,.131f,0,0},
                    new[]{0f,0f,0f,1f,0f},
                    new[]{0f,0f,0f,0f,1f}
                }),
                "Invertir" => new ColorMatrix(new float[][]
                {
                    new[]{-1f,0f,0f,0f,0f},
                    new[]{0f,-1f,0f,0f,0f},
                    new[]{0f,0f,-1f,0f,0f},
                    new[]{0f,0f,0f,1f,0f},
                    new[]{1f,1f,1f,0f,1f}
                }),
                "Rojo" => new ColorMatrix(new float[][]
                {
                    new[]{1f,0f,0f,0f,0f},
                    new[]{0f,0f,0f,0f,0f},
                    new[]{0f,0f,0f,0f,0f},
                    new[]{0f,0f,0f,1f,0f},
                    new[]{0f,0f,0f,0f,1f}
                }),
                "Verde" => new ColorMatrix(new float[][]
                {
                    new[]{0f,0f,0f,0f,0f},
                    new[]{0f,1f,0f,0f,0f},
                    new[]{0f,0f,0f,0f,0f},
                    new[]{0f,0f,0f,1f,0f},
                    new[]{0f,0f,0f,0f,1f}
                }),
                "Azul" => new ColorMatrix(new float[][]
                {
                    new[]{0f,0f,0f,0f,0f},
                    new[]{0f,0f,0f,0f,0f},
                    new[]{0f,0f,1f,0f,0f},
                    new[]{0f,0f,0f,1f,0f},
                    new[]{0f,0f,0f,0f,1f}
                }),
                "Alto contraste" => new ColorMatrix(new float[][]
                {
                    new[]{2f,0f,0f,0f,0f},
                    new[]{0f,2f,0f,0f,0f},
                    new[]{0f,0f,2f,0f,0f},
                    new[]{0f,0f,0f,1f,0f},
                    new[]{-.5f,-.5f,-.5f,0f,1f}
                }),
                "Cálido" => new ColorMatrix(new float[][]
                {
                    new[]{1.2f,0f,0f,0f,0f},
                    new[]{0f,1.0f,0f,0f,0f},
                    new[]{0f,0f,0.8f,0f,0f},
                    new[]{0f,0f,0f,1f,0f},
                    new[]{0f,0f,0f,0f,1f}
                }),
                "Frío" => new ColorMatrix(new float[][]
                {
                    new[]{0.8f,0f,0f,0f,0f},
                    new[]{0f,1.0f,0f,0f,0f},
                    new[]{0f,0f,1.3f,0f,0f},
                    new[]{0f,0f,0f,1f,0f},
                    new[]{0f,0f,0f,0f,1f}
                }),
                _ => new ColorMatrix()
            };
            using var ia = new ImageAttributes();
            ia.SetColorMatrix(cm);
            g.DrawImage(src, new Rectangle(0, 0, src.Width, src.Height),
                0, 0, src.Width, src.Height, GraphicsUnit.Pixel, ia);
            return bmp;
        }

        // ── Pintura ──────────────────────────────────────────────────────────
        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (!chkPintar.Checked || _imagenActual == null) return;
            _pintando = true;
            _ultimoPunto = ImagenPt(e.Location);
            _gPintura = Graphics.FromImage(_imagenActual);
            _gPintura.SmoothingMode = SmoothingMode.AntiAlias;
        }

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_pintando || _imagenActual == null || _gPintura == null) return;
            var pt = ImagenPt(e.Location);
            using var pen = new Pen(_colorPincel, _grosorPincel)
                { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round };
            _gPintura.DrawLine(pen, _ultimoPunto, pt);
            _ultimoPunto = pt;
            RefrescarVista();
        }

        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
            _pintando = false;
            _gPintura?.Dispose();
            _gPintura = null;
        }

        private Point ImagenPt(Point p)
        {
            if (_imagenActual == null) return p;
            var pb = picBox;
            float sx = (float)_imagenActual.Width / pb.Width;
            float sy = (float)_imagenActual.Height / pb.Height;
            float sc = Math.Max(sx, sy);
            int ox = (int)((pb.Width  - _imagenActual.Width  / sc) / 2);
            int oy = (int)((pb.Height - _imagenActual.Height / sc) / 2);
            int ix = (int)((p.X - ox) * sc);
            int iy = (int)((p.Y - oy) * sc);
            ix = Math.Clamp(ix, 0, _imagenActual.Width  - 1);
            iy = Math.Clamp(iy, 0, _imagenActual.Height - 1);
            return new Point(ix, iy);
        }

        // ── Rotación ─────────────────────────────────────────────────────────
        private static Bitmap RotarBmp(Bitmap src, float ang)
        {
            double r = ang * Math.PI / 180;
            double c = Math.Abs(Math.Cos(r)), s = Math.Abs(Math.Sin(r));
            int nw = (int)(src.Width * c + src.Height * s);
            int nh = (int)(src.Width * s + src.Height * c);
            var dst = new Bitmap(nw, nh, PixelFormat.Format32bppArgb);
            using var g = Graphics.FromImage(dst);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TranslateTransform(nw / 2f, nh / 2f);
            g.RotateTransform(ang);
            g.DrawImage(src, -src.Width / 2f, -src.Height / 2f);
            return dst;
        }

        // ── Guardar ──────────────────────────────────────────────────────────
        private void GuardarImagen(string ruta)
        {
            if (_imagenActual == null) return;
            var visible = _filtroActivo == "Original" ? _imagenActual : AplicarFiltro(_imagenActual, _filtroActivo);
            var fmt = Path.GetExtension(ruta).ToLowerInvariant() switch
            {
                ".png" => ImageFormat.Png,
                ".bmp" => ImageFormat.Bmp,
                ".gif" => ImageFormat.Gif,
                ".tiff"=> ImageFormat.Tiff,
                _      => ImageFormat.Jpeg
            };
            visible.Save(ruta, fmt);
            if (visible != _imagenActual) visible.Dispose();
        }

        // ── Refrescar vista ──────────────────────────────────────────────────
        private void RefrescarVista()
        {
            if (_imagenActual == null) return;
            var visible = _filtroActivo == "Original" ? _imagenActual : AplicarFiltro(_imagenActual, _filtroActivo);
            if (picBox.Image != null && picBox.Image != _imagenActual)
                picBox.Image.Dispose();
            picBox.Image = visible;
        }

        // ── Eventos UI ───────────────────────────────────────────────────────
        private void btnFiltro_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                _filtroActivo = btn.Tag as string ?? "Original";
                lblFiltroActivo.Text = "Filtro activo: " + _filtroActivo;
                RefrescarVista();
            }
        }

        private void btnRotarIzq_Click(object sender, EventArgs e)
        {
            if (_imagenActual == null) return;
            var r = RotarBmp(_imagenActual, -90);
            _imagenActual.Dispose();
            _imagenActual = r;
            RefrescarVista();
        }

        private void btnRotarDer_Click(object sender, EventArgs e)
        {
            if (_imagenActual == null) return;
            var r = RotarBmp(_imagenActual, 90);
            _imagenActual.Dispose();
            _imagenActual = r;
            RefrescarVista();
        }

        private void btnVoltearH_Click(object sender, EventArgs e)
        {
            _imagenActual?.RotateFlip(RotateFlipType.RotateNoneFlipX);
            RefrescarVista();
        }

        private void btnVoltearV_Click(object sender, EventArgs e)
        {
            _imagenActual?.RotateFlip(RotateFlipType.RotateNoneFlipY);
            RefrescarVista();
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            if (_imagenOriginal == null) return;
            _imagenActual?.Dispose();
            _imagenActual = new Bitmap(_imagenOriginal);
            _filtroActivo = "Original";
            lblFiltroActivo.Text = "Filtro activo: Original";
            RefrescarVista();
        }

        private void btnColorPincel_Click(object sender, EventArgs e)
        {
            using var dlg = new ColorDialog { Color = _colorPincel, FullOpen = true };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _colorPincel = dlg.Color;
                btnColorPincel.BackColor = _colorPincel;
                btnColorPincel.ForeColor = dlg.Color.GetBrightness() < 0.5f ? Color.White : Color.Black;
            }
        }

        private void trackGrosor_Scroll(object sender, EventArgs e)
        {
            _grosorPincel = trackGrosor.Value;
            lblGrosor.Text = $"Grosor: {_grosorPincel}";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog
            {
                Title = "Guardar imagen",
                Filter = "JPEG|*.jpg;*.jpeg|PNG|*.png|BMP|*.bmp|TIFF|*.tiff",
                FileName = Path.GetFileName(_rutaArchivo)
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                GuardarImagen(dlg.FileName);
                MessageBox.Show("Imagen guardada correctamente.", "Listo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnGuardarSobre_Click(object sender, EventArgs e)
        {
            var r = MessageBox.Show($"¿Sobreescribir \"{Path.GetFileName(_rutaArchivo)}\"?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                GuardarImagen(_rutaArchivo);
                MessageBox.Show("Guardado sobre el archivo original.", "Listo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnMostrarMapa_Click(object sender, EventArgs e)
        {
            if (double.TryParse(txtLat.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var lat) &&
                double.TryParse(txtLon.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var lon))
            {
                _lat = lat; _lon = lon;
                EscribirCoordenadas(_lat, _lon);   // ← guarda GPS en EXIF de la imagen
                lblGPS.Text = $"✔ GPS: {_lat:F6}, {_lon:F6}";
                lblGPS.ForeColor = Color.Green;
                _ = MostrarMapaAsync(_lat, _lon);
            }
            else
                MessageBox.Show("Ingrese coordenadas válidas (ej. 25.4161, -100.9930).",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // ── Escritura de coordenadas GPS en EXIF ─────────────────────────────
        /// <summary>
        /// Escribe los tags GPS EXIF en _imagenActual (y _imagenOriginal)
        /// para que se conserven al guardar la imagen.
        /// </summary>
        private void EscribirCoordenadas(double lat, double lon)
        {
            if (_imagenActual == null) return;

            // Helper: convierte un double a racional EXIF (3 pares num/den)
            // Representa grados, minutos y segundos × 10000 para precisión.
            static byte[] ToRacional(double value)
            {
                value = Math.Abs(value);
                uint deg  = (uint)value;
                uint minN = (uint)((value - deg) * 60 * 10000);
                uint minD = 10000u;
                uint secN = 0, secD = 1;
                // segundos ya están absorbidos en los minutos con 4 decimales
                var buf = new byte[24];
                void WriteU(uint v, int pos) { var b = BitConverter.GetBytes(v); Array.Copy(b, 0, buf, pos, 4); }
                WriteU(deg,  0); WriteU(1,    4);   // grados   num/den
                WriteU(minN, 8); WriteU(minD, 12);  // minutos  num/den
                WriteU(secN,16); WriteU(secD, 20);  // segundos num/den
                return buf;
            }

            // Helper: crea un PropertyItem con el tag id indicado
            PropertyItem MakeProp(int id, short type, int len, byte[] data)
            {
                // PropertyItem no tiene constructor público; usamos el de la imagen
                var pi = (PropertyItem)System.Runtime.Serialization.FormatterServices
                    .GetUninitializedObject(typeof(PropertyItem));
                pi.Id   = id;
                pi.Type = type;
                pi.Len  = len;
                pi.Value = data;
                return pi;
            }

            // Referencia latitud: 'N' o 'S'
            var latRef = Encoding.ASCII.GetBytes(lat >= 0 ? "N\0" : "S\0");
            var lonRef = Encoding.ASCII.GetBytes(lon >= 0 ? "E\0" : "W\0");
            var latVal = ToRacional(lat);
            var lonVal = ToRacional(lon);

            // Aplicar a _imagenActual
            _imagenActual.SetPropertyItem(MakeProp(0x0001, 2, 2, latRef));
            _imagenActual.SetPropertyItem(MakeProp(0x0002, 5, 24, latVal));
            _imagenActual.SetPropertyItem(MakeProp(0x0003, 2, 2, lonRef));
            _imagenActual.SetPropertyItem(MakeProp(0x0004, 5, 24, lonVal));

            // También en _imagenOriginal para que Restaurar no borre las coords
            if (_imagenOriginal != null)
            {
                _imagenOriginal.SetPropertyItem(MakeProp(0x0001, 2, 2, latRef));
                _imagenOriginal.SetPropertyItem(MakeProp(0x0002, 5, 24, latVal));
                _imagenOriginal.SetPropertyItem(MakeProp(0x0003, 2, 2, lonRef));
                _imagenOriginal.SetPropertyItem(MakeProp(0x0004, 5, 24, lonVal));
            }

            // Refrescar la lista de metadatos para que aparezcan los nuevos tags
            LeerMetadatos();
        }

        private void btnAbrirGMaps_Click(object sender, EventArgs e)
        {
            string ls = _lat.ToString("F6", CultureInfo.InvariantCulture);
            string ns = _lon.ToString("F6", CultureInfo.InvariantCulture);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = $"https://www.google.com/maps?q={ls},{ns}",
                UseShellExecute = true
            });
        }

        // ── Utilidades ───────────────────────────────────────────────────────
        private static string ObtenerTamanoLegible(long bytes)
        {
            string[] s = { "B","KB","MB","GB" };
            double size = bytes; int i = 0;
            while (size >= 1024 && i < s.Length - 1) { size /= 1024; i++; }
            return $"{size:0.##} {s[i]}";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _imagenOriginal?.Dispose();
            _imagenActual?.Dispose();
            _gPintura?.Dispose();
        }
    }
}


