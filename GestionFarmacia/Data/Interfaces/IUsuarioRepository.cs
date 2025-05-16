using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionFarmacia.Entities;

namespace GestionFarmacia.Data.Repositories
{
    public interface IUsuarioRepository
    {
        bool Insertar(Usuario usuario);
        bool Actualizar(Usuario usuario);
        bool Eliminar(int usuarioID);
        List<Usuario> Consultar(int? usuarioID = null);
        Usuario ObtenerPorNombreUsuario(string nombreUsuario);
    }
}
