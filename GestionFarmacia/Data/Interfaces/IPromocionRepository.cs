using System;
using System.Collections.Generic;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Data.Interfaces
{
    public interface IPromocionRepository
    {
        bool Insertar(Promocion promocion);
        bool Actualizar(Promocion promocion);
        bool Eliminar(int promocionId);
        List<Promocion> ObtenerTodas();
        Promocion ObtenerPromocionVigentePorProducto(int productoId, DateTime fecha);
    }
}
