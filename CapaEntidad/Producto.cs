using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Producto
    {
		public int IDProducto { get; set; }
		public string Nombre { get; set; }
		public string Descripcion { get; set; }
		public Marca oMarca { get; set; }
		public Categoria oCategoria { get; set; }
		public decimal Precio { get; set; }
		public string PrecioTexto { get; set; } //para recibir el valor desde la web y poder convertilo a formato string y validar por region 
		public int Stock { get; set; }
		public string RutaImagen { get; set; }
		public string NombreImagen { get; set; }
		public bool Activo { get; set; }

		//agregamos para la parte de imagen, el cual es la visualizacion y la extension .png .jpg 
		public string Base64 { get; set; }

		public string Extension { get; set; }

	}
}
