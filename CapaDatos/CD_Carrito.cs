using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Carrito
    {

        //METODO PARA VER SI EXISTE ALGUN PRODUCTO DENTRO DEL CARRITO DE UN CLIENTE 
        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ExisteCarrito", oconexion); //llamamos al meteodo creado y pasamos parametros
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery(); //ejecutamos comando 

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }



        //METODO PARA REALIZAR LA OPERACION PARA EL CARRITO 
        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string Mensaje)
        {
            bool resultado = true;

            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_OperacionCarrito", oconexion); //llamamos al meteodo creado y pasamos parametros
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.AddWithValue("Sumar", sumar);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery(); //ejecutamos comando 

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




        //METODO PARA OBTENER LA CANTIDAD EN MI CARRITO 
        public int CantidadEnCarrito(int idcliente)
        {
            int resultado = 0;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("select count (*) from CARRITO where IDCliente = @IdCliente", oconexion);
                    cmd.Parameters.AddWithValue("@IdCliente", idcliente);
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




        //METODO PARA OBTENER LA LISTA DE PRODUCTOS DEL CARRITO DEL CLIENTE 
        public List<Carrito> ListarProductos(int idcliente)
        {
            List<Carrito> lista = new List<Carrito>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    string query = "select * from fn_obtenerCarritoCliente(@idcliente)";


                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text; //indicamos que el comando va a ser de tipo texto 

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader()) //nos da lectura al comando de la ejecucion 
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Carrito()
                                {
                                    oProducto = new Producto()
                                    {
                                        IDProducto = Convert.ToInt32(dr["IDProducto"]),
                                        Nombre = dr["Nombre"].ToString(),
                                        oMarca = new Marca() { Descripcion = dr["DesMarca"].ToString() },
                                        Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("en-US")),
                                        RutaImagen = dr["RutaImagen"].ToString(),
                                        NombreImagen = dr["NombreImagen"].ToString()
                                    },
                                    Cantidad = Convert.ToInt32(dr["Cantidad"])
                                });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Carrito>();
            }


            return lista;
        }



        //METODO PARA ELIMINAR PRODUCTOS DEL CARRITO DEL CLIENTE 
        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarCarrito", oconexion); //llamamos al meteodo creado y pasamos parametros
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery(); //ejecutamos comando 

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }
    }
}
