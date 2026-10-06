using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WellDoneGarageApi.Data;
using WellDoneGarageApi.DTOs;
using WellDoneGarageApi.Enums;
using WellDoneGarageApi.Models;

namespace WellDoneGarageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTransactionsController : ControllerBase
    {
        private readonly GarageDbContext _context;
        public StockTransactionsController(GarageDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockTransactionResponseDto>>> GetStockTransactions()
        {
            var stockTransactions = await _context.StockTransactions
                .Select(st => new StockTransactionResponseDto
                {
                    Id = st.Id,
                    ItemId = st.ItemId,
                    ItemName = st.Item.Name,
                    Quantity = st.Quantity,
                    TransactionType = st.TransactionType.ToString(),
                    ReferenceType = st.ReferenceType.ToString(),
                    ReferenceId = st.ReferenceId,
                    CreatedAt = st.CreatedAt,
                    UpdatedAt = st.UpdatedAt
                })
                .ToListAsync();

            return Ok(stockTransactions);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<StockTransactionResponseDto>> GetStockTransaction(int id)
        {
            var stockTransaction = await _context.StockTransactions
                .Where(st => st.Id == id)
                .Select(st => new StockTransactionResponseDto
                {
                    Id = st.Id,
                    ItemId = st.ItemId,
                    ItemName = st.Item.Name,
                    Quantity = st.Quantity,
                    TransactionType = st.TransactionType.ToString(),
                    ReferenceType = st.ReferenceType.ToString(),
                    ReferenceId = st.ReferenceId,
                    CreatedAt = st.CreatedAt,
                    UpdatedAt = st.UpdatedAt
                })
                .FirstOrDefaultAsync();
            if (stockTransaction == null)
            {
                return NotFound();
            }

            return Ok(stockTransaction);
        }
        [HttpPost]
        public async Task<ActionResult<StockTransactionResponseDto>> CreateStockTransaction(StockTransactionCreateDto stockTransactionCreateDto)
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(i => i.Id == stockTransactionCreateDto.ItemId);
            if (item == null)
            {
                return NotFound("Item not found.");
            }
            if (stockTransactionCreateDto.TransactionType == TransactionType.IN)
            {
                item.CurrentStock += stockTransactionCreateDto.Quantity;
            }
            else if (stockTransactionCreateDto.TransactionType == TransactionType.OUT)
            {
                if (item.CurrentStock < stockTransactionCreateDto.Quantity)
                {
                    return BadRequest("Insufficient stock.");
                }

                item.CurrentStock -= stockTransactionCreateDto.Quantity;
            }

            item.UpdatedAt = DateTime.UtcNow;

            var stockTransaction = new StockTransaction
            {
                ItemId = stockTransactionCreateDto.ItemId,
                Quantity = stockTransactionCreateDto.Quantity,
                TransactionType = stockTransactionCreateDto.TransactionType,
                ReferenceType = stockTransactionCreateDto.ReferenceType,
                ReferenceId = stockTransactionCreateDto.ReferenceId,
                CreatedAt = DateTime.UtcNow
            };
            _context.StockTransactions.Add(stockTransaction);
            await _context.SaveChangesAsync();

            var stockTransactionResponseDto = new StockTransactionResponseDto
            {
                Id = stockTransaction.Id,
                ItemId = stockTransaction.ItemId,
                ItemName = stockTransaction.Item.Name,
                Quantity = stockTransaction.Quantity,
                TransactionType = stockTransaction.TransactionType.ToString(),
                ReferenceType = stockTransaction.ReferenceType.ToString(),
                ReferenceId = stockTransaction.ReferenceId,
                CreatedAt = stockTransaction.CreatedAt,
                UpdatedAt = stockTransaction.UpdatedAt
            };

            return CreatedAtAction(nameof(GetStockTransaction), new { id = stockTransaction.Id }, stockTransactionResponseDto);
        }
    }
}
