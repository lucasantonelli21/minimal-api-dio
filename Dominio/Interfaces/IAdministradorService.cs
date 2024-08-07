using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimals_api.Dominio.DTOs;
using minimals_api.Dominio.Entities;

namespace minimals_api.Dominio.Interfaces
{
    public interface IAdministradorService
    {
        Administrador? Login(LoginDTO loginDTO);
        void Incluir(Administrador administrador);
        List<Administrador> Todos(int? pagina);
        Administrador? BuscarPorId(int id);
    }
}