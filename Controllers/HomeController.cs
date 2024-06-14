using Domain.Entidades;
using Infraestrutura;
using Microsoft.AspNetCore.Mvc;

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
    }
 }
