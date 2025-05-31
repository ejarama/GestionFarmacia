using GestionFarmacia.Data;
using GestionFarmacia.Data.Repositories;
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
    public partial class FrmVentas : Form
    {
        private List<DetalleVenta> detalleVenta = new List<DetalleVenta>();
        private Usuario _usuario;

        public FrmVentas(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;

            lblUsuario.Text = $"Usuario: {_usuario.NombreUsuario}";
            lblFecha.Text = $"Fecha: {DateTime.Now:dd/MM/yyyy}";

            CargarProductos();
        }


        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            try
            {
                if (detalleVenta.Count == 0)
                {
                    MessageBox.Show("No hay productos en la venta.");
                    return;
                }

                Venta venta = VentaFactory.CrearVenta(_usuario.UsuarioID, detalleVenta);

                var unitOfWork = new UnitOfWork(); // Implementaremos después
                bool exito = unitOfWork.RegistrarVenta(venta);

                if (exito)
                {
                    MessageBox.Show("Venta registrada con éxito.");
                    btnRegistrarVenta.Enabled = false ;
                    btnBuscarVenta.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Ocurrió un error al registrar la venta.");
                }
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProducto.SelectedItem == null)
                {
                    MessageBox.Show("Selecciona un producto.");
                    return;
                }

                if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
                {
                    MessageBox.Show("Cantidad inválida.");
                    return;
                }

                var producto = (Producto)cmbProducto.SelectedItem;

                if (cantidad > producto.CantidadStock)
                {
                    MessageBox.Show("No hay suficiente stock disponible.");
                    return;
                }

                var existente = detalleVenta.Find(d => d.ProductoID == producto.ProductoID);
                if (existente != null)
                {
                    existente.Cantidad += cantidad;
                }
                else
                {
                    detalleVenta.Add(new DetalleVenta
                    {
                        ProductoID = producto.ProductoID,
                        NombreProducto = producto.Nombre,
                        Cantidad = cantidad,
                        PrecioUnitario = producto.Precio
                    });
                }

                ActualizarDetalle();
                txtCantidad.Clear();
                cmbProducto.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }


        private void ActualizarDetalle()
        {
            try
            {
                dgvDetalleVenta.DataSource = null;
                dgvDetalleVenta.DataSource = detalleVenta;

                decimal total = 0;
                foreach (var item in detalleVenta)
                    total += item.Subtotal;

                lblTotal.Text = $"Total: ${total:F2}";
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }


        private void LimpiarFormulario()
        {
            try
            {
                detalleVenta.Clear();
                ActualizarDetalle();
                cmbProducto.SelectedIndex = -1;
                txtCantidad.Clear();
                lblTotal.Text = "Total: $0.00";
                lblUsuario.Text = $"Usuario: {_usuario.NombreUsuario}";
                lblFecha.Text = $"Fecha: {DateTime.Now:dd/MM/yyyy}";
                btnRegistrarVenta.Enabled = true;
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }


        private void dgvDetalleVenta_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvDetalleVenta.Columns[e.ColumnIndex].Name == "Eliminar")
                {
                    int productoID = (int)dgvDetalleVenta.Rows[e.RowIndex].Cells["ProductoID"].Value;
                    detalleVenta.RemoveAll(d => d.ProductoID == productoID);
                    ActualizarDetalle();
                }
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
                var productoRepo = new ProductoRepository(); // o inyectado si prefieres
                var productos = productoRepo.ObtenerTodos();

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

        private void btnBuscarVenta_Click(object sender, EventArgs e)
        {
            FrmBuscarVenta buscar = new FrmBuscarVenta();
            if (buscar.ShowDialog() == DialogResult.OK && buscar.VentaIDSeleccionada.HasValue)
            {
                try
                {
                    var venta = new VentaRepository().ObtenerPorId(buscar.VentaIDSeleccionada.Value);
                    if (venta == null)
                    {
                        MessageBox.Show("Venta no encontrada.");
                        return;
                    }

                    // Bloqueamos el botón de registrar
                    btnRegistrarVenta.Enabled = false;

                    // Mostramos datos de la venta consultada
                    detalleVenta = venta.Detalles;
                    ActualizarDetalle();

                    lblFecha.Text = $"Fecha: {venta.FechaVenta:dd/MM/yyyy}";
                    lblUsuario.Text = $"Usuario ID: {venta.UsuarioID}";
                    lblTotal.Text = $"Total: ${venta.TotalVenta:F2}";

                    MessageBox.Show("Venta cargada correctamente.");
                }
                catch (Exception ex)
                {
                    ManejadorErrores.Mostrar(ex);
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }
    }
}
