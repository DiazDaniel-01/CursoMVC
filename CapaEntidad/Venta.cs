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
        public int Id_Cliente { get; set; }
        public int Total_Producto { get; set; }
        public decimal Monto_Total { get; set; }
        public string Id_Barrio { get; set; }
       public string Id_Transaccion { get; set; }
        public List<Detale_Venta> oDetale_Venta { get; set; }



    }
}
