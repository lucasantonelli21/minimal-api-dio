using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimals_api.Dominio.Enums;

namespace minimals_api.Dominio.ModelViews
{
    public class AdministradorLogado
    {
        public string Email { get; set; }
        public string Perfil { get; set; }
        public string Token { get; set; }
    }
}