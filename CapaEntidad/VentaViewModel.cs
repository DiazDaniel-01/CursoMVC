using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class VentaViewModel
    {
        public int Id_VentaViewModel { get; set; }
        public Cliente oCliente { get; set; }
        public Localidad oLocalidad { get; set; }
        public DateTime Fecha_Venta { get; set; }
    }
}
