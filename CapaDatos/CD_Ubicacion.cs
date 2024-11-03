using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;

using System.Data.SqlClient;
using System.Data;


namespace CapaDatos
{
    public class CD_Ubicacion
    {
        public List<Localidad> ObtenerLocalidad()
        {
            List<Localidad> lista = new List<Localidad>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from LOCALIDAD";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Localidad()
                                {
                                    Departamento = dr["Departamento"].ToString(),
                                    Referencia = dr["Referencia"].ToString(),
                                    
                                }
                            );
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


    }
}
