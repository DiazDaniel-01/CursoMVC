using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Extra_Producto
    {
        public int Id_ExtraP { get; set; }
        public Producto oProducto { get; set; }
        public string Ingrediente_Extra { get; set; }
        public decimal Precio { get; set; }

    }
}
