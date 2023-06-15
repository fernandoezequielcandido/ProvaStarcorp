using Microsoft.EntityFrameworkCore;
using Starcorp.Models;

namespace Starcorp.Repository
{
    public class TestDbContext: DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public TestDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Tarefa>()
                .HasIndex(u => u.Nome)
                .IsUnique();
        }

        public static List<Tarefa> getFake()
        {
            List<Tarefa> returnValue = new List<Tarefa>();
            returnValue.Add(new Tarefa { Id = 1, Concluida = false, DataParaConclusao = new DateTime(2023, 12, 30, 08, 34, 58), Nome = "Tarefa 1" });
            returnValue.Add(new Tarefa { Id = 2, Concluida = false, DataParaConclusao = new DateTime(2023, 11, 25, 12, 10, 30), Nome = "Tarefa 2" });
            returnValue.Add(new Tarefa { Id = 3, Concluida = false, DataParaConclusao = new DateTime(2023, 10, 12, 10, 08, 24), Nome = "Tarefa 3" });
            return returnValue;
        }

        public virtual DbSet<Tarefa> Tarefas { get; set; }
    }
}
