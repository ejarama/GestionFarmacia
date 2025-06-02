using GestionFarmacia.Data;
using GestionFarmacia.Data.Repositories;
using GestionFarmacia.Entities;
using GestionFarmacia.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GestionFarmacia.Forms
{
    public partial class FrmReportes : Form
    {
        private List<Venta> _ventas;

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
            string tipoReporte = "Ventas"; 

            var reporte = ReporteFactory.CrearReporte(tipoReporte);
            var datos = reporte.Generar(inicio, fin, filtro);

            _ventas = datos.Cast<Venta>().ToList();

            dgvReporte.DataSource = null;
            dgvReporte.DataSource = _ventas;
            ConfigurarColumnas();
        }

        private void ConfigurarColumnas()
        {
            dgvReporte.Columns.Clear();
            dgvReporte.AutoGenerateColumns = false;

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Venta ID",
                DataPropertyName = "VentaID",
                ReadOnly = true
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Fecha",
                DataPropertyName = "FechaVenta",
                ReadOnly = true,
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            });

            dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Usuario",
                DataPropertyName = "UsuarioID",
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
                HeaderText = "Total",
                DataPropertyName = "TotalVenta",
                ReadOnly = true,
                DefaultCellStyle = { Format = "C2" }
            });
        }
    }
}