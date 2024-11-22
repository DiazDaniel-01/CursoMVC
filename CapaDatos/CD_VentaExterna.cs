using CapaEntidad;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_VentaExterna
    {
        public int InsertarCliente(Cliente obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertarCliente", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", obj.Apellido);
                    cmd.Parameters.AddWithValue("@Celular", obj.Celular);

                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje += ex.Message;
            }
            return idautogenerado;
        }

        public int InsertarLocalidad(Localidad obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertarLocalidad", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Barrio", obj.Barrio);
                    cmd.Parameters.AddWithValue("@CodigoPostal", obj.Codigo_Postal);
                    cmd.Parameters.AddWithValue("@Departamento", obj.Departamento);
                    cmd.Parameters.AddWithValue("@Calle", obj.Calle);
                    cmd.Parameters.AddWithValue("@Calle2", obj.Calle_2);
                    cmd.Parameters.AddWithValue("@Piso", obj.Piso);
                    cmd.Parameters.AddWithValue("@Referencia", obj.Referencia);
                    cmd.Parameters.AddWithValue("@CostoEnvio", obj.Costo_Envio);

                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje += ex.Message;
            }
            return idautogenerado;
        }


        public List<Cliente> ListarCliente()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT Id_Cliente, Nombre, Celular FROM CLIENTE";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cliente()
                            {
                                Id_Cliente = Convert.ToInt32(dr["Id_Cliente"]),
                                Nombre = dr["Nombre"].ToString(),
                                Celular = dr["Celular"].ToString()
                            });
                        }
                    }
                }
            }

            catch
            {
                lista = new List<Cliente>();
            }
            return lista;
        }

        public List<Localidad> ListarLocalidad()
        {
            List<Localidad> lista = new List<Localidad>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT Id_Localidad, Barrio FROM LOCALIDAD";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Localidad()
                            {
                                Id_Localidad = Convert.ToInt32(dr["Id_Localidad"]),
                                Barrio = dr["Barrio"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Localidad>();
            }
            return lista;
        }

        public int RegistrarVenta(Venta obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("InsertarVenta", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.AddWithValue("IdCliente", obj.oCliente.Id_Cliente);
                    cmd.Parameters.AddWithValue("IdLocalidad", obj.oLocalidad.Id_Localidad);
                    cmd.Parameters.AddWithValue("FechaVenta", obj.Fecha_Venta);
                    cmd.Parameters.AddWithValue("TotalPago", obj.Total_Pago);
                    cmd.Parameters.AddWithValue("TotalProductos",obj.Total_Productos);
                   

                    // Parámetros de salida
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    // Obtener valores de salida
                    idautogenerado = cmd.Parameters["Resultado"].Value != DBNull.Value
                        ? Convert.ToInt32(cmd.Parameters["Resultado"].Value)
                        : 0;
                    Mensaje = cmd.Parameters["Mensaje"].Value?.ToString() ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = "Error al registrar la venta: " + ex.Message;
            }

            return idautogenerado;
        }
    }
}
