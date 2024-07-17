using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {
        private CD_Ubicacion objCapaDato = new CD_Ubicacion();

        public List<Local_Negocio> ObtenerLocal()
        {
            return objCapaDato.ObtenerLocal();
        }


        public List<Provincia> ObtenerProvincia(string idlocal)
        {
            return objCapaDato.ObtenerProvincia(idlocal);
        }


        public List<Localidad> ObtenerLocalidad(string idlocal, string idprovincia)
        {
            return objCapaDato.ObtenerLocalidad(idlocal, idprovincia);
        }
    }
}
