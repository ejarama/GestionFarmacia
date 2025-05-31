using GestionFarmacia.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

public static class VentaFactory
{
    public static Venta CrearVenta(int usuarioID, List<DetalleVenta> detalles)
    {
        return new Venta
        {
            UsuarioID = usuarioID,
            FechaVenta = DateTime.Now,
            Detalles = detalles,
            TotalVenta = detalles.Sum(d => d.Subtotal)
        };
    }
}
