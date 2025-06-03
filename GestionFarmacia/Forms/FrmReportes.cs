using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Forms
{
    public partial class FrmReportes : Form
    {
        private List<ReporteVentas> _reporteDatos;

        public FrmReportes()
        {
            InitializeComponent();
            CargarFiltroEntrega();
        }

        private void FrmReportes_Load(object sender, EventArgs e)
        {
            dtpFechaInicio.Value = DateTime.Today;
            dtpFechaFin.Value = DateTime.Today;
        }

        private void CargarFiltroEntrega()
        {
            cmbFiltro.Items.Clear();
            cmbFiltro.Items.Add("Todos");
            cmbFiltro.Items.Add("Normal");
            cmbFiltro.Items.Add("Express");
            cmbFiltro.SelectedIndex = 0;
        }

        private void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            DateTime inicio = dtpFechaInicio.Value.Date;
            DateTime fin = dtpFechaFin.Value.Date.AddDays(1).AddSeconds(-1);
            string filtro = cmbFiltro.SelectedItem?.ToString();



            IReporte reporte = ReporteFactory.CrearReporte();
            _reporteDatos = reporte.ObtenerReporteVentas(inicio, fin);

            dgvReporte.DataSource = null;
            dgvReporte.DataSource = _reporteDatos;
            ConfigurarColumnas();
        }

        private void ConfigurarColumnas()
        {
            dgvReporte.Columns.Clear();
            dgvReporte.AutoGenerateColumns = false;

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Fecha Venta",
                DataPropertyName = "FechaVenta",
                ReadOnly = true,
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Producto ID",
                DataPropertyName = "ProductoID",
                ReadOnly = true
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Producto",
                DataPropertyName = "ProductoNombre",
                ReadOnly = true
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Cantidad Vendida",
                DataPropertyName = "Cantidad",
                ReadOnly = true
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Precio Unitario",
                DataPropertyName = "PrecioUnitario",
                ReadOnly = true,
                DefaultCellStyle = { Format = "C2" }
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Stock Actual",
                DataPropertyName = "StockActual",
                ReadOnly = true
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tipo Entrega",
                DataPropertyName = "TipoEntrega",
                ReadOnly = true
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Total Parcial",
                DataPropertyName = "TotalParcial",
                ReadOnly = true,
                DefaultCellStyle = { Format = "C2" }
            });
        }

        
    }
}