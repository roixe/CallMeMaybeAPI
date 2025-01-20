using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        // Récupérer tous les Services
        [HttpGet]
        [Route("[controller]/get/all")]
        public ActionResult GetAll()
        {

            var data = _context.Service;




            return Ok(data);
        }
        // Créer un Service 

        [HttpPost]
        [Route("[controller]/create")]
        public IActionResult Create([FromBody] Service service)
        {
            _context.Service.Add(service);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetAll), new { id = service.id }, service);
        }


        // Supprimer un Service par son ID

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var serviceToDelete = await _context.Service.FindAsync(id);

                if (serviceToDelete == null)
                {
                    return NotFound($"Le service avec l'ID {id} n'existe pas.");
                }

                _context.Service.Remove(serviceToDelete);
                await _context.SaveChangesAsync();
                return Ok($"Le service avec l'ID {id} a été supprimé avec succès.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erreur lors de la suppression du service : {ex.Message}");
            }
        }

        // Mettre à jour un Service par son ID
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Service service)
        {
            if (id != service.id)
            {
                return BadRequest("L'ID dans l'URL ne correspond pas à l'ID du service.");
            }

            try
            {
                var serviceToUpdate = await _context.Service.FindAsync(id);

                if (serviceToUpdate == null)
                {
                    return NotFound($"Le service avec l'ID {id} n'existe pas.");
                }

                // Mise à jour des champs

                serviceToUpdate.nom = service.nom;


                _context.Entry(serviceToUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok($"Le service avec l'ID {id} a été mis à jour avec succès.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erreur lors de la mise à jour du service : {ex.Message}");
            }
        }

    }
}
    


