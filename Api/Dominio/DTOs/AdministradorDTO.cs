using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimals_api.Dominio.Enums;
namespace minimals_api.Dominio.DTOs
{
    public class AdministradorDTO
    {
         public string Email { get; set;}
        public string Password { get; set;}
        public Perfil Perfil { get; set; }
  
    }
}