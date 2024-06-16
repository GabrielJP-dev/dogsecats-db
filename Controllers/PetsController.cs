using Microsoft.AspNetCore.Mvc;
using Domain.Entidades;
using Infraestrutura;
using Microsoft.EntityFrameworkCore;

namespace dogsecats.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly DogseCatsDbContext _db;

        public PetsController(DogseCatsDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        private bool PetExists(int id)
        {
            return _db.Pets.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pets>>> GetPets()
        {
            return await _db.Pets.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pets>> GetPet(int id)
        {
            var pet = await _db.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            return pet;
        }

        [HttpPost]
        public async Task<ActionResult<Pets>> PostPet(Pets pet)
        {
            _db.Pets.Add(pet);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPet), new { id = pet.Id }, pet);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPet(int id, Pets pet)
        {
            if (id != pet.Id)
            {
                return BadRequest();
            }

            _db.Entry(pet).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetExists(id))
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
        public async Task<IActionResult> DeletePet(int id)
        {
            var pet = await _db.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            _db.Pets.Remove(pet);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
