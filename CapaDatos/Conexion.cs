using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace CapaDatos
{
    public class Conexion
    {
        //ALMACENAMOS LA CADENA QUE AGREGAMOS EN WEB.CONFIG EN CN 
        public static string cn = ConfigurationManager.ConnectionStrings["cadena"].ToString();
    }
}
