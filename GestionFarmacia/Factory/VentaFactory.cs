using System;
using System.Collections.Generic;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Utils
{
    public static class VentaFactory
    {
        /// <summary>
        /// Crea una venta con sus detalles.
        /// </summary>
        /// <param name="usuarioId">ID del usuario que realiza la venta</param>
        /// <param name="detalles">Lista de productos con cantidad y precio</param>
        /// <returns>Venta armada</returns>
        public static Venta CrearVenta(int usuarioId, List<DetalleVenta> detalles)
        {
            if (detalles == null || detalles.Count == 0)
                throw new ArgumentException("La venta no tiene productos.");

            decimal total = 0;
            foreach (var item in detalles)
            {
                total += item.Subtotal;
            }

            return new Venta
            {
                UsuarioID = usuarioId,
                FechaVenta = DateTime.Now,
                TotalVenta = total,
                Detalles = detalles
            };
        }
    }
}
