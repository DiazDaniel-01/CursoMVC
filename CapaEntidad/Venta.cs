using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {
        public int Id_Venta { get; set; }
        public Cliente oCliente { get; set; }
        public Producto oProducto { get; set; }
        public int Total_Productos { get; set; }
        public decimal Total_Pago { get; set; }
        public DateTime Fecha_Venta { get; set; }
    }
}
