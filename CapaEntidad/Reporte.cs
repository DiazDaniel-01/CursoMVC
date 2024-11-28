using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Reporte
    {
        public string Id_Venta { get; set; }
        public string Clientes { get; set; }
        public string Barrio { get; set; }
        public string Productos { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string FechaVenta { get; set; }
        public decimal Total_Pago { get; set; }

    }
}
