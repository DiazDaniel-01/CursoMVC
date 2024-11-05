using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_ExtraP
    {
        public List<Extra_Producto> ListarExtraP()
        {
            List<Extra_Producto> lista = new List<Extra_Producto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("Select e.Id_ExtraP, e.Ingrediente_Extra, e.Precio, e.Id_Producto,");
                    sb.AppendLine("p.Id_Producto, p.Nombre");
                    sb.AppendLine("from EXTRA_PRODUCTO e");
                    sb.AppendLine("inner join PRODUCTO p on p.Id_Producto = e.Id_Producto");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Extra_Producto()
                            {
                                Id_ExtraP = Convert.ToInt32(dr["Id_ExtraP"]),
                                oProducto = new Producto() { Id_Producto = Convert.ToInt32(dr["Id_Producto"]), Nombre = dr["Nombre"].ToString() },
                                Ingrediente_Extra = dr["Ingrediente_Extra"].ToString(),
                                Precio = Convert.ToDecimal(dr["Precio"])
                            });
                        }
                    }
                }
            }

            catch
            {
                lista = new List<Extra_Producto>();
            }
            return lista;
        }

        public int RegistrarExtraP(Extra_Producto obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarExtraP", oconexion);
                    cmd.Parameters.AddWithValue("Id_Producto", obj.oProducto.Id_Producto);
                    cmd.Parameters.AddWithValue("Ingrediente_Extra", obj.Ingrediente_Extra);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();
                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje += ex.Message;
            }
            return idautogenerado;
        }

        public bool EditarExtraP(Extra_Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarExtraP", oconexion);
                    cmd.Parameters.AddWithValue("Id_ExtraP", obj.Id_ExtraP);
                    cmd.Parameters.AddWithValue("Id_Producto", obj.oProducto.Id_Producto);
                    cmd.Parameters.AddWithValue("Ingrediente_Extra", obj.Ingrediente_Extra);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool EliminarExtraP(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarExtraP", oconexion);
                    cmd.Parameters.AddWithValue("Id_ExtraP", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
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
