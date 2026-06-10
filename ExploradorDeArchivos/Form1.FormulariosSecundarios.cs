using System;
using System.Windows.Forms;

namespace ExploradorDeArchivos
{
    partial class Form1
    {
        // ══════════════════════════════════════════════════════════════════════
        //  FORMULARIOS SECUNDARIOS
        // ══════════════════════════════════════════════════════════════════════

        private void MenuItemAbrirCamara_Click(object sender, EventArgs e)
        {
            if (_formCamara == null || _formCamara.IsDisposed)
            {
                _formCamara = new FormCamara();
                _formCamara.FormClosed += (s, ev) => _formCamara = null;
                _formCamara.Show(this);
            }
            else { _formCamara.BringToFront(); }
        }

        private void MenuItemGrabarAudio_Click(object sender, EventArgs e)
        {
            if (_formGrabador == null || _formGrabador.IsDisposed)
            {
                _formGrabador = new FormGrabadorAudio();
                _formGrabador.FormClosed += (s, ev) => _formGrabador = null;
                _formGrabador.Show(this);
            }
            else { _formGrabador.BringToFront(); }
        }

        private void BtnMigrarDatos_Click(object sender, EventArgs e)
        {
            if (_formMigrar == null || _formMigrar.IsDisposed)
            {
                _formMigrar = new FormMigrarDatos();
                _formMigrar.FormClosed += (s, ev) => _formMigrar = null;
                _formMigrar.Show(this);
            }
            else { _formMigrar.BringToFront(); }
        }

        private void BtnLimpiarDatos_Click(object sender, EventArgs e)
        {
            if (_formLimpieza == null || _formLimpieza.IsDisposed)
            {
                _formLimpieza = new FormLimpiezaDatos();
                _formLimpieza.FormClosed += (s, ev) => _formLimpieza = null;
                _formLimpieza.Show(this);
            }
            else { _formLimpieza.BringToFront(); }
        }
    }
}
