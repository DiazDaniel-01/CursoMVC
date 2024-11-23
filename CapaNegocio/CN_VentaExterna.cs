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

        public int InsertarVenta(VentaViewModel obj, out string Mensaje)
        {
            // Inicializamos Mensaje
            Mensaje = string.Empty;

            // Validaciones de los campos que no pueden ser nulos
            if (obj.oCliente == null || obj.oCliente.Id_Cliente == 0)
            {
                Mensaje = "Debe seleccionar un cliente";
            }
            else if (obj.oLocalidad == null || obj.oLocalidad.Id_Localidad == 0)
            {
                Mensaje = "Debe seleccionar una localidad";
            }
            else if (obj.Fecha_Venta == DateTime.MinValue)
            {
                Mensaje = "La fecha de venta no puede ser vacía";
            }

            // Si no se encontró ningún error en la validación
            if (string.IsNullOrEmpty(Mensaje))
            {
                // Llamar al método InsertarVenta de CapaDatos
                return objCapaDato.InsertarVenta(obj, out Mensaje);
            }
            else
            {
                // Retorna 0 en caso de que haya un error de validación
                return 0;
            }
        }

        public int InsertarDetalleVenta(ProductoViewModel obj, out string Mensaje)
        {
            // Inicializamos Mensaje
            Mensaje = string.Empty;

            // Validaciones de los campos que no pueden ser nulos
            if (obj.oVentaViewModel == null || obj.oVentaViewModel.Id_VentaViewModel == 0)
            {
                Mensaje = "Debe seleccionar una venta válida";
            }
            else if (obj.oProducto == null || obj.oProducto.Id_Producto == 0)
            {
                Mensaje = "Debe seleccionar un producto válido";
            }
            else if (obj.Cantidad <= 0)
            {
                Mensaje = "La cantidad debe ser mayor a cero";
            }
            else if (obj.PrecioUnitario <= 0)
            {
                Mensaje = "El precio unitario debe ser mayor a cero";
            }
            else if (obj.Subtotal <= 0)
            {
                Mensaje = "El subtotal debe ser mayor a cero";
            }
            else if (obj.Total_Pago <= 0)
            {
                Mensaje = "El total a pagar debe ser mayor a cero";
            }

            // Si no se encontró ningún error en la validación
            if (string.IsNullOrEmpty(Mensaje))
            {
                // Llamar al método InsertarDetalleVenta de CapaDatos
                return objCapaDato.InsertarDetalleVenta(obj, out Mensaje);
            }
            else
            {
                // Retorna 0 en caso de que haya un error de validación
                return 0;
            }
        }


    }
}