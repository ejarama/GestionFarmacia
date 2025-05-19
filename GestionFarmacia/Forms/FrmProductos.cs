using Data.Repositories;
using GestionFarmacia.Data;
using GestionFarmacia.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionFarmacia.Forms
{
    public partial class FrmProductos : Form
    {
        private readonly IProductoRepository _repo;
        private int productoSeleccionadoId = -1;

        public FrmProductos()
        {
            InitializeComponent();
            _repo = new ProductoRepository();
            CargarProductos();
        }

        private void CargarProductos()
        {
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = _repo.ObtenerTodos();
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            numPrecio.Value = 0;
            numMinimo.Value = 0;
            productoSeleccionadoId = -1;
        }

        private Producto ObtenerProductoDesdeFormulario()
        {
            return new Producto
            {
                ProductoID = productoSeleccionadoId,
                Nombre = txtNombre.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                Precio = numPrecio.Value,
                CantidadStock = (int)numMinimo.Value
            };
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Producto p = ObtenerProductoDesdeFormulario();

            if (string.IsNullOrEmpty(p.Nombre))
            {
                MessageBox.Show("El nombre es obligatorio.");
                return;
            }

            _repo.Insertar(p);
            CargarProductos();
            LimpiarCampos();
            MessageBox.Show("Producto guardado correctamente.");
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (productoSeleccionadoId == -1)
            {
                MessageBox.Show("Debe seleccionar un producto para modificar.");
                return;
            }

            Producto p = ObtenerProductoDesdeFormulario();
            _repo.Actualizar(p);
            CargarProductos();
            LimpiarCampos();
            MessageBox.Show("Producto actualizado.");
        }


        private void FrmProductos_Load(object sender, EventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            {
                if (productoSeleccionadoId == -1)
                {
                    MessageBox.Show("Seleccione un producto para eliminar.");
                    return;
                }

                var confirm = MessageBox.Show("¿Está seguro de eliminar el producto?", "Confirmar", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    _repo.Eliminar(productoSeleccionadoId);
                    CargarProductos();
                    LimpiarCampos();
                    MessageBox.Show("Producto eliminado.");
                }
            }

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow != null)
            {
                var producto = (Producto)dgvProductos.CurrentRow.DataBoundItem;

                productoSeleccionadoId = producto.ProductoID;
                txtNombre.Text = producto.Nombre;
                txtDescripcion.Text = producto.Descripcion;
                numPrecio.Value = producto.Precio;
                numMinimo.Value = producto.CantidadStock;
            }
        }

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            Producto p = ObtenerProductoDesdeFormulario();

            if (string.IsNullOrEmpty(p.Nombre))
            {
                MessageBox.Show("El nombre es obligatorio.");
                return;
            }

            _repo.Insertar(p);
            CargarProductos();
            LimpiarCampos();
            MessageBox.Show("Producto guardado correctamente.");
        }

        private void btnModificar_Click_1(object sender, EventArgs e)
        {
            if (productoSeleccionadoId == -1)
            {
                MessageBox.Show("Debe seleccionar un producto para modificar.");
                return;
            }

            Producto p = ObtenerProductoDesdeFormulario();
            _repo.Actualizar(p);
            CargarProductos();
            LimpiarCampos();
            MessageBox.Show("Producto actualizado.");
        }

    }
}
