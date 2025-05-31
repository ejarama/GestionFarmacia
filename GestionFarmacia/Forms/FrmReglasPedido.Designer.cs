
using System.Windows.Forms;

namespace GestionFarmacia.Forms
{
    partial class FrmReglasPedido
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbProducto = new System.Windows.Forms.ComboBox();
            this.cmbProveedor = new System.Windows.Forms.ComboBox();
            this.nudCantidadSugerida = new System.Windows.Forms.NumericUpDown();
            this.chkActiva = new System.Windows.Forms.CheckBox();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.dgvReglas = new System.Windows.Forms.DataGridView();
            this.lblProducto = new System.Windows.Forms.Label();
            this.lblProveedor = new System.Windows.Forms.Label();
            this.lblCantidad = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.nudCantidadSugerida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReglas)).BeginInit();
            this.SuspendLayout();

            // 
            // cmbProducto
            // 
            this.cmbProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProducto.FormattingEnabled = true;
            this.cmbProducto.Location = new System.Drawing.Point(120, 20);
            this.cmbProducto.Name = "cmbProducto";
            this.cmbProducto.Size = new System.Drawing.Size(200, 21);

            // 
            // cmbProveedor
            // 
            this.cmbProveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProveedor.FormattingEnabled = true;
            this.cmbProveedor.Location = new System.Drawing.Point(120, 60);
            this.cmbProveedor.Name = "cmbProveedor";
            this.cmbProveedor.Size = new System.Drawing.Size(200, 21);

            // 
            // nudCantidadSugerida
            // 
            this.nudCantidadSugerida.Location = new System.Drawing.Point(120, 100);
            this.nudCantidadSugerida.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.nudCantidadSugerida.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudCantidadSugerida.Name = "nudCantidadSugerida";
            this.nudCantidadSugerida.Size = new System.Drawing.Size(200, 20);
            this.nudCantidadSugerida.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // 
            // chkActiva
            // 
            this.chkActiva.AutoSize = true;
            this.chkActiva.Checked = true;
            this.chkActiva.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActiva.Location = new System.Drawing.Point(120, 135);
            this.chkActiva.Name = "chkActiva";
            this.chkActiva.Size = new System.Drawing.Size(56, 17);
            this.chkActiva.Text = "Activa";
            this.chkActiva.UseVisualStyleBackColor = true;

            // 
            // btnRegistrar
            // 
            this.btnRegistrar.Location = new System.Drawing.Point(350, 20);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(100, 30);
            this.btnRegistrar.Text = "Registrar";
            this.btnRegistrar.UseVisualStyleBackColor = true;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);

            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(350, 60);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(100, 30);
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);

            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(350, 100);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(100, 30);
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(350, 140);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(100, 30);
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);

            // 
            // dgvReglas
            // 
            this.dgvReglas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReglas.Location = new System.Drawing.Point(20, 190);
            this.dgvReglas.Name = "dgvReglas";
            this.dgvReglas.Size = new System.Drawing.Size(580, 200);
            this.dgvReglas.CellClick += new DataGridViewCellEventHandler(this.dgvReglas_CellClick);

            // 
            // lblProducto
            // 
            this.lblProducto.AutoSize = true;
            this.lblProducto.Location = new System.Drawing.Point(20, 23);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(53, 13);
            this.lblProducto.Text = "Producto:";

            // 
            // lblProveedor
            // 
            this.lblProveedor.AutoSize = true;
            this.lblProveedor.Location = new System.Drawing.Point(20, 63);
            this.lblProveedor.Name = "lblProveedor";
            this.lblProveedor.Size = new System.Drawing.Size(59, 13);
            this.lblProveedor.Text = "Proveedor:";

            // 
            // lblCantidad
            // 
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Location = new System.Drawing.Point(20, 102);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(96, 13);
            this.lblCantidad.Text = "Cantidad sugerida:";

            // 
            // FrmReglasPedido
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 410);
            this.Controls.Add(this.lblCantidad);
            this.Controls.Add(this.lblProveedor);
            this.Controls.Add(this.lblProducto);
            this.Controls.Add(this.cmbProducto);
            this.Controls.Add(this.cmbProveedor);
            this.Controls.Add(this.nudCantidadSugerida);
            this.Controls.Add(this.chkActiva);
            this.Controls.Add(this.btnRegistrar);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.dgvReglas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmReglasPedido";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administrar Reglas de Pedido";

            ((System.ComponentModel.ISupportInitialize)(this.nudCantidadSugerida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReglas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }


        private System.Windows.Forms.ComboBox cmbProducto;
        private System.Windows.Forms.ComboBox cmbProveedor;
        private System.Windows.Forms.NumericUpDown nudCantidadSugerida;
        private System.Windows.Forms.CheckBox chkActiva;

        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnLimpiar;

        private System.Windows.Forms.DataGridView dgvReglas;

        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.Label lblProveedor;
        private System.Windows.Forms.Label lblCantidad;

    }
    #endregion
}