using GestionFarmacia.Entities;

namespace GestionFarmacia.Utils
{
    public static class ValidadorEntradas
    {
        public static bool ValidarProducto(Producto producto, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(producto.Nombre))
            {
                mensaje = "El nombre del producto es obligatorio.";
            }
            else if (string.IsNullOrWhiteSpace(producto.Descripcion))
            {
                mensaje = "La descripción del producto es obligatoria.";
            }
            else if (producto.Precio <= 0)
            {
                mensaje = "El precio debe ser mayor que cero.";
            }
            else if (producto.CantidadStock < 0)
            {
                mensaje = "La cantidad en stock no puede ser negativa.";
            }
            else if (producto.StockMinimo < 0)
            {
                mensaje = "El stock mínimo no puede ser negativo.";
            }

            return string.IsNullOrEmpty(mensaje);
        }
    }
}
