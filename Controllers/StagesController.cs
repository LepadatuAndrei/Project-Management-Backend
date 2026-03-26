using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ResearchProjectManagementSystem.Models;
using ResearchProjectManagementSystem.Payloads;
using ResearchProjectManagementSystem.Services;

namespace ResearchProjectManagementSystem.Controllers
{
    [Route("[controller]")]
    public class StagesController: Controller
    {
        private ResearchProjectsContext _db;

        public StagesController(ResearchProjectsContext db)
        {
            _db = db;
        }

        private string? ValidatePayload(StagePayload payload)
        {
            if(payload.Name.Length == 0 || payload.Name.Length > 100)
            {
                return "Stage name must be between 1 and 100 characters long";
            }
            if(payload.Tasks.Length > 10000)
            {
                return "Stage tasks must not be more than 10000 characters long";
            }

            return null;
        }

        [HttpPost("[action]")]
        public IActionResult Create(int idProject, [FromBody] StagePayload payload)
        {
            ResearchProjectModel? project = _db.ResearchProjects.SingleOrDefault(p => p.IdProject == idProject);
            if (project == null)
            {
                return NotFound();
            }

            string? err = ValidatePayload(payload);
            if (err != null)
            {
                return BadRequest(new { message = err });
            }

            if(_db.Stages.Any(s => s.StageName == payload.Name && s.IdResearchProject == idProject))
            {
                return BadRequest(new { message = "A stage with that name already exists for this project" });
            }

            StageModel newStage = new StageModel() {
                StageName = payload.Name,
                Tasks = payload.Tasks,
                StageStatus = payload.Status == null ? "Not started" : payload.Status,
                IdResearchProject = idProject,
                ResearchProject = project,
                Budget = new BudgetModel()
                {
                    Amount = 0,
                    Currency = "EUR"
                },
                Reports = new List<ReportModel>(),
                ResearchResults = new List<ResearchResultModel>()
            };

            EntityEntry<StageModel> entry = _db.Stages.Add(newStage);
            _db.SaveChanges();

            return Ok(new { stage = entry.Entity });
        }

        [HttpPost("[action]")]
        public IActionResult Update(int idStage, [FromBody] StagePayload payload)
        {
            if (payload == null)
            {
                return BadRequest(new {message = "Invalid request"});
            }

            string? err = ValidatePayload(payload);
            if (err != null)
            {
                return BadRequest(new { message = err });
            }

            StageModel? stage = _db.Stages.SingleOrDefault(s => s.IdStage == idStage);
            if (stage == null)
            {
                return NotFound();
            }
            
            stage.StageName = payload.Name;
            stage.Tasks = payload.Tasks;
            stage.StageStatus = payload.Status == null ? stage.StageStatus : payload.Status;

            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int idStage)
        {
            StageModel? stage = _db.Stages
                .Include(s => s.Budget)
                .SingleOrDefault(s => s.IdStage == idStage);
            if (stage == null)
            {
                return NotFound(new {message = "Invalid request"});
            }
            _db.Budgets.Remove(stage.Budget);
            _db.Stages.Remove(stage);
            _db.SaveChanges();
            return Ok();
        }
    }
}
