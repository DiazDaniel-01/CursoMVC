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
                Extra_Producto = p.Extra_Producto,
                Precio = p.Precio,
                Stock = p.Stock,
                Ruta_Imagen = p.Ruta_Imagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.Ruta_Imagen, p.Nombre_Imagen), out conversion),
                Extension = Path.GetExtension(p.Nombre_Imagen),
                Activo = p.Activo

            }).Where(p =>
            p.oCategoria.Id_Categoria == (idcategoria == 0 ? p.oCategoria.Id_Categoria : idcategoria) &&
            p.Stock > 0 && p.Activo == true).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;
            return jsonresult;
        }

    }

}