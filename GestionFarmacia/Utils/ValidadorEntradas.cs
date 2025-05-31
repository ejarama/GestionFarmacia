using GestionFarmacia.Entities;
using System.Windows.Forms;

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

        public static bool ValidarLogin(string nombreUsuario, string contrasena, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                mensaje = "El campo de usuario es obligatorio.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(contrasena))
            {
                mensaje = "El campo de contraseña es obligatorio.";
                return false;
            }

            return true;
        }

        
        public static bool EsEnteroNoNegativo(string texto, out int resultado)
        {
            return int.TryParse(texto, out resultado) && resultado >= 0;
        }

       
        public static bool TextoNoVacio(string texto)
        {
            return !string.IsNullOrWhiteSpace(texto);
        }

        public static bool EsDecimalPositivo(string texto, out decimal resultado)
        {
            return decimal.TryParse(texto, out resultado) && resultado > 0;
        }

        public static bool ValidarCantidadRecibida(DataGridView dgv, string columnaCantidad)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                var valor = row.Cells[columnaCantidad].Value;
                if (valor == null || !int.TryParse(valor.ToString(), out int cantidad) || cantidad < 0)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
