using Domain.Entidades;
using Infraestrutura;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dogsecats.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DogsController : ControllerBase
    {
        private DogseCatsDbContext _db;

        public DogsController(DogseCatsDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        //CRUD Atendimento

        private bool AtendimentoExists(int id)
        {
            return _db.Atendimentos.Any(e => e.AppointmentId == id);
        }


        [HttpGet]
        public IActionResult Get()
        {
            var atendimentos = _db.Atendimentos.ToList();
            return Ok(atendimentos);
        }

        [HttpPost]
        public IActionResult Add(Atendimentos atendimento)
        {
            var atendimentos = _db.Atendimentos.Add(atendimento);
            _db.SaveChanges();

            return Ok(atendimentos.Entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Atendimentos atendimento)
        {
            if (id != atendimento.AppointmentId)
            {
                return BadRequest();
            }

            _db.Entry(atendimento).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AtendimentoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAtendimento(int id)
        {
            var atendimento = await _db.Atendimentos.FindAsync(id);
            if (atendimento == null)
            {
                return NotFound();
            }

            _db.Atendimentos.Remove(atendimento);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
 }
