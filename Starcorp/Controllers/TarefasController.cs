using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Starcorp.Models;
using Starcorp.Repository;

namespace Starcorp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly TestDbContext _db;

        public TarefasController(TestDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefas()
        {
          if (_db.Tarefas == null)
          {
              return NotFound();
          }
            return await _db.Tarefas.Where(x => x.Concluida == false).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tarefa>> GetTarefa(int id)
        {
          if (_db.Tarefas == null)
          {
              return NotFound();
          }
            var tarefa = await _db.Tarefas.FindAsync(id);

            if (tarefa == null)
            {
                return NotFound();
            }

            return tarefa;
        }

        [HttpPost]
        [Route("ConcluirTarefa")]
        public async Task<IActionResult> ConcluirTarefa(Tarefa tarefa)
        {
            tarefa.Concluida = true;
            Tarefa? t = await _db.Tarefas.FirstOrDefaultAsync(x => x.Nome == tarefa.Nome);

            if (t != null)
            { 
                t.Concluida = true;
                await _db.SaveChangesAsync();
            }
            else { return NotFound(); }
           
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Tarefa>> PostTarefa(Tarefa tarefa)
        {
          if (_db.Tarefas == null)
          {
              return Problem("Entity set 'TestDbContext.Tarefas'  is null.");
          }
          _db.Tarefas.Add(tarefa);
          await _db.SaveChangesAsync();

          return CreatedAtAction("GetTarefa", new { id = tarefa.Id }, tarefa);
        }
    }
}
