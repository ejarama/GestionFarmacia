using System;
using System.Collections.Generic;
using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Entities;
using GestionFarmacia.Services.Interfaces;

namespace GestionFarmacia.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepo;
        private readonly IProveedorProductoRepository _proveedorProductoRepo;

        public ProductoService(IProductoRepository productoRepo, IProveedorProductoRepository proveedorProductoRepo)
        {
            _productoRepo = productoRepo;
            _proveedorProductoRepo = proveedorProductoRepo;
        }

        public bool RegistrarProducto(Producto producto)
        {
            // Podrías validar existencia por nombre si lo deseas
            _productoRepo.Insertar(producto);
            return true;
        }

        public bool ActualizarProducto(Producto producto)
        {
            _productoRepo.Actualizar(producto);
            return true;
        }

        public bool EliminarProducto(int productoId)
        {
            _productoRepo.Eliminar(productoId);
            return true;
        }

        public List<Producto> ObtenerProductos()
        {
            return _productoRepo.ObtenerTodos();
        }

        public Producto ObtenerPorNombre(string nombre)
        {
            return _productoRepo.ObtenerPorNombre(nombre);
        }

        public void AsignarProveedoresAProducto(int productoId, List<int> proveedorIds)
        {
            var actuales = _proveedorProductoRepo.ObtenerProveedoresPorProducto(productoId);
            foreach (var id in actuales)
                _proveedorProductoRepo.EliminarProveedorDeProducto(productoId, id);

            foreach (var id in proveedorIds)
                _proveedorProductoRepo.AsignarProveedorAProducto(productoId, id);
        }

        public List<int> ObtenerProveedoresPorProducto(int productoId)
        {
            return _proveedorProductoRepo.ObtenerProveedoresPorProducto(productoId);
        }
    }
}
