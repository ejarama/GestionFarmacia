using GestionFarmacia.Data;
using GestionFarmacia.Entities;
using GestionFarmacia.Utils;
using System;
using System.Collections.Generic;

namespace GestionFarmacia.Services
{
    public class VentaService : IVentaService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly VentaRepository _ventaRepository;

        public VentaService()
        {
            _unitOfWork = new UnitOfWork();
            _ventaRepository = new VentaRepository();
        }

        public bool RegistrarVenta(Venta venta)
        {
            // Aquí podrías aplicar reglas de negocio (descuentos, validaciones, etc.)
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
