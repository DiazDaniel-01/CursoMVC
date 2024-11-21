using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Carrito
    {
        private CD_Carrito objCapaDato = new CD_Carrito();


        public bool ExisteCarrito(int idproducto)
        {
            return objCapaDato.ExisteCarrito(idproducto);
        }
        public bool OperacionCarrito(int idproducto, bool Sumar, out string Mensaje)
        {
            return objCapaDato.OperacionCarrito(idproducto, Sumar, out Mensaje);
        }

        public int CantidadEnCarrito()
        {
            return objCapaDato.CantidadEnCarrito();
        }

        public List<Carrito> ListarProducto()
        {
            return objCapaDato.ListarProducto();
        }

        public bool EliminarCarrito(int Id_Producto)
        {
            return objCapaDato.EliminarCarrito(Id_Producto);
        }
    }
}
