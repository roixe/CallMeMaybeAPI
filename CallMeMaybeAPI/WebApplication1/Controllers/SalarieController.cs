using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Route principale pour le contrôleur
    public class SalarieController : ControllerBase
    {
        private readonly CallMeMaybeDbContext _context;

        public SalarieController(CallMeMaybeDbContext context)
        {
            _context = context;
        }

        // Récupérer tous les salariés
        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await (from salarie in _context.Salarie
                                  join service in _context.Service on salarie.idService equals service.id
                                  join site in _context.Site on salarie.idSite equals site.id
                                  select new
                                  {
                                      salarie.id,
                                      salarie.nom,
                                      salarie.prenom,
                                      salarie.telFixe,
                                      salarie.telMobile,
                                      salarie.email,
                                      ServiceNom = service.nom,
                                      salarie.idService,
                                      VilleNom = site.ville,
                                      salarie.idSite
                                      

                                  }).ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erreur lors de la récupération des données : {ex.Message}");
            }
        }

        // Créer un salarié
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Salarie salarie)
        {
            if (salarie == null)
            {
                return BadRequest("Les données du salarié sont nulles.");
            }

            try
            {
                await _context.Salarie.AddAsync(salarie);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAll), new { id = salarie.id }, salarie);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erreur lors de la création du salarié : {ex.Message}");
            }
        }

        // Supprimer un salarié par son ID
        
        [HttpDelete("delete/{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var salarieToDelete = await _context.Salarie.FindAsync(id);


                if (salarieToDelete == null)
                {
                    return NotFound($"Le salarié avec l'ID {id} n'existe pas.");
                }

                _context.Salarie.Remove(salarieToDelete);
                await _context.SaveChangesAsync();
                return Ok($"Le salarié avec l'ID {id} a été supprimé avec succès.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erreur lors de la suppression du salarié : {ex.Message}");
            }
        }

        // Mettre à jour un salarié par son ID
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Salarie salarie)
        {
            if (id != salarie.id)
            {
                return BadRequest("L'ID dans l'URL ne correspond pas à l'ID du salarié.");
            }

            try
            {
                var salarieToUpdate = await _context.Salarie.FindAsync(id);

                if (salarieToUpdate == null)
                {
                    return NotFound($"Le salarié avec l'ID {id} n'existe pas.");
                }

                // Mise à jour des champs
                salarieToUpdate.nom = salarie.nom;
                salarieToUpdate.prenom = salarie.prenom;
                salarieToUpdate.telFixe = salarie.telFixe;
                salarieToUpdate.telMobile = salarie.telMobile;
                salarieToUpdate.email = salarie.email;
                salarieToUpdate.idService = salarie.idService; // Mise à jour de l'ID service
                salarieToUpdate.idSite = salarie.idSite;       // Mise à jour de l'ID site

                _context.Entry(salarieToUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok($"Le salarié avec l'ID {id} a été mis à jour avec succès.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erreur lors de la mise à jour du salarié : {ex.Message}");
            }
        }
    }
}
