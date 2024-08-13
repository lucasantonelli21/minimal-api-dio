using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimals_api.Dominio.Entities;
namespace test.Domain
{
[TestClass]
    public class AdministradorTeste
    {
        [TestMethod]
        public void TestarGetSetPropriedades(){
            //Arrange
            var adm = new Administrador("teste","teste","editor");
            //Action
            adm.Id = 1;
            adm.Email = "teste@teste.com";
            adm.Password = "senhateste";
            adm.Perfil = "adm";
            //Assert
            Assert.AreEqual(1,adm.Id);
            Assert.AreEqual("teste@teste.com",adm.Email);
            Assert.AreEqual("senhateste",adm.Password);
            Assert.AreEqual("adm",adm.Perfil);
        }
    }
}