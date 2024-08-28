using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Oferta
    {
        public int Id_Oferta { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Nombre_Imagen { get; set; }
        public string Ruta_Imagen { get; set; }
        public bool Activo { get; set; }
    }
}
