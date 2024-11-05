using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionTienda.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new CN_Categoria().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProductoporCategorias(int idcategoria)
        {
            List<Producto> lista = new CN_Producto().ListarProductoporCategorias(idcategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idproducto)
        {
            List<Producto> lista = new CN_Producto().Listar().Select(p => new Producto()
            {
                Id_Producto = p.Id_Producto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oCategoria = p.oCategoria,
                Extra_Producto = p.Extra_Producto,
                Precio = p.Precio,
                Ruta_Imagen = p.Ruta_Imagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.Ruta_Imagen, p.Nombre_Imagen), out bool conversion),
                Extension = Path.GetExtension(p.Nombre_Imagen),
                Activo = p.Activo
            })
            .Where(p =>
                (idcategoria == 0 || p.oCategoria.Id_Categoria == idcategoria) &&
                p.Stock > 0 && p.Activo)
            .ToList();

            var jsonResult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }




        [HttpPost]
        public JsonResult AgregarCarrito(int Id_Cliente, int Id_Producto)
        {
            bool existe = new CN_Carrito().ExisteCarrito(Id_Cliente, Id_Producto);
            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CN_Carrito().OperacionCarrito(Id_Cliente, Id_Producto, true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CantidaddeCarrito(int Id_Cliente)
        {
            int cantidad = new CN_Carrito().CantidadEnCarrito(Id_Cliente);
            return Json(new { cantidad }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ListarProductosCarrito()
        {
            int Id_Cliente = ((Cliente)Session["Cliente"]).Id_Cliente;
            List<Carrito> oLista = new List<Carrito>();
            bool conversion;

            
            oLista = new CN_Carrito().ListarProducto(Id_Cliente).Select(oc => new Carrito()
            {
                oProducto = new Producto()
                {
                    Id_Producto = oc.oProducto.Id_Producto,
                    Nombre = oc.oProducto.Nombre,
                    oCategoria = oc.oProducto.oCategoria,
                    Precio = oc.oProducto.Precio,
                    Ruta_Imagen = oc.oProducto.Ruta_Imagen,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.Ruta_Imagen, oc.oProducto.Nombre_Imagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.Nombre_Imagen)
                },
                Cantidad = oc.Cantidad
            }).ToList();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OperacionCarrito(int Id_Producto, bool sumar)
        {
            int Id_Cliente = ((Cliente)Session["Cliente"]).Id_Cliente;

            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().OperacionCarrito(Id_Cliente,Id_Producto, true, out mensaje);

           
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int Id_Producto)
        {
            int Id_Cliente = ((Cliente)Session["Cliente"]).Id_Cliente;

            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(Id_Cliente, Id_Producto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]

        public JsonResult ObtenerLocalidad()
        {
            List<Localidad> oLista = new List<Localidad>();

            oLista = new CN_Ubicacion().ObtenerLocalidad();

            return Json(new { Lista = oLista }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult Carrito()
        {
            return View();
        }
               
    }
}