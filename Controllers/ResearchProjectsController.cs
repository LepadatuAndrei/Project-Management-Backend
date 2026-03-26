#nullable disable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResearchProjectManagementSystem.Services;
using ResearchProjectManagementSystem.Models;
using ResearchProjectManagementSystem.Payloads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ResearchProjectManagementSystem.Controllers
{
    [Route("[controller]")]
    public class ResearchProjectsController : Controller
    {
        private ResearchProjectsContext _db;

        public ResearchProjectsController(ResearchProjectsContext db)
        {
            _db = db;
        }

        private string? ValidatePayload(ResearchProjectPayload payload)
        {
            if (payload.Name.Length < 5 || payload.Name.Length > 100)
            {
                return "Project name must be between 5 and 100 characters long";
            }
            if (_db.ResearchProjects.Any(p => p.ProjectName == payload.Name))
            {
                return "A project with that name already exists";
            }
            if (payload.Description.Length > 10000)
            {
                return "Project description must not be more than 10000 characters long";
            }
            if (payload.Objectives.Length > 10000)
            {
                return "Project objectives must not be more than 10000 characters long";
            }

            return null;
        }

        [HttpGet("[action]")]
        public IActionResult GetPage(int pageNum, int pageSize, string orderBy, bool orderAscending)
        {
            IEnumerable<ResearchProjectModel> projects = null;
            switch (orderBy)
            {
                case "ProjectName":
                    if(orderAscending)
                        projects = _db.ResearchProjects.OrderBy(p => p.ProjectName);
                    else
                        projects = _db.ResearchProjects.OrderByDescending(p => p.ProjectName);
                    break;
                case "IdProject":
                default:
                    if(orderAscending)
                        projects = _db.ResearchProjects.OrderBy(p => p.IdProject);
                    else
                        projects= _db.ResearchProjects.OrderByDescending(p => p.IdProject);
                    break;
            }
            int totalPageNumber = (int) Math.Ceiling((decimal) _db.ResearchProjects.Count()/pageSize);
            List<ResearchProjectModel> page = projects
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return Ok(new { page = page, pageNumber = totalPageNumber });
        }
        
        [HttpGet("[action]")]
        public IActionResult GetDetails(int id)
        {
            ResearchProjectModel project = _db.ResearchProjects
                .Include(r => r.Stages)
                .Include(r => r.Budget)
                .Include(r => r.TeamMembers)
                .Include(r => r.Acquisitions)
                .Include(r => r.ResourceAllocations)
                .ThenInclude(alloc => alloc.Resource)
                .SingleOrDefault(r => r.IdProject == id);
            if(project == null)
            {
                return NotFound();
            }
            return Ok(new {project = project});
        }

        [HttpPost("[action]")]
        public IActionResult Create([FromBody] ResearchProjectPayload projectPayload)
        {
            if (projectPayload == null || projectPayload.UserId == null || !_db.Users.Any(u => u.IdUser == projectPayload.UserId))
            {
                return BadRequest(new {message="Request Invalid"});
            }
            string? err = ValidatePayload(projectPayload);
            if(err != null)
            {
                return BadRequest(new {message=err});
            }
            ResearchProjectModel newProject = new ResearchProjectModel()
            {
                ProjectName = projectPayload.Name,
                ProjectDescription = projectPayload.Description,
                ProjectObjectives = projectPayload.Objectives,
                ProjectCreationDate = DateTime.UtcNow,
                TeamMembers = new List<TeamMemberModel>(),
                Budget = new BudgetModel()
                {
                    Amount = 0,
                    Currency = "EUR"
                }
            };

            // Add project's creator as project manager
            TeamMemberModel projectManager = new TeamMemberModel()
            {
                TeamJoinDate = DateTime.UtcNow,
                IdResearcher = (int) projectPayload.UserId,
                TeamMemberTasks = new List<TeamMemberTaskModel>(),
                ResearchProject = newProject
            };
            projectManager.TeamMemberRole = new TeamMemberRoleModel()
            {
                RoleName = "Project Manager",
                TeamMember = projectManager
            };
            newProject.TeamMembers.Add(projectManager);

            EntityEntry<ResearchProjectModel> entry = _db.ResearchProjects.Add(newProject);
            _db.SaveChanges();
            return Ok(new {id = entry.Entity.IdProject});
        }

        [HttpPatch("[action]")]
        public IActionResult Update([FromBody] ResearchProjectPayload payload)
        {
            ResearchProjectModel project = _db.ResearchProjects.SingleOrDefault(p => p.IdProject == payload.Id);
            if(project == null || payload == null)
            {
                return BadRequest(new {message="Request Invalid"});
            }

            string? err = ValidatePayload(payload);
            if (err != null)
            {
                return BadRequest(new { message = err });
            }

            project.ProjectName = payload.Name;
            project.ProjectDescription = payload.Description;
            project.ProjectObjectives = payload.Objectives;

            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int idProject)
        {
            ResearchProjectModel project = _db.ResearchProjects
                .Include(p => p.Budget)
                .Include(p => p.Stages)
                .ThenInclude(s => s.Budget)
                .Include(p => p.TeamMembers)
                .ThenInclude(t => t.TeamMemberRole)
                .Include(p => p.TeamMembers)
                .ThenInclude(t => t.TeamMemberTasks)
                .Include(p => p.ResourceAllocations)
                .Include(p => p.Acquisitions)
                .SingleOrDefault(p => p.IdProject == idProject);
            if (project == null)
            {
                return NotFound(new {message = "Invalid request"});
            }

            foreach(StageModel stage in project.Stages)
            {
                _db.Budgets.Remove(stage.Budget);
            }
            _db.Budgets.Remove(project.Budget);
            _db.ResearchProjects.Remove(project);
            _db.SaveChanges();
            return Ok();
        }
    }
}
