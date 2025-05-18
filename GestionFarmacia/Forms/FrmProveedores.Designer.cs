
namespace GestionFarmacia.Forms
{
    partial class FrmProveedores
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
            this.lblProveedores = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblTelefono = new System.Windows.Forms.Label();
            this.lblProductos = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.clbProductos = new System.Windows.Forms.CheckedListBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblProveedores
            // 
            this.lblProveedores.AutoSize = true;
            this.lblProveedores.Location = new System.Drawing.Point(308, 38);
            this.lblProveedores.Name = "lblProveedores";
            this.lblProveedores.Size = new System.Drawing.Size(162, 16);
            this.lblProveedores.TabIndex = 0;
            this.lblProveedores.Text = " Registro de Proveedores";
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(98, 92);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(144, 16);
            this.lblNombre.TabIndex = 1;
            this.lblNombre.Text = "Nombre del proveedor";
            // 
            // lblTelefono
            // 
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Location = new System.Drawing.Point(108, 146);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new System.Drawing.Size(134, 16);
            this.lblTelefono.TabIndex = 2;
            this.lblTelefono.Text = "Teléfono de contacto";
            // 
            // lblProductos
            // 
            this.lblProductos.AutoSize = true;
            this.lblProductos.Location = new System.Drawing.Point(98, 200);
            this.lblProductos.Name = "lblProductos";
            this.lblProductos.Size = new System.Drawing.Size(157, 16);
            this.lblProductos.TabIndex = 3;
            this.lblProductos.Text = "Productos que suministra";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(297, 92);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(100, 22);
            this.txtNombre.TabIndex = 4;
            // 
            // txtTelefono
            // 
            this.txtTelefono.Location = new System.Drawing.Point(297, 146);
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(100, 22);
            this.txtTelefono.TabIndex = 5;
            // 
            // clbProductos
            // 
            this.clbProductos.FormattingEnabled = true;
            this.clbProductos.Location = new System.Drawing.Point(297, 200);
            this.clbProductos.Name = "clbProductos";
            this.clbProductos.Size = new System.Drawing.Size(157, 38);
            this.clbProductos.TabIndex = 6;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(124, 286);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(75, 23);
            this.btnGuardar.TabIndex = 7;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnEditar
            // 
            this.btnEditar.Location = new System.Drawing.Point(311, 286);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(75, 23);
            this.btnEditar.TabIndex = 8;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(464, 286);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(75, 23);
            this.btnEliminar.TabIndex = 9;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            // 
            // FrmProveedores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.clbProductos);
            this.Controls.Add(this.txtTelefono);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblProductos);
            this.Controls.Add(this.lblTelefono);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.lblProveedores);
            this.Name = "FrmProveedores";
            this.Text = "FrmProveedores";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProveedores;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblTelefono;
        private System.Windows.Forms.Label lblProductos;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.CheckedListBox clbProductos;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
    }
}