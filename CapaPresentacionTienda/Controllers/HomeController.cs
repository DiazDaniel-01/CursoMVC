using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


using CapaEntidad;
using CapaNegocio;
using System.IO;
using CapaDatos;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using System;


namespace CapaPresentacionTienda.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CD_Oferta ofertaDatos = new CD_Oferta();
            List<Oferta> ofertas = ofertaDatos.Listar() ?? new List<Oferta>(); // Evita que la lista sea null
            return View(ofertas); // Pasar la lista de ofertas a la vista
        }



        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new CN_Categoria().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProductoporCategorias(int Id_Categoria)
        {
            List<Producto> lista = new CN_Producto().ListarProductoporCategorias(Id_Categoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProducto(int Id_Categoria, int Id_Producto)
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
                (Id_Categoria == 0 || p.oCategoria.Id_Categoria == Id_Categoria) &&
                p.Stock > 0 && p.Activo)
            .ToList();

            var jsonResult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        [HttpGet]
        public JsonResult ListarOfertas()
        {
            List<Oferta> oLista = new CN_Oferta().Listar();

            // Convertir las fechas a un formato legible
            bool conversion;
            var ofertasFormateadas = oLista.Select(o => new
            {
                o.Id_Oferta,
                o.Nombre,
                o.Descripcion,
                o.PrecioOriginal,
                o.PrecioOferta,
                FechaInicio = o.FechaInicio.ToString("dd/MM/yyyy"),  // Formateamos la fecha de inicio
                FechaFin = o.FechaFin.ToString("dd/MM/yyyy"),        // Formateamos la fecha de fin
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(o.Ruta_Imagen, o.Nombre_Imagen), out conversion),
                Extension = Path.GetExtension(o.Nombre_Imagen),
                o.Activo
            }).ToList();

            return Json(new { data = ofertasFormateadas }, JsonRequestBehavior.AllowGet);
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
        public JsonResult ListarProductosCarrito(int? idCliente = null)
        {
            int Id_Cliente = idCliente ?? 0;
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
        public JsonResult OperacionCarrito(int Id_Producto, bool sumar, int? idCliente = null)
        {
            int Id_Cliente = idCliente ?? 0;

            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().OperacionCarrito(Id_Cliente, Id_Producto, true, out mensaje);


            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int Id_Producto, int? idCliente = null)
        {
            int Id_Cliente = idCliente ?? 0;

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
        public ActionResult DetalleProducto(int Id_Producto = 0)
        {
            Producto oProducto = new Producto();
            bool conversion;

            oProducto = new CN_Producto().Listar().Where(p => p.Id_Producto == Id_Producto).FirstOrDefault();

            if (oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.Ruta_Imagen, oProducto.Nombre_Imagen), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.Nombre_Imagen);
            }
            return View(oProducto);
        }     


        
    }
}
