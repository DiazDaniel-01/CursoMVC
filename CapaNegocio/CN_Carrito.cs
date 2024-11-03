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

        public bool ExisteCarrito(int Id_Cliente, int Id_Producto)
        {
            return objCapaDato.ExisteCarrito(Id_Cliente, Id_Producto);
        }
        public bool OperacionCarrito(int Id_Cliente, int Id_Producto, bool sumar, out string Mensaje)
        {
            return objCapaDato.OperacionCarrito(Id_Cliente, Id_Producto, sumar, out Mensaje);
        }
        public int CantidadEnCarrito(int Id_Cliente)
        {
            return objCapaDato.CantidadEnCarrito(Id_Cliente);
        }
        public List<Carrito> ListarProducto(int Id_Cliente)
        {
            return objCapaDato.ListarProducto(Id_Cliente);
        }
        public bool EliminarCarrito(int Id_Cliente, int Id_Producto)
        {
            return objCapaDato.EliminarCarrito(Id_Cliente, Id_Producto);
        }

    }
}