using GestionFarmacia.Data;
using GestionFarmacia.Data.Repositories;
using GestionFarmacia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionFarmacia.Reportes
{
    public class ReporteVentasInventario : IReporte
    {
        public List<object> Generar(DateTime fechaInicio, DateTime fechaFin, string filtroEntrega)
        {
            var ventaRepo = new VentaRepository();
            var productoRepo = new ProductoRepository(); // Debes tenerlo creado

            var ventas = ventaRepo.ObtenerPorRangoFecha(fechaInicio, fechaFin);
            var productos = productoRepo.ObtenerTodos(); // Método que retorne todos los productos con stock

            // Aplicar filtro por tipo de entrega
            if (filtroEntrega != "Todos")
            {
                ventas = ventas.Where(v => v.TipoEntrega == filtroEntrega).ToList();
            }

            // Combinar detalles con inventario
            var resultado = new List<object>();

            foreach (var venta in ventas)
            {
                foreach (var detalle in venta.Detalles)
                {
                    var producto = productos.FirstOrDefault(p => p.ProductoID == detalle.ProductoID);

                    resultado.Add(new
                    {
                        venta.FechaVenta,
                        detalle.ProductoID,
                        ProductoNombre = producto?.Nombre ?? "No encontrado",
                        detalle.Cantidad,
                        detalle.PrecioUnitario,
                        StockActual = producto?.CantidadStock ?? 0,
                        TipoEntrega = venta.TipoEntrega,
                        TotalParcial = detalle.Cantidad * detalle.PrecioUnitario
                    });
                }
            }

            return resultado;
        }
    }
}