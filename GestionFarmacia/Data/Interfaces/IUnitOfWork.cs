using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionFarmacia.Data;
using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Data.Repositories;
using GestionFarmacia.Entities;

namespace GestionFarmacia
{
    public interface IUnitOfWork
    {
        bool RegistrarVenta(Venta venta);
    }
}
