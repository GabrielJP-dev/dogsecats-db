using Microsoft.AspNetCore.Mvc;
using Domain.Entidades;
using Infraestrutura;
using Microsoft.EntityFrameworkCore;

namespace dogsecats.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private DogseCatsDbContext _db;

        public UsersController(DogseCatsDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        //CRUD Users

        private bool PersonExists(string id)
        {
            return _db.Users.Any(e => e.Cpf == id);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetPeople()
        {
            return await _db.Users.ToListAsync();
        }

        [HttpGet("{Cpf}")]
        public async Task<ActionResult<Users>> GetPerson(string Cpf)
        {
            var person = await _db.Users.FindAsync(Cpf);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        [HttpPost]
        public async Task<ActionResult<Users>> PostPerson(Users users)
        {
            _db.Users.Add(users);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPerson), new { id = users.Cpf }, users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(string id, Users users)
        {
            if (id != users.Cpf)
            {
                return BadRequest();
            }

            _db.Entry(users).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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
        public async Task<IActionResult> DeletePerson(string id)
        {
            var person = await _db.Users.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _db.Users.Remove(person);
            await _db.SaveChangesAsync();

            return NoContent();
        }


    }
}
