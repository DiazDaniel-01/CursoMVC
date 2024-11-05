using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }
        public ActionResult Extra_Producto()
        {
            return View();
        }
        //Se crean los metodos para listar, registar, editar y eliminar CATEGORIAS
        #region CATEGORIA

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = new CN_Categoria().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;
            if (objeto.Id_Categoria == 0)
            {
                resultado = new CN_Categoria().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Categoria().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //Se crean los metodos para listar, registar, editar y eliminar PRODUCTOS

        #region PRODUCTO

        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> oLista = new List<Producto>();
            oLista = new CN_Producto().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            Producto oProducto = new Producto();

            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

            decimal precio;

            if (decimal.TryParse(oProducto.PrecioTexto,NumberStyles.AllowDecimalPoint,new CultureInfo("es-AR"), out precio)) { 
                oProducto.Precio = precio;
            }

            else{
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##.##" },JsonRequestBehavior.AllowGet);

            }

            if (oProducto.Id_Producto == 0){

                int idProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);

                if (idProductoGenerado != 0)
                {
                    oProducto.Id_Producto = idProductoGenerado;
                }
                else { 
                    operacion_exitosa = false;
                }
            }
            else{

                operacion_exitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }

            if (operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    // Obtiene la ruta física donde se guardarán las imágenes
                    string ruta_guardar = Server.MapPath(ConfigurationManager.AppSettings["ServidorFotos"]);
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oProducto.Id_Producto.ToString(), extension);

                    try
                    {
                        // Guarda la imagen en la ruta especificada
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones
                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }

                    if (guardar_imagen_exito)
                    {
                        // Si la imagen se guardó correctamente, actualiza los datos del producto
                        oProducto.Ruta_Imagen = Path.Combine(ConfigurationManager.AppSettings["ServidorFotos"], nombre_imagen);
                        oProducto.Nombre_Imagen = nombre_imagen;

                        // Guarda los datos del producto
                        bool rspta = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardó el producto, pero hubo problemas con la imagen";
                    }
                }
            }
            return Json(new { operacionExitosa = operacion_exitosa,idGenerado = oProducto.Id_Producto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            Producto oproducto = new CN_Producto().Listar().FirstOrDefault(p => p.Id_Producto == id);

            // Verifica que se haya encontrado el producto
            if (oproducto == null)
            {
                return Json(new
                {
                    conversion = false,
                    mensaje = "Producto no encontrado"
                }, JsonRequestBehavior.AllowGet);
            }

            // Retorna el nombre de la imagen y la información necesaria
            return Json(new
            {
                conversion = true,
                nombreImagen = oproducto.Nombre_Imagen, // Asegúrate de que este campo contenga el nombre del archivo
                extension = Path.GetExtension(oproducto.Nombre_Imagen)
            },
            JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Producto().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //Se crean los metodos para listar, registar, editar y eliminar EXTRA_PRODUCTO
        #region EXTRA_PRODUCTO

        [HttpGet]
        public JsonResult ListarExtraP()
        {
            List<Extra_Producto> oLista = new List<Extra_Producto>();
            oLista = new CN_ExtraP().ListarExtraP();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegistrarExtraP(Extra_Producto objeto)
        {
            object resultado;
            string mensaje = string.Empty;
            if (objeto.Id_ExtraP == 0)
            {
                resultado = new CN_ExtraP().RegistrarExtraP(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_ExtraP().EditarExtraP(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarExtraP(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_ExtraP().EliminarExtraP(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}