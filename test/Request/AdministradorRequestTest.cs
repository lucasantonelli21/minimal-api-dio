using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using minimals_api.Dominio.DTOs;
using minimals_api.Dominio.Entities;
using test.Helpers;

namespace test.Request
{
    [TestClass]
    public class AdministradorRequestTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context){
            Setup.ClassInit(context);
        }

        [ClassCleanup]
        public static void ClassCleanup(TestContext context){
            Setup.ClassCleanup();
        }

        [TestMethod]
        public async Task TestarGetSetPropriedades(){
            //Arrange
            var loginDTO = new LoginDTO{
                Email = "adm@teste.com",
                Password = "123456"
            };
            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");
            //Action
            var response = await Setup.client.PostAsync("/Administradores/Login", content);
            
            //Assert
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            
        }
    }
}