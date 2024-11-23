using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Carrusel
    {
        public int Id_Carrusel { get; set; }
        public string Nombre_Imagen { get; set; }
        public string Descripcion { get; set; }
        public string Ruta_Imagen { get; set; }
        public string Base64 { get; set; }
        public string Extension { get; set; }
    }
}
