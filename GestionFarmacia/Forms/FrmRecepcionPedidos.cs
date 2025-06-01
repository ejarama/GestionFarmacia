using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Entities;
using GestionFarmacia.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GestionFarmacia.Forms
{
    public partial class FrmRecepcionPedidos : Form
    {
        private readonly IPedidoRepository _pedidoRepo;
        private Pedido pedidoSeleccionado;

        public FrmRecepcionPedidos(IPedidoRepository pedidoRepo)
        {
            InitializeComponent();
            _pedidoRepo = pedidoRepo;
            CargarPedidosPendientes();
        }

        private void FrmRecepcionPedidos_Load(object sender, EventArgs e)
        {
            CargarPedidosPendientes();
        }

        private void CargarPedidosPendientes()
        {
            try
            {
                var pedidos = _pedidoRepo.ObtenerPedidosPendientes();
                dgvPedidosPendientes.DataSource = pedidos;

                if (dgvPedidosPendientes.Columns.Contains("ProveedorID"))
                    dgvPedidosPendientes.Columns["ProveedorID"].Visible = false;

                if (dgvPedidosPendientes.Columns.Contains("Detalles"))
                    dgvPedidosPendientes.Columns["Detalles"].Visible = false;
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

        private void dgvPedidosPendientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                pedidoSeleccionado = (Pedido)dgvPedidosPendientes.Rows[e.RowIndex].DataBoundItem;

                var detalles = _pedidoRepo.ObtenerDetallePedido(pedidoSeleccionado.PedidoID);
                dgvDetallePedido.DataSource = null;
                dgvDetallePedido.DataSource = detalles;

                // Formato
                if (dgvDetallePedido.Columns.Contains("ProductoID"))
                    dgvDetallePedido.Columns["ProductoID"].Visible = false;

                if (dgvDetallePedido.Columns.Contains("DetallePedidoID"))
                    dgvDetallePedido.Columns["DetallePedidoID"].Visible = false;

                dgvDetallePedido.Columns["NombreProducto"].HeaderText = "Producto";
                dgvDetallePedido.Columns["CantidadSolicitada"].HeaderText = "Solicitado";
                dgvDetallePedido.Columns["CantidadRecibida"].HeaderText = "Recibido";
            }
        }

        private void btnRegistrarRecepcion_Click(object sender, EventArgs e)
        {
            try
            {
                if (pedidoSeleccionado == null)
                {
                    MessageBox.Show("Debe seleccionar un pedido primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar cantidades recibidas
                if (!ValidadorEntradas.ValidarCantidadRecibida(dgvDetallePedido, "CantidadRecibida"))
                {
                    MessageBox.Show("Verifica que todas las cantidades recibidas sean válidas y no negativas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Construcción de detalles
                var detallesActualizados = new List<DetallePedido>();
                bool cantidadCeroConfirmada = false;

                foreach (DataGridViewRow row in dgvDetallePedido.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (!int.TryParse(row.Cells["CantidadRecibida"].Value?.ToString(), out int cantidadRecibida))
                        cantidadRecibida = 0; 

                    int productoId = Convert.ToInt32(row.Cells["ProductoID"].Value);
                    int detallePedidoId = Convert.ToInt32(row.Cells["DetallePedidoID"].Value);
                    int cantidadSolicitada = Convert.ToInt32(row.Cells["CantidadSolicitada"].Value);
                    string nombreProducto = row.Cells["NombreProducto"].Value?.ToString();


                    if (cantidadRecibida == 0 && !cantidadCeroConfirmada)
                    {
                        var respuesta = MessageBox.Show(
                            $"La cantidad recibida para el producto '{nombreProducto}' es 0.\n¿Deseas continuar así?",
                            "Cantidad recibida en cero",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );

                        if (respuesta == DialogResult.No)
                           return;

                        cantidadCeroConfirmada = true; // Para no preguntar por cada uno si hay varios con 0
                    }

                    detallesActualizados.Add(new DetallePedido
                    {
                        DetallePedidoID = detallePedidoId,
                        PedidoID = pedidoSeleccionado.PedidoID,
                        ProductoID = productoId,
                        CantidadRecibida = cantidadRecibida,
                        CantidadSolicitada = cantidadSolicitada

                    });
                }

                if (!detallesActualizados.Any(d => d.CantidadRecibida > 0))
                {
                    MessageBox.Show("Debe registrar al menos un producto con cantidad recibida mayor a 0.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("¿Confirmas la recepción de este pedido?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                bool exito = _pedidoRepo.RegistrarRecepcion(pedidoSeleccionado.PedidoID, detallesActualizados);

                if (exito)
                {
                    MessageBox.Show("Recepción registrada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarPedidosPendientes();
                    dgvDetallePedido.DataSource = null;
                    pedidoSeleccionado = null;
                }
                else
                {
                    MessageBox.Show("No se pudo registrar la recepción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarPedidosPendientes();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
