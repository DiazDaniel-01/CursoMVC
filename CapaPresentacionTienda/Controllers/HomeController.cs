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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Menu()
        {
            return View();
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

            lista = new CN_Producto().ListarProductoporCategorias(idcategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarTodosProductos()
        {
            CN_Producto objCN_Producto = new CN_Producto();
            List<Producto> listaProductos = objCN_Producto.Listar();

            return Json(new { data = listaProductos }, JsonRequestBehavior.AllowGet);
        }

    }

}