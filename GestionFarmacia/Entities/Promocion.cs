using System;

namespace GestionFarmacia.Entities
{
    public class Promocion
    {
        public int PromocionID { get; set; }
        public int ProductoID { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal PorcentajeDescuento { get; set; }
    }
}
