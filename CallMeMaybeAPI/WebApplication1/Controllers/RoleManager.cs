using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        [HttpPost("protected-action")]
        public IActionResult ProtectedAction()
        {
            // Vérifiez l'en-tête X-Admin
            if (Request.Headers.TryGetValue("Admin", out var isAdmin) && isAdmin == "true")
            {
                // Effectuer l'action réservée aux administrateurs
                return Ok("Action exécutée avec succès !");
            }
            else
            {
                return Unauthorized("Vous n'avez pas les droits nécessaires.");
            }
        }
    }

}
