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
        public Localidad oLocalidad { get; set; }
        public int Total_Productos { get; set; }
        public string Extra_Producto { get; set; }
        public decimal Total_Pago { get; set; }
        public DateTime Fecha_Venta { get; set; }
        public string Departamento { get; set; }
        public string Direccion { get; set; }
    }
}
