using System;
using System.Collections.Generic;
using System.Text;
using API_REST.Models;
using Microsoft.EntityFrameworkCore;

namespace API_REST.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Produto> Produtos { get ;set; }
        public DbSet<Usuario> Usuarios { get ;set; }
        //Criar a classe de contexto no banco de dados
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}