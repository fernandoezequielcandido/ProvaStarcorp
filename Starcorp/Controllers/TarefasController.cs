using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starcorp.Models;
using Starcorp.Repository;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Starcorp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly TestDbContext _db;
        private readonly IConfiguration _configuration;

        public TarefasController(TestDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Tarefa>> GetTarefas()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("FernandoConnection")))
            {
                var tarefas = connection.Query<Tarefa>("SELECT [ID], [Nome], [DataParaConclusao], [Concluida] FROM Tarefas WHERE Concluida = 'false' ORDER BY [DataParaConclusao]");

                if (tarefas == null)
                {
                    return NotFound();
                }

                return tarefas.ToList();
            }
        }

        [HttpPost]
        [Route("ConcluirTarefa")]
        public IActionResult ConcluirTarefa(Tarefa tarefa)
        {
            var updateSql = @"UPDATE [Tarefas] SET [Concluida] = @Concluida WHERE [Id] = @Id";
           
            using (var connection = new SqlConnection(_configuration.GetConnectionString("FernandoConnection")))
            {
                connection.Execute(updateSql, new
                {
                    Concluida = true,
                    tarefa.Id
                });
            }
            return NoContent();
        }

        [HttpPost]
        public ActionResult<Tarefa> PostTarefa(Tarefa tarefa)
        {
            var insertSql = @"INSERT INTO [Tarefas] (Nome, DataParaConclusao, Concluida) OUTPUT INSERTED.Id VALUES(@Nome, @DataParaConclusao, @Concluida) ";
            
            using (var connection = new SqlConnection(_configuration.GetConnectionString("FernandoConnection")))
            {
                int idTarefa = connection.QuerySingle<int>(insertSql, new {
                    tarefa.Nome,
                    tarefa.DataParaConclusao,
                    tarefa.Concluida});

                tarefa.Id = idTarefa;
                return Ok(tarefa);
            }
            
        }
    }
}
