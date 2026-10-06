using System.ComponentModel.DataAnnotations;
using WellDoneGarageApi.Enums;

namespace WellDoneGarageApi.DTOs
{
    public class StockTransactionCreateDto
    {
        [Required]
        public int ItemId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }
        [Required]
        public ReferenceType ReferenceType { get; set; }
        public int? ReferenceId { get; set; }
    }
}
