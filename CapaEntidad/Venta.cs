using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CapaEntidad
{
    public class Venta
    {
        public int Id_Venta { get; set; }
        public Cliente oCliente { get; set; }
        public Localidad oLocalidad { get; set; }
        public List<ProductoVenta> productos { get; set; } // Asegúrate de que esta propiedad exista
        public DateTime Fecha_Venta { get; set; }
        public decimal Total_Pago { get; set; }

        // Propiedad para el total de productos (cantidad de productos en la venta)
        public int Total_Productos
        {
            get { return productos?.Sum(p => p.Cantidad) ?? 0; }  // Sumar la cantidad de todos los productos
        }

        // Propiedad para acceder a un producto específico si es necesario
        public ProductoVenta oProducto
        {
            get { return productos?.FirstOrDefault(); }  // Retorna el primer producto si existe
        }
    }

    public class ProductoVenta
    {
        public int Id_Producto { get; set; }
        public int Cantidad { get; set; }
    }
}

