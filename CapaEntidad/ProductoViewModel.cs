using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ProductoViewModel
    {
        public int Id_DetalleVenta { get; set; }
        public VentaViewModel oVentaViewModel { get; set; }
        public Producto oProducto { get; set; }
        public string Observacion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
