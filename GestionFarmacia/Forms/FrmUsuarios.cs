using System;
using System.Windows.Forms;
using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Data.Repositories;
using GestionFarmacia.Entities;
using GestionFarmacia.Utils;

namespace GestionFarmacia.Forms
{
    public partial class FrmUsuarios : Form
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private int? usuarioSeleccionadoId = null;

        public FrmUsuarios(IUsuarioRepository usuarioRepo)
        {
            InitializeComponent();
            _usuarioRepo = usuarioRepo;
            cmbRol.Items.AddRange(new[] { "Administrador", "Vendedor", "Almacenero" });
            cmbRol.SelectedIndex = 0;
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = _usuarioRepo.Consultar();
            dgvUsuarios.ClearSelection();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarFormulario()) return;

                var usuario = new Usuario
                {
                    UsuarioID = usuarioSeleccionadoId ?? 0, // 0 para insertar
                    NombreUsuario = txtNombreUsuario.Text.Trim(),
                    Contraseña = txtContrasena.Text,
                    Rol = cmbRol.SelectedItem.ToString()
                };

                bool resultado;

                if (usuarioSeleccionadoId.HasValue)
                    resultado = _usuarioRepo.Actualizar(usuario);
                else
                    resultado = _usuarioRepo.Insertar(usuario);

                if (resultado)
                {
                    MessageBox.Show(usuarioSeleccionadoId.HasValue ? "Usuario actualizado." : "Usuario registrado.");
                    LimpiarFormulario();
                    CargarUsuarios();
                }
                else
                {
                    MessageBox.Show("Error al guardar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            usuarioSeleccionadoId = null;
            txtNombreUsuario.Clear();
            txtContrasena.Clear();
            cmbRol.SelectedIndex = 0;
            txtNombreUsuario.Enabled = true; // Habilitar de nuevo para registro
            btnGuardar.Text = "Registrar";
            dgvUsuarios.ClearSelection();
        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text))
            {
                MessageBox.Show("El nombre de usuario es obligatorio.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("La contraseña es obligatoria.");
                return false;
            }

            return true;
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dgvUsuarios.Rows[e.RowIndex];
                usuarioSeleccionadoId = Convert.ToInt32(fila.Cells["UsuarioID"].Value);
                txtNombreUsuario.Text = fila.Cells["NombreUsuario"].Value.ToString();
                txtContrasena.Text = fila.Cells["Contraseña"].Value.ToString();
                cmbRol.SelectedItem = fila.Cells["Rol"].Value.ToString();

                // Bloquear el nombre de usuario al editar
                txtNombreUsuario.Enabled = false;
                btnGuardar.Text = "Actualizar";
            }
        }
    }
}
