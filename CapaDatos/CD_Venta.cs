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
    public class CD_Venta
    {
        //Registrar VENTA 
        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn)) //conexion 
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarVenta", oconexion); //llamamos al meteodo creado y pasamos parametros
                    cmd.Parameters.AddWithValue("IdCliente", obj.IDCliente);
                    cmd.Parameters.AddWithValue("TotalProducto ", obj.TotalProducto);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("Contacto", obj.Contacto);
                    cmd.Parameters.AddWithValue("IdLocalidad", obj.IDLocalidad);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("Direccion", obj.Direccion);
                    cmd.Parameters.AddWithValue("IdTransaccion", obj.IDTransaccion);
                    cmd.Parameters.AddWithValue("DetalleVenta", DetalleVenta);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure; //indicamos que es un store procedure 

                    oconexion.Open();

                    cmd.ExecuteNonQuery(); //ejecutamos comando 

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta;
        }


        //LISTAR COMPRAS 
        public List<DetalleVenta> ListarCompras(int idcliente)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    string query = "select * from fn_ListarCompra(@idcliente)";


                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text; //indicamos que el comando va a ser de tipo texto 

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader()) //nos da lectura al comando de la ejecucion 
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new DetalleVenta()
                                {
                                    oProducto = new Producto()
                                    {
                                        Nombre = dr["Nombre"].ToString(),
                                        Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("en-US")),
                                        RutaImagen = dr["RutaImagen"].ToString(),
                                        NombreImagen = dr["NombreImagen"].ToString()
                                    },
                                    Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                    Total = Convert.ToDecimal(dr["Total"], new CultureInfo("en-US")),
                                    IDTransaccion = dr["IDTransaccion"].ToString()
                                });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<DetalleVenta>();
            }


            return lista;
        }
    }
}
