using Microsoft.EntityFrameworkCore;
using Moq;
using Starcorp.Controllers;
using Starcorp.Models;
using Starcorp.Repository;
using Moq.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using System.Linq;

namespace TestProject
{
    public class UnitTest1
    {
        private Mock<TestDbContext> testContextMock;
        private TarefasController controller;

        public UnitTest1()
        {
            testContextMock = new Mock<TestDbContext>();
            testContextMock.Setup<DbSet<Tarefa>>(x => x.Tarefas)
                .ReturnsDbSet(TestDbContext.getFake());
            controller = new TarefasController(testContextMock.Object);
        }

        [Fact]
        public async void FakeListThreeItensCount()
        {
            var listItens = await controller.GetTarefas();
            if (listItens != null && listItens.Value != null)
            {
                
                Assert.Equal(3, listItens.Value.Count());
            }
        }

        [Fact]
        public async void MakeConcluidoItemAndTwoItensCount()
        {
            await controller.ConcluirTarefa(new Tarefa { Id = 1, Concluida = false, DataParaConclusao = new DateTime(2023, 12, 30, 08, 34, 58), Nome = "Tarefa 1" });
            var listItens = await controller.GetTarefas();
            if (listItens != null && listItens.Value != null)
            {
                Assert.Equal(2, listItens.Value.Count());
            }
        }

    }
}