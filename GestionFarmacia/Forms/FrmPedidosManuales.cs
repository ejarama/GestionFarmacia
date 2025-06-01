using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Entities;
using GestionFarmacia.Utils;
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
    public partial class FrmPedidosManuales : Form
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IProductoRepository _productoRepo;
        private readonly IProveedorRepository _proveedorRepo;
        private List<DetallePedido> detalles = new List<DetallePedido>();
        public FrmPedidosManuales(
            IPedidoRepository pedidoRepo,
            IProductoRepository productoRepo,
            IProveedorRepository proveedorRepo)
        {
            InitializeComponent();
            _pedidoRepo = pedidoRepo;
            _productoRepo = productoRepo;
            _proveedorRepo = proveedorRepo;

            CargarProveedores();
            CargarProductos();

            btnRegistrarPedido.Enabled = false;
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
        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProducto.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar un producto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (nudCantidad.Value <= 0)
                {
                    MessageBox.Show("La cantidad debe ser mayor a cero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var producto = (Producto)cmbProducto.SelectedItem;
                int cantidad = (int)nudCantidad.Value;

                var existente = detalles.FirstOrDefault(d => d.ProductoID == producto.ProductoID);
                if (existente != null)
                {
                    existente.CantidadSolicitada += cantidad;
                }
                else
                {
                    detalles.Add(new DetallePedido
                    {
                        ProductoID = producto.ProductoID,
                        NombreProducto = producto.Nombre,
                        CantidadSolicitada = cantidad
                    });
                }

                ActualizarGridDetalle();
                cmbProducto.SelectedIndex = -1;
                nudCantidad.Value = 1;
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }


        }

        private void ActualizarGridDetalle()
        {
            dgvDetallePedidoManual.DataSource = null;
            dgvDetallePedidoManual.DataSource = detalles;

            if (dgvDetallePedidoManual.Columns["ProductoID"] != null)
                dgvDetallePedidoManual.Columns["ProductoID"].Visible = false;

            if (dgvDetallePedidoManual.Columns["DetallePedidoID"] != null)
                dgvDetallePedidoManual.Columns["DetallePedidoID"].Visible = false;

            if (dgvDetallePedidoManual.Columns["CantidadRecibida"] != null)
                dgvDetallePedidoManual.Columns["CantidadRecibida"].Visible = false;

            dgvDetallePedidoManual.Columns["NombreProducto"].HeaderText = "Producto";
            dgvDetallePedidoManual.Columns["CantidadSolicitada"].HeaderText = "Cantidad";

            if (dgvDetallePedidoManual.Columns.Contains("ProductoID"))
                dgvDetallePedidoManual.Columns["ProductoID"].Visible = false;

            // Habilitar o deshabilitar el botón según haya productos
            btnRegistrarPedido.Enabled = detalles.Any();
        }

        private void btnRegistrarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProveedor.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar un proveedor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!detalles.Any())
                {
                    MessageBox.Show("Debe agregar al menos un producto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var proveedor = (Proveedor)cmbProveedor.SelectedItem;

                Pedido pedido = new Pedido
                {
                    ProveedorID = proveedor.ProveedorID,
                    FechaPedido = DateTime.Now,
                    Detalles = detalles
                };

                int pedidoId = _pedidoRepo.CrearPedido(pedido);

                foreach (var detalle in detalles)
                {
                    _pedidoRepo.InsertarDetallePedido(pedidoId, detalle);
                }

                MessageBox.Show($"Pedido registrado con éxito. ID: {pedidoId}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                detalles.Clear();
                ActualizarGridDetalle();
                cmbProveedor.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

        private void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDetallePedidoManual.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Selecciona un producto para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var row = dgvDetallePedidoManual.SelectedRows[0];
                int productoId = Convert.ToInt32(row.Cells["ProductoID"].Value);

                // Buscar y eliminar el producto de la lista
                var item = detalles.FirstOrDefault(d => d.ProductoID == productoId);
                if (item != null)
                {
                    detalles.Remove(item);
                    ActualizarGridDetalle(); // Vuelve a cargar la lista en el DataGridView
                }
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                // Limpiar lista
                detalles.Clear();

                // Limpiar campos
                cmbProveedor.SelectedIndex = -1;
                cmbProducto.SelectedIndex = -1;
                nudCantidad.Value = 1;

                // Limpiar grid
                ActualizarGridDetalle();
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

    }
}
