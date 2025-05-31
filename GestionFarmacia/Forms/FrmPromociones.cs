using GestionFarmacia.Data.Interfaces;
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
    public partial class FrmPromociones : Form
    {

        private IPromocionRepository _repoPromocion;
        private IProductoRepository _repoProducto;
        private int? _promocionIdSeleccionada = null;

        public FrmPromociones()
        {
            InitializeComponent();
            _repoPromocion = new PromocionRepository();
            _repoProducto = new ProductoRepository();

            CargarProductos();
            CargarPromociones();
        }

        private void CargarProductos()
        {
            var productos = _repoProducto.ObtenerTodos();
            cmbProducto.DataSource = productos;
            cmbProducto.DisplayMember = "Nombre";
            cmbProducto.ValueMember = "ProductoID";
            cmbProducto.SelectedIndex = -1;
        }

        private void CargarPromociones()
        {
            var lista = _repoPromocion.ObtenerTodas();
            dgvPromociones.DataSource = lista;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProducto.SelectedItem == null || !decimal.TryParse(txtDescuento.Text, out decimal descuento))
                {
                    MessageBox.Show("Datos inválidos.");
                    return;
                }

                var promo = new Promocion
                {
                    ProductoID = (int)cmbProducto.SelectedValue,
                    FechaInicio = dtInicio.Value.Date,
                    FechaFin = dtFin.Value.Date,
                    PorcentajeDescuento = descuento
                };

                _repoPromocion.Insertar(promo);
                MessageBox.Show("Promoción registrada.");
                LimpiarFormulario();
                CargarPromociones();
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_promocionIdSeleccionada == null)
            {
                MessageBox.Show("Selecciona una promoción.");
                return;
            }

            try
            {
                var promo = new Promocion
                {
                    PromocionID = _promocionIdSeleccionada.Value,
                    ProductoID = (int)cmbProducto.SelectedValue,
                    FechaInicio = dtInicio.Value.Date,
                    FechaFin = dtFin.Value.Date,
                    PorcentajeDescuento = decimal.Parse(txtDescuento.Text)
                };

                _repoPromocion.Actualizar(promo);
                MessageBox.Show("Promoción actualizada.");
                LimpiarFormulario();
                CargarPromociones();
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_promocionIdSeleccionada == null)
            {
                MessageBox.Show("Selecciona una promoción.");
                return;
            }

            try
            {
                _repoPromocion.Eliminar(_promocionIdSeleccionada.Value);
                MessageBox.Show("Promoción eliminada.");
                LimpiarFormulario();
                CargarPromociones();
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }
        private void dgvPromociones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dgvPromociones.Rows[e.RowIndex].DataBoundItem as Promocion;
                if (fila != null)
                {
                    _promocionIdSeleccionada = fila.PromocionID;
                    cmbProducto.SelectedValue = fila.ProductoID;
                    dtInicio.Value = fila.FechaInicio;
                    dtFin.Value = fila.FechaFin;
                    txtDescuento.Text = fila.PorcentajeDescuento.ToString();
                }
            }
        }

        private void LimpiarFormulario()
        {
            cmbProducto.SelectedIndex = -1;
            dtInicio.Value = DateTime.Today;
            dtFin.Value = DateTime.Today;
            txtDescuento.Clear();
            _promocionIdSeleccionada = null;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }
    }
}
