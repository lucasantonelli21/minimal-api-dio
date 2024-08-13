using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using minimals_api.Dominio.Entities;
using minimals_api.Dominio.Services;
using minimals_api.Infraestrutura.Context;

namespace test.Domain
{
    [TestClass]
    public class AdministradorServicoTeste
    {

        private OrganizadorContext CriarContextTest(){
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();
            var configuration = builder.Build();
            var connectionString = configuration.GetConnectionString("ConexaoPadrao");
            var options = new DbContextOptionsBuilder<OrganizadorContext>().UseSqlServer("Server=.\\SQLEXPRESS; Initial Catalog=MinimalsApi_Teste;Integrated Security=SSPI;TrustServerCertificate=True").Options;
            return new OrganizadorContext(options);
        }


        [TestMethod]
        public void TestandoSalvarAdm(){
            var adm = new Administrador("teste","teste","editor");
            var context = CriarContextTest();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");
            //Action
            
            adm.Email = "teste@teste.com";
            adm.Password = "senhateste";
            adm.Perfil = "adm";
            var admService = new AdministradorService(context);
            admService.Incluir(adm);


            Assert.AreEqual(1, admService.Todos(1).Count());
            
        }
    }
}