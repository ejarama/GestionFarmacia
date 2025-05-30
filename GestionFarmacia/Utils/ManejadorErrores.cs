using System;
using System.Windows.Forms;

namespace GestionFarmacia.Utils
{
    public static class ManejadorErrores
    {
        /// <summary>
        /// Muestra un mensaje de error en un MessageBox, a partir de una excepción.
        /// </summary>
        /// <param name="ex">La excepción capturada.</param>
        public static void Mostrar(Exception ex)
        {
            string mensaje = ObtenerMensajeDetallado(ex);
            MessageBox.Show(
                mensaje,
                "Error del sistema",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        /// <summary>
        /// Extrae el mensaje más interno posible de una excepción anidada.
        /// </summary>
        /// <param name="ex">La excepción principal.</param>
        /// <returns>Mensaje detallado.</returns>
        private static string ObtenerMensajeDetallado(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;

            return $"Se produjo un error inesperado:\n\n{ex.Message}";
        }
    }
}
