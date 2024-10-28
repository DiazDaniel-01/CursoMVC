using CapaNegocio;
using System;
using System.Collections.Generic;
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
    }
}