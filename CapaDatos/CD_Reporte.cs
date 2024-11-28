using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Reporte
    {

        public List<Reporte> Venta(string FechaInicio, string FechaFin, string Id_Venta)
        {
            List<Reporte> lista = new List<Reporte>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteVentas", oconexion);
                    cmd.Parameters.AddWithValue("FechaInicio", FechaInicio);
                    cmd.Parameters.AddWithValue("FechaFin", FechaFin);
                    cmd.Parameters.AddWithValue("Id_Venta", Id_Venta);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Reporte()
                                {
                                    Id_Venta = dr["Id_Venta"].ToString(),
                                    Clientes = dr["Cliente"].ToString(),
                                    Barrio = dr["Barrio"].ToString(),
                                    Productos = dr["Producto"].ToString(),
                                    Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                    PrecioUnitario = Convert.ToDecimal(dr["PrecioUnitario"], new CultureInfo("es-AR")),
                                    FechaVenta = dr["FechaVenta"].ToString(),
                                    Total_Pago = Convert.ToDecimal(dr["Total_Pago"], new CultureInfo("es-AR"))
                                }
                            );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Reporte>();
            }

            return lista;
        }


        public DashBoard VerDashBoard()
        {
            DashBoard objeto = new DashBoard();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_ReporteDashboardTotales", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objeto = new DashBoard()
                            {
                                TotalClientes = Convert.ToInt32(dr["TotalClientes"]),
                                TotalVentas = Convert.ToInt32(dr["TotalVentas"]),
                                TotalProductos = Convert.ToInt32(dr["TotalProductos"])
                            };
                        }
                    }
                }
            }

            catch
            {
                objeto = new DashBoard();
            }

            return objeto;
        }

        public bool EliminarVenta(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    {
                        SqlCommand cmd = new SqlCommand("sp_EliminarVentaCompleta", oconexion);
                        cmd.Parameters.AddWithValue("@Id_Venta", id);
                        cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        cmd.CommandType = CommandType.StoredProcedure;
                        oconexion.Open();
                        cmd.ExecuteNonQuery();
                        resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                        Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

    }
}
