using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using System.Net;
using System.IO;


namespace CapaNegocio
{
    public class CN_Recursos
    {
        //encriptacion de TEXTO en SHA256
        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();

            //usar la referencia de "System.Security.Cryptography"
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }


        //METODO Generar clave (funcion que genera una clave autogenerada) 
        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6); //permite generar un codigo unico de c#, lo que devuevle clave aleatoria de 6 digitos

            return clave;
        }



        //METODO para enviar correo 
        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo); //a quien dirigido el msj
                mail.From = new MailAddress("leonel.gorg@gmail.com");//desde que cuenta esta siendo enviado el correo. 
                mail.Subject = asunto; //asunto
                mail.Body = mensaje; //mensaje
                mail.IsBodyHtml = true; //formato html

                //configuramos servidor del cual va a ser el encargado de enviar el msj 
                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("leonel.gorg@gmail.com", "rmzvjcwyegjvezfs"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);

                resultado = true;
            }
            catch(Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }



        //METODO PARA CONVERTIR LA IMAGEN EN BASE64
        public static string ConvertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch
            {
                conversion = false;
            }

            return textoBase64;
        }
    }
}
