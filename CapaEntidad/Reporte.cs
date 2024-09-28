using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Reporte
    {
        public string FechaVenta { get; set; }
        public string Clientes { get; set; }
        public string Productos { get; set; }
        public decimal Precio { get; set; }
        public int Total_Producto { get; set; }
        public decimal Total_Pago { get; set; }
        public string Id_Venta { get; set; }

    }
}
