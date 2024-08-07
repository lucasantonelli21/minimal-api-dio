using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimals_api.Dominio.ModelViews
{
   public struct Home{
      public string Mensagem{get => "Bem Vindo a minha API de veiculos";}
      public string Documentacao{get => "/swagger";}
   }
}