using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using System.IO;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Menu()
        {
            return View();
        }

        #region CATEGORIAS

        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new List<Categoria>();

            lista = new CN_Categoria().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProductoporCategorias(int idcategoria)
        {
            List<Producto> lista = new List<Producto>();

            lista = new CN_Producto().ListarProductoporCategorias(idcategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PRODUCTOS
        [HttpGet]
        public JsonResult ListarTodosProductos()
        {
            CN_Producto objCN_Producto = new CN_Producto();
            List<Producto> listaProductos = objCN_Producto.Listar();

            return Json(new { data = listaProductos }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto oproducto = new CN_Producto().Listar().Where(p => p.Id_Producto == id).FirstOrDefault();

            if (oproducto != null)
            {
                // Resolver la ruta física desde la ruta relativa usando Server.MapPath
                string rutaFisica = Server.MapPath(Path.Combine(oproducto.Ruta_Imagen, oproducto.Nombre_Imagen));

                // Convertir la imagen a base64
                string textoBase64 = CN_Recursos.ConvertirBase64(rutaFisica, out conversion);

                return Json(new
                {
                    conversion = conversion,
                    textoBase64 = textoBase64,
                    extension = Path.GetExtension(oproducto.Nombre_Imagen)
                },
                JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Si no se encuentra el producto, devuelve un mensaje de error
                return Json(new
                {
                    conversion = false,
                    textoBase64 = string.Empty,
                    extension = string.Empty,
                    mensaje = "Producto no encontrado."
                },
                JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region CARRUSEL
        [HttpGet]
        public JsonResult ListarCarrusel()
        {
            List<Carrusel> oLista = new List<Carrusel>();
            oLista = new CN_Carrusel().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImagenCarrusel(int id)
        {
            bool conversion;
            Carrusel oCarrusel = new CN_Carrusel().Listar().FirstOrDefault(p => p.Id_Carrusel == id);

            if (oCarrusel != null)
            {
                // Obtener ruta física usando Server.MapPath
                string ruta_fisica = Server.MapPath(Path.Combine(oCarrusel.Ruta_Imagen, oCarrusel.Nombre_Imagen));
                string textoBase64 = CN_Recursos.ConvertirBase64(ruta_fisica, out conversion);

                return Json(new
                {
                    conversion = conversion,
                    textoBase64 = textoBase64,
                    extension = Path.GetExtension(oCarrusel.Nombre_Imagen)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    conversion = false,
                    textoBase64 = string.Empty,
                    extension = string.Empty
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Cliente y Localidad
        [HttpGet]
        public JsonResult ListarBarrio()
        {
            List<Barrios> oLista = new List<Barrios>();

            oLista = new CN_VentaExterna().ListarBarrio();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearLocalidad(Localidad objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.Id_Localidad == 0)
            {
                // Insertar la localidad y obtener el ID de la nueva localidad
                resultado = new CN_VentaExterna().InsertarLocalidad(objeto, out mensaje);
            }
            else
            {
                return View();
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearCliente(Cliente objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.Id_Cliente == 0)
            {
                // Insertar el cliente y obtener el ID del nuevo cliente
                resultado = new CN_VentaExterna().InsertarCliente(objeto, out mensaje);
            }
            else
            {
                return View();
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult InsertarVenta(VentaViewModel objeto)
        {
            // Verifica si el objeto llega correctamente
            if (objeto != null)
            {
                Console.WriteLine($"Total_Pago recibido: {objeto.Total_Pago}");
            }
            else
            {
                Console.WriteLine("El objeto es nulo");
            }

            object resultado;
            string mensaje = string.Empty;

            if (objeto.Id_VentaViewModel == 0)
            {
                resultado = new CN_VentaExterna().InsertarVenta(objeto, out mensaje);
                if (resultado != null)
                {
                    return Json(new { success = true, data = new { Id_VentaViewModel = resultado }, message = mensaje });
                }
            }
            else
            {
                return Json(new { success = false, message = "La venta ya existe." });
            }

            return Json(new { success = false, message = "No se pudo crear la venta." });
        }




        [HttpPost]
        public ActionResult InsertarDetalleVenta(List<ProductoViewModel> detallesVenta)
        {
            bool success = true;
            string mensaje = string.Empty;

            try
            {
                // Verifica que la lista de detalles de la venta no esté vacía
                if (detallesVenta != null && detallesVenta.Count > 0)
                {
                    // Recorre cada detalle de venta enviado desde el frontend
                    foreach (var detalle in detallesVenta)
                    {
                        if (detalle.Id_DetalleVenta == 0)
                        {
                            // Insertar el detalle de la venta y manejar los errores
                            int resultado = new CN_VentaExterna().InsertarDetalleVenta(detalle, out string mensajeDetalle);

                            if (resultado == 0) // Si '0' indica fracaso
                            {
                                success = false;
                                mensaje += $"Error al insertar el producto {detalle.oProducto.Id_Producto}: {mensajeDetalle} ";
                            }
                        }
                    }
                }
                else
                {
                    success = false;
                    mensaje = "No se han proporcionado detalles de venta para insertar.";
                }
            }
            catch (Exception ex)
            {
                success = false;
                mensaje = $"Ocurrió un error al insertar los detalles de la venta: {ex.Message}";
            }

            // Retornar el resultado del proceso
            return Json(new { success = success, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }

}