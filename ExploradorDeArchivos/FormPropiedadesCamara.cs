using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    /// <summary>
    /// Diálogo de propiedades de la cámara: muestra resolución,
    /// índice de dispositivo y permite ajustar brillo y contraste.
    /// </summary>
    public partial class FormPropiedadesCamara : Form
    {
        private readonly int    _indice;
        private readonly int    _anchoActual;
        private readonly int    _altoActual;

        // Propiedades ajustadas (las lee el llamador después de DialogResult.OK)
        public int Brillo    { get; private set; } = 50;
        public int Contraste { get; private set; } = 50;

        public FormPropiedadesCamara(int indice, int ancho, int alto,
            int briActual = 50, int conActual = 50)
        {
            _indice      = indice;
            _anchoActual = ancho;
            _altoActual  = alto;
            Brillo       = briActual;
            Contraste    = conActual;

            InitializeComponent();
            CargarValores();
        }

        private void CargarValores()
        {
            _lblDispositivoVal.Text  = $"Cámara {_indice}";
            _lblResolucionVal.Text   = $"{_anchoActual} × {_altoActual} px";
            _lblFpsVal.Text          = "~30 fps";

            _trackBrillo.Value    = Math.Clamp(Brillo,    0, 100);
            _trackContraste.Value = Math.Clamp(Contraste, 0, 100);
            _lblBrilloNum.Text    = _trackBrillo.Value.ToString();
            _lblContrasteNum.Text = _trackContraste.Value.ToString();
        }

        private void TrackBrillo_Scroll(object sender, EventArgs e)
        {
            _lblBrilloNum.Text = _trackBrillo.Value.ToString();
        }

        private void TrackContraste_Scroll(object sender, EventArgs e)
        {
            _lblContrasteNum.Text = _trackContraste.Value.ToString();
        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            Brillo    = _trackBrillo.Value;
            Contraste = _trackContraste.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}


