
namespace GestionFarmacia.Forms
{
    partial class FrmBuscarVenta
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
            this.lblBuscarVenta = new System.Windows.Forms.Label();
            this.txtVentaID = new System.Windows.Forms.TextBox();
            this.lblBuscarVentaTitulo = new System.Windows.Forms.Label();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblBuscarVenta
            // 
            this.lblBuscarVenta.AutoSize = true;
            this.lblBuscarVenta.Location = new System.Drawing.Point(79, 65);
            this.lblBuscarVenta.Name = "lblBuscarVenta";
            this.lblBuscarVenta.Size = new System.Drawing.Size(123, 13);
            this.lblBuscarVenta.TabIndex = 0;
            this.lblBuscarVenta.Text = "Ingrese el ID de la venta";
            // 
            // txtVentaID
            // 
            this.txtVentaID.Location = new System.Drawing.Point(82, 90);
            this.txtVentaID.Name = "txtVentaID";
            this.txtVentaID.Size = new System.Drawing.Size(120, 20);
            this.txtVentaID.TabIndex = 1;
            // 
            // lblBuscarVentaTitulo
            // 
            this.lblBuscarVentaTitulo.AutoSize = true;
            this.lblBuscarVentaTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuscarVentaTitulo.Location = new System.Drawing.Point(59, 20);
            this.lblBuscarVentaTitulo.Name = "lblBuscarVentaTitulo";
            this.lblBuscarVentaTitulo.Size = new System.Drawing.Size(154, 20);
            this.lblBuscarVentaTitulo.TabIndex = 2;
            this.lblBuscarVentaTitulo.Text = "Buscar Venta por ID";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAceptar.Location = new System.Drawing.Point(47, 134);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(81, 32);
            this.btnAceptar.TabIndex = 3;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Location = new System.Drawing.Point(157, 134);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(81, 32);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // FrmBuscarVenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 192);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.lblBuscarVentaTitulo);
            this.Controls.Add(this.txtVentaID);
            this.Controls.Add(this.lblBuscarVenta);
            this.Name = "FrmBuscarVenta";
            this.Text = "FrmBuscarVenta";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBuscarVenta;
        private System.Windows.Forms.TextBox txtVentaID;
        private System.Windows.Forms.Label lblBuscarVentaTitulo;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}