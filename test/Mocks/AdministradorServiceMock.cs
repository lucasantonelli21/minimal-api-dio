using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimals_api.Dominio.DTOs;
using minimals_api.Dominio.Entities;
using minimals_api.Dominio.Interfaces;

namespace test.Mocks
{
    public class AdministradorServicoMock : IAdministradorService
    {   
        private static List<Administrador> administradores = new List<Administrador>(){
            new Administrador{
                Id = 1,
                Email = "adm@teste.com",
                Password = "123456",
                Perfil = "Adm"
            },
            new Administrador{
                Id = 2,
                Email = "editor@teste.com",
                Password = "123456",
                Perfil = "Editor"
            }
        };
        public Administrador? BuscarPorId(int id)
        {
            return administradores.Find(a => a.Id == id);
        }

        public void Incluir(Administrador administrador)
        {
            administrador.Id = administradores.Count() + 1;
            administradores.Add(administrador);
        }

        public Administrador? Login(LoginDTO loginDTO)
        {
           return administradores.Find(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password);
        }

        public List<Administrador> Todos(int? pagina)
        {
           return administradores;
        }
    }
}