using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
        public class Producto
        {
            public int Id_Producto { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public Categoria oCategoria { get; set;  }
            public decimal Precio { get; set; }
            public int Stock { get; set; }
            public string Ruta_Imagen { get; set; }
            public string Nombre_Imagen { get; set; }
            public bool Activo { get; set; }
        }
}
