using System;
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
        public JsonResult EliminarUsuario(int id) { 
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
        public JsonResult VistaDashBoard() { 
            DashBoard objeto = new CN_Reporte().VerDashBoard();


            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);
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
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Total_Producto", typeof(int));
            dt.Columns.Add("Total_Pago", typeof(decimal));
            dt.Columns.Add("Id_Venta", typeof(string));

            foreach (Reporte rp in oLista)
            {
                dt.Rows.Add(new object[] {
                    rp.FechaVenta,
                    rp.Clientes,
                    rp.Productos,
                    rp.Precio,
                    rp.Total_Producto,
                    rp.Total_Pago,
                    rp.Id_Venta
                });
            }

            string rutaPlantilla = Server.MapPath("~/Templates/Royal Tech - Historial_de_Ventas.xlsx");

            using (XLWorkbook wb = new XLWorkbook(rutaPlantilla))
            {
                var worksheet = wb.Worksheet(1); // Selecciona la hoja de trabajo


                // Definir el rango de formato para copiar
                var rangoFormato = worksheet.Range("B15:H15");

                int filaInicio = 16; // Empieza a insertar desde la fila 16

                foreach (DataRow row in dt.Rows)
                {
                    // Copiar formato
                    rangoFormato.CopyTo(worksheet.Row(filaInicio));

                    // Insertar los datos solo en las columnas correctas (B a H)
                    worksheet.Cell("B" + filaInicio).Value = row["FechaVenta"].ToString();
                    worksheet.Cell("C" + filaInicio).Value = row["Clientes"].ToString();
                    worksheet.Cell("D" + filaInicio).Value = row["Productos"].ToString();
                    worksheet.Cell("E" + filaInicio).Value = Convert.ToDecimal(row["Precio"]);
                    worksheet.Cell("F" + filaInicio).Value = Convert.ToInt32(row["Total_Producto"]);
                    worksheet.Cell("G" + filaInicio).Value = Convert.ToDecimal(row["Total_Pago"]);
                    worksheet.Cell("H" + filaInicio).Value = Convert.ToInt32(row["Id_Venta"]);

                    // Establecer formato de números (Precio y Total_Pago con 2 decimales)
                    worksheet.Cell("E" + filaInicio).Style.NumberFormat.Format = "#,##0.00";
                    worksheet.Cell("G" + filaInicio).Style.NumberFormat.Format = "#,##0.00";
                    worksheet.Cell("F" + filaInicio).Style.NumberFormat.Format = "#,##0";

                    // Opcional: Si también necesitas restablecer bordes manualmente
                    worksheet.Range("B" + filaInicio + ":H" + filaInicio).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Range("B" + filaInicio + ":H" + filaInicio).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    filaInicio++;
                }

                // Limpia la columna A de filas no deseadas (si es necesario)
                worksheet.Range("A16:A100").Clear(); // Ajusta el rango si es necesario para más filas

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx");
                }
            }
        }
        #endregion

        #region Cliente y Localidad
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
        public JsonResult RegistrarVenta(Venta venta)
        {
            string mensaje = string.Empty;

            // Verificar que el objeto Venta y sus sub-objetos no sean nulos
            if (venta == null)
    {
                return Json(new { success = false, message = "Error: Algunos datos obligatorios están incompletos." });
            }

            // Realizar lógica de inserción solo si es una nueva venta (Id_Venta == 0)
            if (venta.Id_Venta == 0)
            {
                try
                {
                    // Registrar la venta
                    var resultado = new CN_VentaExterna().RegistrarVenta(venta, out mensaje);

                    // Verificar si la operación fue exitosa
                    if (resultado is int nuevaVentaId && nuevaVentaId > 0)
                    {
                        return Json(new { success = true, message = "Venta creada con éxito.", nuevaVentaId });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error al registrar la venta: " + mensaje });
                    }
                }
                catch (Exception ex)
                {
                    // Capturar y devolver cualquier error que ocurra durante la inserción
                    return Json(new { success = false, message = "Excepción al registrar la venta: " + ex.Message });
                }
            }
            else
            {
                // Si el Id_Venta es distinto de 0, significa que ya existe y se ignora la operación
                return Json(new { success = false, message = "La venta ya existe y no se puede registrar de nuevo." });
            }
        }

        #endregion

    }
}