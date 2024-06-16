using Microsoft.AspNetCore.Mvc;
using Domain.Entidades;
using Infraestrutura;
using Microsoft.EntityFrameworkCore;

namespace dogsecats.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicosController : Controller
    {

        private DogseCatsDbContext _db;

        public ServicosController(DogseCatsDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        //CRUD Serviço

        private bool ServiceExists(int id)
        {
            return _db.Servicos.Any(e => e.ServiceId == id);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            return await _db.Servicos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
            var service = await _db.Servicos.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return service;
        }

        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            _db.Servicos.Add(service);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetService), new { id = service.ServiceId }, service);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, Service service)
        {
            if (id != service.ServiceId)
            {
                return BadRequest();
            }

            _db.Entry(service).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _db.Servicos.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _db.Servicos.Remove(service);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
