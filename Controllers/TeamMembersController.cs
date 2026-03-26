using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResearchProjectManagementSystem.Models;
using ResearchProjectManagementSystem.Payloads;
using ResearchProjectManagementSystem.Services;

namespace ResearchProjectManagementSystem.Controllers
{
    [Route("[controller]")]
    public class TeamMembersController: ControllerBase
    {
        private ResearchProjectsContext _db;

        public TeamMembersController(ResearchProjectsContext db)
        {
            _db = db;
        }

        [HttpGet("[action]")]
        private IActionResult Get(int id)
        {
            List<TeamMemberModel> teamMembers = _db.TeamMembers
                .Include(t => t.Researcher)
                .Include(t => t.TeamMemberTasks)
                .Include(t => t.TeamMemberRole)
                .Where(t => t.IdResearchProject == id)
                .ToList();
            return Ok(new { teamMembers = teamMembers });
        }

        [HttpPost("[action]")]
        private IActionResult Add(int idProject, int idUser)
        {
            ResearchProjectModel? project = _db.ResearchProjects.SingleOrDefault(p => p.IdProject == idProject);
            UserModel? user = _db.Users.SingleOrDefault(u => u.IdUser == idUser);

            if(project == null || user == null)
            {
                return BadRequest(new { message = "Invalid request" });
            }

            TeamMemberModel teamMember = new TeamMemberModel()
            {
                TeamJoinDate = DateTime.Now,
                IdResearcher = idUser,
                IdResearchProject = idProject,
                TeamMemberTasks = new List<TeamMemberTaskModel>(),
                TeamMemberRole = null
            };

            _db.TeamMembers.Add(teamMember);
            _db.SaveChanges();
            return Ok(new { teamMember = teamMember });
        }

        [HttpDelete("[action]")]
        private IActionResult Remove(int id)
        {
            TeamMemberModel? teamMember = _db.TeamMembers
                .Include(m => m.TeamMemberTasks)
                .Include(m => m.TeamMemberRole)
                .SingleOrDefault(m => m.IdTeamMember == id);

            if(teamMember == null)
            {
                return NotFound();
            }

            if(teamMember.TeamMemberRole != null)
            {
                _db.TeamMemberRoles.Remove(teamMember.TeamMemberRole);
            }
            _db.TeamMemberTasks.RemoveRange(teamMember.TeamMemberTasks);
            _db.TeamMembers.Remove(teamMember);

            _db.SaveChanges();
            return Ok();
        }
    }
}
