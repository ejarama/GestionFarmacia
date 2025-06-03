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
           
        }

        private void FrmReportes_Load(object sender, EventArgs e)
        {
            dtpFechaInicio.Value = DateTime.Today;
            dtpFechaFin.Value = DateTime.Today;
        }

      

        private void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            DateTime inicio = dtpFechaInicio.Value.Date;
            DateTime fin = dtpFechaFin.Value.Date.AddDays(1).AddSeconds(-1);
            



            IReporte reporte = ReporteFactory.CrearReporte();
            _reporteDatos = reporte.ObtenerReporteVentas(inicio, fin);

            dgvReporte.DataSource = null;
            dgvReporte.DataSource = _reporteDatos;
            //ConfigurarColumnas();
        }

       
        
    }
}