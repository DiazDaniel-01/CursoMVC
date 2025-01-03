﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuario()
        {
            return View();
        }
        public ActionResult Cliente()
        {
            return View();
        }
        public ActionResult Localidad()
        {
            return View();
        }
        public ActionResult CrearVenta()
        {
            // Obtener el último cliente y localidad guardados en la sesión
            int? ultimoClienteId = Session["UltimoClienteId"] as int?;
            int? ultimaLocalidadId = Session["UltimaLocalidadId"] as int?;

            ViewBag.UltimoClienteId = ultimoClienteId;
            ViewBag.UltimaLocalidadId = ultimaLocalidadId;

            return View();
        }

        #region Usuario
        [HttpGet]
        public JsonResult ListarUsuario()
        {

            List<Usuario> oLista = new List<Usuario>();

            oLista = new CN_Usuarios().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario objeto)
        {
            object resultado;
            string mensaje = string.Empty;
            if (objeto.Id_Usuario == 0)
            {
                resultado = new CN_Usuarios().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Usuarios().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Reportes
        [HttpGet]
        public JsonResult ListaReporte(string FechaInicio, string FechaFin, string Id_Venta)
        {
            List<Reporte> oLista = new List<Reporte>();

            oLista = new CN_Reporte().Venta(FechaInicio, FechaFin, Id_Venta);


            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult VistaDashBoard()
        {
            DashBoard objeto = new CN_Reporte().VerDashBoard();


            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarVenta(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Reporte().EliminarVenta(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportarVenta(string FechaInicio, string FechaFin, string Id_Venta)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_Reporte().Venta(FechaInicio, FechaFin, Id_Venta);

            DataTable dt = new DataTable();
            dt.Locale = new System.Globalization.CultureInfo("es-AR");
            dt.Columns.Add("FechaVenta", typeof(string));
            dt.Columns.Add("Clientes", typeof(string));
            dt.Columns.Add("Productos", typeof(string));
            dt.Columns.Add("PrecioUnitario", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total_Pago", typeof(decimal));
            dt.Columns.Add("Id_Venta", typeof(string));

            foreach (Reporte rp in oLista)
            {
                dt.Rows.Add(new object[] {
                    rp.FechaVenta,
                    rp.Clientes,
                    rp.Productos,
                    rp.PrecioUnitario,
                    rp.Cantidad,
                    rp.Total_Pago,
                    rp.Id_Venta
                });
            }

            string rutaPlantilla = Server.MapPath("~/Templates/Royal Tech - Historial_de_Ventas.xlsx");

            using (XLWorkbook wb = new XLWorkbook(rutaPlantilla))
            {
                var worksheet = wb.Worksheet(1); // Selecciona la hoja de trabajo

                // Asigna las fechas a las celdas B11 y C11
                worksheet.Cell("B11").Value = FechaInicio;
                worksheet.Cell("C11").Value = FechaFin;

                // Aplica el formato a las fechas
                worksheet.Cell("B11").Style.Font.Bold = true;
                worksheet.Cell("C11").Style.Font.Bold = true;

                // Ajustar automáticamente el ancho de las columnas para las celdas de encabezado
                worksheet.Columns("B:H").AdjustToContents();

                // Aplicar formato a la fila de encabezado (B13:H13)
                worksheet.Range("B13:H13").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Range("B13:H13").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("B13:H13").Style.Font.Bold = true;

                // Define el rango de formato para copiar
                var rangoFormato = worksheet.Range("B13:H13");

                int filaInicio = 14; // Empieza a insertar desde la fila 14

                foreach (DataRow row in dt.Rows)
                {
                    // Copiar formato
                    rangoFormato.CopyTo(worksheet.Row(filaInicio));

                    // Insertar los datos solo en las columnas correctas (B a H)
                    worksheet.Cell("B" + filaInicio).Value = row["FechaVenta"].ToString();
                    worksheet.Cell("C" + filaInicio).Value = row["Clientes"].ToString();
                    worksheet.Cell("D" + filaInicio).Value = row["Productos"].ToString();
                    worksheet.Cell("E" + filaInicio).Value = Convert.ToDecimal(row["PrecioUnitario"]);
                    worksheet.Cell("F" + filaInicio).Value = Convert.ToInt32(row["Cantidad"]);
                    worksheet.Cell("G" + filaInicio).Value = Convert.ToDecimal(row["Total_Pago"]);
                    worksheet.Cell("H" + filaInicio).Value = Convert.ToInt32(row["Id_Venta"]);


                    filaInicio++;
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx");
                }
            }
        }

        #endregion

        #region Cliente y Localidad
        [HttpGet]
        public JsonResult ListarClientes()
        {
            List<Cliente> oLista = new List<Cliente>();

            oLista = new CN_VentaExterna().ListarCliente();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarLocalidad()
        {
            List<Localidad> oLista = new List<Localidad>();

            oLista = new CN_VentaExterna().ListarLocalidad();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarBarrio()
        {
            List<Barrios> oLista = new List<Barrios>();

            oLista = new CN_VentaExterna().ListarBarrio();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Localidadsinventa()
        {
            List<Localidad> oLista = new List<Localidad>();

            oLista = new CN_VentaExterna().Localidadsinventa();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Clientessinventas()
        {
            List<Cliente> oLista = new List<Cliente>();

            oLista = new CN_VentaExterna().Clientessinventas();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
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

                // Verificar si la inserción fue exitosa y guardar el ID en la sesión
                if (resultado is int nuevoClienteId && nuevoClienteId > 0)
                {
                    Session["UltimoClienteId"] = nuevoClienteId;
                }
            }
            else
            {
                return View();
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
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

                // Verificar si la inserción fue exitosa y guardar el ID en la sesión
                if (resultado is int nuevaLocalidadId && nuevaLocalidadId > 0)
                {
                    Session["UltimaLocalidadId"] = nuevaLocalidadId;
                }
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

        [HttpPost]
        public JsonResult EliminarLocalidad(int id)
        {
            bool respuesta = false;

            respuesta = new CN_VentaExterna().EliminarLocalidad(id);

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCliente(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_VentaExterna().EliminarCliente(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}