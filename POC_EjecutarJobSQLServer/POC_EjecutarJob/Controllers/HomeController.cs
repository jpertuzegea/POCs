using Newtonsoft.Json;
using POC_EjecutarJob.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace POC_EjecutarJob.Controllers
{
    /*
     * .Net Core 3.1 - VisualvStudio 2019
     * Esta prueba de concepto permite ejecutar un Job de Sql Server, desde C#.
     * Previamente se creo un job (tarea programada), con el nombre Jorge, 
     * la cual ejecuta un procedimeinto y este realiza un insert a una tabla 
     */

    public class HomeController : Controller
    {

         
        public ActionResult Index()
        {
            //Bll_POC Bll_POC = new Bll_POC();
            //var listaBD = Bll_POC.listarBD();
            //ViewBag.BD = new SelectList(listaBD, "Value", "Text");

            //var listaJobs = Bll_POC.listarJob();
            //ViewBag.Jobs = new SelectList(listaJobs, "Value", "Text");



            return View();
        }

        [HttpPost]
        public ActionResult Index(ModelJob ModelJob)
        {
            ViewBag.Mensaje = "Job NO Ejecutado";

            Bll_POC Bll_POC = new Bll_POC();
              
            if (Bll_POC.EjecutarJob(ModelJob.NombreJob))
            {
                ViewBag.Mensaje = "Job Ejecutado con exito";
            }

            return View();
        }


        [HttpGet]
        public JsonResult ListarBDs()
        {
            try
            {

                return Json(new Bll_POC().listarBD(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        [HttpPost]
        public JsonResult ListarJobs(string DataBaseName)
        {
            try
            { 
                return Json(new Bll_POC().listarJob(DataBaseName), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) {
                return Json(e.Message);
            }
        }

       
    }
}