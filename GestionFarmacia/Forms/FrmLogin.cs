using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestionFarmacia.Data;
using GestionFarmacia.Data.Repositories;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Forms
{
    public partial class FrmLogin : Form
    {
        private readonly IUsuarioRepository _usuarioRepo;

        public string NombreUsuario { get; private set; }
        public string RolUsuario { get; private set; }
        public FrmLogin(IUsuarioRepository usuarioRepo)
        {
            InitializeComponent();
            _usuarioRepo = usuarioRepo;
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            try
            {
                string usuarioIngresado = txtUsuario.Text.Trim();
                string claveIngresada = txtContraseña.Text;

                if (string.IsNullOrWhiteSpace(usuarioIngresado) || string.IsNullOrWhiteSpace(claveIngresada))
                {
                    MessageBox.Show("Por favor, ingresa usuario y contraseña.", "Campos obligatorios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                    Usuario usuario = _usuarioRepo.ObtenerPorNombreUsuario(usuarioIngresado);

                if (usuario != null && usuario.Contraseña == claveIngresada)
                {
                    NombreUsuario = usuario.NombreUsuario;
                    RolUsuario = usuario.Rol;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos", "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar sesión. Contacta al administrador.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Error en FrmLogin: " + ex.Message);
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
