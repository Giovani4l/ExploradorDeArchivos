using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using OpenCvSharp;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace ExploradorDeArchivos
{
    /// <summary>
    /// Cámara en vivo usando OpenCvSharp (VideoCapture).
    /// Captura frames en un hilo de fondo y los muestra en un PictureBox.
    /// Compatible con cualquier cámara USB / integrada en Windows.
    /// </summary>
    public partial class FormCamara : Form
    {
       

        // ── captura ──────────────────────────────────────────────────────────
        private VideoCapture? _cap;
        private Thread? _hilo;
        private volatile bool _corriendo;
        private Bitmap? _ultimoFrame;
        private readonly object _lock = new();
        private VideoWriter? _writer;
        private volatile bool _grabando;
        private readonly object _lockGrabar = new();

        // ── Colores de estado del botón grabar ───────────────────────────────
        private static readonly Color ColorBtnGrabarActivo  = Color.FromArgb(200, 40, 40);
        private static readonly Color ColorBtnGrabarNormal  = Color.FromArgb(0, 122, 204);

        private string _carpeta;

        public FormCamara()
        {
            InitializeComponent();

            _carpeta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                "Explorador_Capturas");
            Directory.CreateDirectory(_carpeta);

            // Suscribir eventos
            _btnConectar.Click += BtnConectar_Click;
            _btnCaptura.Click += BtnCaptura_Click;
            _btnCarpeta.Click += (s, e) =>
                System.Diagnostics.Process.Start("explorer.exe", _carpeta);

            Shown += (s, e) => BuscarCamaras();
            FormClosing += (s, e) => Detener();
        }

        // ── Detectar cámaras probando índices 0-4 ────────────────────────────
        private void BuscarCamaras()
        {
            _cboCamaras.Items.Clear();
            SetEstado("Buscando cámaras…");

            int encontradas = 0;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    using var test = new VideoCapture(i, VideoCaptureAPIs.DSHOW);
                    if (test.IsOpened())
                    {
                        _cboCamaras.Items.Add($"Cámara {i}");
                        encontradas++;
                    }
                }
                catch { }
            }

            if (encontradas == 0)
            {
                // Intentar también con CAP_ANY
                try
                {
                    using var test = new VideoCapture(0);
                    if (test.IsOpened())
                    {
                        _cboCamaras.Items.Add("Cámara 0 (auto)");
                        encontradas++;
                    }
                }
                catch { }
            }

            if (encontradas == 0)
            {
                SetEstado("⚠️  No se encontró ninguna cámara. Conecta una y presiona Reconectar.");
                _cboCamaras.Items.Add("Cámara 0");
            }
            else
            {
                SetEstado($"Se encontr{(encontradas == 1 ? "ó" : "aron")} {encontradas} cámara(s). Presiona Conectar.");
            }

            _cboCamaras.SelectedIndex = 0;

            // Auto-conectar
            Conectar(0);
        }

        // ── Conectar ─────────────────────────────────────────────────────────
        private void BtnConectar_Click(object sender, EventArgs e)
        {
            Detener();
            int idx = _cboCamaras.SelectedIndex;
            if (idx < 0) idx = 0;
            Conectar(idx);
        }

        private void Conectar(int idx)
        {
            SetEstado($"Conectando cámara {idx}…");
            try
            {
                _cap = new VideoCapture(idx, VideoCaptureAPIs.DSHOW);

                if (!_cap.IsOpened())
                {
                    // Fallback a CAP_ANY
                    _cap.Dispose();
                    _cap = new VideoCapture(idx);
                }

                if (!_cap.IsOpened())
                {
                    SetEstado("⚠️  No se pudo abrir la cámara.");
                    return;
                }

                // Resolución deseada
                _cap.Set(VideoCaptureProperties.FrameWidth, 1280);
                _cap.Set(VideoCaptureProperties.FrameHeight, 720);

                _corriendo = true;
                _hilo = new Thread(BucleCaptura)
                {
                    IsBackground = true,
                    Name = "CameraThread"
                };
                _hilo.Start();

                SetEstado($"✅  Cámara {idx} activa  ·  {_carpeta}");
            }
            catch (Exception ex)
            {
                SetEstado($"⚠️  Error: {ex.Message}");
            }
        }

        // ── Hilo de captura ──────────────────────────────────────────────────
        private void BucleCaptura()
        {
            using var frame = new Mat();

            while (_corriendo && _cap != null && _cap.IsOpened())
            {
                try
                {
                    if (!_cap.Read(frame) || frame.Empty())
                    {
                        Thread.Sleep(30);
                        continue;
                    }

                    // 🎥 ¡ESTA ES LA LÍNEA CLAVE QUE FALTA!
                    // Si la bandera está activa, cose este cuadro al archivo de video
                    if (_grabando)
                    {
                        lock (_lockGrabar)
                        {
                            _writer?.Write(frame);
                        }
                    }

                    // Convertir BGR (OpenCV) → Bitmap (GDI+) sin invertir colores
                    var bmp = MatToBitmap(frame);

                    lock (_lock)
                    {
                        _ultimoFrame?.Dispose();
                        _ultimoFrame = bmp;
                    }

                    // Mostrar en el PictureBox (UI Thread)
                    if (_picPreview.IsHandleCreated && !_picPreview.IsDisposed)
                    {
                        _picPreview.BeginInvoke(() =>
                        {
                            Bitmap? mostrar = null;
                            lock (_lock)
                            {
                                if (_ultimoFrame != null)
                                    mostrar = (Bitmap)_ultimoFrame.Clone();
                            }
                            if (mostrar != null)
                            {
                                var viejo = _picPreview.Image;
                                _picPreview.Image = mostrar;
                                viejo?.Dispose();
                            }
                        });
                    }

                    Thread.Sleep(33); // ~30 fps
                }
                catch (Exception)
                {
                    Thread.Sleep(100);
                }
            }
        }

        // ── Captura foto ─────────────────────────────────────────────────────
        private void BtnCaptura_Click(object sender, EventArgs e)
        {
            Bitmap? snap = null;
            lock (_lock)
            {
                if (_ultimoFrame != null)
                    snap = (Bitmap)_ultimoFrame.Clone();
            }

            if (snap == null)
            {
                SetEstado("⚠️  No hay imagen. Conecta la cámara primero.");
                return;
            }

            var nombre = $"Captura_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var ruta = Path.Combine(_carpeta, nombre);
            snap.Save(ruta, ImageFormat.Png);

            _picMiniatura.Image?.Dispose();
            _picMiniatura.Image = snap;
            SetEstado($"✅  Guardado: {nombre}");
        }

        // ── Detener ──────────────────────────────────────────────────────────
        private void Detener()
        {
            DetenerGrabacion();
            _corriendo = false;
            _hilo?.Join(500);
            _cap?.Dispose();
            _cap = null;
            lock (_lock) { _ultimoFrame?.Dispose(); _ultimoFrame = null; }
        }

        private void SetEstado(string msg)
        {
            if (_lblEstado.InvokeRequired)
                _lblEstado.Invoke(() => _lblEstado.Text = msg);
            else
                _lblEstado.Text = msg;
        }

        private static Bitmap MatToBitmap(Mat mat)
        {
            // Verificar que el Mat tiene datos válidos
            if (mat.Empty())
                return new Bitmap(1, 1);

            var bmp = new Bitmap(mat.Cols, mat.Rows, PixelFormat.Format24bppRgb);
            var data = bmp.LockBits(
                new Rectangle(0, 0, mat.Cols, mat.Rows),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            try
            {
                int stride = data.Stride;
                int bytesPerRow = mat.Cols * 3;

                // Copiar línea por línea para garantizar correcta alineación
                for (int row = 0; row < mat.Rows; row++)
                {
                    byte[] lineBuffer = new byte[bytesPerRow];
                    nint srcPtr = mat.Ptr(row);
                    System.Runtime.InteropServices.Marshal.Copy(srcPtr, lineBuffer, 0, bytesPerRow);
                    System.Runtime.InteropServices.Marshal.Copy(lineBuffer, 0, data.Scan0 + (row * stride), bytesPerRow);
                }
            }
            finally
            {
                bmp.UnlockBits(data);
            }

            return bmp;
        }


        private void DetenerGrabacion()
        {
            if (_grabando)
            {
                _grabando = false;
                Thread.Sleep(50); // Tiempo de cortesía para el último cuadro

                lock (_lockGrabar)
                {
                    _writer?.Release();
                    _writer?.Dispose();
                    _writer = null;
                }
            }
        }

        private void FormCamara_Load(object sender, EventArgs e)
        {

        }

        private void _btnGrabar_Click(object sender, EventArgs e)
        {
            if (!_corriendo || _cap == null || !_cap.IsOpened())
            {
                SetEstado("⚠️ Primero debes conectar la cámara.");
                return;
            }

            if (!_grabando)
            {
                // ─── INICIAR GRABACIÓN ───
                var nombreVideo = $"Video_{DateTime.Now:yyyyMMdd_HHmmss}.mp4";
                var rutaVideo = Path.Combine(_carpeta, nombreVideo);

                int width = (int)_cap.Get(VideoCaptureProperties.FrameWidth);
                int height = (int)_cap.Get(VideoCaptureProperties.FrameHeight);

                // Codec estándar de video para máxima compatibilidad
                int codec = VideoWriter.FourCC('X', 'V', 'I', 'D');
                double fps = 60.0;

                try
                {
                    _writer = new VideoWriter(rutaVideo, codec, fps, new OpenCvSharp.Size(width, height));

                    if (!_writer.IsOpened())
                    {
                        SetEstado("⚠️ No se pudo inicializar el grabador.");
                        _writer.Dispose();
                        _writer = null;
                        return;
                    }

                    _grabando = true;
                    _btnGrabar.Text = "🛑 Detener";
                    _btnGrabar.BackColor = ColorBtnGrabarActivo;
                    SetEstado($"🎥 Grabando: {nombreVideo}");
                }
                catch (Exception ex)
                {
                    SetEstado($"⚠️ Error al iniciar video: {ex.Message}");
                }
            }
            else
            {
                // ─── DETENER GRABACIÓN ───
                DetenerGrabacion();
                _btnGrabar.Text = "🎥 Grabar Video";
                _btnGrabar.BackColor = ColorBtnGrabarNormal;
                SetEstado("✅ Video guardado con éxito.");
            }


        }
    }
    
}


