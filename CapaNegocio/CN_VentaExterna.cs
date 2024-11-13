﻿using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_VentaExterna
    {
        private CD_VentaExterna objCapaDato = new CD_VentaExterna();

        public int InsertarCliente(Cliente obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre de la categoria no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.InsertarCliente(obj, out Mensaje);

            }
            else
            {
                return 0;
            }

        }

        public int InsertarLocalidad(Localidad obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (obj.Codigo_Postal == 0)
            {

                Mensaje = "Debe ingresar su Codigo Postal";
            }

            else if (string.IsNullOrEmpty(obj.Calle) || string.IsNullOrWhiteSpace(obj.Calle))
            {
                Mensaje = "El nombre de la Calle no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.InsertarLocalidad(obj, out Mensaje);

            }
            else
            {
                return 0;
            }


        }
    }
}