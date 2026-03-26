using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ResearchProjectManagementSystem.Models;
using ResearchProjectManagementSystem.Payloads;
using ResearchProjectManagementSystem.Services;

namespace ResearchProjectManagementSystem.Controllers
{
    [Route("[controller]")]
    public class ResultsController: Controller
    {
        private ResearchProjectsContext _db;

        public ResultsController(ResearchProjectsContext db)
        {
            _db = db;
        }

        private string? ValidatePayload(ResultPayload payload)
        {
            if (payload == null)
            {
                return "Invalid request";
            }

            if (payload.Name.Length == 0)
            {
                return "Result name required";
            }

            if (payload.Name.Length > 50)
            {
                return "Result name must not be longer than 50 characters";
            }

            if (payload.Id == 0)
            {
                //Creation payload validation

                if (payload.IdStage == null || _db.Stages.Any(s => s.IdStage == payload.IdStage))
                {
                    return "Invalid stage";
                }
                if (_db.ResearchResults.Any(r => r.IdProjectStage == payload.IdStage && r.ResultName == payload.Name))
                {
                    return "A result with that name already exists";
                }
            }
            else
            {
                //Update payload Validation

                ResearchResultModel? result = _db.ResearchResults.SingleOrDefault(r => r.IdResult == payload.Id);
                if (result == null)
                {
                    return "Result does not exist";
                }

                if (_db.ResearchResults.Any(r => r.IdProjectStage == result.IdProjectStage && r.ResultName == payload.Name))
                {
                    return "A result with that name already exists";
                }
            }
            return null;
        }

        [HttpPost("[action]")]
        public IActionResult Create([FromBody] ResultPayload payload)
        {
            string? err = ValidatePayload(payload);
            if (err != null)
            {
                return BadRequest(new { message = err });
            }

            ResearchResultModel newResult = new ResearchResultModel()
            {
                ResultName = payload.Name,
                ResultDate = DateTime.Now,
                IdProjectStage = (int)payload.IdStage
            };

            EntityEntry<ResearchResultModel> entry = _db.ResearchResults.Add(newResult);
            _db.SaveChanges();
            return Ok(new { id = entry.Entity.IdResult });
        }

        [HttpPatch("[action]")]
        public IActionResult Update([FromBody] ResultPayload payload)
        {
            string? err = ValidatePayload(payload);
            if (err != null)
            {
                return BadRequest(new { message = err });
            }

            ResearchResultModel result = _db.ResearchResults.Single(r => r.IdResult == payload.Id);

            result.ResultName = payload.Name;

            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id)
        {
            ResearchResultModel? result = _db.ResearchResults.SingleOrDefault(r => r.IdResult == id);
            if (result == null)
            {
                return NotFound();
            }

            _db.ResearchResults.Remove(result);
            _db.SaveChanges();
            return Ok();
        }
    }
}
