using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimals_api.Dominio.DTOs;
using minimals_api.Dominio.Entities;
using minimals_api.Dominio.Interfaces;
using minimals_api.Infraestrutura.Context;

namespace minimals_api.Dominio.Services
{
    public class AdministradorService : IAdministradorService
    {
        private readonly OrganizadorContext _context;
        public AdministradorService(OrganizadorContext db)
        {
            this._context=db;    
        }

        public Administrador? BuscarPorId(int id)
        {
            var adm = _context.Administradores.Where(a => a.Id == id).FirstOrDefault();
            return adm;
        }

        public void Incluir(Administrador administrador)
        {
            _context.Add(administrador);
            _context.SaveChanges();
        }

        public Administrador? Login(LoginDTO loginDTO){
            return _context.Administradores.Where(x=> x.Email== loginDTO.Email && x.Password== loginDTO.Password).FirstOrDefault(); }

        public List<Administrador> Todos(int? pagina)
        {
            var query = _context.Administradores.AsQueryable();
            int itensPorPagina = 10;
            if(pagina !=null){
                query = query.Skip(((int)pagina-1)*itensPorPagina).Take(itensPorPagina);
            }
            return query.ToList();
        }
    }
}