using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Data.Repositories;
using GestionFarmacia.Entities;
using GestionFarmacia.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GestionFarmacia.Forms
{
    public partial class FrmReglasPedido : Form
    {
        private readonly IProductoRepository _productoRepo;
        private readonly IProveedorRepository _proveedorRepo;
        private readonly IReglaPedidoRepository _reglaRepo;
        private int? reglaSeleccionadaId = null;

        public FrmReglasPedido()
        {
            InitializeComponent();
            _productoRepo = new ProductoRepository();
            _proveedorRepo = new ProveedorRepository();
            _reglaRepo = new ReglaPedidoRepository();

            CargarProductos();
            CargarProveedores();
            CargarReglas();
        }

        private void CargarProductos()
        {
            try
            {
                var productos = _productoRepo.ObtenerTodos();
                cmbProducto.DataSource = productos;
                cmbProducto.DisplayMember = "Nombre";
                cmbProducto.ValueMember = "ProductoID";
                cmbProducto.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

        private void CargarProveedores()
        {
            try
            {
                var proveedores = _proveedorRepo.Consultar();
                cmbProveedor.DataSource = proveedores;
                cmbProveedor.DisplayMember = "Nombre";
                cmbProveedor.ValueMember = "ProveedorID";
                cmbProveedor.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

        private void CargarReglas()
        {
            try
            {
                dgvReglas.DataSource = _reglaRepo.ObtenerTodas();
                dgvReglas.ClearSelection();
                reglaSeleccionadaId = null;
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProducto.SelectedItem == null || cmbProveedor.SelectedItem == null)
                {
                    MessageBox.Show("Selecciona un producto y un proveedor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var regla = new ReglaPedido
                {
                    ProductoID = (int)cmbProducto.SelectedValue,
                    ProveedorID = (int)cmbProveedor.SelectedValue,
                    CantidadSugerida = (int)nudCantidadSugerida.Value,
                    Activa = chkActiva.Checked
                };

                bool exito = _reglaRepo.Insertar(regla);
                if (exito)
                {
                    MessageBox.Show("Regla registrada con éxito.");
                    LimpiarFormulario();
                    CargarReglas();
                }
                else
                {
                    MessageBox.Show("No se pudo registrar la regla.");
                }
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (reglaSeleccionadaId == null)
                {
                    MessageBox.Show("Selecciona una regla para actualizar.");
                    return;
                }

                var regla = new ReglaPedido
                {
                    ReglaID = reglaSeleccionadaId.Value,
                    ProductoID = (int)cmbProducto.SelectedValue,
                    ProveedorID = (int)cmbProveedor.SelectedValue,
                    CantidadSugerida = (int)nudCantidadSugerida.Value,
                    Activa = chkActiva.Checked
                };

                bool exito = _reglaRepo.Actualizar(regla);
                if (exito)
                {
                    MessageBox.Show("Regla actualizada.");
                    LimpiarFormulario();
                    CargarReglas();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar la regla.");
                }
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (reglaSeleccionadaId == null)
                {
                    MessageBox.Show("Selecciona una regla para eliminar.");
                    return;
                }

                if (MessageBox.Show("¿Estás seguro de eliminar esta regla?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                bool exito = _reglaRepo.Eliminar(reglaSeleccionadaId.Value);
                if (exito)
                {
                    MessageBox.Show("Regla eliminada.");
                    LimpiarFormulario();
                    CargarReglas();
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar la regla.");
                }
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            cmbProducto.SelectedIndex = -1;
            cmbProveedor.SelectedIndex = -1;
            nudCantidadSugerida.Value = 1;
            chkActiva.Checked = true;
            reglaSeleccionadaId = null;
            dgvReglas.ClearSelection();
        }

        private void dgvReglas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var regla = (ReglaPedido)dgvReglas.Rows[e.RowIndex].DataBoundItem;
                reglaSeleccionadaId = regla.ReglaID;

                cmbProducto.SelectedValue = regla.ProductoID;
                cmbProveedor.SelectedValue = regla.ProveedorID;
                nudCantidadSugerida.Value = regla.CantidadSugerida;
                chkActiva.Checked = regla.Activa;
            }
        }
    }
}
