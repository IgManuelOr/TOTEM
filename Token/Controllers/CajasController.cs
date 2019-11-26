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
    public class CajasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Instancias de la conexión a la base de datos
        private CajasDAL GestorCajas = new CajasDAL();


        // Insert: Cajas
        public JsonResult GetCajas(int IdCaja = 0)
        {

            List<Caja> ListadoCajas = GestorCajas.GetCajas(IdCaja);
            var data = ListadoCajas.OrderBy(c => c.idCaja);
            return Json(data);
        }

        // Insert: Cajas
        public JsonResult InsertCaja(int Clientes, string Modelo, string Operador)
        {
            //Instancias de una nueva caja para inserción en base de datos.
            Caja caja = new Caja();
            caja.clientes = Clientes;
            caja.modelo = Modelo;
            caja.operador = Operador;

            int Msg = GestorCajas.InsertCaja(caja);

            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCajas(int IdCaja, int Clientes, int Atendidos, string Modelo, string Operador)
        {
            //Instancias de una nueva caja para inserción en base de datos.
            Caja caja = new Caja();
            caja.idCaja = IdCaja;
            caja.clientes = Clientes;
            caja.atendidos = Atendidos;
            caja.modelo = Modelo;
            caja.operador = Operador;

            int Msg = GestorCajas.UpdateCaja(caja);

            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BorrarCaja(int IdCaja)
        {
            int Msg = GestorCajas.DeleteCaja(IdCaja);

            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CambiarEstadoCaja(int IdCaja)
        {
            int Msg = GestorCajas.CambiarEstadoCaja(IdCaja);

            return Json(Msg, JsonRequestBehavior.AllowGet);
        }

        // GET: Cajas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cajas/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cajas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cajas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cajas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cajas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
