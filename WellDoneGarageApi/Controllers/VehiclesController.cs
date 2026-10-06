using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellDoneGarageApi.Data;
using WellDoneGarageApi.DTOs;
using WellDoneGarageApi.Models;

namespace WellDoneGarageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly GarageDbContext _context;
        public VehiclesController(GarageDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleResponseDto>>> GetVehicles()
        {
            List<VehicleResponseDto> vehicles = await _context.Vehicles
                .Select(v => new VehicleResponseDto
                {
                    Id = v.Id,
                    CustomerId = v.CustomerId,
                    CustomerName = v.Customer.Name,
                    VehicleNumber = v.VehicleNumber,
                    Model = v.Model,
                    Description = v.Description,
                    CreatedAt = v.CreatedAt,
                    UpdatedAt = v.UpdatedAt
                })
                .ToListAsync();

            return Ok(vehicles);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleResponseDto>> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles
                .Select(v => new VehicleResponseDto
                {
                    Id = v.Id,
                    CustomerId = v.CustomerId,
                    CustomerName = v.Customer.Name,
                    VehicleNumber = v.VehicleNumber,
                    Model = v.Model,
                    Description = v.Description,
                    CreatedAt = v.CreatedAt,
                    UpdatedAt = v.UpdatedAt
                })
                .FirstOrDefaultAsync(v => v.Id == id);
            if(vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }
        [HttpPost]
        public async Task<ActionResult<VehicleResponseDto>> CreateVehicle([FromBody]VehicleCreateDto vehicleDto)
        {
            var existingCustomer = await _context.Customers
                .AnyAsync(c => c.Id == vehicleDto.CustomerId);
            if (!existingCustomer)
            {
                return BadRequest("Customer does not exist.");
            }

            var existingVehicle = await _context.Vehicles
                .AnyAsync(v => v.VehicleNumber == vehicleDto.VehicleNumber);
            if (existingVehicle)
            {
                return Conflict("Vehicle number already exists.");
            }

            var vehicle = new Vehicle
            {
                CustomerId = vehicleDto.CustomerId,
                VehicleNumber = vehicleDto.VehicleNumber,
                Model = vehicleDto.Model,
                Description = vehicleDto.Description,
                CreatedAt = DateTime.UtcNow
            };
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            var vehicleResponseDto = await _context.Vehicles
                .Where(v => v.Id == vehicle.Id)
                .Select(v => new VehicleResponseDto
                {
                    Id = v.Id,
                    CustomerId = v.CustomerId,
                    CustomerName = v.Customer.Name,
                    VehicleNumber = v.VehicleNumber,
                    Model = v.Model,
                    Description = v.Description,
                    CreatedAt = v.CreatedAt,
                    UpdatedAt = v.UpdatedAt
                })
                .FirstAsync();

            return CreatedAtAction(nameof(GetVehicle), new {id = vehicle.Id}, vehicleResponseDto);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleResponseDto>> UpdateVehicle([FromBody]VehicleUpdateDto vehicleDto, int id)
        {
            var vehicle = await _context.Vehicles
                .FindAsync(id);
            if (vehicle == null )
            {
                return NotFound();
            }

            var customerExists = await _context.Customers
                .AnyAsync(v => v.Id == vehicleDto.CustomerId);
            if(!customerExists)
            {
                return BadRequest("Customer does not exist.");
            }

            var vehicleNumberExists = await _context.Vehicles
               .AnyAsync(v => v.VehicleNumber == vehicleDto.VehicleNumber && id != v.Id);
            if (vehicleNumberExists)
            {
                return Conflict("Vehicle number already exists.");
            }

            vehicle.CustomerId = vehicleDto.CustomerId;
            vehicle.VehicleNumber = vehicleDto.VehicleNumber;
            vehicle.Model = vehicleDto.Model;
            vehicle.Description = vehicleDto.Description;
            vehicle.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var vehicleResponseDto = await _context.Vehicles
                .Where(v => v.Id == vehicle.Id)
                .Select(v => new VehicleResponseDto
                {
                    Id = v.Id,
                    CustomerId = v.CustomerId,
                    CustomerName = v.Customer.Name,
                    VehicleNumber = v.VehicleNumber,
                    Model = v.Model,
                    Description = v.Description,
                    CreatedAt = v.CreatedAt,
                    UpdatedAt = v.UpdatedAt
                })
                .FirstAsync();

            return Ok(vehicleResponseDto);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            vehicle.IsDeleted = true;
            vehicle.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
