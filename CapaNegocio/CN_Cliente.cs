using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Cliente objCapaDato = new CD_Cliente();

        //LISTAR
        public List<Cliente> Listar()
        {
            return objCapaDato.Listar();
        }



        //REGISTRAR
        public int Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            //validamos datos. 
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellido) || string.IsNullOrWhiteSpace(obj.Apellido))
            {
                Mensaje = "El apellido del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del cliente no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {

                obj.Clave = CN_Recursos.ConvertirSha256(obj.Clave); //encriptamos la calve con el metodo del recurso
                return objCapaDato.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;
            }


        }



        //CAMBIAR CLAVE
        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idcliente, nuevaclave, out Mensaje);
        }



        //RESTABLECER CLAVE
        public bool RestablecerClave(int idcliente, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            //obtiene clave autogenerada
            string nuevaclave = CN_Recursos.GenerarClave();

            bool resultado = objCapaDato.RestablecerClave(idcliente, CN_Recursos.ConvertirSha256(nuevaclave), out Mensaje);

            if (resultado)
            {
                string asunto = "Contraseña Reestablecida";
                string mensaje_correo = "<h3>Su cuenta fue restablecida correctamente</h3> </br> <p>Su contraseña para acceder ahora es: !clave! </p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaclave); //reemplazamos 

                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensaje_correo);

                //Validamos respuesta del correo 
                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo.";
                    return false;
                }

            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña.";
                return false;
            }
        }
    }
}
