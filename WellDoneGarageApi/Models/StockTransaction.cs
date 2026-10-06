using WellDoneGarageApi.Enums;

namespace WellDoneGarageApi.Models
{
    public class StockTransaction : BaseEntity
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public TransactionType TransactionType { get; set; }
        public ReferenceType ReferenceType { get; set; }
        public int? ReferenceId { get; set; }

        // Navigation property
        public Item? Item { get; set; }
    }
}
