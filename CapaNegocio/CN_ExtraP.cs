using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_ExtraP
    {
        private CD_ExtraP objCapaDato = new CD_ExtraP();

        public List<Extra_Producto> ListarExtraP()
        {
            return objCapaDato.ListarExtraP();
        }

        public int RegistrarExtraP(Extra_Producto obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Ingrediente_Extra) || string.IsNullOrWhiteSpace(obj.Ingrediente_Extra))
            {
                Mensaje = "El Nombre del Ingrediente no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.RegistrarExtraP(obj, out Mensaje);

            }
            else
            {
                return 0;
            }

        }

        public bool EditarExtraP(Extra_Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Ingrediente_Extra) || string.IsNullOrWhiteSpace(obj.Ingrediente_Extra))
            {
                Mensaje = "El Nombre del Ingredienteno puede ser vacio";
            }



            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.EditarExtraP(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarExtraP(int id, out string Mensaje)
        {
            return objCapaDato.EliminarExtraP(id, out Mensaje);
        }

    }
}
