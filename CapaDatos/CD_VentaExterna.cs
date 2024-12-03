using CapaEntidad;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_VentaExterna
    {
        public List<Cliente> ListarCliente()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM CLIENTE";
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
                                Apellido = dr["Apellido"].ToString(),
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
                    string query = @"SELECT L.Id_Localidad, B.Codigo_Postal, B.Barrio, B.Costo_Envio, L.Departamento, L.Calle, L.Calle_2, L.Piso, L.Referencia
                                FROM 
                                    Localidad L
                                INNER JOIN 
                                    Barrio B
                                ON 
                                    L.Codigo_Postal = B.Codigo_Postal;";
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
                                oBarrios = new Barrios() { 
                                    Codigo_Postal = Convert.ToInt32(dr["Codigo_Postal"]), 
                                    Barrio = dr["Barrio"].ToString(), 
                                    Costo_Envio = Convert.ToDecimal(dr["Costo_Envio"], new CultureInfo("es-AR")) 
                                },
                                Departamento = dr["Departamento"].ToString(),
                                Calle = dr["Calle"].ToString(),
                                Calle_2 = dr["Calle_2"].ToString(),
                                Piso = Convert.ToInt32(dr["Piso"]),
                                Referencia = dr["Referencia"].ToString()
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

        public List<Barrios> ListarBarrio()
        {
            List<Barrios> lista = new List<Barrios>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT * FROM BARRIO";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Barrios()
                            {
                                Codigo_Postal = Convert.ToInt32(dr["Codigo_Postal"]),
                                Barrio = dr["Barrio"].ToString(),
                                Costo_Envio = Convert.ToDecimal(dr["Costo_Envio"]),
                            });
                        }
                    }
                }
            }

            catch
            {
                lista = new List<Barrios>();
            }
            return lista;
        }

        public List<Localidad> Localidadsinventa()
        {
            List<Localidad> lista = new List<Localidad>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = @"SELECT 
                        L.Id_Localidad,
                        B.Codigo_Postal,
                        B.Barrio,
                        B.Costo_Envio,
                        L.Departamento,
                        L.Calle,
                        L.Calle_2,
                        L.Piso,
                        L.Referencia
                    FROM 
                        Localidad L
                    INNER JOIN 
                        Barrio B
                    ON 
                        L.Codigo_Postal = B.Codigo_Postal
                    LEFT JOIN 
                        VENTA V
                    ON 
                        L.Id_Localidad = V.Id_Localidad
                    WHERE 
                        V.Id_Venta IS NULL;
                    ";
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
                                oBarrios = new Barrios()
                                {
                                    Codigo_Postal = Convert.ToInt32(dr["Codigo_Postal"]),
                                    Barrio = dr["Barrio"].ToString(),
                                    Costo_Envio = Convert.ToDecimal(dr["Costo_Envio"], new CultureInfo("es-AR"))
                                },
                                Departamento = dr["Departamento"].ToString(),
                                Calle = dr["Calle"].ToString(),
                                Calle_2 = dr["Calle_2"].ToString(),
                                Piso = Convert.ToInt32(dr["Piso"]),
                                Referencia = dr["Referencia"].ToString()
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
        public List<Cliente> Clientessinventas()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = @"
                        SELECT 
                            C.Id_Cliente, C.Nombre, C.Apellido, C.Celular
                        FROM 
                            CLIENTE C
                        LEFT JOIN 
                            VENTA V
                        ON 
                            C.Id_Cliente = V.Id_Cliente
                        WHERE 
                            V.Id_Cliente IS NULL";

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
                                Apellido = dr["Apellido"].ToString(),
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
                    cmd.Parameters.AddWithValue("@Codigo_Postal", obj.oBarrios.Codigo_Postal);
                    cmd.Parameters.AddWithValue("@Departamento", obj.Departamento);
                    cmd.Parameters.AddWithValue("@Calle", obj.Calle);
                    cmd.Parameters.AddWithValue("@Calle2", obj.Calle_2);
                    cmd.Parameters.AddWithValue("@Piso", obj.Piso);
                    cmd.Parameters.AddWithValue("@Referencia", obj.Referencia);

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

        public bool EliminarLocalidad(int id)
        {
            bool resultado = false;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarLocalidad", oconexion);
                    cmd.Parameters.AddWithValue("Id_Localidad", id); // Agrega el parámetro del procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open(); // Abre la conexión
                    int filasAfectadas = cmd.ExecuteNonQuery(); // Ejecuta el procedimiento almacenado

                    // Verifica si se eliminó al menos un registro
                    resultado = filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.WriteLine($"Error: {ex.Message}");
                resultado = false;
            }

            return resultado;
        }
        public bool EliminarCliente(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("delete top (1) from Cliente where Id_Cliente = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public int InsertarVenta(VentaViewModel obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertarVenta", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id_Cliente", obj.oCliente.Id_Cliente);
                    cmd.Parameters.AddWithValue("@Id_Localidad", obj.oLocalidad.Id_Localidad);
                    cmd.Parameters.AddWithValue("@Fecha_Venta", obj.Fecha_Venta);
                    cmd.Parameters.AddWithValue("@Total_Pago", obj.Total_Pago);
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

        public int InsertarDetalleVenta(ProductoViewModel obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertarDetalleVenta", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id_Venta", obj.oVentaViewModel.Id_VentaViewModel);
                    cmd.Parameters.AddWithValue("@Id_Producto", obj.oProducto.Id_Producto);
                    cmd.Parameters.AddWithValue("@Cantidad", obj.Cantidad);
                    cmd.Parameters.AddWithValue("@Observacion", obj.Observacion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", obj.PrecioUnitario);
                    cmd.Parameters.AddWithValue("@Subtotal", obj.Subtotal);
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
    }
}
