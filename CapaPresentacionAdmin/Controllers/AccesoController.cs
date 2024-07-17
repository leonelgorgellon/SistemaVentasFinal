using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;

using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }


        //METODO PARA INICIAR SESION 
        [HttpPost] 
        public ActionResult Index(string correo, string clave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            //validamos usuario con las credenciales que esta recibiendo 
            if (oUsuario == null)
            {
                ViewBag.Error = "Correo o contraseña no correcta";
                return View();
            }
            else
            {
                if (oUsuario.Reestablecer)
                {
                    TempData["IDUsuario"] = oUsuario.IDUsuario;

                    return RedirectToAction("CambiarClave", "Acceso");
                }

                //creamos autenticacion con el correo del usuario para el logeo. 
                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);



                ViewBag.Error = null;

                return RedirectToAction("Index", "Home");
            }


        }




        //METODO CAMBIAR CLAVE
        [HttpPost]
        public ActionResult CambiarClave(string idusuario, string claveactual, string nuevaclave, string confirmarclave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.IDUsuario == int.Parse(idusuario)).FirstOrDefault();

            if (oUsuario.Clave != CN_Recursos.ConvertirSha256(claveactual))
            {
                //para cuando la contraseña que ingresa actual no es la correcta 
                TempData["IDUsuario"] = idusuario;
                ViewData["vclave"] = "";

                ViewBag.Error = "La constraseña actual no es correcta";
                return View();
            }
            else if (nuevaclave != confirmarclave)
            {
                //para cuando la contraseña al cambiarla no coinciden en su repeticion. 
                TempData["IDUsuario"] = idusuario;
                ViewData["vclave"] = claveactual;

                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            ViewData["vclave"] = "";

            nuevaclave = CN_Recursos.ConvertirSha256(nuevaclave); //encripto la nueva clave q recibo 

            string mensaje = string.Empty;

            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idusuario), nuevaclave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IDUsuario"] = idusuario;

                ViewBag.Error = mensaje;
                return View();
            }


        }



        //METODO REESTABLECER CONTRASEÑA
        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "No se encontro un usuario relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().RestablecerClave(oUsuario.IDUsuario, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }



        //METODO CERRAR SESION
        public ActionResult CerrarSesion()
        {
            //eliminamos el registro de sesion del usuario
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Acceso");
        }
    }
}