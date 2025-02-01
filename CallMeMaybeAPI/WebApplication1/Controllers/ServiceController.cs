using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly CallMeMaybeDbContext _context;


        public ServiceController(CallMeMaybeDbContext context)

        {
            _context = context;


        }
        // Récupérer tous les Services
        [HttpGet("get/all")]
        
        public async Task<IActionResult> GetAll()
        {
            if (!Request.Headers.TryGetValue("X-App-Identifier", out var appIdentifier))
            {
                return Unauthorized(new { message = "En-tête X-App-Identifier manquant." });
            }
            if (appIdentifier != "CallMeMaybe")
            {
                return Unauthorized(new { message = "Accès refusé : X-App-Identifier invalide." });
            }


            var data = await (from service in _context.Service
                             select new
                             {   service.id,
                                 service.nom
                             
                             }).ToListAsync(); 




            return Ok(data);
        }
        // Créer un Service 

        [HttpPost]
        [Route("create")]
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
                var serviceToDelete = await _context.Service.FirstOrDefaultAsync(s => s.id == id); 

                if (serviceToDelete == null)
                {
                    return NotFound($"Le service avec l'ID {id} n'existe pas.");
                }
                if (!Request.Headers.TryGetValue("X-App-Identifier", out var appIdentifier))
                {
                    return Unauthorized(new { message = "En-tête X-App-Identifier manquant." });
                }
                if (appIdentifier != "CallMeMaybe")
                {
                    return Unauthorized(new { message = "Accès refusé : X-App-Identifier invalide." });
                }


                var salarieWithKey = await _context.Salarie
                .FirstOrDefaultAsync(s => s.idService == id);

                if (salarieWithKey != null && serviceToDelete.id == salarieWithKey.idService)

                {
                    return BadRequest(new { Message = $"Impossible de supprimer le service car un salarié y est associé.{salarieWithKey.nom} " });


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
            if (!Request.Headers.TryGetValue("X-App-Identifier", out var appIdentifier))
            {
                return Unauthorized(new { message = "En-tête X-App-Identifier manquant." });
            }
            if (appIdentifier != "CallMeMaybe")
            {
                return Unauthorized(new { message = "Accès refusé : X-App-Identifier invalide." });
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
    


