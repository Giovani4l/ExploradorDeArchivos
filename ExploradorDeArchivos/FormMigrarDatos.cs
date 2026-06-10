using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using DrawingColor = System.Drawing.Color;

namespace ExploradorDeArchivos
{
    /// <summary>
    /// Formulario para migrar datos desde archivos (CSV, Excel, JSON, TXT)
    /// hacia SQL Server o HeidiSQL (MariaDB/MySQL).
    /// </summary>
    public partial class FormMigrarDatos : Form
    {
        private string _rutaArchivoSeleccionado = string.Empty;
        private string _motorSeleccionado = "SQL Server";

        public FormMigrarDatos()
        {
            InitializeComponent();
            ConfigurarEventosFormulario();
            InicializarEstadoFormulario();
        }

        private void InicializarEstadoFormulario()
        {
            _zonaCargar.Visible = true;
            _zonaConfig.Visible = false;
            _zonaMigrando.Visible = false;
            _zonaResultado.Visible = false;
            OcultarError();
        }

        private void ConfigurarEventosFormulario()
        {
            _zonaArrastrar.DragEnter += ZonaArrastrar_DragEnter;
            _zonaArrastrar.DragLeave += ZonaArrastrar_DragLeave;
            _zonaArrastrar.DragDrop += ZonaArrastrar_DragDrop;
            _zonaArrastrar.Click += ZonaArrastrar_Click;

            _lblDropIcon.Click += (s, e) => ZonaArrastrar_Click(s, e);
            _lblDropText.Click += (s, e) => ZonaArrastrar_Click(s, e);
            _lblDropSub.Click += (s, e) => ZonaArrastrar_Click(s, e);
            _btnSeleccionar.Click += ZonaArrastrar_Click;

            _btnSqlServer.Click += (s, e) => SeleccionarMotorDB("SQL Server");
            _btnHeidi.Click += (s, e) => SeleccionarMotorDB("HeidiSQL");
            _btnIniciarMigracion.Click += BtnIniciarMigracion_Click;
            _btnNuevaMigracion.Click += (s, e) => InicializarEstadoFormulario();
        }

        private void ZonaArrastrar_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                _zonaArrastrar.BackColor = DrawingColor.FromArgb(20, 32, 59);
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void ZonaArrastrar_DragLeave(object sender, EventArgs e)
        {
            _zonaArrastrar.BackColor = DrawingColor.FromArgb(13, 21, 37);
        }

        private void ZonaArrastrar_DragDrop(object sender, DragEventArgs e)
        {
            _zonaArrastrar.BackColor = DrawingColor.FromArgb(13, 21, 37);
            string[] archivos = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (archivos != null && archivos.Length > 0)
            {
                ProcesarArchivo(archivos[0]);
            }
        }

        private void ZonaArrastrar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Seleccionar archivo limpio para migrar";
                ofd.Filter = "Archivos de datos (*.csv;*.txt)|*.csv;*.txt";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ProcesarArchivo(ofd.FileName);
                }
            }
        }

        private void ProcesarArchivo(string ruta)
        {
            if (File.Exists(ruta))
            {
                _rutaArchivoSeleccionado = ruta;
                _lblInfoArchivo.Text = $"✔️ Archivo cargado: {Path.GetFileName(ruta)}";
                _lblInfoArchivo.Visible = true;
                OcultarError();

                _zonaCargar.Visible = false;
                _zonaConfig.Visible = true;

                if (string.IsNullOrEmpty(_txtServidor.Text) || _txtServidor.Text == "Servidor (Host)")
                    _txtServidor.Text = "localhost";

                SeleccionarMotorDB(_motorSeleccionado);
            }
        }

        private void SeleccionarMotorDB(string motor)
        {
            _motorSeleccionado = motor;
            _lblMotorInfo.Text = $"Configurando conexión para: {motor}";
            _txtPuerto.Text = (motor == "SQL Server") ? "1433" : "3306";
        }

        private void BtnIniciarMigracion_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_txtServidor.Text) || _txtBaseDatos.Text == "Base de Datos" || _txtTabla.Text == "Nombre de la Tabla Destino")
            {
                MostrarError("Llena los campos obligatorios para establecer la conexión.");
                return;
            }

            OcultarError();
            _zonaConfig.Visible = false;
            _zonaMigrando.Visible = true;

            ProcesarMigracionDeDatos();
        }

        // ─────────────────────────────────────────────────────────────────────
        //  MIGRACIÓN — usa el driver correcto según el motor seleccionado
        // ─────────────────────────────────────────────────────────────────────
        private void ProcesarMigracionDeDatos()
        {
            _barProg.Value = 0;
            _lblProgTexto.Text = "Leyendo estructura del archivo...";

            try
            {
                // 1. Leer archivo
                if (!File.Exists(_rutaArchivoSeleccionado))
                {
                    MostrarResultadoFinal(false, "El archivo seleccionado ya no está disponible.");
                    return;
                }

                string[] lineas;
                string extension = Path.GetExtension(_rutaArchivoSeleccionado).ToLower();

                // Procesar según tipo de archivo
                if (extension == ".docx" || extension == ".doc")
                {
                    lineas = ExtraerDatosDeWord(_rutaArchivoSeleccionado);
                }
                else
                {
                    lineas = File.ReadAllLines(_rutaArchivoSeleccionado);
                }

                if (lineas.Length == 0)
                {
                    MostrarResultadoFinal(false, "El archivo seleccionado está vacío.");
                    return;
                }

                char separador = lineas[0].Contains(";") ? ';' : ',';
                string[] columnasOriginales = lineas[0].Split(separador);
                List<string> columnasLimpias = new List<string>();

                foreach (var col in columnasOriginales)
                {
                    string colLimpia = col.Trim().Replace(" ", "_");
                    colLimpia = Regex.Replace(colLimpia, @"[^a-zA-Z0-9_]", "");
                    if (string.IsNullOrEmpty(colLimpia)) colLimpia = "Columna_SinNombre";
                    columnasLimpias.Add(colLimpia);
                }

                // 2. Delegar al motor correcto
                if (_motorSeleccionado == "SQL Server")
                    MigrarSqlServer(lineas, columnasLimpias, separador);
                else
                    MigrarMySQL(lineas, columnasLimpias, separador);
            }
            catch (Exception ex)
            {
                MostrarResultadoFinal(false, $"Error al migrar datos: {ex.Message}");
            }
        }

        // ─── EXTRACCIÓN DE DATOS WORD ──────────────────────────────────────
        private string[] ExtraerDatosDeWord(string rutaArchivo)
        {
            List<string> lineas = new List<string>();

            try
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(rutaArchivo, false))
                {
                    if (doc.MainDocumentPart == null)
                    {
                        throw new Exception("No se pudo acceder al contenido del documento Word.");
                    }

                    var body = doc.MainDocumentPart.Document.Body;
                    var tablas = body.Elements<Table>();

                    if (tablas.Count() > 0)
                    {
                        // Procesar la primera tabla encontrada
                        foreach (var tabla in tablas.Take(1))
                        {
                            foreach (var fila in tabla.Elements<TableRow>())
                            {
                                List<string> celdas = new List<string>();
                                foreach (var celda in fila.Elements<TableCell>())
                                {
                                    string contenidoCelda = ExtraerTextoDeElemento(celda).Trim();
                                    celdas.Add(contenidoCelda);
                                }
                                if (celdas.Count > 0)
                                {
                                    lineas.Add(string.Join(",", celdas));
                                }
                            }
                        }
                    }
                    else
                    {
                        // Si no hay tablas, extraer todo el texto del documento
                        foreach (var parrafo in body.Elements<Paragraph>())
                        {
                            string textoParrafo = ExtraerTextoDeElemento(parrafo);
                            if (!string.IsNullOrWhiteSpace(textoParrafo))
                            {
                                lineas.Add(textoParrafo);
                            }
                        }
                    }
                }

                if (lineas.Count == 0)
                {
                    throw new Exception("El documento Word no contiene datos válidos.");
                }

                return lineas.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al procesar archivo Word: {ex.Message}");
            }
        }

        // Método auxiliar para extraer texto de cualquier elemento
        private string ExtraerTextoDeElemento(OpenXmlElement elemento)
        {
            StringBuilder texto = new StringBuilder();

            if (elemento == null)
                return string.Empty;

            // Extraer texto de todos los nodos de texto recursivamente
            foreach (var nodo in elemento.Descendants())
            {
                if (nodo is Text textoNodo)
                {
                    texto.Append(textoNodo.Text);
                }
                else if (nodo is TabChar)
                {
                    texto.Append("\t");
                }
            }

            return texto.ToString();
        }

        // ─── SQL SERVER ───────────────────────────────────────────────────────
        private void MigrarSqlServer(string[] lineas, List<string> columnas, char sep)
        {
            string host = _txtServidor.Text;
            string puerto = _txtPuerto.Text;
            string user = _txtUsuario.Text;
            string pass = _txtPassword.Text;
            string db = _txtBaseDatos.Text;
            string tabla = _txtTabla.Text;

            string cadenaMaster = $"Server={host},{puerto};User ID={user};Password={pass};Database=master;TrustServerCertificate=True;";
            string cadenaDestino = $"Server={host},{puerto};User ID={user};Password={pass};Database={db};TrustServerCertificate=True;";

            // Crear BD
            using (var con = new SqlConnection(cadenaMaster))
            {
                con.Open();
                string q = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{db}') CREATE DATABASE [{db}]";
                using var cmd = new SqlCommand(q, con);
                cmd.ExecuteNonQuery();
            }

            _barProg.Value = 20; _lblProgPct.Text = "20%";
            _lblProgTexto.Text = "Verificando tabla destino...";

            // Crear tabla
            if (_chkCrearTabla.Checked)
            {
                using var con = new SqlConnection(cadenaDestino);
                con.Open();
                string q = $"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{tabla}]') AND type='U') BEGIN CREATE TABLE [dbo].[{tabla}] ([Id_Autogenerado] INT IDENTITY(1,1) PRIMARY KEY,";
                foreach (var col in columnas) q += $" [{col}] NVARCHAR(MAX) NULL,";
                q = q.TrimEnd(',') + " ) END";
                using var cmd = new SqlCommand(q, con);
                cmd.ExecuteNonQuery();
            }

            _barProg.Value = 40; _lblProgPct.Text = "40%";
            _lblProgTexto.Text = "Insertando registros...";

            // Insertar filas
            using (var con = new SqlConnection(cadenaDestino))
            {
                con.Open();
                string cols = string.Join(", ", columnas.Select(c => $"[{c}]"));
                string pars = string.Join(", ", columnas.Select((c, i) => $"@p{i}"));
                string queryInsert = $"INSERT INTO [{tabla}] ({cols}) VALUES ({pars})";

                int total = lineas.Length - 1;
                int insertadas = 0;

                for (int i = 1; i < lineas.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lineas[i])) continue;
                    string[] celdas = lineas[i].Split(sep);

                    using var cmd = new SqlCommand(queryInsert, con);
                    for (int j = 0; j < columnas.Count; j++)
                    {
                        string valor = j < celdas.Length ? celdas[j].Trim() : "";
                        cmd.Parameters.AddWithValue($"@p{j}", string.IsNullOrEmpty(valor) ? (object)DBNull.Value : valor);
                    }
                    cmd.ExecuteNonQuery();
                    insertadas++;

                    if (insertadas % 10 == 0 || insertadas == total)
                    {
                        int pct = 40 + (int)(((double)insertadas / total) * 60);
                        _barProg.Value = pct;
                        _lblProgPct.Text = $"{pct}%";
                        _lblProgTexto.Text = $"Migrando: {insertadas} de {total} registros...";
                        Application.DoEvents();
                    }
                }
            }

            _barProg.Value = 100; _lblProgPct.Text = "100%";
            MostrarResultadoFinal(true, $"¡Migración completada con éxito! Registros insertados en la tabla '{tabla}'.");
        }

        // ─── MySQL / HeidiSQL ─────────────────────────────────────────────────
        private void MigrarMySQL(string[] lineas, List<string> columnas, char sep)
        {
            try
            {
                string host = _txtServidor.Text;
                string puerto = _txtPuerto.Text;
                string user = _txtUsuario.Text;
                string pass = _txtPassword.Text;
                string db = _txtBaseDatos.Text;
                string tabla = _txtTabla.Text;

                // Validar que no estén vacíos
                if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(user) || 
                    string.IsNullOrWhiteSpace(db) || string.IsNullOrWhiteSpace(tabla))
                {
                    MostrarResultadoFinal(false, "Por favor completa todos los campos: Servidor, Usuario, Base de Datos y Tabla.");
                    return;
                }

                // Cadena sin base de datos (para crearla si no existe)
                string cadenaSinDB = $"Server={host};Port={puerto};Uid={user};Pwd={pass};SslMode=None;AllowPublicKeyRetrieval=true;";
                string cadenaConDB = $"Server={host};Port={puerto};Database={db};Uid={user};Pwd={pass};SslMode=None;AllowPublicKeyRetrieval=true;CharSet=utf8mb4;";

                // Crear BD
                _lblProgTexto.Text = "Creando base de datos...";
                using (var con = new MySqlConnection(cadenaSinDB))
                {
                    con.Open();
                    string q = $"CREATE DATABASE IF NOT EXISTS `{db}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";
                    using var cmd = new MySqlCommand(q, con);
                    cmd.ExecuteNonQuery();
                }

                _barProg.Value = 20; _lblProgPct.Text = "20%";
                _lblProgTexto.Text = "Creando tabla destino...";

                // Crear tabla (siempre, para asegurar que exista)
                using (var conTabla = new MySqlConnection(cadenaConDB))
                {
                    conTabla.Open();
                    string queryTabla = $"CREATE TABLE IF NOT EXISTS `{tabla}` (`Id_Autogenerado` INT AUTO_INCREMENT PRIMARY KEY,";
                    foreach (var col in columnas) queryTabla += $" `{col}` TEXT NULL,";
                    queryTabla = queryTabla.TrimEnd(',') + ");";
                    using var cmdTabla = new MySqlCommand(queryTabla, conTabla);
                    cmdTabla.ExecuteNonQuery();
                }

                _barProg.Value = 40; _lblProgPct.Text = "40%";
                _lblProgTexto.Text = "Insertando registros...";

                // Insertar filas
                using (var con = new MySqlConnection(cadenaConDB))
                {
                    con.Open();
                    string cols = string.Join(", ", columnas.Select(c => $"`{c}`"));
                    string pars = string.Join(", ", columnas.Select((c, i) => $"@p{i}"));
                    string queryInsert = $"INSERT INTO `{tabla}` ({cols}) VALUES ({pars})";

                    int total = lineas.Length - 1;
                    int insertadas = 0;

                    for (int i = 1; i < lineas.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(lineas[i])) continue;
                        string[] celdas = lineas[i].Split(sep);

                        using var cmd = new MySqlCommand(queryInsert, con);
                        for (int j = 0; j < columnas.Count; j++)
                        {
                            string valor = j < celdas.Length ? celdas[j].Trim() : "";
                            cmd.Parameters.AddWithValue($"@p{j}", string.IsNullOrEmpty(valor) ? (object)DBNull.Value : valor);
                        }
                        cmd.ExecuteNonQuery();
                        insertadas++;

                        if (insertadas % 10 == 0 || insertadas == total)
                        {
                            int pct = 40 + (int)(((double)insertadas / total) * 60);
                            _barProg.Value = pct;
                            _lblProgPct.Text = $"{pct}%";
                            _lblProgTexto.Text = $"Migrando: {insertadas} de {total} registros...";
                            Application.DoEvents();
                        }
                    }
                }

                _barProg.Value = 100; _lblProgPct.Text = "100%";
                MostrarResultadoFinal(true, $"¡Migración completada con éxito! Registros insertados en la tabla '{tabla}'.");
            }
            catch (MySqlException mex)
            {
                if (mex.Message.Contains("Access denied"))
                {
                    MostrarResultadoFinal(false, $"Acceso denegado: Verifica que el usuario '{_txtUsuario.Text}' tenga permisos en MySQL/HeidiSQL.");
                }
                else if (mex.Message.Contains("Unknown database"))
                {
                    MostrarResultadoFinal(false, $"Base de datos '{_txtBaseDatos.Text}' no encontrada. El servidor rechazó crear la BD.");
                }
                else
                {
                    MostrarResultadoFinal(false, $"Error MySQL: {mex.Message}");
                }
            }
            catch (Exception ex)
            {
                MostrarResultadoFinal(false, $"Error al migrar datos: {ex.Message}");
            }
        }

        // ─── Helpers UI ──────────────────────────────────────────────────────
        private void MostrarResultadoFinal(bool exito, string msg)
        {
            _zonaMigrando.Visible = false;
            _zonaResultado.Visible = true;

            if (exito)
            {
                _lblResultIcon.Text = "🚀";
                _lblResultIcon.ForeColor = DrawingColor.FromArgb(52, 211, 153);
                _lblResultMsg.Text = "¡Migración Exitosa!";
                _lblResultDetalle.Text = msg;
            }
            else
            {
                _lblResultIcon.Text = "❌";
                _lblResultIcon.ForeColor = DrawingColor.FromArgb(248, 113, 113);
                _lblResultMsg.Text = "Error en el proceso";
                _lblResultDetalle.Text = msg;
            }
        }

        private void MostrarError(string mensaje)
        {
            _lblError.Text = $"  ⚠️ {mensaje}";
            _lblError.Height = 35;
            _lblError.Visible = true;
        }

        private void OcultarError()
        {
            _lblError.Visible = false;
            _lblError.Height = 0;
        }
    }
}


