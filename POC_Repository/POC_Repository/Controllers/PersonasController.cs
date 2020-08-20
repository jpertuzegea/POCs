using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using DAO.Entitys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace POC_Repository.Controllers
{
    public class PersonasController : Controller
    {
        public async Task<ActionResult> Index()
        {
            BLL_Persona BLL_Persona = new BLL_Persona();
            List<Personas> List = await BLL_Persona.GetAll();

            return View(List);
        }

        public async Task<ActionResult> Create()
        {
            BLL_Persona BLL_Persona = new BLL_Persona();
            await BLL_Persona.CrearPersona();

            return RedirectToAction("Index");
        }

    }
}
