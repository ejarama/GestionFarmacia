using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestionFarmacia.Forms;
using GestionFarmacia.Data.Repositories;
using GestionFarmacia.Data.Interfaces;
using GestionFarmacia.Entities;

namespace GestionFarmacia
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IUsuarioRepository usuarioRepo = new UsuarioRepository();
            FrmLogin login = new FrmLogin(usuarioRepo);

            if (login.ShowDialog() == DialogResult.OK)
            {
                Usuario usuario = login.UsuarioAutenticado;
                Application.Run(new FrmPrincipal(usuario)); 
            }
            else
            {
                Application.Exit();
            }

        }
    }
}
