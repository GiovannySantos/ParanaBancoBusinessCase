using CadastroClientes.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using SQLitePCL;

namespace CadastroClientes.Infra.Factiories
{
    public class CadastroClientesDbContextFactory : IDesignTimeDbContextFactory<CadastroClientesDbContext>
    {
        public CadastroClientesDbContext CreateDbContext(string[] args)
        {
            Batteries.Init();

            var optionsBuilder = new DbContextOptionsBuilder<CadastroClientesDbContext>();

            // Caminho para o banco no design time (pode ser ajustado)
            var dbPath = Path.GetFullPath(Path.Combine("..", "clientes.db"));

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new CadastroClientesDbContext(optionsBuilder.Options);
        }
    }
}
