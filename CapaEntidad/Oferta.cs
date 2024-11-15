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
        public decimal PrecioOriginal { get; set; }   // Precio original
        public decimal PrecioOferta { get; set; }     // Precio con descuento
        public DateTime FechaInicio { get; set; }     // Nueva propiedad: Fecha de inicio
        public DateTime FechaFin { get; set; }        // Nueva propiedad: Fecha de fin
        public string Nombre_Imagen { get; set; }
        public string Ruta_Imagen { get; set; }
        public bool Activo { get; set; }
        public string Base64 { get; set; }
        public string Extension { get; set; }
    }
}
