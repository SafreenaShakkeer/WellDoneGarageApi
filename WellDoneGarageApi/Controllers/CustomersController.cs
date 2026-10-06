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
    public class CustomersController : ControllerBase
    {
        private readonly GarageDbContext _context;
        public CustomersController(GarageDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetCustomers()
        {
            List<CustomerResponseDto> customers = await _context.Customers
                .Select(c => new CustomerResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Phone = c.Phone,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync();
            
            return Ok(customers);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDto>> GetCustomer([FromRoute] int id)
        {
            var customer = await _context.Customers
                .Where(c => c.Id == id)
                .Select(c => new CustomerResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Phone = c.Phone,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt= c.UpdatedAt
                })
                .FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }
        [HttpPost]
        public async Task<ActionResult<CustomerResponseDto>> CreateCustomer([FromBody] CustomerCreateDto customerDto)
        {
            var customer = new Customer
            {
                Name = customerDto.Name,
                Phone = customerDto.Phone,
                CreatedAt = DateTime.UtcNow
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            var customerResponseDto = new CustomerResponseDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            };

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customerResponseDto);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerResponseDto>> UpdateCustomer(int id,[FromBody] CustomerUpdateDto customerDto)
        {
            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }
            existingCustomer.Name = customerDto.Name;
            existingCustomer.Phone = customerDto.Phone;
            existingCustomer.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            var customerResponseDto = new CustomerResponseDto {
                Id = existingCustomer.Id,
                Name = existingCustomer.Name,
                Phone = existingCustomer.Phone,
                CreatedAt = existingCustomer.CreatedAt,
                UpdatedAt = existingCustomer.UpdatedAt
            };

            return Ok(customerResponseDto);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }
            existingCustomer.IsDeleted = true;
            existingCustomer.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
