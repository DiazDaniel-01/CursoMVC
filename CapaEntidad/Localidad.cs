using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Localidad
    {
        public int Id_Localidad { get; set; }
        public Barrios oBarrios { get; set; }
        public string Departamento { get; set; }
        public string Calle { get; set; }
        public string Calle_2 { get; set; }
        public int Piso { get; set; }
        public string Referencia { get; set; }
    }
}