using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers



{
    [ApiController]
    [Route("api/[controller]")]
    public class SiteController : ControllerBase
    {
        private readonly CallMeMaybeDbContext _context;


        public SiteController(CallMeMaybeDbContext context)

        {
            _context = context;


        }
        [HttpGet]
        [HttpGet("get/all")]

        public async Task<IActionResult> GetAll()
        {

            var data = await (from site in _context.Site
                              select new
                              {
                                  site.id,
                                  site.ville

                              }).ToListAsync();




            return Ok(data);
        }
        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] Site site)
        {
            _context.Site.Add(site);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetAll), new { id = site.id }, site);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var siteToDelete = await _context.Site.FindAsync(id);

                if (siteToDelete == null)
                {
                    return NotFound($"Le site avec l'ID {id} n'existe pas.");
                }

                var salarieWithKey = await _context.Salarie
                .FirstOrDefaultAsync(s => s.idSite == id);

                if (siteToDelete.id == salarieWithKey.idSite)
                
                {
                    return BadRequest(new { Message = $"Impossible de supprimer le site car un salarié y est associé.{ salarieWithKey.nom  } "});


                }
                

                _context.Site.Remove(siteToDelete);
                await _context.SaveChangesAsync();
                return Ok($"La ville avec l'ID {id} a été supprimé avec succès.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erreur lors de la suppression du site : {ex.Message}");
            }
        }

        // Mettre à jour une Ville par son ID
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Site site)
        {
            if (id != site.id)
            {
                return BadRequest("L'ID dans l'URL ne correspond pas à l'ID de la ville.");
            }

            try
            {
                var siteToUpdate = await _context.Site.FindAsync(id);

                if (siteToUpdate == null)
                {
                    return NotFound($"La ville avec l'ID {id} n'existe pas.");
                }

                // Mise à jour des champs

                siteToUpdate.ville = site.ville;


                _context.Entry(siteToUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok($"La ville avec l'ID {id} a été mis à jour avec succès.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erreur lors de la mise à jour de la ville : {ex.Message}");
            }
        }



    }
}

