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
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Instancias de la conexión a la base de datos
        private CajasDAL GestorCajas = new CajasDAL();
        private ClientesDAL GestorClientes = new ClientesDAL();
        public ActionResult Index()
        {
            List <Caja> ListadoCajas = GestorCajas.GetCajas(0);

            //Dummy Cliente
            Cliente cliente = new Cliente();
            cliente.idCaja = 0;
            cliente.idCliente = 0;

            List<Cliente> ListadoClientes = GestorClientes.GetClientes(cliente);
            ViewBag.Cajas = ListadoCajas;
            ViewBag.Clientes = ListadoClientes;
            return View();
        }
    }
}