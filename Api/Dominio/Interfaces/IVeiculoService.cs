using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimals_api.Dominio.DTOs;
using minimals_api.Dominio.Entities;

namespace minimals_api.Dominio.Interfaces
{
    public interface IVeiculoService
    {
        void Incluir(Veiculo veiculo);
        List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null);
        Veiculo? BuscaPorId(int id);
        void Atualizar(Veiculo veiculo);
        void Apagar(Veiculo veiculo);
    }
}