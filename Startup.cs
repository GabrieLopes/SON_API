using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_REST.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API_REST
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //Adicionando nosso DbContext
            services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            //Swagger
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API DE PRODUTOS", Version = "v1" });
            });

            string ChaveDeSeguranca = "GFT_STARTER_TOKEN"; // Chave de segurança
            var chaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ChaveDeSeguranca));

            // Usando JWT como forma de autenticação
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                { // Como o sistema deve ler o token
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    //Dados de validação de um JWT
                    ValidIssuer = "Gabriel Lopes",
                    ValidAudience = "usuario_comum",
                    IssuerSigningKey = chaveSimetrica
                };  // É o que diz se o token é valido ou não para o nosso sistema


            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication(); // Aplica o sistema de autenticação na aplicação

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(config =>
            {
                // config.RouteTemplate = "gabriel/{documentName}/swagger.json" Configura qual o diretório do Swagger
            }); //Gerar um arquivo JSON - Swagger.json
            app.UseSwaggerUI(config =>
            { // Views HTML do Swagger
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "v1 docs");
            });
        }
    }
}
