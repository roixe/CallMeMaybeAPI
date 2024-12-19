using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers



{
    [ApiController]
    public class VilleController : ControllerBase
{
    private readonly CallMeMaybeDbContext _context;


    public VilleController(CallMeMaybeDbContext context)

    {
        _context = context;


    }
    [HttpGet]
    [Route("[controller]/get/all")]
    public ActionResult GetAll()
    {

        var data = _context.Ville;




        return Ok(data);
    }
    [HttpPost]
    [Route("[controller]/create")]
    public IActionResult Create([FromBody] Ville ville)
    {
        _context.Salarie.Add(ville);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAll), new { id = ville.id }, ville);
    }

}
}
