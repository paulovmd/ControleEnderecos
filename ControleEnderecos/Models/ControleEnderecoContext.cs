using Microsoft.EntityFrameworkCore;

namespace ControleEnderecos.Models
{
    public class ControleEnderecoContext : DbContext

    {
        public ControleEnderecoContext(
               DbContextOptions<ControleEnderecoContext> options) 
               : base(options) {            
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Cliente> Cliente { get; set; }    

        public DbSet<Endereco> Endereco { get; set; } 

        public DbSet<Cidade> Cidade { get; set; }

        public DbSet<Estado> Estado { get; set;}


    }
}
