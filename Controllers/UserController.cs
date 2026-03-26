using Microsoft.AspNetCore.Mvc;
using ResearchProjectManagementSystem.Models;
using ResearchProjectManagementSystem.Services;

namespace ResearchProjectManagementSystem.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private ResearchProjectsContext _db;

        public UserController(ResearchProjectsContext db)
        {
            _db = db;
        }

        [HttpGet("[action]")]
        public IActionResult GetUser(int id)
        {
            UserModel? user = _db.Users.SingleOrDefault(u => u.IdUser == id);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(new {
                idUser = user.IdUser,
                username = user.Username,
                emailAddress = user.EmailAddress,
                displayName = user.DisplayName,
                registerDate = user.RegisterDate,
                userRole = user.UserRole
            });
        }
    }
}
