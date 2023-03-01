//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Marzo 2023</date>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POC_JWT_NetCore6.Services;

namespace POC_JWT_NetCore6.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiController : ControllerBase
    {

        private readonly IConfiguration configuration;

        public ApiController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }


        [HttpGet("Login")]
        [AllowAnonymous]
        public string Login()
        {
            TokenServices TokenServices = new TokenServices(configuration);
            return TokenServices.GenerateToken();
        }


        [HttpGet("Securit")]
        public string Securit()
        {
            return "Ingreso bien";
        }


    }
}
