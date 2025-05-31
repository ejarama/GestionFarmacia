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
    public partial  class FrmBuscarVenta : Form
    {
        public int? VentaIDSeleccionada { get; private set; }

        public FrmBuscarVenta()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtVentaID.Text, out int ventaId))
            {
                VentaIDSeleccionada = ventaId;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Ingrese un ID válido.");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

      
    }

}
