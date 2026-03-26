using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ResearchProjectManagementSystem.Models;
using ResearchProjectManagementSystem.Payloads;
using ResearchProjectManagementSystem.Services;

namespace ResearchProjectManagementSystem.Controllers
{
    [Route("[controller]")]
    public class ReportsController: Controller
    {
        private ResearchProjectsContext _db;

        public ReportsController(ResearchProjectsContext db)
        {
            _db = db;
        }

        private string? ValidatePayload(ReportPayload payload)
        {
            if (payload == null)
            {
                return "Invalid request";
            }

            if(payload.Name.Length == 0)
            {
                return "Report name required";
            }

            if(payload.Name.Length > 50)
            {
                return "Report name must not be longer than 50 characters";
            }

            if(payload.Id == 0)
            {
                //Creation payload validation

                if(payload.IdStage == null || _db.Stages.Any(s => s.IdStage == payload.IdStage))
                {
                    return "Invalid stage";
                }
                if (_db.Reports.Any(r => r.IdProjectStage == payload.IdStage && r.ReportName == payload.Name))
                {
                    return "A report with that name already exists";
                }
            }
            else
            {
                //Update payload Validation

                ReportModel? report = _db.Reports.SingleOrDefault(r => r.IdReport == payload.Id);
                if (report == null)
                {
                    return "Report does not exist";
                }

                if(_db.Reports.Any(r => r.IdProjectStage == report.IdProjectStage && r.ReportName == payload.Name))
                {
                    return "A report with that name already exists";
                }
            }
            return null;
        }

        [HttpPost("[action]")]
        public IActionResult Create([FromBody] ReportPayload payload)
        {
            string? err = ValidatePayload(payload);
            if (err != null)
            {
                return BadRequest(new {message = err});
            }

            ReportModel newReport = new ReportModel()
            {
                ReportName = payload.Name,
                CreationDate = DateTime.Now,
                IdProjectStage = (int) payload.IdStage
            };

            EntityEntry<ReportModel> entry = _db.Reports.Add(newReport);
            _db.SaveChanges();
            return Ok(new {id = entry.Entity.IdReport});
        }

        [HttpPatch("[action]")]
        public IActionResult Update([FromBody] ReportPayload payload)
        {
            string? err = ValidatePayload(payload);
            if(err != null)
            {
                return BadRequest(new {message = err});
            }

            ReportModel report = _db.Reports.Single(r => r.IdReport == payload.Id);

            report.ReportName = payload.Name;

            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete("[action]")]
        public IActionResult Delete(int id)
        {
            ReportModel? report = _db.Reports.SingleOrDefault(r => r.IdReport == id);
            if(report == null)
            {
                return NotFound();
            }

            _db.Reports.Remove(report);
            _db.SaveChanges();
            return Ok();
        }
    }
}
