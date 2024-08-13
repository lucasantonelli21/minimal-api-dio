using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimals_api.Dominio.Entities;

namespace test.Domain
{
    [TestClass]
    public class VeiculosTeste
    {
         [TestMethod]
        public void TestarGetSetPropriedadesVeiculo(){
            //Arrange
            var adm = new Veiculo();
            //Action
            adm.Id = 1;
            adm.Nome = "teste@teste.com";
            adm.Marca = "teste";
            adm.Ano = 2020;
            //Assert
            Assert.AreEqual(1,adm.Id);
            Assert.AreEqual("teste@teste.com",adm.Nome);
            Assert.AreEqual("teste",adm.Marca);
            Assert.AreEqual(2020,adm.Ano);
        }
    }
}