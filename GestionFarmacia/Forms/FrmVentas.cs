using GestionFarmacia.Data;
using GestionFarmacia.Data.Repositories;
using GestionFarmacia.Entities;
using GestionFarmacia.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace GestionFarmacia.Forms
{
    public partial class FrmVentas : Form
    {
        private List<DetalleVenta> detalleVenta = new List<DetalleVenta>();
        private Usuario _usuario;
        private PrintDocument printDocument = new PrintDocument();
        private decimal totalFactura;

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

                var unitOfWork = new UnitOfWork();
                var resultado = unitOfWork.RegistrarVenta(venta);

                if (resultado.Exito)
                {
                    string mensaje = $"Venta registrada con éxito (ID: {resultado.VentaID}).";

                    if (resultado.PedidoAutomaticoGenerado)
                    {
                        mensaje += "\n\nSe generó automáticamente un pedido a proveedor por bajo stock.";
                    }

                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnRegistrarVenta.Enabled = false;
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
                var promoRepo = new PromocionRepository();
                var promocion = promoRepo.ObtenerPromocionVigentePorProducto(producto.ProductoID, DateTime.Now);

                decimal porcentajeDescuento = promocion?.PorcentajeDescuento ?? 0;

                if (cantidad > producto.CantidadStock)
                {
                    MessageBox.Show("No hay suficiente stock disponible.");
                    return;
                }

                var existente = detalleVenta.FirstOrDefault(d => d.ProductoID == producto.ProductoID);
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
                        PrecioUnitario = producto.Precio,
                        PorcentajeDescuento = porcentajeDescuento,
                        Cantidad = cantidad
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
            dgvDetalleVenta.DataSource = null;
            dgvDetalleVenta.DataSource = detalleVenta;
            ConfigurarColumnasDgvDetalle();

            decimal total = detalleVenta.Sum(d => d.Subtotal);
            lblTotal.Text = $"Total: ${total:F2}";
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
                btnBuscarVenta.Enabled = true;
                btnAgregar.Enabled = true;
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
                var productoRepo = new ProductoRepository();
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

                    detalleVenta = venta.Detalles;

                    lblFecha.Text = $"Fecha: {venta.FechaVenta:dd/MM/yyyy}";
                    lblUsuario.Text = $"Usuario ID: {venta.UsuarioID}";
                    lblTotal.Text = $"Total: ${venta.TotalVenta:F2}";

                    btnRegistrarVenta.Enabled = false;
                    btnAgregar.Enabled = false;

                    ActualizarDetalle();
                }
                catch (Exception ex)
                {
                    ManejadorErrores.Mostrar(ex);
                }
            }
        }

        private void ConfigurarColumnasDgvDetalle()
        {
            dgvDetalleVenta.Columns.Clear();
            dgvDetalleVenta.AutoGenerateColumns = false;

            dgvDetalleVenta.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Producto",
                DataPropertyName = "NombreProducto",
                ReadOnly = true
            });

            dgvDetalleVenta.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Precio Unitario",
                DataPropertyName = "PrecioUnitario",
                ReadOnly = true,
                DefaultCellStyle = { Format = "C2" }
            });

            dgvDetalleVenta.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "% Descuento",
                DataPropertyName = "PorcentajeDescuento",
                ReadOnly = true,
                DefaultCellStyle = { Format = "N2" }
            });

            dgvDetalleVenta.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Cantidad",
                DataPropertyName = "Cantidad",
                ReadOnly = true
            });

            dgvDetalleVenta.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Subtotal",
                DataPropertyName = "Subtotal",
                ReadOnly = true,
                DefaultCellStyle = { Format = "C2" }
            });
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (detalleVenta.Count == 0)
            {
                MessageBox.Show("No hay productos en la venta.");
                return;
            }

            totalFactura = detalleVenta.Sum(d => d.Subtotal);

            PrintPreviewDialog preview = new PrintPreviewDialog();
            printDocument.PrintPage += new PrintPageEventHandler(PrintFactura);
            preview.Document = printDocument;
            preview.ShowDialog();
        }

        private void PrintFactura(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font font = new Font("Arial", 10);
            int startX = 10;
            int startY = 20;
            int offset = 20;

            g.DrawString("FARMACIA SALUD TOTAL", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, startX, startY);
            offset += 40;

            g.DrawString("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), font, Brushes.Black, startX, startY + offset);
            offset += 20;

            g.DrawString("Detalle de Productos:", font, Brushes.Black, startX, startY + offset);
            offset += 20;

            foreach (var d in detalleVenta)
            {
                string linea = $"{d.NombreProducto} x{d.Cantidad} @ {d.PrecioUnitario:C2} = {(d.Cantidad * d.PrecioUnitario):C2}";
                g.DrawString(linea, font, Brushes.Black, startX, startY + offset);
                offset += 20;
            }

            offset += 10;
            g.DrawString($"TOTAL: {totalFactura:C2}", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, startX, startY + offset);
        }
    }
}