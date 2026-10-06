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
    public class ItemsController : ControllerBase
    {
        private readonly GarageDbContext _context;
        public ItemsController(GarageDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetItems()
        {
            var items = await _context.Items
                .Select(i => new ItemResponseDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    UnitPrice = i.UnitPrice,
                    CurrentStock = i.CurrentStock,
                    CreatedAt = i.CreatedAt,
                    UpdatedAt = i.UpdatedAt
                })
                .ToListAsync();

            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemResponseDto>> GetItem([FromRoute] int id)
        {
            var item = await _context.Items
                .Where(i => i.Id == id)
                .Select(i => new ItemResponseDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    UnitPrice = i.UnitPrice,
                    CurrentStock = i.CurrentStock,
                    CreatedAt = i.CreatedAt,
                    UpdatedAt = i.UpdatedAt
                })
                .FirstOrDefaultAsync();
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
        [HttpPost]
        public async Task<ActionResult<ItemResponseDto>> CreateItem([FromBody] ItemCreateDto itemDto)
        {
            var item = new Item
            {
                Name = itemDto.Name,
                Description = itemDto.Description,
                UnitPrice = itemDto.UnitPrice,
                CurrentStock = 0, // Default stock to 0 when creating a new item
                CreatedAt = DateTime.UtcNow
            };
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            var itemResponseDto = new ItemResponseDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                UnitPrice = item.UnitPrice,
                CurrentStock = item.CurrentStock,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            };
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, itemResponseDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem([FromRoute] int id, [FromBody] ItemUpdateDto itemDto)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            item.Name = itemDto.Name;
            item.Description = itemDto.Description;
            item.UnitPrice = itemDto.UnitPrice;
            item.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var itemResponseDto = new ItemResponseDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                UnitPrice = item.UnitPrice,
                CurrentStock = item.CurrentStock,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            };

            return Ok(itemResponseDto);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            item.IsDeleted = true;
            item.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
