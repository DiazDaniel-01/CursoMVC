using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Carrito
    {
        public int Id_Carrito { get; set; }
        public Cliente oCliente { get; set; }
        public Producto oProducto { get; set; }
        public Oferta oOferta { get; set; }
        public DateTime FechaAgregado { get; set; }
        public int Cantidad { get; set; } //me olvide de agregar



    }
}
