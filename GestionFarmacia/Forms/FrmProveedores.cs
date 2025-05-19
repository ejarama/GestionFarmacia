using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GestionFarmacia.Entities;
using GestionFarmacia.Data.Repositories;

namespace GestionFarmacia.Presentation
{
    public partial class FrmProveedores : Form
    {
        private readonly ProveedorRepository proveedorRepo;

        public FrmProveedores()
        {
            InitializeComponent();
            proveedorRepo = new ProveedorRepository();
        }

        private void FrmProveedores_Load(object sender, EventArgs e)
        {
            CargarProveedores();
            LimpiarCampos();
        }

        private void CargarProveedores()
        {
            dgvProveedores.DataSource = proveedorRepo.Consultar();
        }

        private void LimpiarCampos()
        {
            txtProveedorID.Text = "";
            txtNombre.Text = "";
            txtContacto.Text = "";
            btnGuardar.Text = "Agregar";
            dgvProveedores.ClearSelection();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Proveedor proveedor = new Proveedor
            {
                Nombre = txtNombre.Text.Trim(),
                Contacto = txtContacto.Text.Trim()
            };

            bool resultado;
            if (string.IsNullOrEmpty(txtProveedorID.Text))
            {
                resultado = proveedorRepo.Insertar(proveedor);
                if (resultado)
                    MessageBox.Show("Proveedor agregado exitosamente.");
            }
            else
            {
                proveedor.ProveedorID = Convert.ToInt32(txtProveedorID.Text);
                resultado = proveedorRepo.Actualizar(proveedor);
                if (resultado)
                    MessageBox.Show("Proveedor actualizado exitosamente.");
            }

            if (resultado)
            {
                CargarProveedores();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Hubo un error al guardar el proveedor.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProveedorID.Text))
            {
                MessageBox.Show("Seleccione un proveedor a eliminar.");
                return;
            }

            int id = Convert.ToInt32(txtProveedorID.Text);
            var confirm = MessageBox.Show("¿Está seguro que desea eliminar este proveedor?", "Confirmar", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                bool eliminado = proveedorRepo.Eliminar(id);
                if (eliminado)
                {
                    MessageBox.Show("Proveedor eliminado correctamente.");
                    CargarProveedores();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("Error al eliminar proveedor.");
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void dgvProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtProveedorID.Text = dgvProveedores.Rows[e.RowIndex].Cells["ProveedorID"].Value.ToString();
                txtNombre.Text = dgvProveedores.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                txtContacto.Text = dgvProveedores.Rows[e.RowIndex].Cells["Contacto"].Value.ToString();
                btnGuardar.Text = "Actualizar";
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProveedores.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor seleccione un proveedor para editar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Obtenemos el ID del proveedor seleccionado
                int proveedorId = Convert.ToInt32(dgvProveedores.SelectedRows[0].Cells["ProveedorID"].Value);

                // Validamos los campos requeridos
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El nombre del proveedor es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Creamos la entidad con los nuevos valores
                var proveedorActualizado = new Proveedor
                {
                    ProveedorID = proveedorId,
                    Nombre = txtNombre.Text.Trim(),
                    Contacto = txtContacto.Text.Trim()
                };

                // Llamamos al repositorio
                var proveedorRepo = new ProveedorRepository();
                proveedorRepo.Actualizar(proveedorActualizado);

                MessageBox.Show("Proveedor actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refrescamos el DataGridView
                CargarProveedores();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar el proveedor: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string filtro = txtNombre.Text.Trim();

                if (string.IsNullOrWhiteSpace(filtro))
                {
                    CargarProveedores(); // Si el campo está vacío, muestra todos
                    return;
                }

                var proveedorRepo = new ProveedorRepository();
                var listaFiltrada = proveedorRepo.BuscarPorNombre(filtro); // Método personalizado

                dgvProveedores.DataSource = listaFiltrada;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar proveedores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
    }
}
