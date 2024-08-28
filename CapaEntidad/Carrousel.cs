using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Carrousel
    {
        public int Id_Carrousel { get; set; }
        public Oferta oOferta { get; set; }
        public Producto oProducto { get; set; }
        public bool Activo { get; set; }
    }
}
