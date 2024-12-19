using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

        [ApiController]
        public class ServiceController : ControllerBase
        {
            private readonly CallMeMaybeDbContext _context;


            public ServiceController(CallMeMaybeDbContext context)

            {
                _context = context;


            }
            [HttpGet]
            [Route("[controller]/get/all")]
            public ActionResult GetAll()
            {

                var data = _context.Service;




                return Ok(data);
            }
            [HttpPost]
            [Route("[controller]/create")]
            public IActionResult Create([FromBody] Service service)
            {
                _context.Service.Add(service);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetAll), new { id = service.id }, service);
            }

        }
    }


