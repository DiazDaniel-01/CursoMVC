using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Oferta
    {
        private CD_Oferta objCapaDato = new CD_Oferta();

        public List<Oferta> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Oferta obj, out string Mensaje)
        {
            Mensaje = ValidarOferta(obj);
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Registrar(obj, out Mensaje);
            }
            return 0; // Retorna 0 si hay un error de validación
        }

        public bool Editar(Oferta obj, out string Mensaje)
        {
            Mensaje = ValidarOferta(obj);
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            return false; // Retorna false si hay un error de validación
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }

        public bool GuardarDatosImagen(Oferta obj, out string Mensaje)
        {
            return objCapaDato.GuardarDatosImagen(obj, out Mensaje);
        }

        private string ValidarOferta(Oferta obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Nombre))
                return "El nombre de la oferta no puede estar vacío.";

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
                return "La descripción de la oferta no puede estar vacía.";

            if (obj.PrecioOriginal <= 0)
                return "Debe ingresar un precio válido para la oferta.";

            if (obj.PrecioOferta <= 0)
                return "Debe ingresar un precio válido para la oferta.";




            return string.Empty; // Sin errores
        }
    }
}
