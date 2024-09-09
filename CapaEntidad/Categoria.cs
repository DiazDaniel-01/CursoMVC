using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{//create table CATEGORIA (
    //Id_Categoria int primary key identity,
    //Nombre varchar(100),
    //Activo bit default 1
//) 
    public class Categoria
    {
        public int Id_Categoria { get; set; }
        public string Nombre { get; set;  }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
