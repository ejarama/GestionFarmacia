using GestionFarmacia.Data;
using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Data.Repositories;
using GestionFarmacia.Entities;
using System;
using System.Collections.Generic;

namespace GestionFarmacia.Services
{
    public class VentaService : IVentaService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly VentaRepository _ventaRepository;
        private readonly IPromocionRepository _promocionRepository;

        public VentaService()
        {
            _unitOfWork = new UnitOfWork();
            _ventaRepository = new VentaRepository();
            _promocionRepository = new PromocionRepository();
        }

        public bool RegistrarVenta(Venta venta)
        {
            DateTime fechaActual = DateTime.Now;

            foreach (var detalle in venta.Detalles)
            {
                var promocion = _promocionRepository.ObtenerPromocionVigentePorProducto(detalle.ProductoID, fechaActual);
                if (promocion != null)
                {
                    decimal descuento = detalle.PrecioUnitario * (promocion.PorcentajeDescuento / 100);
                    detalle.PrecioUnitario -= descuento;
                }
            }

            // Recalcular total después de aplicar descuentos
            venta.TotalVenta = 0;
            foreach (var d in venta.Detalles)
                venta.TotalVenta += d.Subtotal;

            return _unitOfWork.RegistrarVenta(venta);
        }

        public Venta ObtenerPorId(int ventaId)
        {
            return _ventaRepository.ObtenerPorId(ventaId);
        }

        public List<Venta> ObtenerTodos()
        {
            return _ventaRepository.ObtenerTodos();
        }

        public List<Venta> ObtenerPorRangoFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            return _ventaRepository.ObtenerPorRangoFecha(fechaInicio, fechaFin);
        }
    }
}
