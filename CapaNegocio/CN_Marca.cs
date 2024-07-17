using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Marca
    {
        private CD_Marca objCapaDato = new CD_Marca();

        //LISTAR
        public List<Marca> Listar()
        {
            return objCapaDato.Listar();
        }



        //REGISTRAR
        public int Registrar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            //validamos datos. 
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción de la marca no puede ser vacio";
            }



            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Registrar(obj, out Mensaje);

            }
            else
            {
                return 0;
            }


        }




        //EDITAR
        public bool Editar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            //validamos datos. 
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción de la marca no puede ser vacio";
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



        //METODO LISTAR MARCA POR CATEGORIA 
        public List<Marca> ListarMarcaPorCategoria(int idcategoria)
        {
            return objCapaDato.ListarMarcaPorCategoria(idcategoria);
        }

    }
}
