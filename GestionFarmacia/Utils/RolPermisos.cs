using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFarmacia.Utils
{
    public static class RolPermisos
    {
        public static bool PuedeAccederUsuarios(string rol) => rol == "Administrador";
        public static bool PuedeAccederReportes(string rol) => rol == "Administrador" || rol == "Almacenero";
        public static bool PuedeAccederProveedores(string rol) => rol == "Administrador" || rol == "Almacenero";
        public static bool PuedeAccederProductos(string rol) => rol == "Administrador" || rol == "Almacenero";
        public static bool PuedeAccederVentas(string rol) => rol == "Administrador" || rol == "Vendedor";
        public static bool PuedeAccederPromociones(string rol) => rol == "Administrador";
        
    }
}
