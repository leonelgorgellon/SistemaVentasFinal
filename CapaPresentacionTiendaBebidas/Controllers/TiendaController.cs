using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using System.IO;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;

using CapaPresentacionTiendaBebidas.Filter;

namespace CapaPresentacionTiendaBebidas.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetalleProducto(int idproducto =0)
        {
            Producto oProducto = new Producto();
            bool conversion;

            oProducto = new CN_Producto().Listar().Where(p => p.IDProducto == idproducto).FirstOrDefault();

            if(oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.Extension);

            }

            return View(oProducto);
        }


        //METODO PARA LISTAR LAS MARCAS EN MI TIENDA
        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new List<Categoria>();
            lista = new CN_Categoria().Listar();

            return Json(new {data= lista }, JsonRequestBehavior.AllowGet);  
        }


        [HttpPost]
        public JsonResult ListarMarcaPorCategoria(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();
            lista = new CN_Marca().ListarMarcaPorCategoria(idcategoria);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }


        //METODO PARA LISTAR TODOS LOS PRODUCTOS DE ACUERDO A UNA CATEGORIA Y UNA MARCA SELECCIONADA
        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca)
        {
            List<Producto> lista = new List<Producto>();

            bool conversion;

            lista = new CN_Producto().Listar().Select(p => new Producto()
            {
                IDProducto = p.IDProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p =>   //agregamos filtros 
                    p.oCategoria.IDCategoria == (idcategoria == 0 ? p.oCategoria.IDCategoria : idcategoria) &&
                    p.oMarca.IDMarca == (idmarca == 0 ? p.oMarca.IDMarca : idmarca) &&
                    p.Stock > 0 && p.Activo == true
                    ).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }


        //METODO AGREGAR AL CARRITO 
        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IDCliente; //obtengo solo el id del cliente el cual accede al sistema

            bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);

            bool respuesta = false;

            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito.";
            }
            else
            {
                respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, sumar: true, out mensaje);
            }

            return Json(new {respuesta = respuesta, mensaje = mensaje}, JsonRequestBehavior.AllowGet);

        }



        //METODO PARA OBTENER LA CANTIDAD DE PRODUCTOS SEGUN EL CARRITO DEL CLIENTE. 
        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IDCliente;
            int cantidad = new CN_Carrito().CantidadEnCarrito(idcliente);

            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }


        //METODO PARA LISTAR LOS PRODUCTOS DEL CARRITO 
        [HttpPost]
        public JsonResult ListarProductosCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IDCliente;

            List<Carrito> oLista = new List<Carrito>();

            bool conversion;

            oLista = new CN_Carrito().ListarProductos(idcliente).Select(oc => new Carrito()
            {
                oProducto = new Producto()
                {
                    IDProducto = oc.oProducto.IDProducto,
                    Nombre = oc.oProducto.Nombre,
                    oMarca = oc.oProducto.oMarca,
                    Precio = oc.oProducto.Precio,
                    RutaImagen = oc.oProducto.RutaImagen,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)
                },
                Cantidad = oc.Cantidad

            }).ToList();


            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        //METODO PARA VER LAS CANTIDADES QUE TENGO DENTRO DE MI CARRITO 
        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IDCliente; //obtengo solo el id del cliente el cual accede al sistema


            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, sumar , out mensaje);


            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }


        //METODO PARA ELIMINAR UN PRODCUTO DENTRO DEL CARRITO 
        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IDCliente;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(idcliente, idproducto);

            return Json(new {respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        //_------------ METODOS PARA OBTENER UBICACION PARA EL ENVIO 

        //METODO PARA OBTENER LOCAL 
        [HttpPost]
        public JsonResult ObtenerLocal()
        {
            List<Local_Negocio> oLista = new List<Local_Negocio>();

            oLista = new CN_Ubicacion().ObtenerLocal();

            return Json(new {lista = oLista}, JsonRequestBehavior.AllowGet);
        }


        //METODO PARA OBTENER LAS PROVINCIAS 
        [HttpPost]
        public JsonResult ObtenerProvincia(string idlocal)
        {
            List<Provincia> oLista = new List<Provincia>();

            oLista = new CN_Ubicacion().ObtenerProvincia(idlocal);

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }


        //METODO PARA OBTENER LAS LOCALIDADES 
        [HttpPost]
        public JsonResult ObtenerLocalidad(string idlocal, string idprovincia)
        {
            List<Localidad> oLista = new List<Localidad>();

            oLista = new CN_Ubicacion().ObtenerLocalidad(idlocal, idprovincia);

            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }



        //METODO PARA HACER LA VISTA DEL CARRITO 
        //[ValidarSession]
        //[Authorize]
        public ActionResult Carrito()
        {
            return View();
        }




        //METODO PARA TERMINAR Y PROCESAR PAGO 
        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> oListaCarrito, Venta oVenta)
        {
            decimal total = 0;

            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new CultureInfo("en-US");
            detalle_venta.Columns.Add("IdProducto", typeof(string));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(decimal));

            foreach(Carrito oCarrito in oListaCarrito)
            {
                decimal subtotal = Convert.ToDecimal(oCarrito.Cantidad.ToString()) * oCarrito.oProducto.Precio;

                total += subtotal;

                detalle_venta.Rows.Add(new object[]
                {
                    oCarrito.oProducto.IDProducto,
                    oCarrito.Cantidad,
                    subtotal
                });
            }

            oVenta.MontoTotal = total;
            oVenta.IDCliente = ((Cliente)Session["Cliente"]).IDCliente;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;

            return Json(new {Status = true, Link = "/Tienda/PagoEfectuado?idTransaccion=code0001&status=true"}, JsonRequestBehavior.AllowGet);
        }


        //METODO PARA LA VISTA DE PAGO EFECTUADO. 
        //[ValidarSession]
        //[Authorize]
        public async Task<ActionResult> PagoEfectuado()
        {
            //datos que recibo de mi LINK de Procesar pago 
            string idtransaccion = Request.QueryString["idTransaccion"];
            bool status = Convert.ToBoolean(Request.QueryString["status"]);

            ViewData["Status"] = status;

            if (status)
            {
                Venta oVenta = (Venta)TempData["Venta"];

                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];

                oVenta.IDTransaccion = idtransaccion;

                string mensaje = string.Empty;

                bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje);

                ViewData["IDTransaccion"] = oVenta.IDTransaccion;
            }

            return View();

        }



        //LISTAR COMPRAS 
        //[ValidarSession]
        //[Authorize]
        public ActionResult MisCompras()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IDCliente;

            List<DetalleVenta> oLista = new List<DetalleVenta>();

            bool conversion;

            oLista = new CN_Venta().ListarCompras(idcliente).Select(oc => new DetalleVenta()
            {
                oProducto = new Producto()
                {
                    Nombre = oc.oProducto.Nombre,
                    Precio = oc.oProducto.Precio,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)
                },
                Cantidad = oc.Cantidad,
                Total = oc.Total,
                IDTransaccion = oc.IDTransaccion

            }).ToList();


            return View(oLista);
        }
    }
}