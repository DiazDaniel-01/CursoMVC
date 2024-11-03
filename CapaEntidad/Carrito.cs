using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CapaEntidad
{
    public class Carrito
    {
        public Producto oProducto { get; set; }
        public int Cantidad { get; set; }

        public decimal Subtotal
        {
            get
            {
                return oProducto != null ? oProducto.Precio * Cantidad : 0;
            }
        }
    }
}


