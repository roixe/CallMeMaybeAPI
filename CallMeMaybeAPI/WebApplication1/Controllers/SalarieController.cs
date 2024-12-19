
using Microsoft.AspNetCore.Mvc;
using WebApplication1;
using WebApplication1.Models;
using System.Text.Json;



namespace WebApplication1.Controllers



{
    [ApiController]
    public class SalarieController : ControllerBase
    {
        private readonly CallMeMaybeDbContext _context;


        public SalarieController(CallMeMaybeDbContext context)

        {
            _context = context;


        }
        [HttpGet]
        [Route("[controller]/get/all")]
        public ActionResult GetAll()
        {

            var data = _context.Salarie;




            return Ok(data);
        }
        [HttpPost]
        [Route("[controller]/create")]
        public IActionResult Create([FromBody] Salarie salarie)
        {
            _context.Salarie.Add(salarie);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetAll), new { id = salarie.id }, salarie);
        }

    }
}
