using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellDoneGarageApi.Data;
using WellDoneGarageApi.DTOs;
using WellDoneGarageApi.Enums;
using WellDoneGarageApi.Models;

namespace WellDoneGarageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCardsController : ControllerBase
    {
        private readonly GarageDbContext _context;
        public JobCardsController(GarageDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobCardResponseDto>>> GetJobCards()
        {
            var jobcards = await _context.JobCards
                .Select(j => new JobCardResponseDto
                {
                    Id = j.Id,
                    VehicleId = j.VehicleId,
                    VehicleNumber = j.Vehicle.VehicleNumber,
                    Model = j.Vehicle.Model,
                    CustomerId = j.CustomerId,
                    CustomerName = j.Vehicle.Customer.Name,
                    AssignedMechanic = j.AssignedMechanic,
                    ComplaintDescription = j.ComplaintDescription,
                    Status = j.Status.ToString(),
                    CompletedAt = j.CompletedAt,
                    CreatedAt = j.CreatedAt,
                    UpdatedAt = j.UpdatedAt
                })
                .ToListAsync();

            return Ok(jobcards);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<JobCardResponseDto>> GetJobCardById(int id)
        {
            var jobcard = await _context.JobCards
                .Where(j => j.Id == id)
                .Select(j => new JobCardResponseDto
                {
                    Id = j.Id,
                    VehicleId = j.VehicleId,
                    VehicleNumber = j.Vehicle.VehicleNumber,
                    Model = j.Vehicle.Model,
                    CustomerId = j.CustomerId,
                    CustomerName = j.Vehicle.Customer.Name,
                    AssignedMechanic = j.AssignedMechanic,
                    ComplaintDescription = j.ComplaintDescription,
                    Status = j.Status.ToString(),
                    CompletedAt = j.CompletedAt,
                    CreatedAt = j.CreatedAt,
                    UpdatedAt = j.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (jobcard == null)
            {
                return NotFound();
            }

            return Ok(jobcard);
        }
        [HttpPost]
        public async Task<ActionResult<JobCardResponseDto>> CreateJobCard(JobCardCreateDto jobCardCreateDto)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.Customer)
                .FirstOrDefaultAsync(v => v.Id == jobCardCreateDto.VehicleId);
            if (vehicle == null)
            {
                return NotFound("Vehicle not found.");
            }
            if (vehicle.Customer == null)
            {
                return BadRequest("The vehicle is not associated with an active customer.");
            }

            var jobcard = new JobCard
            {
                VehicleId = vehicle.Id,
                CustomerId = vehicle.CustomerId,
                AssignedMechanic = jobCardCreateDto.AssignedMechanic,
                ComplaintDescription = jobCardCreateDto.ComplaintDescription,
                Status = JobStatus.Pending,
                CompletedAt = null,
                CreatedAt = DateTime.Now
            };

            _context.JobCards.Add(jobcard);
            await _context.SaveChangesAsync();

            var jobCardResponseDto = new JobCardResponseDto
            {
                Id = jobcard.Id,
                VehicleId = jobcard.VehicleId,
                VehicleNumber = vehicle.VehicleNumber,
                Model = vehicle.Model,
                CustomerId = jobcard.CustomerId,
                CustomerName = vehicle.Customer.Name,
                AssignedMechanic = jobcard.AssignedMechanic,
                ComplaintDescription = jobcard.ComplaintDescription,
                Status = jobcard.Status.ToString(),
                CompletedAt = jobcard.CompletedAt,
                CreatedAt = jobcard.CreatedAt,
                UpdatedAt = jobcard.UpdatedAt
            };

            return CreatedAtAction(nameof(GetJobCardById), new { id = jobcard.Id }, jobCardResponseDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobCard(int id,[FromBody] JobCardUpdateDto jobCardUpdateDto)
        {
            var jobcard = await _context.JobCards
                .Include(j => j.Vehicle)
                .ThenInclude(v => v.Customer)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobcard == null)
            {
                return NotFound();
            }

            if (!Enum.IsDefined(typeof(JobStatus), jobCardUpdateDto.Status))
            {
                return BadRequest("Invalid job status.");
            }

            jobcard.AssignedMechanic = jobCardUpdateDto.AssignedMechanic;
            jobcard.ComplaintDescription = jobCardUpdateDto.ComplaintDescription;
            jobcard.Status = jobCardUpdateDto.Status;
            jobcard.UpdatedAt = DateTime.Now;

            if (jobCardUpdateDto.Status == JobStatus.Completed)
            {
                if (!jobcard.CompletedAt.HasValue)
                {
                    jobcard.CompletedAt = DateTime.Now;
                }
            }
            else
            {
                jobcard.CompletedAt = null;
            }
            
            await _context.SaveChangesAsync();

            var jobCardResponseDto = new JobCardResponseDto
            {
                Id = jobcard.Id,
                VehicleId = jobcard.VehicleId,
                VehicleNumber = jobcard.Vehicle.VehicleNumber,
                Model = jobcard.Vehicle.Model,
                CustomerId = jobcard.CustomerId,
                CustomerName = jobcard.Vehicle.Customer.Name,
                AssignedMechanic = jobcard.AssignedMechanic,
                ComplaintDescription = jobcard.ComplaintDescription,
                Status = jobcard.Status.ToString(),
                CompletedAt = jobcard.CompletedAt,
                CreatedAt = jobcard.CreatedAt,
                UpdatedAt = jobcard.UpdatedAt
            };

            return Ok(jobCardResponseDto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteJobCard(int id)
        {
            var jobCard = await _context.JobCards
                .FirstOrDefaultAsync(j => j.Id == id);
            if (jobCard == null)
            {
                return NotFound();
            }
            if(jobCard.Status == JobStatus.Completed)
            {
                return BadRequest("Job card cannot be deleted.");
            }
            jobCard.IsDeleted = true;
            jobCard.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
