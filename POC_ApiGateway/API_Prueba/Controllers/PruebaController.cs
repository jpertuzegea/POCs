using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Prueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PruebaController : ControllerBase
    { 
        [HttpGet]
        public string Get()
        {
            return "Esta es una Prueba Exitosa del API";
        }
         
    }
}
