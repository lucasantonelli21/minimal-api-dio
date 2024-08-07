using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using minimals_api.Dominio.Entities;
using minimals_api.Dominio.Interfaces;
using minimals_api.Infraestrutura.Context;
namespace minimals_api.Dominio.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly OrganizadorContext _context;
        public VeiculoService(OrganizadorContext db)
        {
            this._context=db;    
        }

        public void Apagar(Veiculo veiculo)
        {
           _context.Veiculos.Remove(veiculo);
           _context.SaveChanges();
        }

        public void Atualizar(Veiculo veiculo)
        {
           _context .Veiculos.Update(veiculo);
           _context.SaveChanges();
        }

        public Veiculo? BuscaPorId(int id)
        {
            return _context.Veiculos.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Incluir(Veiculo veiculo)
        {
            _context.Veiculos.Add(veiculo);
            _context.SaveChanges();
        }

        public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
        {
            var query = _context.Veiculos.AsQueryable();
            if(!string.IsNullOrEmpty(nome)){
                query = query.Where(v=>v.Nome.ToLower().Contains(nome.ToLower()));
            }
            int itensPorPagina = 10;
            if(pagina !=null){
                query = query.Skip(((int)pagina-1)*itensPorPagina).Take(itensPorPagina);
            }
            return query.ToList();
        }


    }
}