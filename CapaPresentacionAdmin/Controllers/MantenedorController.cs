﻿using CapaEntidad;
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
        public ActionResult Carrusel()
        {
            return View();
        }
        public ActionResult Barrio()
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

            if (decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-AR"), out precio))
            {
                oProducto.Precio = precio;
            }

            else
            {
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);

            }

            if (oProducto.Id_Producto == 0)
            {

                int idProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);

                if (idProductoGenerado != 0)
                {
                    oProducto.Id_Producto = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {

                operacion_exitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }

            if (operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    string ruta_guardar = Server.MapPath(ConfigurationManager.AppSettings["FotosProductos"]);
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oProducto.Id_Producto.ToString(), extension);


                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }

                    if (guardar_imagen_exito)
                    {

                        oProducto.Ruta_Imagen = ConfigurationManager.AppSettings["FotosProductos"];
                        oProducto.Nombre_Imagen = nombre_imagen;
                        bool rspta = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);

                    }
                    else
                    {
                        mensaje = "Se guardo el producto pero hubo problemas con la imagen";

                    }
                }
            }
            return Json(new { operacionExitosa = operacion_exitosa, idGenerado = oProducto.Id_Producto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
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



        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Producto().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //Se crean los metodos para listar, registar, editar y eliminar CARRUSEL
        #region CARRUSEL
        [HttpGet]
        public JsonResult ListarCarrusel()
        {
            List<Carrusel> oLista = new List<Carrusel>();
            oLista = new CN_Carrusel().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCarrusel(string objeto, HttpPostedFileBase archivoImagen)
        {
            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            Carrusel oCarrusel = new Carrusel();

            oCarrusel = JsonConvert.DeserializeObject<Carrusel>(objeto);


            if (oCarrusel.Id_Carrusel == 0)
            {

                int idCarruselGenerado = new CN_Carrusel().Registrar(oCarrusel, out mensaje);

                if (idCarruselGenerado != 0)
                {
                    oCarrusel.Id_Carrusel = idCarruselGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {

                operacion_exitosa = new CN_Carrusel().Editar(oCarrusel, out mensaje);
            }

            if (operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    string ruta_guardar = Server.MapPath(ConfigurationManager.AppSettings["FotosCarrusel"]);
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oCarrusel.Id_Carrusel.ToString(), extension);


                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }

                    if (guardar_imagen_exito)
                    {

                        oCarrusel.Ruta_Imagen = ConfigurationManager.AppSettings["FotosCarrusel"];
                        oCarrusel.Nombre_Imagen = nombre_imagen;
                        bool rspta = new CN_Carrusel().GuardarDatosImagen(oCarrusel, out mensaje);

                    }
                    else
                    {
                        mensaje = "Se guardo el Carrusel pero hubo problemas con la imagen";

                    }
                }
            }
            return Json(new { operacionExitosa = operacion_exitosa, idGenerado = oCarrusel.Id_Carrusel, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
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


        [HttpPost]
        public JsonResult EliminarCarrusel(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrusel().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region BARRIO
        [HttpGet]
        public JsonResult ListarBarrio()
        {
            List<Barrios> oLista = new List<Barrios>();

            oLista = new CN_VentaExterna().ListarBarrio();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegistrarBarrio(Barrios objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            // Obtener la lista de barrios existentes
            List<Barrios> listaBarrios = new CN_Barrio().ListarBarrio();

            // Verificar si el Código Postal ya existe en la lista
            bool existe = listaBarrios.Any(b => b.Codigo_Postal == objeto.Codigo_Postal);

            if (!existe) // Si no existe, registrar
            {
                resultado = new CN_Barrio().RegistrarBarrio(objeto, out mensaje);
            }
            else // Si existe, editar
            {
                resultado = new CN_Barrio().EditarBarrio(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarBarrio(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Barrio().EliminarBarrio(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

}





