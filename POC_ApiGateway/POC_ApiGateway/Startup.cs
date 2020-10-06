using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace POC_ApiGateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            // Para hacer la prueba se debe crear un api con cualquier metodo, y configurar crossdomain para solo aceptar peticiones del APigateway
            // y desde un front apuntar directamente a ese api, este no debera responder por la politica de crossdomain
            // luego apuntar el front al apigateway y ahi si debe responder el microservicio 
            services.AddOcelot(Configuration);
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
             
            await app.UseOcelot();
        }
    }
}
