using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;
using System.Security.Permissions;

namespace CapaDatos
{
    public class CD_Producto
    {


        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("select p.Id_Producto, p.Nombre, p.Descripcion,");
                    sb.AppendLine("c.Id_Categoria, c.Nombre[NomCategoria],");
                    sb.AppendLine("p.Precio, p.Ruta_Imagen, p.Nombre_Imagen, p.Activo");
                    sb.AppendLine("from PRODUCTO p");
                    sb.AppendLine("inner join CATEGORIA c on c.Id_Categoria = p.Id_Categoria");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto()
                            {
                                Id_Producto = Convert.ToInt32(dr["Id_Producto"]),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                oCategoria = new Categoria() { Id_Categoria = Convert.ToInt32(dr["Id_Categoria"]), Nombre = dr["NomCategoria"].ToString() },
                                Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-AR")),
                                Ruta_Imagen = dr["Ruta_Imagen"].ToString(),
                                Nombre_Imagen = dr["Nombre_Imagen"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Producto>();
            }
            return lista;
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarProducto", oconexion);
                    cmd.Parameters.AddWithValue("Id_Categoria", obj.oCategoria.Id_Categoria);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
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

        public bool Editar(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", oconexion);
                    cmd.Parameters.AddWithValue("Id_Producto", obj.Id_Producto);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Id_Categoria", obj.oCategoria.Id_Categoria);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
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

        public bool GuardarDatosImagen(Producto obj, out string Mensaje) { 
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "update PRODUCTO set Ruta_Imagen = @Ruta_Imagen, Nombre_Imagen = @Nombre_Imagen where Id_Producto = @Id_Producto";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@Ruta_Imagen", obj.Ruta_Imagen);
                    cmd.Parameters.AddWithValue("@Nombre_Imagen", obj.Nombre_Imagen);
                    cmd.Parameters.AddWithValue("@Id_Producto", obj.Id_Producto);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar imagen";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje += ex.Message;
            }
            return (resultado);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarProducto", oconexion);
                    cmd.Parameters.AddWithValue("Id_Producto", id);
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
    
    public List<Producto> ListarProductoporCategorias(int idcategoria)
    {
        List<Producto> lista = new List<Producto>();

        try
        {
            using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("SELECT DISTINCT p.Id_Producto, p.Descripcion FROM PRODUCTO p");
                sb.AppendLine("INNER JOIN CATEGORIA c ON c.Id_Categoria = p.Id_Categoria AND c.Activo = 1");
                sb.AppendLine("WHERE c.Id_Categoria = iif(@Id_Categoria = 0, c.Id_Categoria = @Id_Categoria");


                SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
                cmd.Parameters.AddWithValue("@idcategoria", idcategoria);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Producto()
                        {
                            Id_Producto = Convert.ToInt32(dr["Id_Producto"]),

                            Descripcion = dr["Descripcion"].ToString(),

                        });
                    }
                }
            }
        }

        catch
        {
            lista = new List<Producto>();
        }
        return lista;
    }
}
}