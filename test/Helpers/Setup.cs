using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using minimals_api;
using minimals_api.Infraestrutura.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using minimals_api.Dominio.Entities;
using test.Mocks;
using minimals_api.Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace test.Helpers
{
    public class Setup
    {
        public const string PORT = "5001";
        public static TestContext testContext = default!;
        public static WebApplicationFactory<Startup> http = default!;
        public static HttpClient client = default!;


        public static void ClassInit(TestContext testContext){
            Setup.testContext = testContext;
            Setup.http = new WebApplicationFactory<Startup>();
            Setup.http = Setup.http.WithWebHostBuilder(builder =>{
                builder.UseSetting("https_port",Setup.PORT).UseEnvironment("Testing");
                builder.ConfigureServices(services =>{
                    services.AddScoped<IAdministradorService,AdministradorServicoMock>();
                    var conexao = "Server=.\\SQLEXPRESS; Initial Catalog=LabMinimalApi;Integrated Security=SSPI;TrustServerCertificate=True";
                    services.AddDbContext<OrganizadorContext>(options =>{
                        options.UseSqlServer(conexao);
                    });
                });
            });
            Setup.client = Setup.http.CreateClient();
        }

        public static void ClassCleanup(){
            Setup.http.Dispose();
        }
        
    }
}