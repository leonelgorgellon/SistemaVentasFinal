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
        //METODO PARA OBTENER LOS LOCALES 
        public List<Local_Negocio> ObtenerLocal()
        {
            List<Local_Negocio> lista = new List<Local_Negocio>();

            //controlamos los posibles errores 
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from LOCAL_NEGOCIO";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    //indicamos a comando cmd que tipo de comando va a ejecutar. de tipo texto
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())    //lee todos los datos de la ejecucion de mi consulta, osea de mi query 
                    {
                        while (dr.Read())  //mientras lea los datos, los guarde en mi lista. 
                        {
                            lista.Add(
                                new Local_Negocio()
                                {
                                    IDLocal = dr["IDLocal"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    
                                }
                                );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Local_Negocio>();
            }

            return lista;
        }



        //METODO PARA OBTENER LAS PROVINCIAS 
        public List<Provincia> ObtenerProvincia(string idlocal)
        {
            List<Provincia> lista = new List<Provincia>();

            //controlamos los posibles errores 
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from PROVINCIA where IDLocal = @idlocal";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idlocal", idlocal);
                    //indicamos a comando cmd que tipo de comando va a ejecutar. de tipo texto
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())    //lee todos los datos de la ejecucion de mi consulta, osea de mi query 
                    {
                        while (dr.Read())  //mientras lea los datos, los guarde en mi lista. 
                        {
                            lista.Add(
                                new Provincia()
                                {
                                    IDProvincia = dr["IDProvincia"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                }
                                );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Provincia>();
            }

            return lista;
        }




        //METODO PARA OBTENER LAS LOCALIDADES 
        public List<Localidad> ObtenerLocalidad(string idlocal, string idprovincia)
        {
            List<Localidad> lista = new List<Localidad>();

            //controlamos los posibles errores 
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from LOCALIDAD where IDProvincia = @idprovincia and IDLocal = @idlocal";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idprovincia", idprovincia);
                    cmd.Parameters.AddWithValue("@idlocal", idlocal);
                    //indicamos a comando cmd que tipo de comando va a ejecutar. de tipo texto
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())    //lee todos los datos de la ejecucion de mi consulta, osea de mi query 
                    {
                        while (dr.Read())  //mientras lea los datos, los guarde en mi lista. 
                        {
                            lista.Add(
                                new Localidad()
                                {
                                    IDLocalidad = dr["IDProvincia"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
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
