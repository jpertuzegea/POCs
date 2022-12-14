using Microsoft.AspNetCore.Mvc;
using POC_ResultModelAPI.Models;

namespace POC_ResultModelAPI.Controllers
{
    [Route("api/Prueba")]
    [ApiController]
    public class HomeController : Controller
    {

        [HttpGet("Index")]
        public async Task<ActionResult<ResultModel<PersonModel>>> Index()
        {
            PersonModel PersonModel = new PersonModel();
            PersonModel.Id = 1;
            PersonModel.FullName = "Jorge Pertuz Egea";
            PersonModel.Email = "jpertuzegea@hotmail.com";

            ResultModel<PersonModel> response = new ResultModel<PersonModel>()
            {
                IsSuccess = true,
                Messages = "",
                Result = PersonModel
            };

            return response;
        }
    }
}
