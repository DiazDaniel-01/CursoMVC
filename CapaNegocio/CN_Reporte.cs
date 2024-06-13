using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Reporte
    {

        private CD_Reporte objCapaDato = new CD_Reporte();

        public DashBoard VerDashBoard()
        {
            return objCapaDato.VerDashBoard();
        }

    }
}
