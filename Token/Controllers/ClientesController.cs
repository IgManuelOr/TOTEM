using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using Entidades;
using Token.Models;

namespace Token.Controllers
{
    public class ClientesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Instancias de la conexión a la base de datos
        private CajasDAL GestorCajas = new CajasDAL();
        private ClientesDAL GestorClientes = new ClientesDAL();

        // Insert: Cajas
        public JsonResult GetClientes(int IdCaja = 0, int IdCliente = 0)
        {
            Cliente cliente = ArmarComboCliente(IdCaja, IdCliente);

            List<Cliente> ListadoClientes = GestorClientes.GetClientes(cliente).OrderBy(c=> c.idCliente).ToList();

            // Flitros via LINQ
            if (IdCliente != 0)
            {
                ListadoClientes = ListadoClientes.Where(c=> c.idCliente == IdCliente).OrderBy(c=> c.idCliente).ToList();
            }

            if (IdCaja != 0)
            {
                ListadoClientes = ListadoClientes.Where(c => c.idCaja == IdCaja).OrderBy(c => c.idCaja).ToList();
            }

            var data = ListadoClientes;
            return Json(data);
        }

        // Insert: Clientes
        public JsonResult InsertClientes(string NombreCliente)
        {
            //Instancias de una nuevo cliente para inserción en base de datos.
            Cliente cliente = ArmarComboCliente(0, 0, NombreCliente);

            int Msg = GestorClientes.InsertCliente(cliente);

            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCliente(int IdCliente, String NombreCliente, int IdCaja = 0)
        {
            //Instancias de una nuevo cliente para inserción en base de datos.
            Cliente cliente = ArmarComboCliente(IdCaja, IdCliente, NombreCliente);

            int Msg = GestorClientes.UpdateCliente(cliente);

            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteCliente(int IdCliente)
        {
            int Msg = GestorClientes.DeleteCliente(IdCliente);

            return Json(Msg);
        }

        public Cliente ArmarComboCliente (int IdCaja = 0, int IdCliente = 0, string NombreCliente = "")
        {
            Cliente cliente = new Cliente();
            cliente.idCaja = IdCaja;
            cliente.idCliente = IdCliente;
            cliente.nombreCliente = NombreCliente;

            return cliente;
        }

        public JsonResult AsociarClientes()
        {
            int Msg = GestorClientes.AsociarClientes();

            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AtenderCliente(int IdCaja)
        {
            int Msg = GestorClientes.AtenderCliente(IdCaja);

            return Json(Msg, JsonRequestBehavior.AllowGet);
        }
    }
}
