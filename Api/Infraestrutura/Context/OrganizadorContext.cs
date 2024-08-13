using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using minimals_api.Dominio.DTOs;
using minimals_api.Dominio.Entities;

namespace minimals_api.Infraestrutura.Context
{
    public class OrganizadorContext : DbContext
    {
        public OrganizadorContext(DbContextOptions<OrganizadorContext> options) : base(options)
        { }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }

    }
}