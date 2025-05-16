using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Data.Interfaces
{
    public interface IProveedorRepository
    {
        bool Insertar(Proveedor proveedor);
        bool Actualizar(Proveedor proveedor);
        bool Eliminar(int proveedorId);
        List<Proveedor> Consultar(int? proveedorId = null);
    }
}
