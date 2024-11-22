using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class VentaViewModel
    {
        public int Id_Cliente { get; set; }
        public int Id_Localidad { get; set; }
        public DateTime Fecha_Venta { get; set; }
        public decimal Total_Pago { get; set; }
        public List<ProductoViewModel> Productos { get; set; }
    }
}
