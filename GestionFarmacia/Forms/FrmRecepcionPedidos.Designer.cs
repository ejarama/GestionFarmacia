
namespace GestionFarmacia.Forms
{
    partial class FrmRecepcionPedidos
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.dgvPedidosPendientes = new System.Windows.Forms.DataGridView();
            this.dgvDetallePedido = new System.Windows.Forms.DataGridView();
            this.btnRegistrarRecepcion = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidosPendientes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetallePedido)).BeginInit();
            this.SuspendLayout();

            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(290, 25);
            this.lblTitulo.Text = "Recepción de Pedidos a Proveedores";

            // 
            // dgvPedidosPendientes
            // 
            this.dgvPedidosPendientes.AllowUserToAddRows = false;
            this.dgvPedidosPendientes.AllowUserToDeleteRows = false;
            this.dgvPedidosPendientes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPedidosPendientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPedidosPendientes.Location = new System.Drawing.Point(20, 55);
            this.dgvPedidosPendientes.MultiSelect = false;
            this.dgvPedidosPendientes.Name = "dgvPedidosPendientes";
            this.dgvPedidosPendientes.ReadOnly = true;
            this.dgvPedidosPendientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPedidosPendientes.Size = new System.Drawing.Size(740, 150);
            this.dgvPedidosPendientes.TabIndex = 0;
            this.dgvPedidosPendientes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPedidosPendientes_CellClick);

            // 
            // dgvDetallePedido
            // 
            this.dgvDetallePedido.AllowUserToAddRows = false;
            this.dgvDetallePedido.AllowUserToDeleteRows = false;
            this.dgvDetallePedido.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetallePedido.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetallePedido.Location = new System.Drawing.Point(20, 230);
            this.dgvDetallePedido.Name = "dgvDetallePedido";
            this.dgvDetallePedido.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetallePedido.Size = new System.Drawing.Size(740, 180);
            this.dgvDetallePedido.TabIndex = 1;

            // 
            // btnRegistrarRecepcion
            // 
            this.btnRegistrarRecepcion.Location = new System.Drawing.Point(20, 430);
            this.btnRegistrarRecepcion.Name = "btnRegistrarRecepcion";
            this.btnRegistrarRecepcion.Size = new System.Drawing.Size(160, 30);
            this.btnRegistrarRecepcion.Text = "Registrar Recepción";
            this.btnRegistrarRecepcion.UseVisualStyleBackColor = true;
            this.btnRegistrarRecepcion.Click += new System.EventHandler(this.btnRegistrarRecepcion_Click);

            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(200, 430);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(120, 30);
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);

            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(640, 430);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(120, 30);
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);

            // 
            // FrmRecepcionPedidos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 481);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.dgvPedidosPendientes);
            this.Controls.Add(this.dgvDetallePedido);
            this.Controls.Add(this.btnRegistrarRecepcion);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.btnCerrar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmRecepcionPedidos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recepción de Pedidos";

            ((System.ComponentModel.ISupportInitialize)(this.dgvPedidosPendientes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetallePedido)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvPedidosPendientes;
        private System.Windows.Forms.DataGridView dgvDetallePedido;
        private System.Windows.Forms.Button btnRegistrarRecepcion;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnCerrar;

       #endregion
    }
}