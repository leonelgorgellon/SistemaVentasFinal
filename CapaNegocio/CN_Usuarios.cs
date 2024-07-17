using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDato = new CD_Usuarios(); // de esta forma accedemos a los metodos de la capa datos de USUARIOS

        //CREAMOS METODO PARA OBTENER LA LISTA DE USUARIO QUE TRAEMOS DE LA CAPA DE DATOS USUARIO 
        //LISTAR 
        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }



        //REGISTRAR
        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            //validamos datos y respondemos con un msj. 
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {

                //enviamos correo al usuario con contraseña para acceder al sistema. 
                string clave = CN_Recursos.GenerarClave();

                string asunto = "Creación de cuenta en LaCavaIT";
                string mensaje_correo = "<h3>Su cuenta fue creada correctamente</h3> </br> <p>Su contraseña para acceder es: !clave! </p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", clave); //reemplazamos 

                bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    obj.Clave = CN_Recursos.ConvertirSha256(clave); //encriptamos la calve con el metodo del recurso
                    return objCapaDato.Registrar(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "No se puede enviar el correo";
                    return 0;
                }
            }
            else
            {
                return 0;
            }


        }




        //EDITAR
        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            //para que los campos no vengan vacios 
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }



        //ELIMINAR
        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }




        //CAMBIAR CLAVE
        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idusuario, nuevaclave, out Mensaje);
        }



        //RESTABLECER CLAVE
        public bool RestablecerClave(int idusuario, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            //obtiene clave autogenerada
            string nuevaclave = CN_Recursos.GenerarClave();

            bool resultado = objCapaDato.RestablecerClave(idusuario, CN_Recursos.ConvertirSha256(nuevaclave), out Mensaje);

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
