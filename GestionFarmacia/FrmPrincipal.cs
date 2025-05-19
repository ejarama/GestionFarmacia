using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Data.Repositories;
using GestionFarmacia.Data;
using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Data.Repositories;
using GestionFarmacia.Forms;
using GestionFarmacia.Presentation;

namespace GestionFarmacia
{
    public partial class FrmPrincipal : Form
    {
        private readonly string _rol;
        private readonly string _usuario;

        // Puedes pasar los repositorios si usas DI o los puedes instanciar aquí si es simple
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IProductoRepository _productoRepo;
        private readonly IProveedorRepository _proveedorRepo;

        public FrmPrincipal(string usuario, string rol)
        {
            InitializeComponent();
            _usuario = usuario;
            _rol = rol;

            // Instanciar repositorios
            _usuarioRepo = new UsuarioRepository();
            _productoRepo = new ProductoRepository();
            _proveedorRepo = new ProveedorRepository();

            ConfigurarAccesoPorRol();
        }

        private void ConfigurarAccesoPorRol()
        {
            // Control de visibilidad según el rol del usuario
            btnUsuarios.Enabled = _rol == "Administrador";
            btnReportes.Enabled = _rol == "Administrador" || _rol == "Almacenero";
            btnProveedores.Enabled = _rol == "Administrador" || _rol == "Almacenero";
            btnProductos.Enabled = _rol == "Administrador" || _rol == "Almacenero";
            btnVentas.Enabled = _rol == "Administrador" || _rol == "Vendedor";
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            var form = new FrmUsuarios();
            form.ShowDialog();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            var form = new FrmProductos();
            form.ShowDialog();
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            var form = new FrmProveedores();
            form.ShowDialog();
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            var form = new FrmVentas(); // Si necesitas pasar el nombre del usuario que realiza la venta
            form.ShowDialog();
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            var form = new FrmReportes();
            form.ShowDialog();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Hide(); // Opcional
            Application.Restart(); // Reinicia y vuelve al login
        }
    }
}
