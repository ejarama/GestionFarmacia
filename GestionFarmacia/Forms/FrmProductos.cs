using System;
using System.Windows.Forms;
using GestionFarmacia.Entities;
using GestionFarmacia.Services;
using GestionFarmacia.Utils;
using GestionFarmacia.Data.Interfaces;
using System.Collections.Generic;
using GestionFarmacia.Utils;
using GestionFarmacia.Services.Interfaces;

namespace GestionFarmacia.Forms
{
    public partial class FrmProductos : Form
    {
        private readonly IProductoService _productoService;
        private readonly IProveedorRepository _proveedorRepo; 
        private int? productoSeleccionadoId = null;

        public FrmProductos(IProductoService productoService, IProveedorRepository proveedorRepo)
        {
            InitializeComponent();
            _productoService = productoService;
            _proveedorRepo = proveedorRepo;

            CargarProductos();
            CargarProveedores();
        }


        private void CargarProductos()
        {
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = _productoService.ObtenerProductos();
            dgvProductos.ClearSelection();
        }

        private void CargarProveedores()
        {
            var proveedores = _proveedorRepo.Consultar();
            clbProveedores.Items.Clear();

            foreach (var p in proveedores)
                clbProveedores.Items.Add(p); // Agrega solo el objeto

            clbProveedores.DisplayMember = "Nombre"; // Asegura que se muestre el nombre
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                var producto = ObtenerProductoFormulario();

                if (!ValidadorEntradas.ValidarProducto(producto, out string mensaje))
                {
                    MessageBox.Show(mensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                _productoService.RegistrarProducto(producto);

                var nuevo = _productoService.ObtenerPorNombre(producto.Nombre);
                var proveedores = ObtenerProveedoresSeleccionados();
                if (proveedores.Count == 0)
                {
                    MessageBox.Show("Debes seleccionar al menos un proveedor.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                _productoService.AsignarProveedoresAProducto(nuevo.ProductoID, proveedores);

                MessageBox.Show("Producto registrado correctamente.");
                LimpiarFormulario();
                CargarProductos();

            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!productoSeleccionadoId.HasValue)
            {
                MessageBox.Show("Selecciona un producto para actualizar.");
                return;
            }

            try
            {
                var producto = ObtenerProductoFormulario();
                producto.ProductoID = productoSeleccionadoId.Value;

                if (!ValidadorEntradas.ValidarProducto(producto, out string mensaje))
                {
                    MessageBox.Show(mensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                _productoService.ActualizarProducto(producto);
                var proveedores = ObtenerProveedoresSeleccionados();
               
                _productoService.AsignarProveedoresAProducto(producto.ProductoID, proveedores);

                MessageBox.Show("Producto actualizado.");
                LimpiarFormulario();
                CargarProductos();
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!productoSeleccionadoId.HasValue)
            {
                MessageBox.Show("Selecciona un producto para eliminar.");
                return;
            }

            var confirm = MessageBox.Show("¿Seguro que deseas eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _productoService.EliminarProducto(productoSeleccionadoId.Value);
                MessageBox.Show("Producto eliminado.");
                LimpiarFormulario();
                CargarProductos();
            }
            catch (Exception ex)
            {
                ManejadorErrores.Mostrar(ex);
            }
        }


        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dgvProductos.Rows[e.RowIndex];

                productoSeleccionadoId = Convert.ToInt32(fila.Cells["ProductoID"].Value);
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtDescripcion.Text = fila.Cells["Descripcion"].Value.ToString();
                txtPrecio.Text = fila.Cells["Precio"].Value.ToString();
                txtCantidad.Text = fila.Cells["CantidadStock"].Value.ToString();
                txtStockMinimo.Text = fila.Cells["StockMinimo"].Value.ToString();
                txtNombre.Enabled = false;

                // Marcar proveedores asociados
                MarcarProveedoresAsociados(productoSeleccionadoId.Value);
            }
        }

        private void MarcarProveedoresAsociados(int productoId)
        {
            // 1. Obtener IDs asociados al producto
            var asociados = _productoService.ObtenerProveedoresPorProducto(productoId);

            // 2. Recorrer los ítems del CheckedListBox
            for (int i = 0; i < clbProveedores.Items.Count; i++)
            {
                var proveedor = (Proveedor)clbProveedores.Items[i];
                bool estaAsociado = asociados.Contains(proveedor.ProveedorID);
                clbProveedores.SetItemChecked(i, estaAsociado);
            }
        }


        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private Producto ObtenerProductoFormulario()
        {
            return new Producto
            {
                Nombre = txtNombre.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                Precio = decimal.TryParse(txtPrecio.Text, out var p) ? p : 0,
                CantidadStock = int.TryParse(txtCantidad.Text, out var c) ? c : 0,
                StockMinimo = int.TryParse(txtStockMinimo.Text, out var m) ? m : 0
            };
        }

        private List<int> ObtenerProveedoresSeleccionados()
        {
            var lista = new List<int>();
            foreach (var item in clbProveedores.CheckedItems)
            {
                var proveedor = (Proveedor)item;
                lista.Add(proveedor.ProveedorID);
            }
            return lista;
        }


        private void LimpiarFormulario()
        {
            productoSeleccionadoId = null;
            txtNombre.Enabled = true;
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtCantidad.Clear();
            txtStockMinimo.Clear();
            clbProveedores.ClearSelected();
            for (int i = 0; i < clbProveedores.Items.Count; i++)
                clbProveedores.SetItemChecked(i, false);
            dgvProductos.ClearSelection();
        }

      
    }
}
