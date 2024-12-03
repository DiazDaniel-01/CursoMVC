using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Barrio
    {
        private CD_Barrio objCapaDato = new CD_Barrio();

        public List<Barrios> ListarBarrio()
        {
            return objCapaDato.ListarBarrio();
        }

        public int RegistrarBarrio(Barrios obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (obj.Codigo_Postal == 0)
            {
                Mensaje = "Debe ingresar su Codigo Postal";
            }
            else if (string.IsNullOrEmpty(obj.Barrio) || string.IsNullOrWhiteSpace(obj.Barrio))
            {
                Mensaje = "El nombre del Barrio no puede ser vacio";
            }
            else if (obj.Costo_Envio == 0)
            {
                Mensaje = "El Costo_Envio debe ser un número válido distinto de 0";
            }
            else if (obj.Costo_Envio < 1) // Si deseas que el número sea mayor que 0
            {
                Mensaje = "El Costo_Envio debe ser un número mayor que 0";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.RegistrarBarrio(obj, out Mensaje);

            }
            else
            {
                return 0;
            }

        }

        public bool EditarBarrio(Barrios obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Codigo_Postal == 0)
            {
                Mensaje = "Debe ingresar su Codigo Postal";
            }
            else if (string.IsNullOrEmpty(obj.Barrio) || string.IsNullOrWhiteSpace(obj.Barrio))
            {
                Mensaje = "El nombre del Barrio no puede ser vacio";
            }
            else if (obj.Costo_Envio == 0)
            {
                Mensaje = "El Costo_Envio debe ser un número válido distinto de 0";
            }
            else if (obj.Costo_Envio < 1) // Si deseas que el número sea mayor que 0
            {
                Mensaje = "El Costo_Envio debe ser un número mayor que 0";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.EditarBarrio(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarBarrio(int id, out string Mensaje)
        {
            return objCapaDato.EliminarBarrio(id, out Mensaje);
        }
    }
}
