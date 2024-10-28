using System;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Carrito
    {
        private Conexion conexion = new Conexion();

        public bool ExisteCarrito(int Id_Cliente, int Id_Producto)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ExisteCarrito", oconexion);
                    cmd.Parameters.AddWithValue("IdCliente", Id_Cliente);
                    cmd.Parameters.AddWithValue("IdProducto", Id_Producto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
            }
            catch (Exception ex)
            {
                // Se podría loguear el error o lanzar la excepción
                resultado = false;
            }

            return resultado;
        }

        public bool OperacionCarrito(int Id_Cliente, int Id_Producto, bool sumar, out string Mensaje)
        {
            bool resultado = true;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_OperacionCarrito", oconexion);
                    cmd.Parameters.AddWithValue("IdCliente", Id_Cliente);
                    cmd.Parameters.AddWithValue("IdProducto", Id_Producto);
                    cmd.Parameters.AddWithValue("Sumar", sumar);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
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
                Mensaje = ex.Message; // Capturar el mensaje de error
            }

            return resultado;
        }

        public int CantidadEnCarrito(int Id_Cliente)
        {
            int resultado = 0;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from carrito where Id_Cliente = @Id_Cliente", oconexion);
                    cmd.Parameters.AddWithValue("@Id_Cliente", Id_Cliente);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                resultado = 0;

            }

            return resultado;
        }
    }
}
