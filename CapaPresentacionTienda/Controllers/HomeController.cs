using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Configuration; // Asegúrate de incluir este namespace



using CapaEntidad;
using CapaNegocio;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web.Services.Description;
using CapaDatos;

namespace CapaPresentacionTienda.Controllers
{
    public class HomeController : Controller
    {



        public ActionResult Index()
        {
            var productos = new CD_Producto().Listar();
            return View(productos);
        }


        public ActionResult Carrito()
        {
            return View();
        }
        public ActionResult DetalleProducto(int idproducto = 0)
        {
            Producto oProducto = new Producto();
            bool conversion;

            oProducto = new CN_Producto().Listar().Where(p => p.Id_Producto == idproducto).FirstOrDefault();

            if (oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.Ruta_Imagen, oProducto.Nombre_Imagen), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.Nombre_Imagen);
            }
            return View(oProducto);
        }


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
            bool conversion;
            lista = new CN_Producto().Listar().Select(p => new Producto()
            {
                Id_Producto = p.Id_Producto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Ruta_Imagen = p.Ruta_Imagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.Ruta_Imagen, p.Nombre_Imagen), out conversion),
                Extension = Path.GetExtension(p.Nombre_Imagen),
                Activo = p.Activo
            }).Where(p =>
                (idcategoria == 0 || p.oCategoria.Id_Categoria == idcategoria) && // Condición para todos los productos si idcategoria es 0
                p.Activo == true
            ).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;
            return jsonresult;
        }



        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idproducto)
        {
            List<Producto> lista = new List<Producto>();
            bool conversion;
            lista = new CN_Producto().Listar().Select(p => new Producto()
            {
                Id_Producto = p.Id_Producto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oCategoria = p.oCategoria,

                Precio = p.Precio,

                Ruta_Imagen = p.Ruta_Imagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.Ruta_Imagen, p.Nombre_Imagen), out conversion),
                Extension = Path.GetExtension(p.Nombre_Imagen),
                Activo = p.Activo

            }).Where(p =>
                p.oCategoria.Id_Categoria == (idcategoria == 0 ? p.oCategoria.Id_Categoria : idcategoria) &&
                p.Id_Producto == (idproducto == 0 ? p.Id_Producto : idproducto) &&
                p.Activo == true
            ).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;
            return jsonresult;
        }

        [HttpGet]
        public JsonResult ListarOfertas()
        {
            // Obtenemos la lista de ofertas desde la capa de negocio.
            List<Oferta> oLista = new CN_Oferta().Listar();

            // Convertimos las fechas a un formato legible y preparamos la lista para el frontend.
            bool conversion;
            var ofertasFormateadas = oLista.Select(o => new
            {
                o.Id_Oferta,
                o.Nombre,
                o.Descripcion,
                o.PrecioOriginal,
                o.PrecioOferta,
                // Formateamos las fechas de inicio y fin a 'dd/MM/yyyy'.
                FechaInicio = o.FechaInicio.ToString("dd/MM/yyyy"),
                FechaFin = o.FechaFin.ToString("dd/MM/yyyy"),
                // Convertimos la imagen a Base64.
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(o.Ruta_Imagen, o.Nombre_Imagen), out conversion),
                Extension = Path.GetExtension(o.Nombre_Imagen),
                o.Activo
            }).ToList();

            // Retornamos el resultado como JSON.
            return Json(new { data = ofertasFormateadas }, JsonRequestBehavior.AllowGet);
        }

        // Controlador para gestionar el carrito
        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {
            if (idproducto <= 0)
            {
                return Json(new { respuesta = false, mensaje = "ID de producto inválido" }, JsonRequestBehavior.AllowGet);
            }

            CN_Carrito carritoNegocio = new CN_Carrito();
            bool existe = carritoNegocio.ExisteCarrito(idproducto);
            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = carritoNegocio.OperacionCarrito(idproducto, true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int cantidad = new CN_Carrito().CantidadEnCarrito(); // Eliminado idcliente
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }





        [HttpGet]
        public JsonResult ListarProductosCarrito()
        {
            List<Carrito> oLista = new List<Carrito>();
            bool conversion;

            // Obtener la ruta desde el archivo de configuración
            string rutaBase = ConfigurationManager.AppSettings["ServidorFotosProductos"];

            oLista = new CN_Carrito().ListarProducto().Select(oc => new CapaEntidad.Carrito()
            {
                oProducto = new Producto()
                {
                    Id_Producto = oc.oProducto.Id_Producto,
                    Nombre = oc.oProducto.Nombre,
                    Precio = oc.oProducto.Precio,
                    Ruta_Imagen = oc.oProducto.Ruta_Imagen,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(rutaBase, oc.oProducto.Nombre_Imagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.Nombre_Imagen),
                },
                Cantidad = oc.Cantidad
            }).ToList();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().OperacionCarrito(idproducto, sumar, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {

            bool respuesta = false;
            string mensaje = string.Empty;


            respuesta = new CN_Carrito().EliminarCarrito(idproducto);


            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

    }

}