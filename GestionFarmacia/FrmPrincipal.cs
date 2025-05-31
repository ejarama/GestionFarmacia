using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Farmacia.Data.Repositories;
using GestionFarmacia.Data;
using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Data.Repositories;
using GestionFarmacia.Entities;
using GestionFarmacia.Forms;
using GestionFarmacia.Presentation;
using GestionFarmacia.Services;
using GestionFarmacia.Utils;

namespace GestionFarmacia
{
    public partial class FrmPrincipal : Form
    {
       
        // Puedes pasar los repositorios si usas DI o los puedes instanciar aquí si es simple
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IProductoRepository _productoRepo;
        private readonly IProveedorRepository _proveedorRepo;

        private readonly Usuario _usuario;
       

        public FrmPrincipal(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;

            _usuarioRepo = new UsuarioRepository();
            _productoRepo = new ProductoRepository();
            _proveedorRepo = new ProveedorRepository();

            ConfigurarAccesoPorRol();
            lblUsuario.Text = $"User: {_usuario.NombreUsuario} - {_usuario.Rol}";
        }


        private void ConfigurarAccesoPorRol()
        {
            string rol = _usuario.Rol;

            btnUsuarios.Enabled = RolPermisos.PuedeAccederUsuarios(rol);
            btnReportes.Enabled = RolPermisos.PuedeAccederReportes(rol);
            btnProveedores.Enabled = RolPermisos.PuedeAccederProveedores(rol);
            btnProductos.Enabled = RolPermisos.PuedeAccederProductos(rol);
            btnVentas.Enabled = RolPermisos.PuedeAccederVentas(rol);
            btnPromociones.Enabled = RolPermisos.PuedeAccederPromociones(rol);
            btnPedidos.Enabled = RolPermisos.PuedeAccederPedidos(rol);
            btnReglasPedido.Enabled = RolPermisos.PuedeAccederReglasPedidos(rol);
        }


        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            var form = new FrmUsuarios(_usuarioRepo);
            form.ShowDialog();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            var productoRepo = new ProductoRepository();
            var proveedorRepo = new ProveedorRepository();
            var proveedorProductoRepo = new ProveedorProductoRepository();

            var productoService = new ProductoService(productoRepo, proveedorProductoRepo);

            FrmProductos frm = new FrmProductos(productoService, proveedorRepo);
            frm.ShowDialog();

        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            var form = new FrmProveedores();
            form.ShowDialog();
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            var form = new FrmVentas(_usuario); // Pasas el objeto completo
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

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void btnPromociones_Click(object sender, EventArgs e)
        {
            var form = new FrmPromociones();
            form.ShowDialog();
        }

        private void btnPedidos_Click(object sender, EventArgs e)
        {
            var pedidoRepo = new PedidoRepository();
            FrmRecepcionPedidos form = new FrmRecepcionPedidos(pedidoRepo);
            form.ShowDialog();
        }

        private void btnReglasPedido_Click(object sender, EventArgs e)
        {
            FrmReglasPedido form = new FrmReglasPedido();
            form.ShowDialog();
        }
    }
}
