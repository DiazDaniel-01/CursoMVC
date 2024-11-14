using CapaDatos;
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

        public List<Cliente> ListarCliente()
        {
            return objCapaDato.ListarCliente();
        }

        public List<Localidad> ListarLocalidad()
        {
            return objCapaDato.ListarLocalidad();
        }
        public int InsertarCliente(Cliente obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del cliente no puede ser vacio";
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

        public int RegistrarVenta(Venta obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (obj.oCliente.Id_Cliente == 0)
            {
                Mensaje = "Debe seleccionar un cliente";
            }
            else if (obj.oLocalidad.Id_Localidad == 0)
            {
                Mensaje = "Debe seleccionar una localidad";
            }
            else if (obj.oProducto.Id_Producto == 0)
            {
                Mensaje = "Debe seleccionar un producto";
            }

            else if (obj.Total_Productos == 0)
            {

                Mensaje = "Debe ingresar una cantidad de productos";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.RegistrarVenta(obj, out Mensaje);

            }
            else
            {
                return 0;
            }

        }

    }
}